namespace BlazorApp.Models;

public class BlogPost
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Images { get; set; } = [];
    public int ReadingTimeMinutes { get; set; }
    public List<string> Tags { get; set; } = [];
    public bool Featured { get; set; }
}
