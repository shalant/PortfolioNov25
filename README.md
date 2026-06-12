# dr codeworks
[![CI/CD](https://img.shields.io/github/actions/workflow/status/YOUR_USERNAME/YOUR_REPO/YOUR_WORKFLOW.yml?label=Build%20%26%20Deploy&logo=github&color=1abc9c&labelColor=060e1a)](https://github.com/YOUR_USERNAME/YOUR_REPO/actions/workflows/YOUR_WORKFLOW.yml)
[![Live Site](https://img.shields.io/badge/Live-dougrosenbergdev.com-1abc9c?style=flat&labelColor=060e1a)](https://dougrosenbergdev.com)

A developer portfolio built as a Blazor WebAssembly SPA. Single-scroll, data-driven, no server required. Dark art deco aesthetic — navy + teal, Cormorant Garamond headings, Courier monospace accents, `mix-blend-mode: screen` art deco overlays composited per section.

**Stack:** Blazor WASM · .NET 10 · C# / Razor · CSS3 · Bootstrap 5 · MudBlazor 8

---

## Getting Started

**Prerequisite:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
git clone <repo>
dotnet run --project src/BlazorApp/BlazorApp.csproj
# → http://localhost:5001
```

After changing `.csproj` or adding files to `wwwroot/`, do a clean build to regenerate the static asset manifest:

```bash
dotnet clean src/BlazorApp/BlazorApp.csproj
dotnet run --project src/BlazorApp/BlazorApp.csproj
```

---

## Project Structure

```
src/BlazorApp/
├── Components/
│   ├── Home.razor                  # Hero — animated title, portrait, CTA
│   ├── About.razor                 # Bio, skill chips, pull quote
│   ├── Experience.razor            # Split-panel interactive timeline
│   ├── ArborKin.razor              # Featured project — deep-dive case study
│   ├── TechnicalSkills.razor       # Skill chip grid + GitHub activity
│   ├── Casual.razor                # Interests + hobby tiles
│   └── Music.razor                 # Composer section, sticky photo
├── Layout/
│   ├── Header.razor                # Sticky nav, scroll-spy, hamburger
│   └── Footer.razor
├── Pages/
│   ├── Index.razor                 # Composes all sections in order
│   ├── ConsultingPage.razor        # /consulting — client-facing services
│   ├── WebDesignPage.razor         # /webdesign — Squarespace + Blazor work
│   └── PreviousPortfolio.razor     # /archive — previous Angular portfolio
├── Services/
│   └── HeroImageService.cs         # Hero image rotation
└── wwwroot/
    ├── css/app.css                  # All styles — 22 sections, ~3,500 lines
    ├── images/                      # Art deco overlays, portraits, hero assets
    ├── logos/                       # SVG + PNG logo variants
    ├── sample-data/                 # JSON content (edit here, not in components)
    └── archive/dist/portfolio/      # Compiled Angular app (previous portfolio)
```

---

## Pages & Routing

| Route | Purpose | Audience |
|-------|---------|----------|
| `/` | Full single-scroll — hero → about → experience → arborkin → skills → casual → music | Corporate / technical hiring |
| `/consulting` | IT consulting services, client CTA | Non-technical small business clients |
| `/webdesign` | Web design portfolio — Squarespace sites + Blazor builds | Design-forward clients |
| `/archive` | Previous Angular portfolio embedded in iframe | Anyone curious about the progression |

Consulting and web design live on separate pages by design — they speak to different audiences and would dilute the main page's corporate dev narrative if inlined.

---

## Content

All copy lives in JSON files in `wwwroot/sample-data/`. Edit content here without touching component code.

| File | Controls |
|------|---------|
| `siteproperties.json` | Name, email, social links |
| `aboutme.json` | Bio text, skill chip list |
| `experience.json` | Timeline entries — logo, role, bullet points, tech chips, link |
| `consulting.json` | Consulting page — headline, subheadline, services array, CTA text, tagline |
| `webdesign.json` | Web design page — headline, subheadline, tools array |
| `skills.json` | Technical skill categories and chip lists |
| `casual.json` | Description, closing quote, hobbies array — each hobby has `name` + `detail` tagline |
| `music.json` | Three paragraphs, press quote + attribution, albums array (`title`, `artist`, `role`, `url`), collaborators array |
| `heroimages.json` | Hero background image rotation list |

---

## Design System

### Colors

```
#060e1a   page background — near-black deep navy
#2c3e50   navy — section backgrounds, structural elements
#1abc9c   teal — accents, active nav, CTAs, chips, code
#eef2f7   light — primary text on dark backgrounds
```

### Typography

| Role | Font | Weight |
|------|------|--------|
| Display headings | Cormorant Garamond | 300 — editorial, high contrast |
| Body / UI text | Montserrat | 200–600 — clean, contemporary |
| Code / nav labels / chips | Courier New | monospace — technical precision |

### Art Deco Overlays

Two source images are composited over dark section backgrounds using `mix-blend-mode: screen`. Because `screen` renders black as fully transparent, the lighter geometric shapes in the images become additive light on dark surfaces — no hard edges, no boxes.

- `artDecoBackground1.png` — Mondrian-style grid circles (used full-bleed)
- `artDecoBackground2.png` — arch columns (used edge-anchored, right or left)

Opacity ranges: `0.08` (subtle background depth) → `0.22` (navbar). Placement and sizing vary per section — see CSS section 20 for the full map.

---

## CSS Architecture

`wwwroot/css/app.css` is the single stylesheet. 22 named sections:

```
 1   Imports & Design System      @import, :root variables
 2   Global Styles                html/body, section variants, background images
 3   Typography                   h1–h3, p, responsive scale breakpoints
 4   Layout (Legacy)              older container and card classes
 5   Navigation & Header          sticky nav, scroll-spy active state, hamburger menu
 6   Cards & Glassmorphism        reusable card variants (custom-card, glassmorphism)
 7   Experience                   split-panel timeline — selector list + detail panel
 8   Todo List                    checkbox list component
 9   Decorative & Utility         scroll-reveal, glow effects, overlay helpers
10   Error UI & Loading           Blazor boot screen and error banner
11   Animations & Keyframes       @keyframes library + animation utility classes
12   Hero Section                 homepage hero layout
13   About Section                bio two-column grid
14   ArborKin                     case study — screenshot, problem cards, stats row
15   Subpages                     shared hero + /consulting + /webdesign specific
16   Archive Page                 iframe wrapper layout
17   Technical Skills             chip grid + GitHub activity embeds
18   Casual                       hobby tile layout
19   Music                        two-column layout, sticky photo sidebar
20   Art Deco Overlays            ::before/::after per section, blend modes, opacity map
21   Web Design Page Identity     additional art deco overlays for /webdesign
22   Footer                       glassmorphism page-end footer, social icons, back-to-top
```

---

## Commit Convention

```
[UI]       Visual — layout, styles, animation
[Feature]  New functionality
[Fix]      Bug fix
[Data]     JSON content update
[Docs]     Documentation
[Refactor] Code cleanup, no behavior change
```

---

Built by Douglas Rosenberg · [doug.rosenberg@gmail.com](mailto:doug.rosenberg@gmail.com)
