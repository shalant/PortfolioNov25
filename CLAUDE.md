# Claude Code Development Guidelines

This document provides guidance for developing the DR Codeworks portfolio using Claude Code.

## Project Overview

**Type:** Blazor WebAssembly SPA  
**Language:** C# (Razor components)  
**Styling:** CSS3 + Bootstrap 5  
**Data:** JSON files in `wwwroot/sample-data/`  
**Key Colors:** Navy (#2c3e50) + Teal (#1abc9c)  

## Getting Started with Claude Code

### Quick Start

```bash
# Open project in Claude Code
claude-code .

# Start dev server in terminal
dotnet run --project src/BlazorApp/BlazorApp.csproj

# Visit http://localhost:5000 in browser
```

## Project Structure Quick Reference

```
src/BlazorApp/                    # Main portfolio (WebAssembly SPA)
├── Components/                  # Reusable .razor components
├── Pages/                       # Page components (Index.razor is main)
├── Services/                    # C# services
├── wwwroot/
│   ├── css/app.css             # Global + component styles
│   ├── index.html              # HTML entry point
│   ├── logos/                  # Brand assets
│   └── sample-data/
│       ├── blog-posts.json     # Published blog posts (includes images as base64)
│       ├── experience.json     # Work history
│       └── ...                 # Other JSON data
└── BlazorApp.csproj

BlogPost-Generator/              # Blog generator tool (Blazor Server)
├── Components/
│   └── BlogEditor.razor        # Photo upload + version selection
├── Services/
│   └── BlogPostService.cs      # Claude API integration + image handling
├── Program.cs                  # Startup config
├── appsettings.json            # Config (store ANTHROPIC_API_KEY in secrets)
└── BlogPost-Generator.csproj
```

## Key Files & Their Purpose

| File | Purpose | Edit When |
|------|---------|-----------|
| `Index.razor` | Main page (component composition) | Adding/removing sections |
| `Header.razor` | Navigation bar with logo | Updating nav links, branding |
| `Experience.razor` | Experience carousel | Styling carousel, loading logic |
| `ToDo.razor` | Todo list with checkboxes | Editing todos, changing styling |
| `Blog/BlogIndex.razor` | Blog landing page with featured posts | Adding sections, styling |
| `Blog/BlogPost.razor` | Individual blog post with TOC & sharing | Post display, layout |
| `Blog/BlogSearch.razor` | Search and filter interface | Search logic, filters |
| `Blog/BlogArchive.razor` | Year/month grouped archive view | Archive layout |
| `app.css` | Global styles + animations | Updating colors, fonts, effects |
| `siteproperties.json` | Name, email, social links | Personal information |
| `experience.json` | Experience entries for carousel | Work history, projects |
| `blog-posts.json` | Published blog posts (with metadata) | Adding/editing posts (use generator) |

## Code Style & Conventions

### C# / Razor

- **Naming:** `PascalCase` for classes/methods, `camelCase` for variables
- **Components:** Always include `[Parameter, EditorRequired]` for required parameters
- **Services:** Use async/await; inject via `[Parameter, EditorRequired]`
- **Lifecycle:** Prefer `OnInitializedAsync` for async setup; `OnInitialized` for sync

Example:
```csharp
[Parameter, EditorRequired]
public required HttpClient Http { get; set; }

protected override async Task OnInitializedAsync()
{
    items = await Http.GetFromJsonAsync<List<Item>>("sample-data/items.json");
}
```

### CSS

- **Global styles:** `app.css`
- **Component styles:** Embedded in `<style>` blocks
- **Colors:** Use variables consistently
  - Navy: `#2c3e50`
  - Teal: `#1abc9c`
  - Light text: `#eef2f7`
- **Mobile-first:** Use `@media (min-width: ...)` for larger screens
- **Transitions:** Use `transition: property 0.3s ease` for smooth effects

## Common Tasks

### Blog Features

The blog now includes comprehensive features for content discovery and engagement:

**Search & Filtering:**
- Full-text search across all blog posts
- Filter by tags and categories
- Results update in real-time as user types

**Archive & Discovery:**
- Year/month grouped archive view
- Easy browsing of older posts
- Related posts suggestions on individual post pages

**Content Enhancements:**
- Reading time estimates (calculated based on word count)
- Auto-generated table of contents for posts
- Featured posts section on blog landing page
- Social sharing buttons (copy link, share to social media)

**Data Structure (blog-posts.json):**
Blog post objects now include:
```json
{
  "id": "unique-id",
  "title": "Post Title",
  "content": "HTML content",
  "excerpt": "Brief summary",
  "publishDate": "2026-07-23",
  "tags": ["tag1", "tag2"],
  "category": "Category Name",
  "isFeatured": true,
  "readingTimeMinutes": 5,
  "tableOfContents": [
    {"title": "Section Title", "id": "section-id"}
  ],
  "images": ["base64-encoded-images"],
  "relatedPosts": ["related-post-ids"]
}
```

### Use the Blog Post Generator

The **BlogPost-Generator** is an internal tool—**do not deploy publicly**. Run locally only:
```bash
dotnet run --project BlogPost-Generator/BlogPost-Generator.csproj
# Runs on http://localhost:5001
```

**Features:**
- Write raw blog post content
- **Upload photos** (jpg, png, gif, webp) - images stored as base64 data URIs in JSON
- Generate 3 polished versions using Claude API
- Preview and publish to blog

**Photo Upload Notes:**
- Multiple images supported (max 10 files, 5MB total per upload call)
- Images appear as thumbnails in preview grid
- Hover to delete images before publishing
- Images stored with blog post in `wwwroot/sample-data/blog-posts.json`
- Base64 encoding keeps everything self-contained (no external image hosting)

**⚠️ Security:** This tool uses your Anthropic API key. Keep it **localhost-only**. Do not expose on public internet without authentication.

### Add a New Component

1. Create `Components/YourComponent.razor`
2. Define parameters with `[Parameter, EditorRequired]`
3. Add HTML template
4. Add `@code` block with logic
5. Include `<style>` if needed
6. Reference in `Index.razor`: `<YourComponent Http="@Http" />`

### Update Styling

1. **Global changes:** Edit `wwwroot/css/app.css`
2. **Component changes:** Edit `<style>` block in `.razor` file
3. **Colors:** Maintain navy (#2c3e50) + teal (#1abc9c) scheme
4. **Test:** Use browser DevTools (F12) to check responsiveness

### Add Data

1. Create/edit JSON in `wwwroot/sample-data/`
2. Load via `Http.GetFromJsonAsync<T>("sample-data/file.json")`
3. Bind to template with `@foreach`, `@if`, etc.

### Test Responsiveness

1. Run dev server: `dotnet run --project src/BlazorApp/BlazorApp.csproj`
2. Open http://localhost:5000
3. Press F12 (DevTools)
4. Toggle device toolbar (mobile view)
5. Test breakpoints: 300px, 420px, 768px, 1024px

## When Using Claude Code

### Things Claude Can Help With

✅ **Do:**
- Refactor components for clarity
- Add new features/components
- Update styles and animations
- Fix bugs and broken links
- Improve accessibility
- Optimize performance
- Write tests

### Things Claude Should Avoid

❌ **Don't:**
- Change node_modules or dependency versions (use package.json)
- Remove components without asking
- Change color scheme without discussion
- Modify git history
- Delete configuration files

### Code Review Checklist

Before committing, Claude should verify:

- [ ] TypeScript/C# compiles without errors
- [ ] No breaking changes to existing components
- [ ] Styles match navy + teal theme
- [ ] Responsive design tested (mobile, tablet, desktop)
- [ ] Navigation links point to correct IDs
- [ ] New assets (images, fonts) are referenced correctly
- [ ] Comments are minimal (code speaks for itself)

## Debugging Tips

### Common Issues

**Issue:** Logo not showing in navbar
- Check: `logos/drdev-logo.png` exists in `wwwroot/`
- Check: CSS path is correct in `Header.razor`
- Check: Browser cache (Ctrl+Shift+Del)

**Issue:** Styles not applying
- Check: CSS class is on the correct element
- Check: No typos in class names
- Check: Global styles aren't overridden
- Check: Cascade (more specific selectors win)

**Issue:** Navigation links broken
- Check: Section IDs match href values (e.g., `id="experience"` ↔ `href="#experience"`)
- Check: Bootstrap `target="_top"` attributes if needed

**Issue:** Component not rendering
- Check: All `[Parameter, EditorRequired]` properties are passed
- Check: Data is loading (check Network tab in DevTools)
- Check: No JavaScript errors (DevTools Console)

## Performance Optimization

### Quick Wins

1. **Lazy load images:** Use `loading="lazy"` attribute
2. **Minimize CSS:** Bootstrap is minified; keep custom CSS lean
3. **Cache data:** JSON files are cached by browser
4. **Optimize images:** Use WebP where supported, compress PNGs
5. **Use CDN:** Bootstrap loads from CDN (with local fallback)

### Monitoring

- Open DevTools → Network tab to check load times
- Open DevTools → Performance tab for rendering analysis
- Test mobile throttling in DevTools (3G, 4G)

## Deployment & Security

See **[SECURITY.md](./SECURITY.md)** for detailed blog infrastructure security & risks.

### Quick Deploy (Azure)

```bash
# Build for production
dotnet publish -c Release -o ./publish src/BlazorApp/BlazorApp.csproj

# Deploy to Azure App Service
az webapp deployment source config-zip --resource-group YOUR_RG --name YOUR_APP --src-path ./publish
```

**GitHub Pages Deployment:**
- Configured to deploy **only on `main` branch** commits
- Feature branches and fixes branches do not trigger deployment
- Ensures stability and prevents incomplete features from going live

### Security Checklist

**Before shipping to production:**

- [ ] **HTTPS only** - Enable HTTPS/TLS (Azure App Service does this by default)
- [ ] **API Keys** - Store `ANTHROPIC_API_KEY` in Azure Key Vault, NOT in code
- [ ] **Environment variables** - Use secrets management, not appsettings.json
- [ ] **Input validation** - Blog title/content validated before sending to Claude API
- [ ] **File size limits** - Images capped at 5MB per upload (already in code)
- [ ] **CSP headers** - Add Content Security Policy headers to prevent XSS
- [ ] **Regular updates** - Keep .NET, MudBlazor, and all packages current
- [ ] **Monitoring** - Enable Application Insights in Azure to catch errors/attacks
- [ ] **Backups** - Back up `blog-posts.json` before major changes

### Environment Setup (Production)

**BlogPost-Generator API Key:**
```csharp
// In Program.cs, use Key Vault for secrets:
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());

// Access in services:
var apiKey = configuration["AI:AnthropicApiKey"];
```

**✅ BlogPost-Generator stays private** - Never deploy publicly. Runs locally only. To publish blog posts: export JSON from generator, copy to `src/BlazorApp/wwwroot/sample-data/blog-posts.json`, deploy portfolio normally.

### Blocking Common Attacks

| Attack | Prevention |
|--------|-----------|
| **XSS** | Input validation on blog content, use `@Html.Raw()` only for trusted sources |
| **CSRF** | Blazor has built-in CSRF protection; don't disable it |
| **SQL Injection** | N/A - using JSON files, not SQL. Still sanitize inputs. |
| **DDoS** | Azure DDoS Protection Standard |

## Git Workflow

### Commit Messages

Format: `[Category] Brief description`

Examples:
- `[UI] Update navbar styling for better contrast`
- `[Feature] Add interactive todo list`
- `[Fix] Correct navigation link to experience section`
- `[Docs] Update README with installation instructions`

### Branch Strategy

- Work on `main` for this project (no other branches)
- Commit frequently with descriptive messages
- Push after each logical change

## Resources

- [Blazor Docs](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [MudBlazor Components](https://mudblazor.com/components/overview)
- [Bootstrap Docs](https://getbootstrap.com/docs/)
- [CSS Tricks](https://css-tricks.com/)
- [MDN Web Docs](https://developer.mozilla.org/)

## Questions?

If Claude encounters ambiguity:

1. **For styling:** Maintain navy + teal color scheme
2. **For component logic:** Check existing components as examples
3. **For data:** Look at JSON structure in `sample-data/`
4. **For layout:** Reference Bootstrap grid system
5. **If still unsure:** Ask the user for clarification

## Project Rules

- ✅ Clean, readable code is better than clever code
- ✅ Comments should explain *why*, not *what*
- ✅ Test changes in browser before committing
- ✅ Keep components focused and reusable
- ✅ Maintain responsive design on all changes
- ✅ No external package additions without discussion

---

**Last Updated:** July 23, 2026  
**Maintained by:** Douglas Rosenberg
