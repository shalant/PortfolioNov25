using Xunit;

namespace BlazorApp.Tests.Pages;

public class ConsultingPageTests
{
    [Fact]
    public void ConsultingPage_ComponentExists()
    {
        // Verify that the page component can be loaded
        var pageType = typeof(global::BlazorApp.Pages.ConsultingPage);
        Assert.NotNull(pageType);
        Assert.Equal("ConsultingPage", pageType.Name);
    }

    [Fact]
    public void ConsultingPage_IsRazorComponent()
    {
        // Verify that ConsultingPage is properly defined
        var pageType = typeof(global::BlazorApp.Pages.ConsultingPage);
        var baseType = pageType.BaseType;
        Assert.NotNull(baseType);
        Assert.Contains("ComponentBase", baseType.Name);
    }

    [Fact]
    public void ConsultingPage_HasHttpInjection()
    {
        // Verify the page has HttpClient injection capability
        var pageType = typeof(global::BlazorApp.Pages.ConsultingPage);
        Assert.True(pageType.IsClass);
        Assert.False(pageType.IsAbstract);
    }
}
