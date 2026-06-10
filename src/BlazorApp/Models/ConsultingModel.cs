namespace BlazorApp.Models;

public class ConsultingModel
{
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string Cta { get; set; } = string.Empty;
    public List<ConsultingService> Services { get; set; } = [];
}

public class ConsultingService
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
