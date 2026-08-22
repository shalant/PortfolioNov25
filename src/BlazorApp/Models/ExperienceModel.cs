namespace BlazorApp.Models
{
    public class ExperienceModel
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icons { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        /// <summary>Optional wider lockup (badge + wordmark) shown in the large detail panel instead of Image. Image (the small circular mark) is still what renders in the side-nav list, where a wordmark wouldn't read.</summary>
        public string? DetailImage { get; set; }
        public List<string> Bulletpoints { get; set; } = new();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
