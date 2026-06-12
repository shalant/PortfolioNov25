namespace BlazorApp.Models
{
    public class CasualModel
    {
        public string Description { get; set; } = string.Empty;
        public List<Hobby> Hobbies { get; set; } = new();
        public string DetailOrQuote { get; set; } = string.Empty;
    }

    public class Hobby
    {
        public string Name { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }
}
