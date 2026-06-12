namespace BlazorApp.Models
{
    public class MusicModel
    {
        public string Paragraph1 { get; set; } = string.Empty;
        public string Paragraph2 { get; set; } = string.Empty;
        public string Paragraph3 { get; set; } = string.Empty;
        public string PressQuote { get; set; } = string.Empty;
        public string PressAttribution { get; set; } = string.Empty;
        public List<MusicAlbum> Albums { get; set; } = new();
        public List<string> Collaborators { get; set; } = new();
    }

    public class MusicAlbum
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
