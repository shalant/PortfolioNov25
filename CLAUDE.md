# Claude Code Development Guidelines

This document provides guidance for developing the DR Codeworks portfolio using Claude Code.

> **📖 Reference docs** — See [`docs/`](./docs/) folder for full documentation:
> - [`PORTFOLIO.md`](./docs/PORTFOLIO.md) — Project context & vision
> - [`COMPONENTS.md`](./docs/COMPONENTS.md) — Component reference
> - [`SECURITY.md`](./docs/SECURITY.md) — Security policy
> - [`PORTFOLIO_TODO.md`](./docs/PORTFOLIO_TODO.md) — Full roadmap
> - [`TASKS.md`](./docs/TASKS.md) — Current sprint tracking

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

BackendTools/                    # Local-only Claude-powered tools (Blazor Server)
├── Components/
│   ├── Layout.razor            # Nav bar between the tools below
│   ├── BlogEditor.razor        # Blog generator: photo upload + version selection
│   └── RingCuration.razor      # Ring curation: re-ranks /webdesign hero images by quality/breadth
├── Services/
│   ├── BlogPostService.cs      # Claude API integration + image handling (blog generator)
│   └── RingCurationService.cs  # Claude vision API integration (ring curation)
├── Program.cs                  # Startup config
├── appsettings.json            # Config (store ANTHROPIC_API_KEY in secrets)
└── BackendTools.csproj
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

### Use Backend Tools

**BackendTools** is an internal Blazor Server app bundling multiple local-only Claude-powered utilities—**do not deploy publicly**. Run locally only:
```bash
dotnet run --project BackendTools/BackendTools.csproj
# Runs on http://localhost:5011 — nav bar switches between the tools below
```

**Blog Generator** (`/`):
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

**Ring Curation** (`/ring-curation`):
- Sends every `/webdesign` case-study image to Claude's vision API and asks it to rank each project's images best-to-worst plus an overall project quality rank, for the `/webdesign` hero's 3D ring
- Outputs a C# snippet matching `WebDesignPage.razor`'s `RingCuration` array — review and paste it in manually rather than having the tool patch source directly
- Run it again whenever case-study images change; nothing about the ring itself calls the Claude API at runtime — see `WebDesignPage.razor`'s `RingCuration` comment for why

**⚠️ Security:** These tools use your Anthropic API key. Keep them **localhost-only**. Do not expose on the public internet without authentication.

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

### Working with Claude Code

Claude Code is more productive when working within clear constraints. This project uses:

- **Code review checklist** (before committing) — catches issues early
- **Branch strategy** (feature branches + PRs) — ensures quality gate
- **Style guidelines** (naming, components, CSS) — maintains consistency
- **Testing strategy** (unit tests + manual verification) — validates correctness

Think of these constraints as quality multipliers. The more specific your guidelines, the better Claude performs. Rather than reading every line of agent-generated code, you can trust the output because it's been constrained and validated by these layers.

### Code Review Checklist

Before committing, Claude should verify:

- [ ] TypeScript/C# compiles without errors
- [ ] No breaking changes to existing components
- [ ] Styles match navy + teal theme
- [ ] Responsive design tested (mobile, tablet, desktop)
- [ ] Navigation links point to correct IDs
- [ ] New assets (images, fonts) are referenced correctly
- [ ] Comments are minimal (code speaks for itself)
- [ ] **Docs updated in this same branch** (see rule below) — not deferred to a follow-up PR

### Pull Request Requirements

**Rule: docs are updated *before* a PR is opened, in the same branch as the change — never as a separate follow-up PR.** If a task completes work tracked in `docs/TASKS.md` or `docs/PORTFOLIO_TODO.md`, check off/update those items as part of the same commit(s) that do the work. If the change adds a feature, test, or architectural pattern worth documenting, update `CLAUDE.md` too. Opening a docs-only PR after the fact is a sign this rule was skipped — fix it by amending the still-open branch, not by adding another PR.

Every PR must include:
- Clear, descriptive commit messages with `[Category]` prefix
- Docs updated per the rule above (CLAUDE.md, TASKS.md, PORTFOLIO_TODO.md — whichever apply)
- All tests passing (if applicable)
- No security or performance regressions

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

See **[SECURITY.md](./SECURITY.md)** for the full security audit and infrastructure details.

**Actual hosting: GitHub Pages. There is no Azure anywhere in this project.** The portfolio is a static Blazor WASM build deployed entirely via GitHub Actions (`.github/workflows/publish-gh-pages.yml`) — no App Service, no Azure Key Vault, no server to configure. If you're about to reach for an Azure Portal setting or a `Web.config`, stop — neither applies here.

### Deploy

Deployment is automatic: push/merge to `main` triggers the GitHub Actions workflow, which builds and publishes to GitHub Pages. There is no manual deploy step and no separate "production" environment to configure.

- Configured to deploy **only on `main` branch** commits
- Feature branches trigger the build+test job but not the deploy job
- Custom domain (`dougrosenbergdev.com`) is wired via the `CNAME` file in `wwwroot/`; DNS is managed externally (see SECURITY.md for what that means for security headers)

### Security Checklist

**Before shipping to production:**

- [x] **HTTPS only** — GitHub Pages enforces this automatically on the custom domain
- [x] **API Keys** — `ANTHROPIC_API_KEY`/`AI:AnthropicApiKey` supplied via environment variable or `dotnet user-secrets` for the local-only BackendTools; never committed. `.gitignore` excludes `.env` and `appsettings.*.json` (except the committed, secret-free base `appsettings.json`).
- [ ] **Input validation** — Blog title/content validated before sending to Claude API
- [x] **File size limits** — Images capped at 5MB per upload (already in code)
- [ ] **CSP headers** — not achievable on plain GitHub Pages; would require a reverse proxy (e.g. Cloudflare) in front of the domain. See SECURITY.md for the honest tradeoff.
- [ ] **Regular updates** — Keep .NET, MudBlazor, and all packages current (no fixed cadence established yet)
- [ ] **Backups** — `blog-posts.json` is versioned in git, which is the backup

### BackendTools

**✅ Stays private** — never deploy publicly. Runs locally only (`dotnet run`, localhost). To publish blog posts: export JSON from the Blog Generator tab, copy to `src/BlazorApp/wwwroot/sample-data/blog-posts.json`, commit, and merge to `main` like any other change. Ring Curation works the same way — copy its generated snippet into `WebDesignPage.razor`'s `RingCuration` array, commit, merge.

### Blocking Common Attacks

| Attack | Prevention |
|--------|-----------|
| **XSS** | Blog content originates from your own writing via the Claude API, not arbitrary user input; `MarkupString` usage is scoped to that trusted content |
| **CSRF** | No state-changing server endpoints exist on the live site to forge a request against |
| **SQL Injection** | N/A — using JSON files, not SQL |
| **DDoS** | Handled by GitHub Pages' own infrastructure; no additional protection configured or needed at this scale |

## Git Workflow

### Branch Strategy

**All code changes must be performed on a feature branch, not main.**

- Create a branch for each feature or fix (e.g., `feature/my-feature`, `fix/bug-name`)
- Work on the branch locally
- Create a pull request to merge into `main` (or merge locally after self-review)
- This keeps `main` stable and deployable at all times

**Local pre-push hook enforces this:** Direct pushes to `main` are blocked; you must push feature branches instead.

### Commit Messages

Format: `[Category] Brief description`

Examples:
- `[UI] Add micro-interactions to buttons`
- `[Feature] Add interactive todo list`
- `[Fix] Correct navigation link to experience section`
- `[Docs] Update README with installation instructions`

### Workflow (Solo Developer)

```bash
# 1. Create feature branch
git checkout -b feature/your-feature

# 2. Make commits (frequently)
git commit -m "[Category] Description"

# 3. Push feature branch
git push -u origin feature/your-feature

# 4. Create PR on GitHub for visibility, or merge locally after review
git checkout main && git merge feature/your-feature && git push origin main

# 5. Delete feature branch
git branch -d feature/your-feature && git push origin -d feature/your-feature
```

### Local Enforcement

- A pre-push hook (`/.git/hooks/pre-push`) prevents pushing directly to `main`
- To bypass (emergency only): `git push --no-verify`
- Feature branches can be pushed freely

### Branch Protection

- `main` branch is protected by local hook
- Feature branches should be descriptive and short-lived
- Delete branches after merging to keep the repo clean

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
