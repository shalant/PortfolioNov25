using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace BlogPost_Generator.Services;

public class BlogPostService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _blogPostsPath;

    public BlogPostService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(60);
        _blogPostsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "src",
            "BlazorApp",
            "wwwroot",
            "sample-data",
            "blog-posts.json"
        );
    }

    public async Task<List<string>> GenerateVersionsAsync(string title, string content)
    {
        var apiKey = _configuration["AI:AnthropicApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("AI:AnthropicApiKey not configured.");
        }

        var prompt = $@"Create 3 different versions of a beautiful, well-structured blog post from this raw content. Each version should:
- Have engaging, polished prose
- Include a compelling introduction
- Maintain the core message and technical accuracy
- Use proper HTML formatting (p, h2, h3, ul, li, code, strong, em tags)
- Be ready to publish
- Return ONLY the HTML content, no preamble

Title: {title}

Content:
{content}

Format your response EXACTLY as:
===VERSION 1===
<p>...</p>

===VERSION 2===
<p>...</p>

===VERSION 3===
<p>...</p>";

        var request = new
        {
            //model = "claude-3-5-sonnet-20241022",
            model = "claude-sonnet-5",
            max_tokens = 4000,
            messages = new[] {
                new {
                    role = "user",
                    content = prompt
                }
            }
        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        //error here
        var response = await _httpClient.PostAsync("https://api.anthropic.com/v1/messages", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Claude API error ({response.StatusCode}): {errorContent}"
            );
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var responseObj = JsonSerializer.Deserialize<ApiResponse>(responseJson, options);

        var textBlock = responseObj?.Content?.FirstOrDefault(c => c.Type == "text");
        if (textBlock?.Text == null)
        {
            throw new InvalidOperationException("Invalid response from Claude API.");
        }

        var versions = new List<string>();
        var responseText = textBlock.Text;
        Console.WriteLine($"DEBUG: Full response text:\n{responseText}");

        var versionTexts = responseText.Split("===VERSION");
        Console.WriteLine($"DEBUG: versionTexts.Length = {versionTexts.Length}");

        for (int i = 1; i <= 3 && i < versionTexts.Length; i++)
        {
            var versionText = versionTexts[i].Trim();
            var contentStart = versionText.IndexOf("\n") + 1;
            if (contentStart > 0)
            {
                var htmlContent = versionText.Substring(contentStart).Trim();
                versions.Add(htmlContent);
                Console.WriteLine($"DEBUG: Added version {i}");
            }
        }

        Console.WriteLine($"DEBUG: Total versions parsed = {versions.Count}");
        return versions;
    }

    public async Task<BlogPost> SaveBlogPostAsync(string title, string content, string? excerpt = null, List<string>? images = null)
    {
        var posts = await LoadBlogPostsAsync();

        var post = new BlogPost
        {
            Id = Guid.NewGuid().ToString("n").Substring(0, 8),
            Title = title,
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Excerpt = excerpt ?? TruncateText(content, 200),
            Content = content,
            Images = images ?? new List<string>()
        };

        posts.Add(post);

        await SaveBlogPostsAsync(posts);

        return post;
    }

    private async Task<List<BlogPost>> LoadBlogPostsAsync()
    {
        if (!File.Exists(_blogPostsPath))
        {
            return new List<BlogPost>();
        }

        var json = await File.ReadAllTextAsync(_blogPostsPath);
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return JsonSerializer.Deserialize<List<BlogPost>>(json, options) ?? new List<BlogPost>();
    }

    private async Task SaveBlogPostsAsync(List<BlogPost> posts)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(posts, options);
        await File.WriteAllTextAsync(_blogPostsPath, json);
    }

    private string TruncateText(string text, int maxLength)
    {
        var stripped = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]*>", "");
        return stripped.Length > maxLength ? stripped.Substring(0, maxLength) + "..." : stripped;
    }

    public class BlogPost
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("excerpt")]
        public string Excerpt { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("images")]
        public List<string> Images { get; set; } = new();
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
}
