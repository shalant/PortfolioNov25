namespace BlazorApp.Models;

public class ServicesModel
{
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public string Cta { get; set; } = string.Empty;
    public List<ServiceOffering> Offerings { get; set; } = [];
}

public class ServiceOffering
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Includes { get; set; } = [];
    public string Engagement { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
