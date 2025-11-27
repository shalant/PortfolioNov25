namespace BlazorApp.Models
{
    public class CasualModel
    {
        public string Description { get; set; } = string.Empty;
        public List<string> Hobbies { get; set; } = new();
        public string DetailOrQuote { get; set; } = string.Empty;
    }
}

