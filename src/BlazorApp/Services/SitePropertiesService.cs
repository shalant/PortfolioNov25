using System.Net.Http.Json;
using BlazorApp.Models;

namespace BlazorApp.Services;

public sealed class SitePropertiesService : IDisposable
{
    private readonly HttpClient _client;
    private readonly Task<SiteProperties?> _getPropertiesTask;

    public SitePropertiesService(HttpClient client)
    {
        _client = client;
        _getPropertiesTask = _client.GetFromJsonAsync<SiteProperties>("sample-data/siteproperties.json");
    }

    public Task<SiteProperties?> GetAsync() => _getPropertiesTask;

    public void Dispose() => _client.Dispose();
}
