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
- [x] Follow-up: asked whether the toolkit row needed more chips or fewer — flagged that
      `Syncfusion`/`DevExpress` (enterprise grid/reporting suites) sit oddly next to a page
      pitched as client-facing "UI/UX Design & Web Development" for small-business/creative
      leads (Hardware Etc, a musician landing site), and recommended trimming rather than
      growing the list. User agreed — dropped both from `webdesign.json`'s `tools` array. Still
      listed in the main `TechnicalSkills.razor` grid, just not on this page. 13 chips now
      (down from 15), verified live in Chrome and via `dotnet publish -c Release`

**Branch:** `feature/resume-and-skills`

---

## Phase 3: Lead Generation (Week 3)
**Status:** 🟡 In progress — Services page done, rest not started
**Effort:** 8-12 hours  
**ROI:** High (converts leads to clients)

Set up clear services offering, booking, blog preview.

### Tasks
- [x] Create Services page (2026-08-15) — new `/services` route (`ServicesPage.razor` +
      `ServicesModel.cs` + `services.json`), mirrors the existing `/consulting` page's
      `subpage-hero` pattern for visual consistency. 4 offerings: Custom Web & App Development,
      ERP & Business Systems Consulting, AI-Assisted Development & Modernization, Ongoing Support
      & Maintenance — each with a description, an "includes" bullet list, and an **engagement
      model** (hourly/project/retainer). Initially shipped without dollar pricing — that's a real
      business decision, didn't want to fabricate it. User asked for rate-setting advice
      afterward (context: currently $106k salaried, wants solid income but is willing to
      price lower early on to build the portfolio); talked through the standard freelance math
      (salaried-equivalent hourly × 2-3x to cover self-employment tax, no benefits, non-billable
      time) and recommended pricing the four offerings differently rather than one flat rate,
      since they span very different markets. **Added as price ranges per card** (2026-08-15):
        - Custom Web & App Development: **$750 – $3,000/project** — the "pay your dues" tier,
          intentionally priced near what's already being quoted to real leads ($500-1,000)
        - ERP & Business Systems Consulting: **$125 – $175/hr** — this is existing day-job
          expertise, not something that needs to be proven, so priced at senior rates from day one
        - AI-Assisted Development & Modernization: **$100 – $150/hr** — similar band to ERP,
          slightly lower until there are dedicated case studies for this specifically
        - Ongoing Support & Maintenance: **$300 – $800/month** retainer
      **These are Claude's suggested ranges, not verified against real market data — sanity-check
      before this goes live.**
    - Added a nav link (`/services`, briefcase icon) between "blog" and "consulting"
    - Reused an orphaned hero image (`DrPortraitV5.png`, listed in `heroimages.json` under
      "technical skills" but never actually fetched by any component) — resized to its 2x display
      size (760×1140) and converted to WebP: 2.57MB → 74KB, same treatment as Phase 0
    - Content is derived from existing site data (`aboutme.json`'s ERP/full-stack description,
      `webdesign.json`'s stack, `consulting.json`'s "Ongoing Maintenance") — not fabricated from
      nothing, but **worth a read-through before publishing** since I drafted the copy
    - Noted, not addressed: `/consulting` already has its own 4-service grid with a different
      framing ("neighborhood IT partner" for small business) — there's some conceptual overlap
      between the two pages worth thinking about, but consolidating them is a content decision,
      not something to do unilaterally
    - User flagged after seeing it live: nav is genuinely tight now (7 links + CTA — a
      compression breakpoint at 1200px already exists just to keep everything on one line,
      which means it was designed with headroom for fewer items). Discussed grouping options
      (visual divider, "more" dropdown) but no changes made — pending a decision
- [ ] Add Calendly booking integration (30-min discovery call widget) — **on hold, user doesn't
      have a Calendly account yet**
- [ ] Add blog preview section on homepage (3 latest posts)
- [ ] Create 1-2 first blog posts (technical deep-dives)
- [ ] Add testimonials section (2-3 quotes from past colleagues)
- [ ] Add newsletter signup (email capture + freebie: "10 Blazor Tips")
- [x] Create PRIVACY.md (2026-08-16, done overnight — see below)

**Branches:**
- `feature/services-page` (this session)
- `feature/blog-preview`
- `feature/testimonials`
- `feature/newsletter`

---

## Overnight housekeeping (2026-08-16)
**Status:** ✅ Complete
**Effort:** ~1 hour
**Context:** user asked "anything you can work on overnight" after merging the Services page PR.
Picked safe, well-scoped items from the backlog that didn't need content decisions only they
could make (no fabricated blog posts, no invented testimonials, no Calendly signup).

### Tasks
- [x] Audited every image over 300KB still in `wwwroot/images/` for actual usage. Found the
      **only** other live oversized image left after Phase 0/2C: `DrComputerConsultant2.png`
      (2.72MB, `/consulting` page's `subpage-hero__img`). Resized to its 2x display size
      (760×1140) and converted to WebP → 101KB, same treatment as before. Updated
      `ConsultingPage.razor`, `heroimages.json`, and (for correctness, even though it's dead code)
      `Components/Consulting.razor`'s hardcoded reference.
    - Everything else large (`Douglas_Rosenberg_Rev.png` 4.7MB, `DougCartoon4.png` 2.3MB,
      `DrCyberPunk.png` 1.6MB, `tealRectangle.png`) is **orphaned** — referenced only by dead
      components (`Consulting.razor`/`3`/`4`, never routed or embedded anywhere) or unused
      `heroimages.json` entries nothing fetches. Left alone per the "don't remove components
      without asking" rule — noted here for a future cleanup pass, not acted on.
    - Skipped resume PDF compression (still 4.1MB) — would've required installing Ghostscript,
      new system software, which felt like the wrong call to make unattended without asking first.
- [x] Closed out Phase 0's partial mobile-breakpoint item — but not the way originally planned.
      `resize_window` in the Chrome tool reports success without actually changing
      `window.innerWidth` (same limitation noted in an earlier session) — burned two attempts
      confirming this before falling back to a code-level audit instead of a live screenshot:
      verified `.subpage-hero__img` (now used by both `/consulting` and `/services`) has a sane
      `≤820px` override (`min(240px, 60vw)`), well within both images' 760px source resolution,
      and confirmed `.consulting-services__grid` collapses to one column on mobile so the new
      Services page cards stack correctly.
- [x] Addressed the nav crowding/grouping concern flagged after the Services page shipped.
      Discovered `.nav-link--page` already existed in `app.css` (dimmer default color, distinct
      active-state glow/outline) but was **never actually applied to any nav link** — built for
      exactly this grouping and left unused. Reordered the nav into two visual clusters (page
      sections: about/experience/skills, then a divider, then destinations: web
      design/services/consulting/blog) and applied the existing class. No items removed, fully
      reversible, verified live in Chrome including active-state highlighting on the new order.
- [x] Drafted `docs/PRIVACY.md` (Phase 3 item #7 from the original plan). Kept it honest and
      narrow rather than generic boilerplate — verified what the site actually does first
      (grepped for cookies/localStorage/analytics, checked every third-party domain
      `index.html` loads: jsDelivr for Bootstrap, Google Fonts, nothing else active). No forms,
      no tracking, no data collection — contact is a `mailto:` link, so nothing is ever
      transmitted to or stored by the site itself. **Not wired up as a site page/footer link
      yet** — just the doc, since adding a new route is more site-structure surface than
      "documentation" and felt like it deserved a look before shipping live.

**Branch:** `feature/perf-and-nav-cleanup`

---

## Phase 3B: Nav active-state bug + homepage load twitchiness
**Status:** ✅ Complete (2026-08-16)
**Effort:** ~2 hours
**ROI:** Medium — polish/correctness fix, not lead-gen, but a warm lead bouncing off a jittery
first impression on mobile is a real cost

**Why:** User reported the blog nav link sometimes isn't marked active, and the homepage load
still feels "twitchy." Root-caused both, plus a follow-up question ("will a warm lead on mobile
get an equally good experience?") surfaced two mobile-specific issues on top.

**Root causes found:**
- **Nav not-active bug:** `Header.razor`'s `highlightPageLink()` does an exact path match
  (`href === path`), which breaks on sub-routes like `/blog/archive` (and would equally break on
  `/webdesign/{slug}`). Compounding this, the highlighting logic lives in a raw `<script>` block
  embedded in Razor markup (`suppress-error="BL9992"` — the Blazor analyzer's own warning against
  this pattern). Since this is a pure client-side WASM SPA with no full page reloads between
  routes, inline `<script>` tags re-inserted by Blazor's renderer on navigation don't reliably
  re-execute the way they do on a hard page load — the likely source of the "sometimes" flakiness.
- **Homepage twitchiness:** Seven components (`Home`, `About`, `Experience`, `Casual`, `Music`,
  `Contact`, `Footer`) each independently fire their own `Http.GetFromJsonAsync` in
  `OnInitializedAsync`, resolving at slightly different times, so sections pop from "loading…" to
  real content non-atomically. `siteproperties.json` alone is fetched **three separate times**
  (Home, Contact, Footer). Both `Header.razor` and `Index.razor` also paper over Blazor's async
  render timing with blind `setTimeout(fn, 300/800/1200)` guesses instead of being told when
  rendering actually finished.
- **Mobile-specific (won't show up on desktop testing):**
  - Hero section (and 3 other spots) uses `100vh`, not `100dvh` — on mobile Safari/Chrome this is
    measured before the URL bar collapses, so full-height sections visibly resize as a lead
    scrolls. Invisible on desktop.
  - `.nav-toggle` (hamburger button, `app.css:676`) is 32×32px with no real touch padding, below
    the ~44×44px comfortable tap-target minimum.
  - Minor: blog pages use `background-attachment: fixed`, historically janky on iOS Safari repaint
    — low priority since it's not the homepage a lead lands on first.

### Tasks
- [x] Added `SitePropertiesService` (`Services/SitePropertiesService.cs`, same cached-`Task`
      pattern as `HeroImageService`), registered in `Program.cs`, wired into `Home.razor`,
      `Contact.razor`, `Footer.razor` in place of each one's own `Http.GetFromJsonAsync` call.
      `Home.razor`'s now-unused `Http` parameter was dropped (only remaining use was this fetch);
      `Footer.razor`'s too, which meant updating its 3 call sites (`Index.razor`, `BlogPosts.razor`,
      `BlogArchive.razor`) to stop passing `Http`. Verified live: `siteproperties.json` now fetched
      **once** per app load instead of 3x (`performance.getEntriesByType('resource')`), and Home
      hero name, Contact email link, and Footer name all still render correctly from the shared data
- [x] Fixed nav active-link matching to be prefix-aware (`path === href || path.startsWith(href +
      '/')`), now living in `DrNav.highlightActive()` (see below) instead of inline in Header.
      Verified live: `/blog/archive` (both via direct load and via clicking "View Archive →" from
      `/blog`, i.e. Blazor's client-side SPA nav) now keeps the "blog" nav link `.active`
- [x] Moved the Header/Index inline `<script>` logic into `wwwroot/js/nav.js` and
      `wwwroot/js/scrollReveal.js` (loaded once in `index.html`), driven from `OnAfterRenderAsync` /
      `NavigationManager.LocationChanged` in `Header.razor` and `Index.razor` via `IJSRuntime`
      instead of blind `setTimeout` guessing. `nav.js` uses a document-level delegated click
      listener (survives Blazor replacing `#navToggle`/`#navItems` on every SPA navigation) and a
      `MutationObserver`-based settle-then-rescroll for `scrollToHash` instead of a fixed delay.
      `scrollReveal.js` similarly re-scans for newly-rendered `.reveal-on-scroll` targets via
      `MutationObserver` instead of two guessed timeouts. Verified live: clicking nav links via
      Blazor's SPA router (not just hard page loads) correctly re-runs the active-link highlighting
      every time; no console errors from either script
- [x] Swapped `100vh` → `100vh` + `100dvh` (progressive-enhancement fallback, `dvh` wins in
      browsers that support it) on `.hero-section`, `section.dark`, `.archive-container`, and
      `.archive-frame iframe`'s height calc. Verified the cascade resolves to `100dvh` in Chrome
      via `getComputedStyle`
- [x] Increased `.nav-toggle` touch target from 32×32px to 44×44px; gave `.nav-toggle span` an
      explicit 24px width (was `100%` of the button) so the hamburger icon's visual size is
      unchanged — only the tappable area grew. Verified via computed styles in Chrome
- [x] Added a `≤768px` override dropping `background-attachment: fixed` → `scroll` on
      `.blog-posts-section` and `.archive-section` (iOS Safari repaint jank), desktop keeps the
      parallax attachment unchanged
- [x] Verified: `dotnet build` succeeds (2 pre-existing warnings in `Experience.razor`, unrelated).
      Live-tested in Chrome: nav highlighting on `/blog/archive` (direct load + SPA nav), the
      `siteproperties.json` request count, hero `100dvh`/nav-toggle computed styles, and confirmed
      `.reveal-on-scroll` elements get bound (`data-reveal-bound`) correctly — could not visually
      confirm the fade-in itself fires, because the automated tab runs `visibilityState: "hidden"`
      and Chrome throttles `IntersectionObserver` callbacks for hidden tabs; this is an automation
      artifact (same `resize_window` limitation noted in the 2026-08-15 session), not a code issue —
      the binding logic itself is confirmed working and is structurally identical to the original,
      already-shipped implementation
- **Found but not fixed (pre-existing, unrelated to this branch):** `blog-posts.json` fails to
      parse — console shows `InvalidCharacterWithinString, 0x0D` at `$[0].content` — a stray
      carriage-return character embedded in a content string breaks `System.Text.Json`, so the blog
      post list silently fails to load on `/blog`. Not caused by anything in this pass (confirmed via
      `git status`, the file is untouched); worth a follow-up to clean the JSON

**Branch:** `feature/perf-and-nav-cleanup` (continues the current branch)

---

## Phase 3C: Visual bug fixes (hero clip glitch, dark webdesign hero, dead GitHub embed) + footer expansion
**Status:** ✅ Complete (2026-08-16)
**Effort:** ~1.5 hours

**Why:** User reported three visual bugs while reviewing Phase 3B live: (1) the hero name
sometimes shows a blue rectangle instead of text, (2) `/webdesign` reads noticeably darker than
the homepage hero, (3) the GitHub activity cards in Skills look broken. Also requested the footer
appear on the standalone-URL pages, and two new footer icons (music site, Haxbyte).

**Root causes found:**
- **Hero blue-rectangle glitch:** the global `h1` rule in `app.css` sets a silver gradient
  text-clip effect (`background-image` + `background-clip: text` + `-webkit-background-clip: text`
  + `color: transparent`). `.hero-section h1` resets `background`/`background-image` and sets
  `color`/`-webkit-text-fill-color` back to solid, but never resets the vendor-prefixed
  `-webkit-background-clip: text` itself (not covered by the `background: none` shorthand). Under
  certain GPU-compositing conditions (the entrance animation promotes the element to its own
  layer), Chromium intermittently paints the un-clipped text bounding box as a solid block instead
  of correctly having nothing left to clip against — the "blue rectangle" (the hero's own navy
  background showing through). Same latent bug existed in `.subpage-hero__title` (used on
  `/consulting`, `/services`, `/blog`, and the webdesign case-study detail pages) and
  `.wd2-hero__title` (`/webdesign`'s own headline) — neither had been reported yet but shared the
  identical root cause, so fixed all three while in this code.
- **`/webdesign` darker than the homepage hero:** two compounding causes. First, `.wd2-hero__title`
  never overrode the global `h1` rule *at all* for `background-image`/`-webkit-text-fill-color`, so
  the page's biggest, most prominent text (the H1 headline) was rendering as a dim gray gradient
  instead of the intended solid `#eef2f7` — same bug class as above, now fixed. Second, genuinely
  by design: `.wd2-hero__scrim`'s radial gradient went up to 88% dark opacity at the edges, and the
  marquee images were dimmed to `brightness(0.68)`/`brightness(0.95)`, on top of a near-black
  `#05111f` base — all deliberate (a scrim is needed for text legibility over a busy image
  marquee, unlike the homepage's flat gradient hero), but stacked on top of the broken headline it
  read as too dark overall. Moderated the scrim (0.88→0.72 edge opacity, 0.45→0.35 center) and
  marquee brightness (0.68→0.78, 0.95→1.0) to bring it closer while keeping the glass panel legible.
- **GitHub activity cards "busted":** confirmed via `curl` — both
  `github-readme-stats.vercel.app` endpoints this section embeds are currently returning `503`.
  This is the public shared instance of a well-known third-party service with a documented history
  of rate-limiting/outages (already flagged as a known risk in `PORTFOLIO_TODO.md` item #22); not
  a bug in this repo, and the iframe can't be feature-detected for this kind of failure (the HTTP
  request itself succeeds, it just renders an error page inside the iframe, so no `onerror` fires).
  Added a "Cards not loading? View the profile directly on GitHub ↗" fallback link so the section
  isn't a dead end during the next outage; left the embed itself in place since removing/replacing
  it outright is the bigger content decision item #22 already describes.

### Tasks
- [x] Reset `background-clip`/`-webkit-background-clip` in `.hero-section h1`, `.subpage-hero__title`,
      and `.wd2-hero__title` (the latter also needed the full `background`/`-webkit-text-fill-color`
      reset it was missing entirely). Verified live via `getComputedStyle` — all three now resolve
      to `background-clip: border-box` instead of the leaked `text` value
- [x] Softened `.wd2-hero__scrim` and `.wd2-hero__marquee` image `brightness()` filters
- [x] Added a GitHub-profile fallback link in `TechnicalSkills.razor` + matching
      `.skills-github__link` style (dim by default, teal on hover, matches the site's link language)
- [x] Added `<Footer />` to `ConsultingPage.razor`, `ServicesPage.razor`, `WebDesignPage.razor`, and
      `WebDesignDetailPage.razor` (the `/webdesign/{slug}` case-study pages — same page family as
      the listing page, kept for consistency even though not explicitly named). `/blog` and
      `/blog/archive` already had it
- [x] Added `Music` field to `SiteProperties`/`siteproperties.json` (user supplied
      `https://www.dougrosenberg.com`) and a footer icon+link for it, plus an unconditional Haxbyte
      icon+link (`https://haxbyte.com`, already referenced elsewhere on the site as Doug's web
      design studio brand)
- [x] New icon assets: `wwwroot/images/socials/haxbyte.svg` (hand-built hexagon + "H" monogram —
      simple enough geometry to construct precisely without visual iteration) and
      `wwwroot/images/socials/music-note.svg` (simple eighth-note glyph). **Note:** the first ask
      was specifically a treble-clef icon — two hand-drawn attempts at the actual clef spiral
      didn't read clearly as one even after rendering both for visual QA (screenshot-verified via a
      temporary preview page), so per user's choice swapped to a plain music-note glyph instead of
      continuing to iterate blind on the clef
- [x] Verified: `dotnet build` succeeds (same 2 pre-existing warnings). Live-tested in Chrome:
      hero-name background-clip fix confirmed via computed styles, `/webdesign` headline no longer
      gradient-dim (screenshot comparison before/after), footer confirmed present + both new links
      correctly wired (`href`s verified) on `/`, `/consulting`, `/services`, `/webdesign`

**Branch:** `feature/perf-and-nav-cleanup` (continues the current branch)

---

## Phase 3D: Hero copy, GitHub activity removal, blog JSON fix
**Status:** ✅ Complete (2026-08-16)
**Effort:** ~1 hour

**Why:** User's girlfriend looked at the site and asked "what the hell is Blazor?" — the hero
subtitle led with the implementation framework instead of what Doug actually does. Same session,
picked off two more small, self-contained items: the GitHub activity embed had already been
flagged as unreliable (Phase 3C), and the `blog-posts.json` parse bug noted above (Phase 3B,
"found but not fixed") turned out to still be live.

### Tasks
- [x] Reworded the hero subtitle in `Home.razor`'s `FallbackTitle` constant and
      `siteproperties.json`'s `title` field: `"software engineer focused on blazor, erp systems,
      and ux-forward design"` → `"software engineer building custom web apps and business systems
      with an eye for design"`. Left `Blazor` in place in `index.html`'s `<title>`/meta
      description and `TechnicalSkills.razor`'s chips — those are read by search engines and
      technical recruiters, not a first-time visitor's eyeball.
- [x] Removed the `// github activity` section from `TechnicalSkills.razor` entirely (both
      `github-readme-stats.vercel.app` `iframe`s and the "view on GitHub" fallback link added in
      Phase 3C) plus the associated `.skills-github*` rules in `app.css`, rather than continuing
      to patch around a third-party service that intermittently 503s. `docs/PORTFOLIO_TODO.md`
      item #22 marked resolved-via-removal.
- [x] Fixed the `blog-posts.json` parse bug: 10 raw control characters (`\r`/`\n`) embedded
      unescaped inside one post's `content` string (a `<pre><code>` code block) were breaking
      `System.Text.Json`. Wrote a small script that walks the file tracking JSON string
      boundaries/escape state and escapes only control characters found *inside* string values
      (leaving the file's own CRLF formatting outside strings untouched) — safer than a blanket
      find/replace, which would've also mangled legitimate structural whitespace. Verified: valid
      JSON, all 3 posts parse, content byte-identical apart from the escaping.
- [x] Verified: `dotnet build` succeeds after each change (the 2 pre-existing `Experience.razor`
      warnings, unrelated to this branch, still show up on a clean build).

**Branch:** `fix/hero-tagline-jargon`

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
