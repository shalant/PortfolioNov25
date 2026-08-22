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

## Phase 3E: Light Mode — Phase A (homepage)
**Status:** ✅ Complete (2026-08-16)
**Effort:** ~3 hours (planning session + implementation pass, same evening)

**Why:** Followed up on `docs/PORTFOLIO_TODO.md` item #29 — the site had exactly one theme
(dark navy/teal) with no CSS-variable layer to build a second one on top of; the `:root`
block in `app.css` declared `--navy`/`--teal`/etc. but nothing in the file actually
referenced them (`var(--` had zero hits before this pass). Planned the palette and
architecture with the user first (warm cream/parchment background, recolored art-deco
assets, OS-preference + manual toggle), then implemented Phase A — the homepage only —
in an isolated git worktree/branch per the user's "do this safely, overnight" request.

**Architecture:**
- Real CSS custom-property tokens (`--bg`, `--bg-hero`, `--text`, `--text-rgb`, `--accent`,
  `--accent-rgb`, `--surface-rgb`, `--surface-nav-rgb`, `--art-deco-1/2`) replace the old
  decorative `:root` block. Mechanically swapped every matching hardcoded
  `#1abc9c`/`#eef2f7`/`rgba(26,188,156,…)`/`rgba(238,242,247,…)`/`rgba(6,14,26|36,…)` in
  `app.css` (152 hex + ~230 rgba occurrences) to reference the new tokens, verified by
  before/after grep counts at each step rather than trusting the `sed` passes blindly.
- Light-mode values are defined under `html[data-theme="light"] .dr-theme-scope` — scoped to
  a new `.dr-theme-scope` wrapper div around `Index.razor`'s composed content (Header through
  Footer), **not** on bare `:root`. This means the toggle sets `data-theme` globally but only
  the homepage's descendants actually re-theme; every other page (`/webdesign`, `/services`,
  `/consulting`, `/blog`, and the orphaned `Consulting3/4/5/6`/`WebDesignPageOLD`/`/archive`
  pages) keeps resolving the same var()s to the dark `:root` defaults regardless of toggle
  state. Verified live: toggling to light mode on `/consulting` left it fully unchanged.
- Manually re-pointed the ~15 remaining hardcoded colors the mechanical passes couldn't
  safely touch (per-selector triage, not blanket sed): `.hero-section`/`.about-section`/
  `.experience-section`/`.skills-section`/`.casual-section`/`.music-section`/
  `.contact-section` background gradients now use `var(--bg)`/`var(--bg-hero)`;
  `.hero-btn--primary` deliberately kept on the *static* `--teal`/`--teal-dark` tokens
  (not the theme-adjusted `--accent`) since it's a solid filled button that needs its own
  fixed bg/text contrast pair, independent of page theme; nav text-shadows (tuned for
  legibility over a dark/photo backdrop) and two remaining `#ffffff`/`#2fffda` hardcoded
  hover colors got light-mode-only overrides instead of being changed globally, to leave
  dark mode's look untouched.

**Art-deco texture recolor:** the two Bauhaus/art-deco WebP backgrounds turned out to be
photographic blue gradients (hundreds of unique blues forming soft rays), not flat 2-color
shapes as assumed during planning — confirmed via `magick identify -unique-colors`. Exact
color-key substitution would've looked patchy, so used a duotone/gradient-map instead:
grayscale the source for its luminance/shape, then `-clut` against a 256×1 cream→navy
gradient (`#f5f1e8` → `#2c3e50`). Result keeps the original geometry and soft edges exactly,
just recolored. Rendered and viewed both outputs before wiring them in (`artDecoBackground1
/2-light.webp`, comparable file size to the originals: 25KB→23KB, 92KB→84KB). Also swapped
`mix-blend-mode: screen` (lightens a dark backdrop) to `multiply` (darkens a light one) for
light mode via the same `--art-deco-blend` token — screen would've washed the recolored
texture out to near-invisible on cream.

**Toggle:** `wwwroot/js/theme.js` (new, same small-module pattern as `nav.js`/
`scrollReveal.js`) reads `localStorage['dr-theme']`, falls back to
`prefers-color-scheme`, and sets `data-theme` on `<html>`. Loaded as a **blocking** `<script>`
in `index.html`'s `<head>` — has to run before Blazor boots and before first paint, so unlike
the rest of this app's JS interop it can't wait for `OnAfterRenderAsync`. Icon-only sun/moon
toggle added to `Header.razor` next to `.nav-cta`, matching the existing 14×14 thin-stroke
nav-icon style and the 44×44 `.nav-toggle` tap target; icon swap is pure CSS off `[data-theme]`,
no Blazor re-render needed. Kept icon-only (no label) since the nav is already flagged tight
(7 links + CTA, Phase 3 note).

**Contrast verification:** computed actual rendered contrast ratios in Chrome (accounting for
alpha blending against the cream background, not just nominal color) rather than eyeballing.
Found and fixed: `.hero-eyebrow`/`.nav-cta` teal text needed a darker `--accent` in light mode
specifically (`#0f7a63`, ~4.68:1 — plain `#1abc9c` on cream measured ~2.1:1, failing even the
3:1 UI-component floor). Several muted body-copy elements (`.hero-subtitle`,
`.experience-detail__bullets`, `.experience-list__title`, `.casual-description`,
`.music-paragraph`, `.contact-sub`, `.site-footer__title`) were tuned at low alpha (0.35–0.55)
against the dark background for a "muted" look, which read fine there but measured
2.9–4.0:1 once inverted onto cream — bumped to 0.75–0.78 alpha in light mode only (dark mode
unchanged), verified 4.5:1+. **Not exhaustively audited:** ~15 smaller decorative
labels/badges/captions at similarly low alpha (category labels, the footer copyright line,
etc.) were left as-is — common "fine print" convention, but a real gap if a full AA pass is
ever wanted.

**Verification:** `dotnet build` and `dotnet publish -c Release` both succeed (0 new warnings
beyond the 2 pre-existing, unrelated `Experience.razor` ones). Live-tested in Chrome: toggle
flips `data-theme` and persists across reload; dark mode (still the default in this browser's
`prefers-color-scheme`) renders pixel-identical to before this change; light mode confirmed
readable and cohesive scrolling through every homepage section (Home/About/Experience/
Skills/Casual/Music/Contact/Footer); `/consulting` confirmed unaffected by the toggle, proving
the `.dr-theme-scope` isolation works. **Not verified:** mobile breakpoints — this session's
`resize_window` tool exhibited the same limitation noted in earlier sessions (reports success
without actually changing `window.innerWidth`); worth a manual phone check before Phase B.

**Deferred to Phase B (future session):** extend the same token system to `/webdesign`,
`/webdesign/{slug}`, `/services`, `/consulting`, `/blog`, `/blog/archive`.

**Branch:** `feature/light-mode-phase-a` (new branch, isolated worktree — not pushed, not
merged; left for review)

---

## Phase 3F: Light Mode — Phase B (subpages)
**Status:** ✅ Complete (2026-08-17)
**Effort:** ~1.5 hours

**Why:** User asked directly ("please also add the light mode to the links such as /web-design
and the others") after trying Phase A live and approving it, plus several rounds of follow-up
readability fixes made directly on the homepage. This extends the same token system Phase A
built to the rest of the site, per the "Deferred to Phase B" note above.

**Scope:** wrapped `.dr-theme-scope` around `/webdesign` (`WebDesignPage.razor`),
`/webdesign/{Slug}` (`WebDesignDetailPage.razor`), `/services` (`ServicesPage.razor`),
`/consulting` (`ConsultingPage.razor`), `/blog` (`BlogPosts.razor`), and `/blog/archive`
(`BlogArchive.razor`) — same wrapper pattern as `Index.razor` (Header through Footer, inside
the div). Confirmed `Header`/`Footer` sit inside the wrapper on every page, matching the
homepage, so the toggle button behaves identically everywhere.

**Color migration:** most of `app.css`'s subpage-specific sections (`.subpage-hero__title/sub`,
`.consulting-service-card h3`, `.wd-project__name`, `.tech-chip`, `.about-skill-chip`, etc.)
turned out to already reference `var(--text)`/`var(--accent)`/etc. from earlier, unrelated work
— less migration needed than expected. Filled the remaining gaps:
- Panel backgrounds (`.subpage-hero`, `.wd-case`, `.consulting-services`, `.webdesign-tools`,
  `.webdesign-projects`) — hardcoded dark gradients/solids → `var(--bg-hero)`/`var(--bg)`
- "Glass card" tints (`.consulting-service-card`, `.wd-case__highlight-card`) — flipped
  direction: a lightening white wash reads as "raised" on a dark bg, but the same treatment is
  invisible on an already-light bg, so light mode uses a subtle *darkening* navy wash instead
  (`rgba(var(--text-rgb), 0.03)`), same idea, opposite direction
- Image caption strips (`.wd-case__*__figcaption`) and dark scrim overlays over the art-deco
  texture (`.blog-posts-section::before`, `.archive-section::before`) — swapped to
  `rgba(var(--surface-rgb), …)`, an existing token that's already `255,255,255` in light mode /
  `6,14,26` in dark, so the same rule produces a dark scrim in dark mode and a light wash in
  light mode instead of always being a near-black smear over what's now a light-recolored
  texture
- `BlogPosts.razor` and `BlogArchive.razor`'s own embedded `<style>` blocks (not `app.css`) got
  the same hardcoded-hex → token substitution, since they're page-scoped CSS Phase A never
  touched. The reading-modal content (`.modal-dialog` and everything inside it) was
  deliberately left alone — it's already a fixed white card in both themes and reads fine as-is
- Real body-copy alpha (card descriptions, case-study approach paragraphs, highlight text,
  blog excerpts, taglines) bumped from their original 0.5–0.7 to 0.85, matching the value the
  homepage settled on after live feedback that the original Phase A pass (0.75–0.78) still
  read as faint — didn't repeat that under-shoot here

**Two deliberate non-migrations, not oversights:**
1. `/webdesign`'s `.wd2-hero` (the scrolling-screenshot marquee hero) darkens real photo
   assets with a scrim, not a flat color — there's no clean light equivalent without
   regenerating brightness-adjusted screenshot images, out of scope here. Rather than let
   `.wd2-hero__title`/`__sub` inherit `var(--text)` and turn navy against a backdrop that's
   still effectively dark (bad contrast), pinned them to the same near-white they use in dark
   mode, and pinned `.section-eyebrow` inside that specific glass panel to the bright
   `#1abc9c` too (the light-mode `--accent` is a *darker* teal calibrated for cream, which is
   worse contrast against this still-dark panel, not better). This hero stays dark-styled in
   both themes by design — a common pattern for a photo hero on an otherwise light site.
2. `NotFound.razor` (rendered both as `App.razor`'s global catch-all for unmatched routes, and
   inline on `/webdesign/{slug}` for a bad slug) still uses fully hardcoded dark colors. Left
   untouched: it's not one of the six routed pages in scope, and the global catch-all case
   renders *outside* any page's `.dr-theme-scope` wrapper entirely (it's mounted at the
   `App.razor` root), so a scoped CSS override wouldn't reach it anyway — fixing this properly
   means deciding whether to wrap the app-level fallback too, a call worth making deliberately
   rather than as a drive-by inside this pass.

**Also found and fixed while auditing:** `.webdesign-link:hover` hardcoded `#16a085` (the
static `--teal-dark` brand token) as a "darken on hover" step tuned for dark mode's bright
`#1abc9c` base. In light mode `--accent` is already `#0f7a63` (darker, AA-calibrated) —
`#16a085` is actually *lighter* than that, so hovering would have moved toward worse contrast
instead of away from it. Added a light-mode-specific hover color (`#0d6854`, darker still)
instead.

**Verification:** `dotnet build` and `dotnet publish -c Release` both succeed (0 errors, same 2
pre-existing unrelated `Experience.razor` warnings) after every commit. Confirmed via targeted
`grep`/script sweeps of the migrated selector groups that no unaddressed hardcoded colors
remained in-scope (excluding the two deliberate exceptions above and dead CSS for unrouted
`.arborkin-*`/`.archive-header` classes, already confirmed unused in Phase A's research).
**Not verified live in a browser** — this pass ran in an isolated worktree per instructions not
to start a competing `dotnet run` against the shared `bin`/`obj` output (a previous session hit
hours of confusing failures from exactly that: a separate `dotnet run` racing Visual Studio's
own debug session for file locks). Contrast/font-weight values were computed by direct
calculation and pattern-matched against Phase A's already-verified-live values, not measured
against a running render this time — worth a visual pass before merging, same as Phase A's
unverified mobile breakpoints.

**Branch:** `feature/light-mode-phase-b` (new branch, isolated worktree, based on
`feature/light-mode-phase-a` — not pushed, not merged; left for review)

---

## Phase 3G: Light mode follow-ups — webdesign hero, 404 page, loading splash
**Status:** ✅ Complete (2026-08-17)

**Why:** User asked to revisit three spots Phase B left dark: the `/webdesign` marquee hero
(pinned dark on the assumption a light version needed regenerated assets), the 404 page (never
wrapped in `.dr-theme-scope` at all), and the pre-boot loading splash (can't use
`.dr-theme-scope` since it renders before any Razor content exists).

### Tasks
- [x] `.wd2-hero`: the "no clean light equivalent" assumption didn't hold up — the scrim is a flat
      radial-gradient overlay independent of the actual screenshot images, so it can reuse
      `--surface-rgb` (already near-white in light mode) the same way every other glass panel on
      the site does. Lightened `.wd2-hero`'s base background and `.wd2-hero__scrim` to
      theme-aware values, brightened the marquee image filters a step further in light mode (the
      dark-cinema dimming read as muddy against a light scrim), and **removed** the three
      hardcoded-dark pins on `.wd2-hero__title`/`__sub`/eyebrow entirely — they already read
      `var(--text)`/`var(--accent)` in their base rules, so once the backdrop is genuinely light
      those variables just work.
- [x] `NotFound.razor`: wrapped its root markup in `.dr-theme-scope` (same pattern as
      `Index.razor`) and converted its previously self-contained hardcoded-hex `<style>` block to
      the shared CSS custom properties. Dark-mode output is pixel-identical (every hardcoded value
      converted matched the dark `:root` default exactly). Added light-mode alpha bumps for
      `.notfound-subtitle` and `.notfound-suggestions p` (0.7/0.8 → 0.85) — same "real sentences
      need more than bare AA" lesson from the homepage pass. Left the decorative hexagon SVG's
      literal teal fill alone (low-opacity art, not text).
- [x] Loading splash (`.app-loading`, static markup in `index.html`, styled in `app.css`): added
      `html[data-theme="light"]` overrides gated directly on the attribute (not `.dr-theme-scope`,
      which doesn't exist in the DOM yet at this point in the boot sequence) for `body`'s base
      background, the progress-ring track/fill colors, the percentage text, and the pulsing
      eyebrow label. The inline blocking `<script>` in `index.html`'s `<head>` (from Phase A)
      already sets `data-theme` before first paint, so no flash-of-wrong-theme risk here.
- [x] Verified: `dotnet build` succeeds (same 2 pre-existing `Experience.razor` warnings).
      **Not verified live** — same reasoning as Phase B: avoided starting a competing `dotnet run`
      against Visual Studio's active debug session. Worth a visual pass on all three before merging.

**Branch:** `feature/light-mode-phase-b` (continues the same branch/worktree)

---

## Phase 3H: Mobile improvements
**Status:** 🔄 PR'd EOD 2026-08-18 — Samsung tablet touch-dropdown check still pending, see Still open
**Branch:** `feature/mobile-improvements`

**Why:** User confirmed mobile as the next focus after Phase 3G's light-mode follow-ups merged.
Started with a static-code audit (touch-target sizes, dead/duplicate mobile CSS, overflow risks),
then moved to fixing real bugs surfaced by screenshots from the user's actual phone/tablet — code
review alone had already missed two live-only bugs (see below).

### Tasks
- [x] `NotFound.razor`'s mobile layout was assumed dead (gated behind a `.notfound-mobile-active`
      class nothing ever applied) — converted to a real `@@media` block, then **discovered
      `app.css` already had an identical, working `@@media (max-width: 768px)` block for the same
      selectors** ("NotFound (404) Page Animations & Responsive"). Removed the duplicate rather
      than keeping two copies; left a comment pointing at the real one.
- [x] `ProjectMediaFrame.razor`'s hover-triggered crossfade never played on touch devices (no
      `mouseenter`). Added `@@media (hover: none) { animation-play-state: running }` so touch
      devices see the same auto-cycling gallery as a desktop hover.
- [x] Added defensive overflow CSS for raw HTML blog content (`img { max-width: 100% }`, tables
      wrapped in `overflow-x: auto`) — content the Claude-API blog generator produces isn't
      guaranteed to be mobile-safe on its own.
- [x] Grew undersized touch targets: `.site-footer__social` 25.6px → 36px, `.site-footer__top`
      (back-to-top) 32px → 44px. Left `.contact-social`/blog tag chips/share buttons alone — close
      enough to the 44px guideline not to chase further.
- [x] `.experience-list` on mobile split 3-then-1 instead of 2×2 — root cause was `flex-wrap` +
      pixel `flex-basis` (content-width-dependent row splits). Fixed with fixed 2-column CSS Grid.
- [x] Real screenshot from the user (400×729 DevTools) caught two bugs static review missed:
      the theme toggle floating in the middle of the collapsed mobile navbar (a `display:none`
      `<nav>` wrapper still counts as a flex child under `justify-content: space-between` even
      though its contents are hidden — the "phantom flex item" pattern, audited project-wide
      afterward, only the navbar had it), and confirmed the 3-then-1 experience grid live. First
      navbar fix attempt (`nav { display: contents }`) **did not work live** per the user's
      follow-up screenshot — replaced with `.theme-toggle { margin-left: auto }`, confirmed
      working via a second screenshot (toggle + hamburger now grouped at the right edge).
- [x] Tablet-width nav (~820–1200px, between the compression breakpoint and the hamburger
      collapse) still rendered the full 7-link row, cramped — flagged live by the user via a
      screenshot at ~1009px width, and previously noted as "pending a decision" back in the
      Overnight Housekeeping section. Wired up `.nav-has-dropdown`/`.nav-dropdown`/`.nav-caret` —
      a complete hover-dropdown CSS system that already existed in `app.css` but had never been
      applied to any markup — to group the four destination links (web design/services/
      consulting/blog) behind a "more" trigger, cutting the visible top-level link count from 7 to
      4 at every width above the hamburger breakpoint, not just tablet. Trigger is a `<button>`
      (nothing of its own to link to); added `:focus-within` as a keyboard-accessible fallback to
      the hover reveal, and taught `nav.js`'s mobile-menu-auto-close listener to only match real
      `<a>` nav links so tapping the button doesn't close the whole mobile menu.

- [x] Horizontal overflow at 400px width making the About section read as right-clipped /
      not centered — user reported it, gave them a console snippet
      (`[...document.querySelectorAll('*')].filter(el => el.getBoundingClientRect().right >
      window.innerWidth + 1)`) to identify the overflowing element live, since `resize_window`
      (Chrome tool) doesn't actually change `window.innerWidth` in this environment. User ran it
      themselves and the array came back empty — no element overflows at 400px. Resolved (or
      never real; not chased further since there's nothing to fix).

### Still open
- [x] The new "more" tablet-nav dropdown's touch behavior — turned out to be moot for the
      narrow/hamburger case: below the 820px breakpoint `.nav-dropdown` is forced always-expanded
      (no hover/focus needed at all), confirmed live via the user's Samsung tablet screenshot
      showing "more" pre-expanded inline in the mobile menu. Still genuinely unverified in the
      820–1200px tablet-landscape band specifically (where it *is* hover/focus-driven with no JS
      touch fallback) — no device test landed in that exact width range yet. Not blocking; revisit
      if a tablet-landscape user reports it.

---

## Phase 3I: Tablet testing follow-ups + Blazor load-time work
**Status:** 🔄 In progress
**Branch:** `fix/tablet-testing-followups`

**Why:** User tested Phase 3H's merged/deployed changes live — on a Samsung tablet for the mobile
nav, and on a separate budget Android tablet for load time — and found one real rendering bug plus
raised load-time/loading-screen concerns the original mobile pass didn't cover.

### Tasks
- [x] Mobile hamburger menu visibly clipped/ghosted (background page content bleeding through)
      when the user scrolled the page while the menu was open — reported with two tablet photos.
      Root cause: `nav.js` never locked background scroll while `#navItems.active`, so the sticky
      navbar's `backdrop-filter` (which also changes blur radius mid-scroll via the `.scrolled`
      class) and the dropdown's own separate `backdrop-filter` were both recompositing over moving
      content on a mobile GPU — a known trigger for this class of glitch. Fixed by locking body
      scroll (`body.nav-open { overflow: hidden }`, scoped inside the existing `max-width: 820px`
      block) while the mobile menu is open, matching standard mobile-nav practice — removes the
      trigger instead of chasing the compositor.
- [x] The hero `<h1>` showed a visible teal focus-ring box on every page load — traced to
      `App.razor`'s `<FocusOnNavigate Selector="h1" />` (stock Blazor a11y pattern: focuses the
      page heading on route change for screen readers) painting a default browser focus ring even
      though the element is `tabindex="-1"` and never Tab-reachable. Added `h1:focus { outline:
      none }`.
- [x] Load-time audit, prompted by a reported ~5s first load on a budget Android tablet on home
      wifi. Confirmed GitHub Pages/Fastly already compresses `.wasm`/`.js`/`.css` correctly
      (~40% of decoded size transferred) — that was never the bottleneck. Fixed what was:
      - Google Fonts moved from a CSS `@import` (serializes the font fetch behind `app.css`
        finishing download) to a `<link>` in `index.html`'s `<head>` (fetches in parallel from
        the start of page load, already preconnected).
      - Removed `bootstrap.bundle.min.js` (CDN, ~80KB decoded) — grepped the whole app for
        `data-bs-*` attributes and JS Bootstrap component calls, found zero usage anywhere. Pure
        dead weight (an extra external round-trip) on every single page load.
      - Found two unoptimized images actually being shipped to users: `DougCartoon4.png` (2.3MB
        PNG, displayed at 200px wide on `/consulting`) and `DrCyberPunk.png` (1.6MB PNG, the
        "casual" hero image). Resized and converted both to WebP (25KB and 50KB respectively,
        ~98% smaller) and updated the two references.
      - Deleted two images with zero references anywhere in the app (`Douglas_Rosenberg_Rev.png`,
        4.7MB; `tealRectangle.png`, 343KB) — orphaned dead weight in the deployed bundle.
- [ ] Tried replacing the stock two-circle Blazor loading spinner with the site's own hex logo
      mark (reused from the navbar), filling bottom-to-top via the same
      `--blazor-load-percentage` CSS var the old radial ring already consumed. Built and visually
      verified working (isolated static preview, hex fills correctly), but the user wasn't sold on
      it on reflection — **reverted to the original two-circle spinner**, decision deferred. The
      "avoid a recognizable Blazor spinner" goal is still open; next attempt should probably
      explore other directions rather than re-proposing the same hex treatment.

### Discussed, not done
- User asked about avoiding the loading splash entirely; the honest tradeoff is that Blazor WASM
  has to download and boot a full runtime before it can render anything, so *some* gap is
  unavoidable without an architecture change (see Phase 4 below). Considered pre-rendering a
  static copy of the real hero markup directly in `index.html` so the page looks loaded from
  frame one — set aside as a bigger, riskier change (has to stay pixel-matched with
  `Home.razor`'s real hero or the Blazor handoff causes a visible flash/jump). Not started.
- ~~MudBlazor is used in only 4 components...~~ — turned out to be *zero* live components once
  traced properly; fully removed in Phase 3J below.
- A PWA service worker (caches boot assets so repeat visits skip the network almost entirely)
  would directly address "reload" speed specifically. Bigger, riskier addition (cache
  invalidation / staleness risk on a site that updates via blog posts) — flagged as a follow-up
  option, not implemented here.

### To do
- [x] **Cleanup: purge unused pictures.** Done in Phase 3J below — see that section for the
      full list. `motion-background.jpg` was left alone as planned (lower priority, fine as-is).

---

## Phase 3J: MudBlazor removal, orphaned prototype pages, mobile CTA gap
**Status:** ✅ Complete — user can't commit until EOD 2026-08-19, so this is sitting staged
locally on the branch, not yet pushed/PR'd.
**Branch:** `fix/mudblazor-removal-and-cleanup`

**Why:** Follow-up from the Phase 3I conversation — tracing where `/consulting` actually routes
to (`Pages/ConsultingPage.razor`, which has its own hand-rolled markup) revealed that all 4
MudBlazor-using components flagged as "used in only 4 components" weren't actually live at all:
`Consulting3/4/5/6.razor` each declare their own `@page` route (`/consulting3` etc.) but are
linked from nowhere in the site, and `DougCartoon`/`DougCartoon2` are only pulled in by those same
orphaned pages. MudBlazor's entire payload (package + CSS + global providers in `MainLayout`) was
being shipped to every visitor for prototype pages nobody could find. Separately, the user asked
whether the mobile-hidden "get in touch" nav CTA counted as a CTA (yes — the class is literally
`nav-cta`) and flagged it as a gap once confirmed it fully disappears on mobile with no fallback.

### Tasks
- [x] Full route sweep (every `@page` directive vs. every href in `Header.razor`) to find every
      orphaned page before touching anything, not just the ones already suspected. Found one more
      beyond the MudBlazor set: `Pages/WebDesignPageOLD.razor` (`@page "/webdesignOLD"`), also
      linked from nowhere.
- [x] Deleted 8 confirmed-orphaned files: `Consulting.razor` (no `@page`, no references at all —
      not even independently routable), `Consulting3/4/5/6.razor`, `DougCartoon.razor`,
      `DougCartoon2.razor`, `WebDesignPageOLD.razor`.
- [x] Fully removed MudBlazor: `PackageReference` from the `.csproj`, `AddMudServices()` +
      `using MudBlazor.Services;` from `Program.cs`, `@using MudBlazor` from `_Imports.razor`,
      the `MudBlazor.min.css` `<link>` from `index.html`, and the four global providers
      (`MudThemeProvider`/`MudPopoverProvider`/`MudDialogProvider`/`MudSnackbarProvider`) from
      `MainLayout.razor` — those providers wrap every page via the default layout, so they were
      the one piece that could have caused a real regression if missed; build stayed clean and a
      full click-through (home, webdesign, services, consulting, blog, plus one deleted route to
      confirm it 404s cleanly through the site's own `NotFound.razor`) showed no console errors.
- [x] Fixed the mobile nav CTA gap: `.nav-cta` lives outside `<nav>`/`.nav-items` in the DOM and
      is `display: none` below 820px, so it wasn't just visually collapsed into the hamburger —
      it was unreachable from the nav entirely on mobile. Added a second copy of the link as the
      last `<li>` inside `.nav-items` (`.nav-cta-mobile-item` / `.nav-link--cta`), hidden above
      820px so it doesn't duplicate the standalone desktop button, shown only inside the expanded
      mobile menu. True mobile-viewport visual confirmation wasn't possible in this environment
      (same `resize_window` limitation noted elsewhere in this doc) — DOM/CSS structure verified,
      but this one's worth a real-device check.
- [x] Finished the picture purge from Phase 3I's "To do": deleted `woman-with-tablet.jpg` (155KB,
      confirmed orphaned) and `franciscan-friars-modern.svg` (1.2KB, orphaned — found during the
      full cross-reference sweep, not previously flagged); resized/converted `design-desk.jpeg`
      (2400×1610, 487KB) to WebP (57KB, 88% smaller) and updated the `heroimages.json` reference.
      Also deleted `DougCartoon4.webp` — the WebP created in Phase 3I to replace the oversized
      PNG became orphaned itself once `Consulting4.razor` (its only referrer) was deleted here.
      Full cross-reference sweep (every file in `wwwroot/images/` against every `.razor`/`.json`/
      `.css`/`.html` reference, excluding build artifacts) found no further orphans.
- [x] Noticed but did not touch: `experience.json` has a dead `titleOLD` field (not bound by any
      component) on the Friars ERP entry — same "OLD" leftover pattern as the deleted page, but
      it's data content rather than dead code, and out of scope for tonight's cleanup.
- [x] Skills-chip polish, prompted by user feedback on the new Marketing & Analytics chips: added
      a hand-drawn Meta icon (`icons/tech/meta.svg`, a stroked infinity mark in Meta's blue
      gradient — not a trace of the trademark) for Meta Pixel/Meta Conversions API; confirmed
      neither Apple nor GitHub Copilot has an official color mark, so both are now tinted with
      the site's `var(--accent)` via a CSS mask instead of sitting flat gray on a dark plate;
      resized the Syncfusion/DevExpress wordmark chips to roughly match sibling chip text size,
      then sized Syncfusion 10% under DevExpress specifically (scoped to the dark-plate class,
      which only Syncfusion uses among wordmarks) per user follow-up.

---

## Phase 3J.1: Force dark theme as the default
**Status:** ✅ Complete
**Branch:** `fix/dark-mode-default`

**Why:** User's girlfriend saw the site in light mode on first visit. Root cause: `theme.js`
fell back to the visitor's OS/browser `prefers-color-scheme` when no saved preference existed.
Compared both themes live in-browser (dev server + screenshots) — dark reads as the intended
design: the tiled Bauhaus/art-deco background pattern has real geometric contrast in dark mode
and washes out to near-invisible in light; the navy+teal palette and portrait's monitor-glow
bokeh were clearly tuned for dark. Light mode isn't broken, just flatter — kept as an explicit
opt-in via the header toggle, not the first impression.

### Tasks
- [x] `theme.js`: `resolve()` now falls back to `'dark'` unconditionally instead of checking
      `matchMedia('(prefers-color-scheme: light)')`. Saved `localStorage` preference (from the
      header toggle) still wins for returning visitors on either theme.

---

## Phase 3K: Run Copilot's UI assessment through Claude Code
**Status:** ✅ Triage complete (in-chat, not written up as a standalone doc) — folded into Phase 3L
**Branch:** n/a — no code changes of its own

**Why:** User had GitHub Copilot produce a UI assessment of the entire deployed site
(`docs/CopilotAssessmentDrDev19Aug26.txt`, appeared in the working tree 2026-08-19 — not authored
by Claude, from a separate session/tool). Intent is to work through that assessment's findings as
a fresh task once Phase 3J is out of the way, rather than layering more changes onto an
already-large branch.

### Tasks
- [x] Read `docs/CopilotAssessmentDrDev19Aug26.txt` in full.
- [x] Triage its findings against what's already shipped/in-flight in Phases 3H–3J. Most of
      Copilot's "high-impact, low-effort" list turned out to already be shipped: sticky header,
      header CTA, dark/light toggle, tech-stack chips per project, project thumbnails, card CTAs,
      and a pricing section on `/services` all predate the assessment — Copilot appears to have
      inferred site structure from a crawl rather than the live rendered nav (its "Projects and
      Music compete in the navbar" point is wrong — neither is a nav link). Rejected outright:
      fabricating testimonials/Slack-message social proof (dishonest, don't do it — real ones only
      if/when actual clients provide them) and the "premium agency" messaging pivot in Copilot's
      second answer, which directly contradicts its own first answer praising the site for feeling
      "handcrafted rather than template-generated." Genuine gap identified and carried into Phase
      3L: no case-study depth on any project (just logos + bullets, no real outcome/before-after).
- [x] Scope and branch the remaining, still-relevant items — folded into Phase 3L below rather than
      a separate branch, since user's actual next ask ("make this extremely stunning") is the same
      whole-site design work at a larger scope.

---

## Phase 3L: Whole-site "stunning" design pass
**Status:** ✅ Complete — closed out, not merged yet
**Branch:** `feature/stunning-design-pass`

**Why:** User's explicit goal: make the site "extremely stunning." Design-lead take (see chat, not
duplicated here): don't chase generic premium-SaaS-agency tropes — dark-bg-plus-single-accent is
already one of the three clichéd "AI portfolio" looks, and what currently saves this site from
reading generic is the Bauhaus texture, the serif/mono type pairing, and real personal content
(the jazz background), not the hue count. Plan is to push those already-distinctive elements
further rather than import someone else's premium aesthetic. Full token plan (color/type/layout/
signature) was proposed and reviewed with the user before any code — color and font changes
specifically called out per CLAUDE.md's "no color scheme changes without discussion" rule.

### Design plan
- **Color:** kept navy `#2c3e50`/teal `#1abc9c`/cream `#f5f1e8` exactly as-is. Added one new token,
  `--brass` (`#c9a15a`) — pulled from the saxophone, not decoration — reserved for the one-time
  hero entrance and a future divider motif; never a second "regular" accent alongside teal.
- **Type:** kept Cormorant Garamond (display) + Montserrat (body). Swapped the eyebrow/label/tag
  monospace from `'Courier New'` (generic OS fallback) to JetBrains Mono (an actual code-editor
  typeface, loaded from Google Fonts) — more honestly "developer-authentic" than a system fallback.
- **Signature motion — "swing":** a `--ease-swing` cubic-bezier (quick attack, slight overshoot,
  settle) replacing the stock Material `cubic-bezier(0.4,0,0.2,1)` on `.reveal-on-scroll` and the
  skills-chip stagger, plus a one-time hero entrance (`bauhausRise`) where the two Bauhaus/art-deco
  layers settle into place on first paint, bg2 a beat behind bg1 — a nod to jazz phrasing instead
  of default motion-library easing. User flagged a fast-load-time concern for this specifically;
  confirmed it's a pure CSS animation on elements already in the DOM/CSS (no new asset weight,
  can't run before first paint since it animates already-painted content), and is neutralized by
  the existing site-wide `prefers-reduced-motion` rule — the real load-time cost is the ~18MB
  uncompressed Blazor WASM runtime, unrelated and already tracked as the Phase 4 Astro migration.

### Tasks
- [x] Add `--brass`/`--brass-rgb`, `--mono`, `--ease-swing` tokens to `app.css` `:root`.
- [x] Load JetBrains Mono in `index.html`; replace all `'Courier New'` font-family declarations
      (app.css + BlogPosts.razor + Music/Experience/Casual.razor loading states) with `var(--mono)`.
- [x] Re-time `.reveal-on-scroll` and the skills-chip stagger with `var(--ease-swing)`.
- [x] Add the `bauhausRise` hero entrance animation to `.hero-section::before`/`::after`.
- [x] Applied the same signature to `/webdesign` (`.wd2-hero`) on request, since it's the page most
      commonly shown to prospective clients: `.wd2-hero::before`'s Bauhaus layer now reuses
      `bauhausRise`, and `.wd2-hero__glass`'s existing entrance (`wd2-glass-in`) swapped its generic
      `cubic-bezier(0.16,1,0.3,1)` for `var(--ease-swing)`. While reviewing that page: it already
      has real per-project write-ups (Hardware Etc LLC, Sonus Construction Group — named clients,
      platform tag, description, bullets) that the earlier "no case-study depth" gap note didn't
      account for — that gap is more about the homepage's Experience carousel (logos + bullets
      only) than `/webdesign`, which is already close to what Copilot's assessment asked for.
- [x] Experience section, per user request to "reflect my professional life": cross-checked
      `experience.json` against the actual resume PDF (`wwwroot/resume/DouglasRosenbergResumeAugust26.pdf`)
      and found two real gaps — `ExperienceModel.cs` already had unused `StartDate`/`EndDate`
      fields never populated or rendered, and the "Bridgestone Marketing" entry's company name was
      wrong (resume shows the actual employer as Shift; BridgestoneMarketing.com was the client
      project — the entry was already using Shift's logo, `cropped-SHIFTLogo_4-C.png`, so this was
      a stale label, not a design choice). User confirmed the Shift relabel and populating real
      dates from the resume; user then asked to remove the date display from the UI after seeing
      the plan, so dates were added to `experience.json` (harmless, accurate, may be useful later)
      but are not rendered — only the corrected company name now shows as text in the detail panel
      (previously company was only inferable from the logo image, never shown as text anywhere).
      User explicitly declined adding the two Skillstorm roles the resume also revealed (Jan–Dec
      2022, entirely absent from the site) — do not add them without being asked again.
- [x] Section-divider motif: new `<SectionDivider />` component (9-bar brass waveform, staggered
      scaleY entrance on `--ease-swing`, wired to each section's existing `.reveal-on-scroll`
      trigger so it "plays" like an equalizer waking up) added under the H2 in About, Casual,
      Contact, Experience, Music, and TechnicalSkills. Before implementing, found that every one of
      those headings already has an explicit `.xxx-heading::after { display: none !important; }`
      disabling a generic centered teal-gradient underline baked into the base `h2` style
      (`app.css:276`) — a prior, deliberate decision to remove exactly this class of decoration.
      Judged the new version different enough to proceed (subject-specific waveform vs. flat
      generic bar, alignment follows each heading instead of force-centered, animates in via the
      existing scroll-reveal system instead of a blanket fadeInDown) but flagged the tension to the
      user rather than silently reintroducing what a past pass removed. Verified live in both
      themes — reads left-aligned under About/Casual/Experience/Music/TechnicalSkills and centered
      under Contact (inherits `text-align` from each container, no per-section overrides needed);
      landed especially well under Music's "From Saxophone to Software," right before the
      saxophonist content. Hit an unrelated local snag along the way: Visual Studio had the
      project's build output locked, blocking `dotnet run`; user paused their VS session so the
      dev server could run for verification.
- [x] Case-study treatment for Experience entries — dropped. User's reaction: "case studies seem
      cheesy." Agreed — a labeled challenge/solution/result section would clash with the
      developer-authentic tone that's the actual differentiator here (per the original Copilot
      triage). Offered a lighter alternative (fold 1-2 real stats already in the resume into
      existing bullet prose, no new section/template) but user chose to close this out as-is
      rather than pursue either version. Experience section stays exactly as it landed after the
      Shift/dates fix above.
- [ ] Case-study treatment for 2-3 real *Experience* entries specifically (the homepage carousel is
      the thinner of the two project-facing sections now) — not yet started.
- [ ] User to test-drive the brass accent + font swap on this branch before it's considered final —
      approved conditionally ("as long as this is on a branch, i'm happy to test drive changes").

---

## Phase 4: Migrate the whole site to Astro on Cloudflare (separate session/repo)
**Status:** ⏳ Not started — deliberately deferred, not a quick add-on. **Confirmed (2026-08-21):
not happening before September 1** — the FB soft-launch/nav-relabel work takes priority; this stays
parked until after Sept 2 at the earliest.

**Scope — confirmed 2026-08-21, supersedes the webdesign-only plan below:** whole site, not just
the two lead-facing pages — Home/About/Experience/Skills/Blog/Casual/Music/Contact/Services/
Consulting/WebDesign, all of it. Host is **Cloudflare Pages**, matching haxbyte.com's stack, not a
same-domain GitHub Pages merge — that's a real infrastructure change (DNS nameservers move to
Cloudflare, same process SECURITY.md already describes for the CSP-headers item), not just a build
tool swap. Side benefit worth knowing about, not the reason to have decided this: moving to
Cloudflare also resolves the CSP-headers gap SECURITY.md flags as unreachable on plain GitHub
Pages.

**Analytics carries over almost for free (2026-08-21).** Confirmed while setting up GA4 tonight:
the Measurement ID, the custom-event code (`docs/PORTFOLIO_TODO.md` item 9), and every GA4 Admin
setting (Internal Traffic, Data Retention, Enhanced Measurement) are all framework-agnostic — none
of it is Blazor-specific. When the rewrite happens: paste the same `gtag.js` snippet into Astro's
base layout, paste the same plain-JS event handlers onto Astro's version of the contact dialog
buttons (they were written as plain `onclick="..."` specifically because that's what the existing
markup already uses, not Blazor `@onclick` syntax), and nothing else needs to change. **The one
real risk:** if Astro's page URLs differ from Blazor's current routes, that breaks page-level report
continuity across the migration — worth keeping URL structure identical for that reason alone, not
just SEO.

**Effort note:** bigger than the two-page version below, which was already "unscoped, likely
several sessions." A whole-site rewrite in a new framework on a new host is a materially larger
lift — worth scoping properly (even just a rough page/component inventory) before starting, rather
than discovering the size of it mid-migration.

**Original two-page plan, kept below for the deploy-mechanics detail (same-domain GitHub Pages
merge) even though the confirmed scope above has moved past it — those specifics (Astro build step
merged into `publish-gh-pages.yml`, static output winning over the Blazor SPA fallback) may still
be useful reference for how routing/deploy handoff works during a partial migration, if the
whole-site rewrite ends up happening in stages rather than one cutover:
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

**Superseded (2026-08-21):** the line that used to be here said "not now" for the rest of the
site — no longer accurate. Confirmed scope is now the whole site, see the top of this section.

---

## Fix: Deep-Link Routing on GitHub Pages
**Status:** ✅ Complete (2026-08-21)
**Branch:** `fix/spa-deep-link-routing`

**Why:** User reported broken screenshots on live case-study pages (e.g.
`/webdesign/dougrosenberg-music`). Root cause wasn't the images — it was that **every**
`/webdesign/{slug}` deep link (and any other parameterized route) 404'd on direct navigation
or refresh, rendering the app's own `<NotFound />` instead of the page.

**Root cause:** GitHub Pages has no server-side rewrites, so this site relies on the standard
`404.html` → `sessionStorage` → `index.html` SPA-fallback trick (`wwwroot/404.html` stores the
attempted path, redirects to `/`, and an inline script in `index.html` restores it via
`history.replaceState` before Blazor renders). But `index.html`'s
`<script src="_framework/blazor.webassembly.js">` had no `autostart="false"`, so Blazor
auto-started and ran its first route match racing against that restore script. Blazor locked in
"not found" for whatever path it booted with; the later `replaceState` call fixed the address
bar but nothing re-triggers the router off a raw `history.replaceState` (no `popstate` fires), so
the page stayed on `NotFound` even though the URL was correct. Confirmed live: address bar showed
the right path, GA's `page_view` fired with `dl` still `/`, and no `sample-data/webdesign.json`
request was ever made.

**Fix:** Added `autostart="false"` to the Blazor script tag and moved `Blazor.start()` to run
*after* the sessionStorage redirect-restore logic, so the router's first route match always sees
the corrected path. Verified locally: `/webdesign/hardware-etc` now renders the case-study page
(title, content, images) instead of 404. This was a site-wide bug, not specific to the webdesign
section — any parameterized route hit on direct load/refresh was affected.

## Fix: Broken Images on Every Nested Route (bad dynamic base href)
**Status:** ✅ Complete (2026-08-21)
**Branch:** `fix/spa-deep-link-routing` (same branch — found while re-verifying the fix above)

**Why:** After the deep-link fix above, user reported the case-study screenshots still looked
wrong live ("all the screenshots are cut off... having stunning pictures is essential"). Checked
the live site directly: it wasn't cropping, the images were failing to load entirely (broken-image
icon, `naturalWidth: 0`) on every page reached via a 2+-segment route.

**Root cause:** `index.html` had a leftover dynamic `<base>` href script (dated to the initial
commit, comment citing a "Blazor WASM base path problems" guide for apps hosted under a
**subpath** like `username.github.io/repo-name/`). It read `window.location.pathname`, and for
any path with more than 2 segments set `<base href="/" + path[1] + "/">` — treating `path[1]` as
a hosting subpath. But this site is hosted at the domain **root**
(`dougrosenbergdev.com/webdesign/hardware-etc`), so `path[1]` is actually the first *route*
segment ("webdesign"), not a subpath. Every relative asset on a nested route (all `images/...`
srcs) resolved against a wrong base like `/webdesign/`, producing 404s such as
`dougrosenbergdev.com/webdesign/images/webdesign/hardwareetc-hero.jpg`. The `localhost` branch of
the old script explicitly forced base href to `/`, which is why this was invisible in local dev
the whole time — it only ever broke the live site.

**Fix:** Replaced the entire dynamic script with a static `<base href="/" />`. This site is always
served from the domain root (GitHub Pages custom domain, confirmed in `CLAUDE.md`), so there's no
subpath to compute — a static root base is correct in every environment (local dev, GH Pages
custom domain). Verified locally via `img.src`/`naturalWidth` checks that images now resolve
correctly on a nested route.

---

## Hardware Etc: Before/After Case-Study Section
**Status:** ✅ Complete (2026-08-21)
**Branch:** `feature/webdesign-before-after`

**Why:** User asked for a before/after comparison for the two Squarespace client builds
(Hardware Etc, Sonus Construction) on `/webdesign`. Checked with the user first: Sonus was a
fresh build with no prior site, so it gets no before/after (no material to show, not a gap to
fill). Hardware Etc did have a real prior site, so it's the only one with a before/after.

### What was built
- Found a genuine "before" via the Wayback Machine: `hardwareetc.net` captured 2021-12-06 — a
  single unstyled placeholder page (default browser typography, no layout, no branding beyond a
  plain wordmark). Sonus's own Wayback history has no usable capture (one 200 response, but the
  Squarespace CSS/JS never got archived, so it renders blank) — confirms there's nothing to
  recover for Sonus even if we wanted one.
- Captured and cropped that Wayback snapshot into
  `wwwroot/images/webdesign/hardwareetc-before.jpg` (1200×242, ~28KB).
- Added `BeforeImage` / `BeforeCaption` / `BeforeSourceUrl` (optional) to `WebDesignProject`
  (`Models/WebDesignModel.cs`) and populated them only on the `hardware-etc` entry in
  `webdesign.json`. The caption links back to the actual Wayback URL for transparency.
- `WebDesignDetailPage.razor`: renders a before/after block (guarded on `BeforeImage` being set)
  between the hero shot and the stack/approach sections — before panel desaturated
  (`grayscale(0.35) contrast(0.92)`), after panel reuses `project.Images[0]`, both framed at a
  matching 220px height like the existing `.wd-case__shot` tiles. New `.wd-case__before-after`/
  `.wd-case__ba-*` rules in `app.css`, stacking to one column under 640px.
- Fixed a same-specificity CSS ordering bug during dev: `.wd-case__ba-shot--before img`'s
  `object-position: top left` was silently overridden because the generic `.wd-case__ba-shot img`
  rule appeared later in the file at equal specificity — moved the generic rule earlier so the
  `--before` override actually wins.

### Verification
- `dotnet build` and `dotnet test .` both pass (8/8 tests, same 2 pre-existing unrelated
  `Experience.razor` warnings).
- Live-tested in Chrome via `dotnet run`: `/webdesign/hardware-etc` shows the before/after block
  correctly (badges, desaturated "before" panel showing the real placeholder text, working
  "View archived snapshot" link); `/webdesign/sonus-construction` renders with no before/after
  block at all, confirming the guard works and nothing was fabricated for it.

### Also fixed this session (separate branch, merged into this one)
While investigating this, found and fixed a site-wide bug where every `/webdesign/{slug}` deep
link 404'd on direct navigation/refresh on the live GitHub Pages site — see the "Fix: Deep-Link
Routing on GitHub Pages" entry above (`fix/spa-deep-link-routing`, merged into this branch since
it's what made testing this feature live possible in the first place).

### Follow-up: fixed cropped screenshots + hover zoom (2026-08-21)
User flagged (with a screenshot) that the `.wd-case__shots` secondary-image grid was visibly
cropping into the site's own overlay text at both left and right edges. Root cause: the tiles
used a fixed `height: 220px` with `object-fit: cover`, which doesn't match the images' native
1522×784 ratio — cover-cropped ~60px off each side at that resolution, enough to clip words on
screenshots where the text runs close to the edges.

- Fixed by giving `.wd-case__shot img` `aspect-ratio: 1522 / 784` instead of a fixed pixel
  height, so the tile's box ratio matches the source images exactly — `object-fit: cover` no
  longer needs to crop anything at any grid column width.
- Added the requested subtle hover zoom: `transition: transform 0.4s ease-in-out` on the image,
  `transform: scale(1.06)` on `:hover` (same pattern applied to `.wd-case__hero-shot` at
  `scale(1.04)` and `.wd-case__ba-shot` at `scale(1.06)` for a consistent feel across all
  case-study imagery). Containers already had `overflow: hidden`, so the zoom clips cleanly
  inside the rounded corners.
- User also didn't like the pre-existing `.wd-case__shot:hover { transform: translateY(-4px) }`
  card-lift — removed it, keeping only the border-color hover change on the card and the new
  scale-zoom on the image itself, so hovering no longer shifts anything on the Y-axis.
- CSS-only change; skipped a full `dotnet build`/`dotnet test` this time since the user's own
  Visual Studio debug session (`localhost:5001`) held a lock on the shared build output —
  verified the change by reading the compiled rule back instead of rebuilding, and left live
  verification to the user's already-running session rather than competing for the lock.

### Follow-up 2: card polish — box-shadow over border, fill the caption area, zoom everywhere (2026-08-21)
User sent a second screenshot (Haxbyte case study, light mode) showing the real bug behind the
crop complaint's sibling issue: shorter captions in `.wd-case__shots` left a visibly blank strip
of the card's own (transparent) background below the figcaption — grid `align-items: stretch`
equalizes all cards in a row to the tallest, but the figcaption itself only sized to its own text,
so short captions didn't reach the bottom of the stretched card. User also asked, generally: every
photo should get the subtle zoom, and box-shadow is preferred over border for cards.

- **Caption fill:** `.wd-case__shot` is now `display: flex; flex-direction: column`, and its
  `figcaption` is `flex: 1 1 auto` — the caption's own background always stretches to the card's
  true bottom edge now, regardless of how the grid row height stretches. No more second,
  differently-colored blank surface.
- **Border → box-shadow:** removed the flat `border` from `.wd-case__hero-shot`,
  `.wd-case__shot`, `.wd-case__highlight-card`, `.wd-case__ba-shot`, and `.wd-project` (the
  `/webdesign` list-page cards); replaced with soft `box-shadow` (deepening further on hover
  instead of a border-color change). `.wd-case__hero-shot` already had its own large shadow, so
  that one just lost the redundant border.
- **Zoom everywhere:** added the same `transform: scale(...)` + `transition: transform 0.4s
  ease-in-out` hover treatment to `.wd-project__frame-stage` (the crossfading thumbnail on the
  `/webdesign` list page) — scaling the stage rather than the individual crossfading `<img>`s so
  it doesn't interact with their existing opacity keyframe animation.
- Also dropped the `.wd-project:hover` card lift (`transform: translateY(-1px)`) for the same
  reason as the earlier `.wd-case__shot` one — kept the interaction language consistent (shadow
  deepens, photo zooms, nothing shifts vertically) across both the list and detail pages.
- CSS-only again; same VS lock as above, verified by reading the rules back and checking brace
  balance rather than rebuilding.

## Homepage polish: real Friars logo, Experience detail card, divider hover, music link (2026-08-21)
Four small, unrelated requests from the same message, each scoped to a different component.

- **Real Franciscan Friars logo:** the Experience section's Friars ERP entry used
  `franciscan-friars-emblem.png` — visually close to the real friars.us badge but not actually
  sourced from it (likely an earlier AI-regenerated approximation; its odd 115×177 non-square
  canvas was a tell). The repo already had the real logo committed but unused
  (`franciscan-frairs-logo-2023.jpg`, the full lockup with wordmark, matching what's live on
  friars.us). Cropped just the circular badge out of that real file (avoiding the wordmark, which
  wouldn't read at the ~34–150px sizes this asset is shown at) into a new
  `franciscan-friars-badge.png`, pointed `experience.json`'s Friars entry at it, and deleted the
  old approximated emblem file (confirmed via grep it was the only reference).
- **Experience detail-panel card:** user said they like the existing side-nav list but wanted the
  right-hand detail panel to read as a card rather than floating text. `.experience-detail` now
  has a background, border-radius, and box-shadow (matching the site's established
  shadow-over-border card language), with a light-mode background override alongside
  `.wd-case__highlight-card`'s. Adjusted its mobile padding rule (was `padding: 0`) to keep a
  small inset so the card doesn't touch its own edges on small screens.
- **SectionDivider hover micro-interaction:** the 9-bar brass/teal "waveform" divider under
  section headings (About, Casual, Contact, Experience, Music, TechnicalSkills) only ever
  animated once, on scroll-reveal. Added a `:hover` state that re-"plays" it — bars alternate
  taller/shorter in a quick staggered ripple (0.3s, `--ease-swing`), like an equalizer reacting,
  distinct from the slower one-time entrance so it doesn't read as a replay.
- **Music album art:** the "Better Than TV" cover (`dougRosenbergBetterThanTv.jpg`) had a thick
  gray+white picture-frame mat baked directly into the image file (not CSS) — user wanted it
  gone. Cropped it out with ImageMagick (`-shave 82x82`), leaving just the actual cover art (the
  black top/bottom letterbox bars are part of the real cover design, not the mat, and were kept).
  Wrapped the image in a link to `https://www.dougrosenberg.com` (new `.music-photo__link`,
  `cursor: pointer`, `overflow: hidden` for the zoom to clip against), and added the same
  0.4s ease-in-out hover zoom (`scale(1.06)`) used everywhere else this session, alongside the
  existing grayscale-to-color hover effect.
- CSS/JSON/image-only aside from the one Music.razor markup change (wrapping the `<img>` in an
  `<a>`); skipped `dotnet build`/`test` again — Visual Studio's debug session (`localhost:5001`)
  still held the build lock — verified by reading files back and checking `app.css`'s brace count
  stayed balanced (850/850) instead.

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
