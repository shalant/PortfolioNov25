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
    public string PriceRange { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    /// <summary>Structured pricing backing the page's schema.org Offer/PriceSpecification JSON-LD.
    /// PriceRange above stays the human-readable display string on the card ("$750 – $3,000 per
    /// project") — these back the machine-readable version so search/AI engines get exact numbers
    /// instead of having to parse that string.</summary>
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string PriceUnit { get; set; } = string.Empty;
}
