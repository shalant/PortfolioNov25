namespace BlazorApp.Models;

public class WebDesignModel
{
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public List<string> Tools { get; set; } = [];
    public List<WebDesignProject> Projects { get; set; } = [];
}

public class WebDesignProject
{
    public string Slug { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string? Status { get; set; }
    public string SiteLabel { get; set; } = string.Empty;
    public string? ExternalUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public List<string> Images { get; set; } = [];
    public List<string> Bullets { get; set; } = [];

    // Detail page only
    public string Tagline { get; set; } = string.Empty;
    public List<string> Approach { get; set; } = [];
    public List<WebDesignHighlight> Highlights { get; set; } = [];
    public List<WebDesignFact> Facts { get; set; } = [];

    /// <summary>Tech-stack chips shown on the detail page. Optional — projects without one (Squarespace builds) just skip the row.</summary>
    public List<string> Stack { get; set; } = [];

    /// <summary>Parallel to Images by index; empty string (or a short list) means no caption for that image.</summary>
    public List<string> ImageCaptions { get; set; } = [];

    /// <summary>Optional before/after comparison. Only set for projects that had a real prior site to show — omit rather than fabricate one.</summary>
    public string? BeforeImage { get; set; }
    public string? BeforeCaption { get; set; }
    public string? BeforeSourceUrl { get; set; }
}

public class WebDesignHighlight
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class WebDesignFact
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
