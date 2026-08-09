# Current Sprint — Portfolio Upgrade

**Goal:** Transform portfolio into lead-generating machine with discoverable SEO + sparkly UI + clear web design positioning  
**Timeline:** 2-3 weeks (2-3 hrs/day)  
**Started:** 2026-08-09

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
