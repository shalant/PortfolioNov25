namespace BlazorApp.Models
{
    public class ExperienceModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icons { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public List<string> Bulletpoints { get; set; } = new();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
