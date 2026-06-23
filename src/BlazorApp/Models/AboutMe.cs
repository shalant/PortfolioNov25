namespace BlazorApp.Models;

public class AboutMe
{
    public string Description { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
    public List<AboutHighlight> Highlights { get; set; } = [];
    public string DetailOrQuote { get; set; } = string.Empty;
}

public class AboutHighlight
{
    public string Metric { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}