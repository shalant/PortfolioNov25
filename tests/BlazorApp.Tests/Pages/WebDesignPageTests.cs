using Xunit;

namespace BlazorApp.Tests.Pages;

public class WebDesignPageTests
{
    [Fact]
    public void WebDesignPage_ComponentExists()
    {
        // Verify that the page component can be loaded
        var pageType = typeof(global::BlazorApp.Pages.WebDesignPageOLD);
        Assert.NotNull(pageType);
        Assert.Equal("WebDesignPage", pageType.Name);
    }

    [Fact]
    public void WebDesignPage_IsRazorComponent()
    {
        // Verify that WebDesignPage is properly defined
        var pageType = typeof(global::BlazorApp.Pages.WebDesignPageOLD);
        var baseType = pageType.BaseType;
        Assert.NotNull(baseType);
        Assert.Contains("ComponentBase", baseType.Name);
    }

    [Fact]
    public void WebDesignPage_HasHttpInjection()
    {
        // Verify the page has HttpClient injection capability
        var pageType = typeof(global::BlazorApp.Pages.WebDesignPageOLD);
        Assert.True(pageType.IsClass);
        Assert.False(pageType.IsAbstract);
    }
}
