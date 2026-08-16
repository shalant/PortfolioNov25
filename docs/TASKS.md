# Current Sprint — Portfolio Upgrade

**Goal:** Transform portfolio into lead-generating machine with discoverable SEO + sparkly UI + clear web design positioning  
**Timeline:** 2-3 weeks (2-3 hrs/day)  
**Started:** 2026-08-09

---

## Phase 0: Image Optimization (This Week — before lead demo)
**Status:** ✅ Complete (2026-08-15)
**Effort:** ~1.5 hours
**ROI:** High — direct fix for a measured problem, no infrastructure risk, reversible

**Why now:** Site felt fast on mobile but slow on desktop to a warm lead. Measured live on
dougrosenbergdev.com (2026-08-15) with Chrome DevTools Protocol (`performance.getEntriesByType`)
on a fast connection: **total page weight ~10.5MB across 73 requests.** Broke it down by category:

| Category | Size | Share |
|---|---|---|
| Images | 7.5 MB | 71% |
| .NET runtime + assemblies | 2.9 MB | 27% |
| CSS/JS/other | 0.26 MB | 2% |

Three PNGs account for 7.4MB (70% of the whole page):

| File | Size | Natural | Displayed | Notes |
|---|---|---|---|---|
| `DrCorporateHacker.png` | 3.17 MB | 1024×1536 | 420×630 | Hero portrait — ~5.8x more pixels than rendered |
| `artDecoBackground2.png` | 2.40 MB | — | footer strip, 1509×172 @ `background-size: auto 260%` | Bauhaus/art-deco tiling texture — [[design-bauhaus-background]], keep the pattern, just re-encode |
| `artDecoBackground1.png` | 1.90 MB | — | hero/tools decoration | Same tiling motif |

**This supersedes the earlier Cloudflare Pages / Brotli theory (see git history on
`feature/cloudflare-pages-migration`):** confirmed via `content-encoding` headers that GH Pages
serves the `.wasm` gzipped (not Brotli), but the images return `content-encoding: none` because
PNG is already compressed — Brotli at the edge would save ~270KB (2.5% of total) and do nothing
for the 7.5MB of images. Not worth a DNS cutover the same week as the demo. That branch's
migration tasks stay de-prioritized; this phase is the actual fix.

### Tasks
- [x] Resize `DrCorporateHacker.png` to 840×1260 (2x its 420×630 display size — confirmed via
      `.hero-portrait` CSS, capped at `min(420px, 38vw)` on all breakpoints) and convert to WebP —
      installed ImageMagick (`winget install ImageMagick.ImageMagick`) to do the conversion.
      Result: 3.24MB → 180KB
- [x] Re-encode `artDecoBackground1.png` and `artDecoBackground2.png` as WebP at native resolution
      (left dimensions untouched — they're referenced by ~20 different `background-size` rules
      across `app.css`, too risky to also resize in this pass). Viewed both outputs directly
      (Bauhaus geometric shapes) — clean edges, no banding. Results: 1.95MB → 25KB,
      2.46MB → 90KB
- [x] Updated references in `heroimages.json` (hero portrait + the unused "experience" entry),
      `app.css` (19 rules), `BlogArchive.razor`, `BlogPosts.razor` — not `Index.razor`, the actual
      hero `<img>` lives in `Home.razor`, driven by `heroimages.json` via `HeroImageService`.
      Deleted the three old PNGs from `wwwroot/images/`.
- [x] Verified: `dotnet build` and `dotnet publish -c Release` both succeed; published
      `wwwroot/images/` contains the new `.webp` files. Ran the dev server and loaded the live
      page in Chrome — hero portrait and both Bauhaus overlays (navbar strip + footer deco)
      render correctly, no visual artifacts. Re-measured via `performance.getEntriesByType`:
      the three swapped files now total **295KB, down from 7.4MB (96% reduction)**
- [~] Breakpoint spot-check: confirmed the sizing rule (`min(420px, 38vw)` desktop /
      `min(260px, 55vw)` mobile, single CSS source of truth) covers all breakpoints, and visually
      checked the default desktop viewport. Did **not** take actual screenshots at 300/420/768px —
      worth a quick manual check on a real phone before the demo, same as the original mobile
      load-time test

**Also noted, not in scope here:** all assets serve `cache-control: max-age=600` (10 minutes) —
a repeat visitor re-downloads the full page every 10 minutes. Worth revisiting cache headers in
GitHub Pages config separately.

**Bonus fix (found while testing the above in the browser):** the hero section flashed through
3 visible states on load — the native `.app-loading` splash, then Blazor's *first* synchronous
render of `Home.razor` with `property`/`hero` still `null` (an empty `<h1>&nbsp;</h1>`, no
portrait), then a jarring pop once `siteproperties.json`/`heroimages.json` finished fetching and
the component re-rendered. The name/title/portrait are effectively static content, so gated the
first paint on a network round-trip for no reason. Fixed by rendering `property?.Name ??
FallbackName` (etc.) directly in `Home.razor` — the fallback constants match the current JSON
verbatim, so visible output is unchanged, but real content is now present on the very first
render and the existing `fadeInDown`/`fadeInUp` entrance animations play against real content
instead of empty markup.

**Branch:** `feature/image-optimization`

---

## Phase 1: SEO Foundation (Week 1)
**Status:** ✅ Complete (2026-08-09)  
**Effort:** ~2 hours  
**ROI:** High (organic traffic)

SEO makes you discoverable. Google can't index you without meta tags, structured data, and sitemaps.

### Tasks
- [x] Add `<meta name="description">` to `index.html`
- [x] Add Open Graph tags (og:title, og:description, og:image, og:url)
- [x] Add canonical tag to prevent duplicate indexing
- [x] Create `robots.txt` (allow /; disallow admin paths)
- [x] Create `sitemap.xml` (list all pages)
- [x] Add Schema.org structured data (Person, WebSite, SoftwareApplication)
- [x] Improve link text ("visit site" → "Visit Friars ERP", etc. — added `Company` field to experience data)
- [x] Add `alt` text to all images (audited full live component tree)
- [x] Test with Google Search Console — domain verified via DNS TXT record, sitemap submitted (Status: Success, 7 pages discovered)

**Bonus fix:** found and fixed a canonical/OG/sitemap mismatch — `www.dougrosenbergdev.com` 301-redirects to the apex domain, but every canonical tag, OG/Twitter tag, and sitemap `<loc>` was pointing at `www` instead of the URL that actually serves. This was blocking sitemap submission in Search Console.

**Branch:** `feature/seo-foundation` (merged via PR #9)

---

## Phase 2: UI Sparkle Phase 1 (Week 1-2, parallel)
**Status:** ✅ Complete (2026-08-09)  
**Effort:** ~3 hours  
**ROI:** Medium (converts visitors)

Polish the UI with micro-interactions, gradients, and smooth transitions. Immediate visual impact.

### Tasks
- [x] Add button hover effects (scale, color shift, glow) — already implemented (`.hero-btn` lift + shadow)
- [x] Add link hover animations (underline slides in from left) — added animated underline (`transform: scaleX`) to `.experience-detail__link` and `.wd-project__link`; left `.webdesign-link` (permanent prose underline) and `.blog-archive-link`/`.arborkin-gh-link` (already have their own slide/lift treatments) alone
- [x] Add nav item highlights (active section indicator) — already implemented (`.nav-link.active`, scroll-spy)
- [x] Enhance experience carousel cards (lift on hover) — added `translateY(-2px)` + shadow to `.experience-list__item:hover`
- [x] Add gradient shadows to skill tags — added `box-shadow` glow + lift to `.tech-chip:hover`
- [x] Add gradient overlays to hero section — already implemented (bauhaus/art-deco texture overlays + radial grid-dot bg)
- [x] Animate skill tags on scroll (staggered reveal) — new `.reveal-on-scroll` IntersectionObserver pattern (`Index.razor`), applied to all major sections + staggered per-chip delay on `.skills-category__chips`
- [x] Test responsiveness (mobile, tablet, desktop) — the browser device-emulation tool wasn't functional this session (`resize_window` reported success but `window.innerWidth` never actually changed), so this was a **code-level audit** instead of live device screenshots: checked all 25 `@media` breakpoints in `app.css` against every element touched this session (`.experience-list__item`, `.tech-chip`, `.experience-detail__link`, `.wd-project__link`, `.reveal-on-scroll`) — no conflicts found. Live visual mobile testing is still worth doing by hand later.

**Bonus:** swapped the Friars experience entry from a generic stylized icon to the real Franciscan Friars emblem (cropped/made-transparent from `franciscan-frairs-logo-2023.jpg` via `franciscan-friars-emblem.png`). Also caught and fixed a stray unmatched `}` in `app.css` introduced by an earlier edit in this same session (brace count verified balanced: 633/633).

**Branch:** `feature/ui-sparkle-phase1`

---

## Phase 2B: ArborKin Case Study Enhancement (Week 2, optional add-on)
**Status:** ⏳ Not started  
**Effort:** 6-8 hours (can split into separate phase)  
**ROI:** Very high (converts leads)

Make the flagship case study visually stunning and interactive.

### Tasks
- [ ] Add metrics cards (big numbers: 8k LOC, 75 tests, 150ms render, 6 users)
- [ ] Add animated callouts (arrows + hover descriptions for hard problems)
- [ ] Add before/after slider (compare old approach vs new)
- [ ] OR embed demo (read-only ArborKin interface preview)
- [ ] OR add video walkthrough (60-sec Loom of tree interaction)

**Branch:** `feature/arborkin-enhancement`

---

## Phase 2C: Resume Download + Skills Expansion
**Status:** ✅ Complete (2026-08-15)
**Effort:** ~1 hour
**ROI:** Medium — resume is table stakes for recruiter/HR screens; skills expansion keeps the
site accurate as of 2026

### Tasks
- [x] Added a "Download Resume ↓" button in `About.razor`, styled with the existing
      `.hero-btn--ghost` treatment
- [x] Scaffolded `wwwroot/resume/` with a `README.md` placeholder explaining the expected
      filename — **user then dropped in the real PDF** as `DouglasRosenbergResumeAugust26.pdf`
      (not something Claude generates); updated the button's `href` to match the actual filename
      and deleted the now-obsolete README. Verified via `curl` that it 200s with
      `content-type: application/pdf` at the right size (4.1MB, 6 pages — a bit heavy for a
      resume but it's an on-demand download, not part of page load, so out of scope for the
      Phase 0 weight work)
- [x] Added new `TechChip`s to `TechnicalSkills.razor`: `.NET MAUI (Blazor Hybrid)` and `Astro`
      under Frameworks & UI, `macOS` under Cloud & Infrastructure, and a new **AI Tools**
      category (`Claude`, `Gemini`, `Copilot`)
- [x] Fixed a latent bug found while wiring up the Claude chip: `TechChip.razor`'s icon map keys
      `claude-logo.png` (lowercase), but the file on disk was `Claude-logo.png`. Windows'
      case-insensitive filesystem hid this locally; GitHub Pages (Linux) would have 404'd the
      icon in production. Renamed the file to match.
- [x] Fetched real brand icons for Astro, Gemini, and Copilot (user requested this explicitly in
      a follow-up) — Astro from devicon (`astro-plain.svg`, MIT), Copilot/Gemini from Simple
      Icons (CC0). None of the three source SVGs had a fill color baked in (path-only, meant to
      inherit `currentColor`/black), so injected each one's official brand hex directly into the
      path: Astro `#BC52EE`, Gemini `#8E75B2`. Copilot's official hex is `#000000`, which would
      be invisible on this site's dark background, so used the site's existing light-text token
      (`#eef2f7`) instead of the literal brand black. Rendered each on a navy background via
      ImageMagick and viewed them before wiring in. `.NET MAUI` still has no icon — checked
      devicon, Simple Icons, and the `dotnet/maui`/`dotnet/docs-maui` GitHub repos directly, no
      standalone logo asset found; stays a text-only chip
- [x] Verified: `dotnet build` and `dotnet publish -c Release` succeed; loaded both sections live
      in Chrome — resume button renders correctly, all chips render correctly (icon + text-only
      both look right against the existing style)
- [x] Follow-up: user asked to add icons to the remaining text-only chips (`macOS`, `.NET MAUI`)
      plus more tools — clarified "the skills section" meant `webdesign.json`'s `tools` array
      (the /webdesign "custom web dev" page's stack list: Figma, Squarespace, Blazor, Angular,
      Bootstrap, CSS/Animations, MudBlazor, Syncfusion, DevExpress), cross-referenced against
      `TechnicalSkills.razor` — everything was already present except **Squarespace**, added as
      a new chip under Tooling & Design
- [x] `macOS` icon: Simple Icons' Apple logo (CC0), recolored to the site's light-text token
      since the official hex is black
- [x] While fixing Squarespace's icon, found the same latent case bug as Claude's:
      `wwwroot/icons/tech/Squarespace_Logo_2019.png` vs. the lowercase key in `IconMap`. Also,
      that file turned out to be a large opaque-white-background wordmark PNG sourced from
      Wikimedia — would've rendered as a jarring white block on this dark UI even with the case
      fixed. Replaced it entirely with Simple Icons' transparent icon-only mark (CC0), recolored
      to match the rest of the icon set
- [x] `.NET MAUI` still has no icon after also checking the official `dotnet/brand` GitHub repo
      (logo/, extension-icons/, language-icons/ — no MAUI asset anywhere in it); stays text-only
- [x] Follow-up: user pointed out `/webdesign`'s toolkit row was still text-only chips —
      `WebDesignPage.razor` rendered `webDesign.Tools` through plain `<span class="about-skill-chip">`,
      a completely different code path from `TechnicalSkills.razor`'s `<TechChip>`, so it never
      picked up any of the icon work above. Switched it to `<TechChip Name="@tool" />` (same
      `webdesign.json` → `.Tools` data, just a different render). `.wd2-tools__chips` is a plain
      flex-wrap container so the swap needed no CSS changes.
- [x] Added `Canva`, `Astro`, `Claude`, `DALL-E`, `Gemini`, `Copilot` to `webdesign.json`'s
      `tools` array per user request (all already had icons from earlier work except DALL-E)
- [x] Sourced a DALL-E icon: no match in devicon, Simple Icons, or Font Awesome's free tier —
      Simple Icons doesn't carry it at all (OpenAI's marks are a known gap in most open icon
      sets over trademark caution). Found `dalle-color.svg` in `lobehub/lobe-icons`
      (`packages/static-svg/icons/`, MIT) — DALL-E's actual mark (a 5-color striped bar), already
      had its official colors baked in, used as-is with no recoloring needed
- [x] Verified: `dotnet build` and `dotnet publish -c Release` succeed; loaded `/webdesign` live
      in Chrome — all 15 toolkit chips render correctly (icon or graceful text-only fallback for
      `CSS / Animations`, which isn't an exact `IconMap` key match — pre-existing gap, not
      addressed this pass)

**Branch:** `feature/resume-and-skills`

---

## Phase 3: Lead Generation (Week 3)
**Status:** ⏳ Not started  
**Effort:** 8-12 hours  
**ROI:** High (converts leads to clients)

Set up clear services offering, booking, blog preview.

### Tasks
- [ ] Create Services page (3-4 service definitions + pricing/engagement model)
- [ ] Add Calendly booking integration (30-min discovery call widget)
- [ ] Add blog preview section on homepage (3 latest posts)
- [ ] Create 1-2 first blog posts (technical deep-dives)
- [ ] Add testimonials section (2-3 quotes from past colleagues)
- [ ] Add newsletter signup (email capture + freebie: "10 Blazor Tips")
- [ ] Create PRIVACY.md (GDPR/privacy policy snippet)

**Branches:**
- `feature/services-page`
- `feature/blog-preview`
- `feature/testimonials`
- `feature/newsletter`

---

## Phase 4: Port /webdesign + /webdesign2 to Astro (separate session/repo)
**Status:** ⏳ Not started — deliberately deferred, not a quick add-on
**Effort:** Unscoped (new repo, likely several sessions)
**ROI:** High for the lead-facing pages specifically — Blazor WASM's runtime download is the main
cost tonight's design work can't fix with CSS alone.

**Why:** Measured tonight — this app's `_framework` payload is ~18MB uncompressed / ~3.2MB
brotli-compressed just for the .NET WASM runtime shell, before any real content renders. Haxbyte's
entire Astro site (every page, every image) is 561KB total. That gap matters most on exactly the
pages meant to convert a warm referral fast (`/webdesign`, `/webdesign2`), less so on the rest of
the site.

**How it deploys (already confirmed, no redirect/separate host needed):** dougrosenbergdev.com is
static-served via GitHub Pages (`CNAME` + `.github/workflows/publish-gh-pages.yml`) — Blazor WASM's
build output is already just static files. Astro's build output is also static files, so it can sit
in the same published `wwwroot` tree, same domain, same deploy — no cross-origin redirect. GitHub
Pages serves whichever file matches the path; a real static file always wins over the repo's
`404.html` Blazor SPA-routing fallback, so `/webdesign` would load as plain HTML with zero WASM
boot.

### Scope, when picked up
- [ ] New Astro repo (mirrors the Haxbyte pattern), start with just `/webdesign` + `/webdesign2` +
      their `/webdesign/{slug}` case-study detail pages — not the rest of the site
- [ ] Hand-author an Astro equivalent of `<Header>` so nav matches the Blazor pages pixel-for-pixel
- [ ] Add an Astro build step to `publish-gh-pages.yml`, output merged into the same publish
      directory before the Pages deploy step
- [ ] Remove the Blazor `@page "/webdesign"` / `@page "/webdesign/{Slug}"` routes once the static
      pages are live at those paths
- [ ] Port the project data (`webdesign.json` → Astro content collection or direct JSON import),
      the crossfade card component, and the webdesign2 marquee/glass hero

**Not now:** porting the rest of the site (Home/About/Experience/Skills/Blog) — no measured problem
there yet, and it's a much bigger lift than the two lead-facing pages.

---

## Completed ✅

- [x] Consolidate documentation (deleted redundant docs)
- [x] Set up branch-based workflow (pre-push hook)
- [x] Reorganize docs folder structure

---

## Notes

**Design Philosophy:**  
"Spotlight on Specialty" — Leverage your unique differentiators (saxophonist + engineer + ERP specialist). Make every interaction intentional, nothing wasted.

**Workflow:**
- Each phase gets its own feature branch
- Tasks checked off as work completes
- PR created for visibility (no review needed, but good git hygiene)
- Merge to main after self-review

**Priorities if short on time:**
1. **SEO Foundation** (most ROI: makes you discoverable)
2. **UI Phase 1** (satisfying visual progress)
3. **Services + Booking** (converts leads)
4. **Blog + Testimonials** (secondary: builds authority)

---

**Last Updated:** 2026-08-09  
**Next Review:** After Phase 1 complete
