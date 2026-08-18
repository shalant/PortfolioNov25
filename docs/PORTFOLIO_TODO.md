# dougrosenbergdev.com — Prioritized Todo List

**Date:** 2026-08-09  
**Goal:** Transform portfolio into a lead-generation machine; secure it; optimize for SEO

---

## 🚨 Critical Issues (This Week)

### 1. **Security Audit** — ✅ Complete (2026-08-10)
- **Why:** You mentioned uncertainty about security; portfolio is public-facing and may collect contact info
- **Scope:**
  - [x] HTTPS enabled? — yes, GitHub Pages enforces it automatically
  - [x] CSP headers configured? — no, and **not achievable on plain GitHub Pages** (no server config surface exists). Would require a Cloudflare (or similar) reverse proxy in front of the domain. Documented as a deliberate "someday" item in SECURITY.md, not a quick fix.
  - [x] No secrets in code? — confirmed via repo-wide scan (API key patterns, private key blocks): none found
  - [x] Form submission safe? — moot; the "contact form" is actually a `mailto:` link, no submission endpoint exists to attack
  - [x] ~~Azure App Service security settings reviewed?~~ — **there is no Azure.** This item and `docs/SECURITY.md`'s prior Azure-based recommendations were describing infrastructure that doesn't exist; rewrote the doc to match actual GitHub Pages + local-only BlogPost-Generator reality.
  - [x] Blazor bundle size / no embedded credentials? — confirmed no credentials embedded; bundle size not yet measured (see Core Web Vitals item, #10)
  - [x] Contact form email address not scraped — N/A, same as above (mailto link, not a scraped/exposed form)
- **Also fixed:** `.gitignore` had no `.env`/`appsettings.*.json` exclusion despite SECURITY.md recommending `.env` usage — hardened both root and verified it covers `BlogPost-Generator/`. Removed three orphaned/dead components (`Portfolio.razor`, `Consulting2.razor`, and the unfinished `ContactDialog.razor` stub it referenced) found during the audit — unused surface area, not a security hole, but confusing dead code.
- **Risk:** Portfolio compromised, visitor data leaked, spam
- **Effort:** ~1.5 hours (audit + fixes)

### 2. **SEO Foundation** 🔥 High Impact — ✅ Complete (2026-08-09)
- **Why:** You're invisible to search engines; missing organic lead pipeline
- **Scope:**
  - [x] Add `<meta name="description">` to `index.html`
  - [x] Add Open Graph tags (og:title, og:description, og:image, og:url)
  - [x] Add canonical tag to prevent duplicate indexing
  - [x] Create `robots.txt` → allow /; disallow admin paths
  - [x] Create `sitemap.xml` (homepage sections + blog)
  - [x] Add Schema.org structured data (Person, WebSite, SoftwareApplication)
  - [x] Improve link text: "visit site ↗" → "Visit Friars ERP ↗" (etc., per company)
  - [x] Add `alt` text to all images (audited full live component tree)
  - [x] Google Search Console: domain verified (DNS TXT), sitemap submitted successfully
- **Fixed along the way:** canonical/OG/sitemap URLs were all pointing at `www.dougrosenbergdev.com`, which 301-redirects to the apex domain — this mismatch was blocking sitemap submission. Corrected everywhere to use the apex domain.
- **Keyword targets:** "full-stack .NET developer", "Blazor consultant", "ERP systems", "Douglas Rosenberg"
- **Remaining/known limitation:** this is a client-rendered Blazor WASM SPA, so per-page `<PageTitle>`/meta on `/consulting`, `/webdesign`, `/blog` only helps crawlers that execute JS — non-JS crawlers only ever see the root `index.html` meta tags. Not blocking, but worth knowing.

---

## 🎯 Lead Generation Phase (Weeks 2-3)

### 3. **Blog Section: Setup + First 3 Posts**
- **Why:** Blog = organic traffic + SEO + credibility + email capture
- **Current:** Nav links to `/blog` but no content (broken UX)
- **Scope:**
  - [ ] Create `/blog` route in `Index.razor` (or separate `Blog.razor` page)
  - [ ] Blog post list layout (date, title, excerpt, read-time, tags)
  - [ ] Blog post detail page (`/blog/:slug`)
  - [ ] Add to `wwwroot/sample-data/blog-posts.json` format (similar to existing)
  - [ ] Write 3 launch posts:
    1. **"Building a Family Tree App with Blazor Server"** — technical deep-dive on ArborKin, layout engine, drag state
    2. **"Full-Stack .NET: From ERP Design to Deployment"** — Friars case study, lessons learned
    3. **"Blazor Performance: Why I Chose Server Over WebAssembly"** — comparison, trade-offs (positions you as expert)
- **Content Strategy:**
  - Target keywords: "Blazor tutorial", "ERP system design", ".NET best practices"
  - Publish 1x per month minimum for SEO momentum
  - Each post includes CTA: "Need help with your ERP? [Contact me →]"
- **Effort:** 12-16 hours (layout + 3 posts × 3-4 hours each)
- **ROI:** Organic search traffic, email capture, thought leadership

### 4. **"Blog Preview" Section on Homepage**
- **Why:** Drives visitors from homepage to blog (longer session time, more SEO signals)
- **Scope:**
  - [ ] Add new component `LatestBlogPosts.razor` (3-post grid with excerpt + read-more link)
  - [ ] Insert between Skills and Contact sections
  - [ ] Add CTA: "Read more articles → [Link to /blog]"
- **Effort:** 2-3 hours
- **Placement:** After Skills, before Contact → keeps visitor engaged, extends session

### 5. **Newsletter / Email Signup** 
- **Why:** Build repeatable lead pipeline (email list for follow-up, offers, announcements)
- **Current:** No email capture mechanism
- **Scope:**
  - [ ] Add email signup form in Contact section or as separate "Get My Blazor Tips" popup
  - [ ] Connect to email service (Mailchimp free tier, ConvertKit, etc.)
  - [ ] Create simple freebie: "10 Blazor Performance Tips" (PDF or email series)
  - [ ] Add signup CTA in blog posts (end-of-post)
- **Effort:** 4-6 hours (form UI, Mailchimp integration, PDF generation)
- **ROI:** Email list for retargeting, upsells, consulting offers

### 6. **Testimonials / Social Proof Section**
- **Why:** Increases conversion rate (60%+ higher CTR with testimonials)
- **Current:** None
- **Scope:**
  - [ ] Reach out to 2-3 past colleagues/clients for short testimonial (quote + name/title + photo)
  - [ ] Create Testimonials component (grid of 3-4 cards with quote, author, role)
  - [ ] Insert after Case Study or before Contact
  - [ ] If no current clients, ask GitHub collaborators, teammates, mentors
- **Fallback:** Start with 2, add more as you get them
- **Effort:** 2-3 hours (component + outreach)
- **ROI:** Conversions increase 30-50% with social proof

---

## 💼 Services & Positioning (Week 3-4)

### 7. **Services Page** — ✅ Complete (2026-08-15)
- **Why:** "Services" nav link implies you offer services, but no clarity → confuses visitors
- **Scope:**
  - [x] Created `/services` page (`ServicesPage.razor` + `ServicesModel.cs` + `services.json`),
        mirrors `/consulting`'s `subpage-hero` pattern. Nav link added (briefcase icon, between
        blog and consulting)
  - [x] Defined 4 services (ended up different from the original sketch below — these map to
        actual current expertise rather than a generic ERP-architect framing):
    1. **Custom Web & App Development** — Blazor/Angular/Astro, design-led via Figma
    2. **ERP & Business Systems Consulting** — integrations, data pipelines, reporting
    3. **AI-Assisted Development & Modernization** — legacy modernization, AI-assisted workflows
    4. **Ongoing Support & Maintenance** — retainer-based
  - [x] Each service has a description, an "includes" list, an engagement model, and a price range
  - [x] CTA is a `mailto:` link ("Get a Free Quote") — no Calendly yet, see #8
  - [ ] Not yet linked from Homepage "About Me" section
- **Engagement Model (shipped):**
  - Custom Web & App Dev: project-based, **$750–$3,000/project**
  - ERP & Business Systems: hourly/retainer, **$125–$175/hr**
  - AI-Assisted Dev: project or ongoing, **$100–$150/hr**
  - Ongoing Support: monthly retainer, **$300–$800/month**
- **Pricing philosophy (worth remembering, not just this pass):**
  - Chicago market — comfortably above national average, not SF/NYC-tier. The numbers above
    lean toward the lower-middle of a Chicago-appropriate range; skew toward the top of each
    band (e.g. ERP $150–225/hr) if leaning into "solid income," not "getting launched."
  - **The core risk of starting cheap: pricing anchors.** A client who hires you at a low rate
    will resist a higher one later — "starting cheap" doesn't ease you into higher rates, it
    usually means eventually replacing early clients rather than raising their rate. Low pricing
    also does adverse selection: it disproportionately attracts price-sensitive, high-maintenance
    clients optimizing for cost over quality.
  - **This logic applies unevenly across the four services.** "Pay your dues" pricing makes
    sense for Custom Web & App Dev, where there's genuinely no portfolio yet and a client is
    taking a bet. It does **not** apply to ERP & Business Systems Consulting — that's existing
    day-job expertise, not something that needs to be proven from zero. Underpricing that tier
    doesn't buy credibility that's already there; it just leaves money on the table.
  - **Middle path if "paying dues" still feels right:** keep the listed rate where it should be,
    and discount specific early engagements individually (in exchange for a testimonial or
    case-study rights) rather than lowering the public number itself. Keeps the rate card intact
    while still getting the first few reference projects.
- **Effort:** ~2.5 hours across two sessions (page + icons + trim + pricing)

### 8. **Calendly / Booking Integration** — ⏸️ On hold (2026-08-15: no Calendly account yet)
- **Why:** Make it frictionless for leads to schedule a call
- **Scope:**
  - [ ] Sign up for Calendly free tier (1 calendar, unlimited bookings)
  - [ ] Create 30-min "Free Discovery Call" availability (Thu/Fri, 2-4 PM EST)
  - [ ] Embed Calendly widget on Services page + Contact section
  - [ ] Auto-send email confirmation with Zoom link
- **Effort:** 1-2 hours
- **ROI:** Eliminates back-and-forth email chains; converts warm leads faster

---

## 📊 Metrics & Analytics (Week 4)

### 9. **Analytics Integration** 
- **Why:** Can't optimize what you don't measure; need to track traffic, CTA clicks, lead source
- **Scope:**
  - [ ] Set up Google Analytics 4 (GA4) — free tier
  - [ ] Add tracking code to `index.html` (single `<script>` tag)
  - [ ] Define key events: CTA clicks, blog reads, email signup, form submissions
  - [ ] Set up dashboard: traffic source, top pages, conversion funnel
  - [ ] Weekly review: which CTAs convert best? Which pages drive leads?
- **Effort:** 2-3 hours (GA4 setup + goals)
- **ROI:** Data-driven optimization; understand what works

### 10. **Core Web Vitals Check** 
- **Why:** Google ranks fast sites higher; Blazor WASM apps are notoriously slow to load
- **Scope:**
  - [ ] Run PageSpeed Insights on homepage
  - [ ] Check: Largest Contentful Paint (LCP), First Input Delay (FID), Cumulative Layout Shift (CLS)
  - [ ] If slow (>3s): consider optimizations
    - Lazy load GitHub stats card
    - Compress hero image
    - Code splitting on Blazor bundle
  - [ ] Retest after optimizations
- **Effort:** 2-3 hours
- **Nice to have:** Not blocking, but impacts SEO ranking

---

## 🎨 Design Polish — "First-Class" UI (Weeks 2-4)

**Why:** Current design is 7.2/10 (good). Small polish touches push it to 8.7/10 (first-class). Visual polish directly impacts conversions.

### 11. **Micro-Interactions & Hover States** ⭐ Quick Wins
- **Why:** Buttons, links, cards with no hover effects feel static and less premium
- **Current:** No visible hover states; no animations
- **Scope:**
  - [ ] **Button hover effects:** Scale up 5%, background color shifts, subtle box-shadow appears
  - [ ] **Link hover:** Underline animates in from left (smooth 300ms transition)
  - [ ] **Nav items:** Highlight active section, underline on hover
  - [ ] **Experience carousel cards:** Lift on hover (translateY -5px + shadow)
  - [ ] **Skills tags:** Background color shift, slight scale on hover
  - [ ] **Case study screenshots:** Slight zoom + shadow on hover
- **Effort:** 2-3 hours (pure CSS transitions)
- **ROI:** High — visitors perceive site as "more premium" immediately
- **Files to edit:** `app.css`

### 12. **Scroll Animations** (Fade-in + Slide-up)
- **Why:** Content appearing instantly feels static; animations make site feel "alive"
- **Scope:**
  - [ ] Sections fade in + slide up (50px) as user scrolls into view
  - [ ] Staggered animation on skill tags (each appears 50ms apart)
  - [ ] Hero headings fade in on page load
  - [ ] Experience carousel items animate in on scroll
- **Effort:** 3-4 hours (Intersection Observer API + CSS keyframes)
- **Files to edit:** `app.css` + potentially `Index.razor` (add observer logic)

### 13. **Skills Section Visual Redesign**
- **Why:** Current skills are plain text tags; redesign to show proficiency
- **Current:** Text lists (Languages, Frameworks, Cloud, Tooling)
- **Redesign options (pick one):**
  1. **Skill Cards:** Icon + skill name + proficiency bar (0-100%) below each
  2. **Tag Clouds:** Larger tags = more proficient
  3. **Skill Matrix:** Grid showing expertise (Expert/Intermediate/Learning) × skill type
  4. **Radar Chart:** Shows expertise across domains (Languages, Frameworks, Infrastructure)
- **Scope:**
  - [ ] Add proficiency data to skills (`expertise: 95` etc.)
  - [ ] Design skill card component (icon + label + bar)
  - [ ] Animate proficiency bars (count from 0 → target on scroll into view)
  - [ ] OR use chart library (Chart.js or D3.js) for radar chart
- **Effort:** 3-4 hours (component design + data structure)
- **Files:** New component `SkillCard.razor` or update `Skills.razor`

### 14. **ArborKin Case Study Enhancement** 🔴 High Impact
- **Why:** Case study is your best selling point; deserves premium treatment
- **Current:** Good content + screenshots; could be more immersive
- **Scope:**
  - [ ] **Metrics cards:** Big numbers in colored boxes (8k LOC, 75 tests, 150ms render, 6 users)
  - [ ] **Animated callouts:** Arrows pointing to "hard problems solved" with hover descriptions
  - [ ] **Before/After slider:** Drag to compare old approach vs. new (e.g., slow upload → fast upload)
  - [ ] **OR embedded demo:** Read-only embed of ArborKin live (show login screen + sample tree)
  - [ ] **OR video walkthrough:** 60-second loom video showing tree interaction
- **Effort:** 6-8 hours (metrics design + slider component OR demo embed setup)
- **ROI:** Very high — case study drives conversions

### 15. **Experience Section Timeline Redesign**
- **Why:** Current carousel is good but text-heavy; timeline is more visually engaging
- **Redesign:**
  - [ ] Replace carousel with **vertical timeline** (vertical line with dots for each job)
  - [ ] Clicking dot expands job details (smooth collapse/expand)
  - [ ] Add company **logo/icon** next to job title
  - [ ] Highlight current role (or most recent)
  - [ ] **Alternative:** Tabs with company logos as tab headers
- **Effort:** 4-6 hours (redesign component + add company logos/icons)
- **Files:** Update `Experience.razor`

### 16. **Visual Differentiation Between Sections**
- **Why:** All sections feel equal weight; need rhythm to hold attention
- **Scope (pick 1-2):**
  - [ ] **Alternate backgrounds:** Odd sections light, even sections dark (or vice versa)
  - [ ] **Section colors:** Each section has subtle background color (hero = navy, about = teal, experience = navy, etc.)
  - [ ] **Section icons:** Unique icon/illustration for each section header (code icon for experience, briefcase for services, etc.)
  - [ ] **Divider animations:** Smooth SVG divider between sections (wavy line, gradient line with animation)
- **Effort:** 2-3 hours (CSS backgrounds + maybe 5 simple icon graphics)

### 17. **Enhance Saxophone / Personal Story Section**
- **Why:** This is your unique differentiator; leans into it more
- **Current:** Good story text + some details; could be more visual
- **Redesign:**
  - [ ] **Custom illustration:** Saxophone + code symbols, or sheet music with binary
  - [ ] **Timeline graphic:** Decade-by-decade journey (jazz musician 2010-2018 → software engineer 2019-2026)
  - [ ] **Album covers:** Display actual album art (Better Than TV, Underwater) with links
  - [ ] **Parallax background:** Sheet music or subtle musical notation scrolls with content
  - [ ] **Quote styling:** Chicago Tribune quote in a styled callout (larger font, italics, border)
- **Effort:** 4-8 hours (create custom graphics, refactor layout)
- **ROI:** High — memorable uniqueness; stand out from other portfolios

### 18. **Button & Link Polish**
- **Why:** CTAs are critical for conversions; deserve premium treatment
- **Scope:**
  - [ ] **Primary CTA (VIEW WORK):** Larger, brighter teal, icon indicator (→ arrow), hover glow
  - [ ] **Secondary CTA (ABOUT ME):** Outlined style, inverse colors on hover
  - [ ] **All link arrows:** Animate on hover (arrow moves right 3px, smooth transition)
  - [ ] **Contact CTA footer:** Larger, centered, with icon + breathing room around it
  - [ ] **Add gradient overlays:** Subtle radial gradient on buttons (creates depth)
- **Effort:** 2-3 hours (CSS + maybe slight HTML restructure)

### 19. **Gradient Overlays & Depth Effects**
- **Why:** Flat design is clean but feels minimal; gradients/shadows add sophistication
- **Scope:**
  - [ ] **Hero image overlay:** Dark gradient overlay (improves text readability, looks premium)
  - [ ] **Button shadows:** Drop shadows (0 2px 8px rgba...), intensify on hover
  - [ ] **Card shadows:** Subtle shadows on all cards (screenshots, skill cards, testimonials)
  - [ ] **Active nav indicator:** Glowing underline or dot under current section
- **Effort:** 1-2 hours (pure CSS)

### 20. **Hero Section Enhancement**
- **Why:** Hero is first impression; every pixel matters
- **Scope:**
  - [ ] Ensure photo has subtle vignette (darker edges)
  - [ ] Text has good contrast (possibly add dark gradient overlay)
  - [ ] Headline scales responsively (looks good on mobile)
  - [ ] Subheading has subtle animation (fade in on load)
  - [ ] CTAs are prominent, with clear hover states
- **Effort:** 2-3 hours

---

### Design Phase Summary

| Phase | Items | Effort | Expected Score |
|-------|-------|--------|---|
| **Phase 1 (Quick Wins)** | #11, #18, #19 | 5-6 hours | 7.2 → 7.8/10 |
| **Phase 2 (Medium)** | #12, #13, #14, #15 | 15-18 hours | 7.8 → 8.3/10 |
| **Phase 3 (Polish)** | #16, #17, #20 | 8-12 hours | 8.3 → 8.7/10 |

---

## ✨ Nice-to-Have / Phase 3 (Month 2+)

### 21. **Additional Case Studies** (Friars ERP, others)
- **Why:** One case study isn't enough; two projects = pattern, = credibility
- **Scope:** Add Friars ERP full breakdown (like ArborKin):
  - Problem solved (data synchronization, reporting)
  - Technical approach (Salesforce integration, CI/CD)
  - Hard problems + solutions
  - Screenshots or link to live site
  - Metrics: # of reports built, performance gains, user adoption
- **Effort:** 6-8 hours (write-up + screenshots + layout)

### 22. ~~GitHub Stats Optimization~~ — ✅ Resolved (2026-08-16, via removal)
- **Why:** Embedded GitHub Cards (`github-readme-stats.vercel.app`) were unreliable — the shared
  public instance was intermittently 503ing (surfaced during Phase 3C, see `TASKS.md`), a known
  risk with that service.
- **Resolution:** Removed the section entirely from `TechnicalSkills.razor` (both stat/top-langs
  `iframe`s, the "view on GitHub" fallback link, and the `.skills-github*` CSS in `app.css`) rather
  than continuing to patch around third-party flakiness. No replacement added.

### 23. **"Hire Me" / Availability Status** 
- **Why:** Clarify whether you're available for consulting (builds urgency)
- **Scope:**
  - [ ] Add availability badge to Hero or Header: "Available for Q3 2026 engagements"
  - [ ] Update as status changes (or remove if always available)
- **Effort:** 30 min

### 24. **Speaking Engagements / Awards** 
- **Why:** Social proof beyond testimonials
- **Scope:** If applicable, add section: "Speaking" or "Recognition"
  - Conference talks, podcasts, awards
  - Links to recordings or coverage
- **Effort:** 1-2 hours (if content exists)

### 25. **Dark Mode Refinement** 
- **Why:** Works now, but could be polished
- **Scope:**
  - [ ] Audit dark mode colors across all sections
  - [ ] Ensure sufficient contrast (WCAG AA minimum)
  - [ ] Test on real devices
- **Effort:** 1-2 hours

---

## 🔐 Security Hardening (Ongoing)

> **Reality check (2026-08-10):** items #26-28 below were written assuming a real backend contact form and Azure App Service hosting. Neither exists — the "contact form" is a `mailto:` link (no submission endpoint to secure), and hosting is 100% GitHub Pages (no App Service, no Azure account at all). Rewritten to reflect what's actually applicable. Full detail in [`SECURITY.md`](./SECURITY.md).

### 26. **Real Contact Form** (if you decide you want one)
Currently N/A — there's nothing to secure because there's no submission endpoint. Only relevant if you replace the `mailto:` link with an actual form (e.g. a form service, or a small serverless function):
- [ ] Validate email format server-side
- [ ] Rate limit (max 5 submissions per IP per hour)
- [ ] Add CAPTCHA (Cloudflare free tier) if spam becomes issue
- [ ] Log submissions to catch attacks
- [ ] Send confirmation email to visitor ("Thanks, I'll reply within 24 hours")

### 27. **Content Security Policy (CSP)** — deferred, infrastructure decision
Not achievable via GitHub Pages configuration (no server-side headers support). Requires a reverse proxy in front of the domain:
- [ ] Decide whether it's worth moving DNS to Cloudflare (free tier) to gain header-injection capability
- [ ] If yes: add CSP via Cloudflare Transform Rules (starter policy already drafted in `SECURITY.md`)
- [ ] Restrict script sources (self + CDN/fonts actually in use)
- [ ] Restrict image sources (self + googleapis for fonts)

### 28. ~~Azure App Service Hardening~~ — N/A, no Azure
There is no Azure App Service. HTTPS is already enforced automatically by GitHub Pages. `AllowedHosts`/CORS settings apply to `BlogPost-Generator` (local-only ASP.NET Core app), not the live portfolio — review those in `BlogPost-Generator/appsettings.json` only if that tool is ever exposed beyond localhost, which it currently is not and should not be.

---

## 🌗 Theming

### 29. **Light Mode: Planning & Implementation**
- **Why:** Site currently ships dark-only (navy background); no light theme exists at all. Note #25 ("Dark Mode Refinement") assumes a toggle already exists — it doesn't; today there is exactly one theme.
- **Current state (confirmed via direct audit, 2026-08-16):** No real CSS-variable theming layer —
  the `:root` block in `app.css` declares `--navy`/`--teal`/etc. but they're **never referenced**
  anywhere (`var(--` has zero hits in the file). `app.css` (4,793 lines) has 152 hardcoded hex
  colors + 292 `rgba()` calls. 9 `.razor` files have their own embedded `<style>` blocks with
  colors, but most are orphaned/unrouted variant pages (`Consulting3/5/6.razor`,
  `DougCartoon2.razor`, `DougSvg.razor`) — only `NotFound.razor`, `BlogPosts.razor`, and
  `BlogArchive.razor` are live and actually in scope. The Bauhaus/art-deco background
  ([[design-bauhaus-background]]) is a raster WebP image, not CSS shapes, referenced ~15 times
  across `app.css` via the same two hardcoded URLs.
- **Decisions made (2026-08-16, via planning session):**
  1. **Palette:** warm cream/parchment background (e.g. `~#f5f1e8`), not cool gray — fits the
     art-deco period feel. Navy (`#2c3e50`, already an existing-but-unused token) becomes primary
     text. Teal (`#1abc9c`) stays the accent for fills/borders/icons in both modes; use
     `--teal-dark` (`#16a085`) for accent *text* specifically, to hold WCAG AA on a light
     background.
  2. **Art-deco texture:** regenerate recolored WebP variants (same geometry, navy-on-cream)
     rather than a CSS filter hack — keeps the motif fully intact per the "extend, don't replace"
     preference, at the cost of image-editing effort.
  3. **Toggle:** OS `prefers-color-scheme` as the default, with a manual icon-only toggle in
     `Header.razor` (near `.nav-cta`, sized like the existing 44×44px `.nav-toggle`) that
     overrides and persists to `localStorage`. Theme attribute must be set via an inline blocking
     `<script>` in `index.html`'s `<head>` (before Blazor boots) to avoid a flash of the wrong
     theme on load.
- **Architecture:** Build a real `[data-theme="light"]` CSS custom-property layer (`--bg`,
  `--bg-hero-gradient`, `--surface-glass`, `--text`, `--text-muted`, `--border-accent`,
  `--art-deco-1`, `--art-deco-2`, etc.), replacing the currently-decorative `:root` block.
  Centralizing the two art-deco URLs as variables means the ~15 call sites only need to
  reference `var(--art-deco-1/2)` once each instead of each needing its own override.
- **Phasing:**
  - **Phase A:** token system + `theme.js` (new file, same pattern as `nav.js`/`scrollReveal.js`)
    + header toggle + re-theme the homepage only (everything `Index.razor` composes: Header,
    Home/hero, About, Experience, TechnicalSkills, Casual, Music, Contact, Footer).
  - **Phase B (future):** extend to `/webdesign`, `/webdesign/{slug}`, `/services`, `/consulting`,
    `/blog`, `/blog/archive`.
  - **Explicitly out of scope:** orphaned variant pages (`Consulting3/4/5/6.razor`,
    `WebDesignPageOLD.razor`, `/archive` legacy embed) — not worth theming while unrouted.
- **Effort:** Phase A is a real implementation pass (new asset generation + CSS token refactor +
  JS module + Header change), not a quick add-on. Full plan: `feature/light-mode-phase-a` branch.

---

## 📋 Recommended Priority (Next Month)

### **Week 1-2 (Do Now)**
1. **#1** — Security audit (peace of mind)
2. **#2** — SEO foundation (meta tags, structured data, sitemap)

### **Week 2-3 (Design Polish Quick Wins)** ⭐ New Priority
3. **#11** — Micro-interactions & hover states (buttons, links scale/glow on hover)
4. **#18** — Button & link polish (CTAs shine, arrows animate)
5. **#19** — Gradient overlays & depth effects (shadows, glows)

### **Week 3-4 (Lead Gen Setup)**
6. **#3** — Blog section + 3 launch posts (organic traffic pipeline)
7. **#4** — Blog preview on homepage (extends session)
8. **#5** — Newsletter signup (email capture)
9. **#6** — Testimonials (social proof)

### **Week 4-5 (Design Medium Lift)**
10. **#12** — Scroll animations (sections fade in on scroll)
11. **#13** — Skills section redesign (proficiency bars, cards, or radar chart)
12. **#14** — ArborKin case study enhancement (metrics cards, animated callouts, before/after slider)

### **Week 5-6 (Conversion + Measure)**
13. **#7** — Services page (clarity)
14. **#8** — Calendly integration (frictionless booking)
15. **#9** — Analytics (understand what's working)
16. **#10** — Core Web Vitals (speed optimization)

### **Week 6-7 (Design Polish)**
17. **#15** — Experience timeline redesign (vertical timeline vs carousel)
18. **#16** — Section visual differentiation (alternating backgrounds, icons, dividers)
19. **#17** — Saxophone section enhancement (custom illustration, timeline, album art)
20. **#20** — Hero section polish (gradient overlay, responsive scaling, animations)

### **Month 2+ (Scale & Refinements)**
- #21 (More case studies: Friars ERP)
- #23-25 (availability badge, speaking, dark mode refinement) — #22 (GitHub stats) resolved via removal
- #26-28 (Security hardening: form validation, CSP, Azure config)
- #29 (Light mode: planning & implementation)

---

## 🎯 Expected Outcomes

### After Phase 1 (Week 2)
- ✅ Website secure + discoverable
- ✅ Appears in Google search results (6-8 weeks for indexing)
- Expected: 0-5 organic visitors/month (ramping up)

### After Phase 2 (Week 4)
- ✅ 3 blog posts published (organic traffic boosters)
- ✅ Email list started (even if just 10 subscribers)
- ✅ Services + booking clarity
- Expected: 10-30 organic visitors/month, 2-3 consultation bookings

### After Phase 3 (Month 2)
- ✅ Testimonials + case studies (social proof)
- ✅ Analytics dashboards (data-driven)
- ✅ Optimized for speed + accessibility
- Expected: 50-100 organic visitors/month, 5-10 qualified leads/month

---

## Lead Generation Strategy

### The Flywheel:
1. **Attract** — SEO (blog posts + structured data) + referrals
2. **Engage** — case study depth + testimonials + email signup
3. **Convert** — clear services + frictionless booking
4. **Retain** — email follow-up, newsletter, retainer offers

### Your Unique Angle:
- "Full-stack .NET engineer who **understands ERP systems** + **designs for UX**"
- Not a freelancer-for-hire; position as **specialized consultant**
- Target: mid-market nonprofits, startups, enterprises upgrading ERP systems
- Pricing: Higher hourly rate or project-based (not competing on commodity rates)

### Quick Wins:
- [ ] LinkedIn post about Blazor performance (link to blog post)
- [ ] GitHub README on FamilyTree repo (link to portfolio)
- [ ] Answer Stack Overflow questions tagged "blazor" (link in profile)
- [ ] Reach out to former teammates: "Hey, I'm doing consulting now; let me know if your team needs help"

---

## Reference Docs

- `PORTFOLIO.md` — Architecture + current state
- `CLAUDE.md` — Development guidelines
- Blog post templates (to be created)
- Analytics dashboard template (to be created)

---

**Next Action:** Pick #1 and #2 above, start this week. You'll have SEO + security sorted within 1-2 weeks, then the lead pipeline starts flowing.

**Questions?** Review the PORTFOLIO.md section-by-section, check links in Header/Services, verify contact form works.

---

**Last Updated:** 2026-08-16  
**Maintained by:** Douglas Rosenberg
