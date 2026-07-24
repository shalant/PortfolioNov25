using Xunit;

namespace BlazorApp.Tests.Components;

public class ExperienceTests
{
    [Fact]
    public void Experience_ComponentExists()
    {
        // Verify that the component type can be loaded
        var componentType = typeof(global::BlazorApp.Components.Experience);
        Assert.NotNull(componentType);
        Assert.Equal("Experience", componentType.Name);
    }

    [Fact]
    public void Experience_IsRazorComponent()
    {
        // Verify that Experience is properly defined as a component
        var componentType = typeof(global::BlazorApp.Components.Experience);
        var baseType = componentType.BaseType;
        Assert.NotNull(baseType);
        Assert.Contains("ComponentBase", baseType.Name);
    }
}
