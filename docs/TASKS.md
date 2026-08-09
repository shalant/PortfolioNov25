# Current Sprint — Portfolio Upgrade

**Goal:** Transform portfolio into lead-generating machine with discoverable SEO + sparkly UI + clear web design positioning  
**Timeline:** 2-3 weeks (2-3 hrs/day)  
**Started:** 2026-08-09

---

## Phase 1: SEO Foundation (Week 1)
**Status:** ⏳ Not started  
**Effort:** 4-6 hours  
**ROI:** High (organic traffic)

SEO makes you discoverable. Google can't index you without meta tags, structured data, and sitemaps.

### Tasks
- [ ] Add `<meta name="description">` to `index.html`
- [ ] Add Open Graph tags (og:title, og:description, og:image, og:url)
- [ ] Add canonical tag to prevent duplicate indexing
- [ ] Create `robots.txt` (allow /; disallow admin paths)
- [ ] Create `sitemap.xml` (list all pages)
- [ ] Add Schema.org structured data (Person, Experience, Project)
- [ ] Improve link text ("visit site" → "Visit ArborKin deployment")
- [ ] Add `alt` text to all images
- [ ] Test with Google Search Console

**Branch:** `feature/seo-foundation`

---

## Phase 2: UI Sparkle Phase 1 (Week 1-2, parallel)
**Status:** ⏳ Not started  
**Effort:** 5-6 hours  
**ROI:** Medium (converts visitors)

Polish the UI with micro-interactions, gradients, and smooth transitions. Immediate visual impact.

### Tasks
- [ ] Add button hover effects (scale, color shift, glow)
- [ ] Add link hover animations (underline slides in from left)
- [ ] Add nav item highlights (active section indicator)
- [ ] Enhance experience carousel cards (lift on hover)
- [ ] Add gradient shadows to skill tags
- [ ] Add gradient overlays to hero section
- [ ] Animate skill tags on scroll (staggered reveal)
- [ ] Test responsiveness (mobile, tablet, desktop)

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
