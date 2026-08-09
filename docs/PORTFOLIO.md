# dougrosenbergdev.com — Project Context

> Portfolio site for Douglas Rosenberg, full-stack .NET software engineer.  
> Keep this file current as features ship. Last updated: 2026-08-09.

---

## What This Is

A personal portfolio / resume site showcasing work history, a major case study (ArborKin), technical skills, and background. Goal: attract quality leads, demonstrate expertise in Blazor/ERP systems/UX, establish personal brand.

**URL:** https://www.dougrosenbergdev.com  
**Hosting:** Azure App Service (currently deployed)  
**Stack:** Blazor WebAssembly SPA, C#, CSS3, Bootstrap 5  
**Data:** JSON in `wwwroot/sample-data/`  
**Design:** Navy (#2c3e50) + Teal (#1abc9c), responsive  

---

## Current State

### What's Working ✅
- **Strong visual design** — clean header, hero section, professional photo
- **Comprehensive content** — work history, major case study (ArborKin), skills, personal background
- **Good UX flow** — hero CTA → experience → case study → skills → contact
- **Personal touches** — saxophone background, family story, music discography (builds trust)
- **Responsive layout** — works on mobile/tablet
- **Dark mode toggle** — persists to localStorage
- **Case study depth** — ArborKin section includes hard problems solved, stack, screenshots, metrics
- **Multiple CTAs** — "View Work", "About Me", "Get In Touch" buttons throughout
- **Social links** — GitHub, LinkedIn, email footer and header

### What Needs Work 🚩

#### SEO (Critical)
- ❌ **No meta tags** — missing description, OG tags for link sharing
- ❌ **No robots.txt / sitemap.xml** — search engines can't index properly
- ❌ **Missing heading hierarchy** — H1 appears to be styled text, not semantic `<h1>` tags
- ❌ **No structured data** — missing Schema.org (Person, Experience, Project microdata)
- ❌ **Poor link text** — "visit site ↗", "View on GitHub ↗" aren't descriptive for SEO
- ❌ **No canonical tags** — risk of duplicate indexing
- ❌ **Blog section unrealized** — nav link to `/blog` but no blog content (broken expectations)

#### Content & Features
- 🟡 **Blog link in nav but no blog** — navigation points to `/blog` with no content visible
- 🟡 **Services link in nav but no services page** — confusing CTA
- 🟡 **Case study could go deeper** — ArborKin is great, but no metrics (users, performance gains, testimonials)
- 🟡 **No testimonials or social proof** — potential clients can't see feedback from past collaborators
- 🟡 **No "Latest Work" or "Recent Projects"** — only one case study shown (ArborKin)
- 🟡 **GitHub stats embedded** — GitHub Cards are slow to load; consider caching or static snapshot

#### Technical
- 🟡 **No favicon optimization** — favicon is now better, but could add Apple touch icon, favicon.ico variants
- 🟡 **Performance** — Blazor WASM apps are slower to boot; consider optimizing bundle size
- 🟡 **Accessibility** — may be missing ARIA labels, semantic HTML in some sections
- 🟡 **No analytics** — can't measure traffic, user behavior, CTA effectiveness

#### Brand / Positioning
- 🟡 **Tagline is passive** — "software engineer focused on blazor, erp systems, and ux-forward design" (good but could punch harder)
- 🟡 **No clear value prop** — why hire you vs. another .NET dev? Only mentioned in case study.
- 🟡 **Services section not defined** — nav implies you offer services, but no clarity on what or pricing

---

## Section Breakdown

| Section | Status | Notes |
|---------|--------|-------|
| **Header/Nav** | ✅ Clean, professional, responsive hamburger | Mobile nav needs testing |
| **Hero** | ✅ Strong name + photo + two CTAs | Good visual hierarchy |
| **About** | 🟡 Brief bio (3 lines) | Could expand with value prop + credentials |
| **Experience** | ✅ Four jobs, carousel UI, visit links | Detailed role descriptions |
| **ArborKin Case Study** | ✅ Excellent depth (stack, hard problems, screenshots, stats) | Add metrics: users, performance gains, launch date |
| **Skills** | ✅ Well-organized (Languages, Frameworks, Cloud, Tooling) | Good visual layout |
| **GitHub Stats** | 🟡 Embedded cards (slow load) | Consider static snapshot or remove |
| **Personal Background** | ✅ Unique (saxophonist, musician, family focus) | Differentiator; good storytelling |
| **Contact Section** | ✅ Clear CTA + multiple contact methods | Good conversion funnel |
| **Footer** | ✅ Tagline + social links + back-to-top | Professional |

---

## Architecture

```
src/BlazorApp/
├── Components/
│   ├── Header.razor              Navigation + logo
│   ├── Hero.razor                Name, photo, CTAs
│   ├── About.razor               Bio section
│   ├── Experience.razor          Work history carousel
│   ├── CaseStudy.razor           ArborKin deep dive
│   ├── Skills.razor              Tech stack grid
│   ├── GithubStats.razor         Embedded GitHub stats
│   ├── PersonalBackground.razor  Saxophone / family story
│   ├── Contact.razor             Email + social links
│   └── Footer.razor              Footer with links
├── Pages/
│   └── Index.razor               Main landing page
├── wwwroot/
│   ├── css/app.css               Global styles + animations
│   ├── logos/favicon.svg         Favicon (NEW: hexagon mark only)
│   └── sample-data/
│       ├── experience.json       Job history
│       └── ... (other data)
└── BlazorApp.csproj
```

---

## Key Files & Purpose

| File | Purpose | Edit When |
|------|---------|-----------|
| `Index.razor` | Main page (component composition) | Adding/removing sections |
| `Header.razor` | Navigation bar with logo | Updating nav links, adding services page |
| `Experience.razor` | Work history carousel | Updating jobs, adding new roles |
| `CaseStudy.razor` | ArborKin case study | Adding metrics, expanding scope, new case studies |
| `Skills.razor` | Technology grid | Updating tech stack |
| `app.css` | Global styles + animations | Colors, fonts, responsive breakpoints |

---

## Code Style & Conventions

- **Naming:** `PascalCase` for classes/methods, `camelCase` for variables
- **Components:** Always include `[Parameter, EditorRequired]` for required parameters
- **CSS:** Mobile-first; use `@media (min-width: ...)` for larger screens
- **Colors:** Navy (#2c3e50) + Teal (#1abc9c) consistently
- **Transitions:** Use `transition: property 0.3s ease` for smooth effects

---

## Known Issues / Gaps

1. **SEO missing** — no meta tags, no structured data, no sitemap
2. **Blog unrealized** — nav link exists but no content
3. **Services unclear** — nav link exists but no page/definition
4. **GitHub stats slow** — embedded cards add load time
5. **No testimonials** — no social proof from past clients/collaborators
6. **No metrics on case study** — ArborKin is impressive but lacks impact numbers
7. **Favicon could be richer** — could add multiple sizes/formats

---

## Vision: A Portfolio That Converts

### Phase 1 — Foundation ✅ (Complete)
- Landing page with hero + experience
- Case study (ArborKin)
- Skills showcase
- Personal story (saxophone, family)
- Contact CTA + social links
- Dark mode toggle
- Responsive design

---

### Phase 2 — Discoverability 🚩 (Next Priority)
- **SEO overhaul** — meta tags, Open Graph, structured data, robots.txt, sitemap
- **Blog section** — case study breakdowns, technical deep-dives (3-4 posts minimum)
- **Blog preview on homepage** — "Latest Articles" section to drive organic traffic
- **Analytics integration** — understand visitor behavior, CTA effectiveness
- **Performance optimization** — measure Core Web Vitals, optimize Blazor bundle

---

### Phase 3 — Conversion & Trust
- **Testimonials** — quotes from past collaborators / clients
- **More case studies** — showcase 2-3 projects beyond ArborKin (Friars ERP, others)
- **Services page** — clarify what you offer (consulting, full-stack dev, ERP architecture, etc.)
- **Pricing or engagement model** — "Available for X/month retainer" or "Starting at $X/hour"
- **Social proof** — GitHub stars, Medium followers, speaking engagements, awards

---

### Phase 4 — Intelligence & Growth
- **Lead capture** — newsletter signup, "Get my 10 Blazor Tips" freebie
- **Email outreach automation** — track who visits, follow up with cold outreach
- **A/B testing** — CTA text, hero image, color scheme variations
- **SEO growth tracking** — keyword rankings, organic traffic trends

---

## Resources

- [CLAUDE.md](./CLAUDE.md) — Development guidelines
- [Blazor Docs](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Bootstrap Docs](https://getbootstrap.com/docs/)
- [Open Graph Guide](https://ogp.me/)
- [Schema.org Documentation](https://schema.org/)

---

**Last Updated:** 2026-08-09  
**Maintained by:** Douglas Rosenberg
