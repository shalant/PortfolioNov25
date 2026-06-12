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
src/BlazorApp/
├── Components/          # Reusable .razor components
├── Layout/             # Header, navigation, layout wrappers
├── Pages/              # Page components (Index.razor is main)
├── Services/           # C# services (HeroImageService, etc.)
├── wwwroot/
│   ├── css/app.css     # Global + component styles
│   ├── index.html      # HTML entry point
│   ├── logos/          # Brand assets (drdev-logo.png)
│   └── sample-data/    # JSON files (experience, skills, etc.)
└── BlazorApp.csproj
```

## Key Files & Their Purpose

| File | Purpose | Edit When |
|------|---------|-----------|
| `Index.razor` | Main page (component composition) | Adding/removing sections |
| `Header.razor` | Navigation bar with logo | Updating nav links, branding |
| `Experience.razor` | Experience carousel | Styling carousel, loading logic |
| `ToDo.razor` | Todo list with checkboxes | Editing todos, changing styling |
| `app.css` | Global styles + animations | Updating colors, fonts, effects |
| `siteproperties.json` | Name, email, social links | Personal information |
| `experience.json` | Experience entries for carousel | Work history, projects |

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

**Last Updated:** May 29, 2026  
**Maintained by:** Douglas Rosenberg
