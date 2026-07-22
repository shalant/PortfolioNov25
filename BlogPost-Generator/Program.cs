using BlogPost_Generator;
using BlogPost_Generator.Components;
using BlogPost_Generator.Services;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri) });
builder.Services.AddScoped<BlogPostService>();

var webApp = builder.Build();

if (!webApp.Environment.IsDevelopment())
{
    webApp.UseExceptionHandler("/Error", createScopeForErrors: true);
    webApp.UseHsts();
}

webApp.UseHttpsRedirection();
webApp.UseStaticFiles();
webApp.UseAntiforgery();

webApp.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

webApp.Run();
