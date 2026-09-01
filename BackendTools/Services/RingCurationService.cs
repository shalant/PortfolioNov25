using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace BackendTools.Services;

/// <summary>
/// Re-runs the /webdesign hero ring's image curation via Claude's vision API:
/// sends every case-study image, asks Claude to rank each project's images
/// best-to-worst plus an overall project quality rank, and returns a C#
/// snippet matching WebDesignPage.razor's RingCuration array. This is a
/// local, on-demand tool — nothing here runs from the deployed site; you
/// review the output and paste it in yourself. See CLAUDE.md's Ring
/// Curation section.
/// </summary>
public class RingCurationService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _webAppWwwRoot;
    private readonly string _webDesignJsonPath;

    public RingCurationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(180) };
        _webAppWwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", "src", "BlazorApp", "wwwroot");
        _webDesignJsonPath = Path.Combine(_webAppWwwRoot, "sample-data", "webdesign.json");
    }

    public async Task<RingCurationResult> RunAsync(Action<string> onProgress)
    {
        var apiKey = _configuration["AI:AnthropicApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("AI:AnthropicApiKey not configured.");
        }

        onProgress("Reading webdesign.json…");
        var json = await File.ReadAllTextAsync(_webDesignJsonPath);
        var webDesign = JsonSerializer.Deserialize<WebDesignFile>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Could not parse webdesign.json.");

        onProgress($"Found {webDesign.Projects.Count} projects. Loading and encoding images…");

        var content = new List<object>
        {
            new { type = "text", text = BuildInstructions(webDesign.Projects) }
        };

        foreach (var project in webDesign.Projects)
        {
            content.Add(new { type = "text", text = $"\n=== Project \"{project.Slug}\" — {project.Name} ({project.Industry}) ===" });
            for (var i = 0; i < project.Images.Count; i++)
            {
                var (base64, mediaType) = await LoadImageAsync(project.Images[i]);
                content.Add(new { type = "text", text = $"Image index {i}:" });
                content.Add(new
                {
                    type = "image",
                    source = new { type = "base64", media_type = mediaType, data = base64 }
                });
            }
        }

        onProgress("Calling Claude (this can take a minute with this many images)…");

        var request = new
        {
            model = "claude-sonnet-5",
            max_tokens = 4000,
            messages = new[] { new { role = "user", content } }
        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        var requestJson = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync(
            "https://api.anthropic.com/v1/messages",
            new StringContent(requestJson, Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Claude API error ({response.StatusCode}): {errorBody}");
        }

        onProgress("Parsing Claude's response…");

        var responseJson = await response.Content.ReadAsStringAsync();
        var responseObj = JsonSerializer.Deserialize<ApiResponse>(responseJson, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            ?? throw new InvalidOperationException("Empty response from Claude API.");

        var text = responseObj.Content.FirstOrDefault(c => c.Type == "text")?.Text
            ?? throw new InvalidOperationException("No text content in Claude's response.");

        var rankings = ParseRankings(text);
        var snippet = BuildSnippet(rankings, webDesign.Projects);

        return new RingCurationResult(rankings, snippet, text);
    }

    private static string BuildInstructions(List<WebDesignProject> projects)
    {
        return $@"You are curating images for a 3D rotating ""ring"" hero on a web design portfolio page. Only a handful of images from each case study will actually be shown, as small cards (roughly 400x250px), so pick for visual impact at that size — bold color, real photography, a distinctive UI — over images that are mostly whitespace or dense text.

Below are all {projects.Count} case studies with their images, each one labeled ""Image index N"" immediately before it. For EACH project, rank its images from best to worst by how well they'd read as a small ring card — you do not have to include every image, only ones genuinely worth showing (a project can have as few as 1). Then give every project an overall ""projectRank"" from 1 (strongest/most distinctive visual identity) to {projects.Count} (weakest) — every project gets a unique rank, no ties, used to decide which projects get a *second* card once every project already has its best one in.

Respond with ONLY a single JSON object, no other text, no markdown code fence, in exactly this shape:
{{""projects"":[{{""slug"":""guacamayo-band"",""projectRank"":1,""imageIndexesBestFirst"":[0,3,2]}}, ...one entry per project...]}}";
    }

    private async Task<(string Base64, string MediaType)> LoadImageAsync(string relativePath)
    {
        var fullPath = Path.Combine(_webAppWwwRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"Image referenced in webdesign.json not found on disk: {relativePath}", fullPath);
        }

        var bytes = await File.ReadAllBytesAsync(fullPath);
        var mediaType = Path.GetExtension(fullPath).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            _ => throw new NotSupportedException($"Unsupported image type: {fullPath}")
        };

        return (Convert.ToBase64String(bytes), mediaType);
    }

    private static List<ProjectRanking> ParseRankings(string claudeText)
    {
        // Claude was told not to fence the JSON, but strip one if it shows up anyway.
        var trimmed = claudeText.Trim();
        if (trimmed.StartsWith("```"))
        {
            var firstNewline = trimmed.IndexOf('\n');
            var lastFence = trimmed.LastIndexOf("```");
            trimmed = trimmed[(firstNewline + 1)..lastFence].Trim();
        }

        var parsed = JsonSerializer.Deserialize<RankingsPayload>(trimmed, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Could not parse Claude's JSON output. Raw response:\n" + claudeText);

        return parsed.Projects;
    }

    private static string BuildSnippet(List<ProjectRanking> rankings, List<WebDesignProject> projects)
    {
        var byRank = rankings.OrderBy(r => r.ProjectRank).ToList();
        var sb = new StringBuilder();
        sb.AppendLine("private static readonly (string Slug, int ProjectRank, int[] ImageIndexesBestFirst)[] RingCuration =");
        sb.AppendLine("[");
        foreach (var r in byRank)
        {
            var project = projects.FirstOrDefault(p => p.Slug == r.Slug);
            var comment = project is not null ? $" // {project.Name}" : "";
            var indexes = string.Join(", ", r.ImageIndexesBestFirst);
            sb.AppendLine($"    (\"{r.Slug}\", {r.ProjectRank}, [{indexes}]),{comment}");
        }
        sb.AppendLine("];");
        return sb.ToString();
    }

    private class RankingsPayload
    {
        [JsonPropertyName("projects")]
        public List<ProjectRanking> Projects { get; set; } = new();
    }

    private class ApiResponse
    {
        [JsonPropertyName("content")]
        public List<ContentBlock> Content { get; set; } = new();
    }

    private class ContentBlock
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    private class WebDesignFile
    {
        [JsonPropertyName("projects")]
        public List<WebDesignProject> Projects { get; set; } = new();
    }

    public class WebDesignProject
    {
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("industry")]
        public string Industry { get; set; } = string.Empty;

        [JsonPropertyName("images")]
        public List<string> Images { get; set; } = new();
    }
}

public class ProjectRanking
{
    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("projectRank")]
    public int ProjectRank { get; set; }

    [JsonPropertyName("imageIndexesBestFirst")]
    public List<int> ImageIndexesBestFirst { get; set; } = new();
}

public record RingCurationResult(List<ProjectRanking> Rankings, string Snippet, string RawResponse);
