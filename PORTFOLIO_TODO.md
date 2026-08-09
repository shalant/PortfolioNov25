# dougrosenbergdev.com — Prioritized Todo List

**Date:** 2026-08-09  
**Goal:** Transform portfolio into a lead-generation machine; secure it; optimize for SEO

---

## 🚨 Critical Issues (This Week)

### 1. **Security Audit** 
- **Why:** You mentioned uncertainty about security; portfolio is public-facing and may collect contact info
- **Scope:**
  - [ ] HTTPS enabled? (check browser address bar)
  - [ ] CSP headers configured? (check DevTools → Network → Response Headers)
  - [ ] No secrets in code? (API keys, passwords, tokens)
  - [ ] Form submission safe? (input validation, rate limiting on contact form)
  - [ ] Azure App Service security settings reviewed? (CORS, allowed hosts, etc.)
  - [ ] Blazor bundle size / no embedded credentials?
  - [ ] Contact form email address not scraped (anti-spam)
- **Risk:** Portfolio compromised, visitor data leaked, spam
- **Effort:** 2-3 hours (audit + fixes)

### 2. **SEO Foundation** 🔥 High Impact
- **Why:** You're invisible to search engines; missing organic lead pipeline
- **Current Gap:** No meta tags, no sitemap, no structured data, poor link text
- **Scope:**
  - [ ] Add `<meta name="description">` to `index.html`
  - [ ] Add Open Graph tags (og:title, og:description, og:image, og:url)
  - [ ] Add canonical tag to prevent duplicate indexing
  - [ ] Create `robots.txt` → allow /; disallow admin paths (none, but good practice)
  - [ ] Create `sitemap.xml` (list /, /blog, case studies)
  - [ ] Add Schema.org structured data (Person, Experience, Project, BlogPosting)
  - [ ] Improve link text: "visit site ↗" → "Visit Friars ERP deployment ↗"
  - [ ] Add `alt` text to all images (hero photo, ArborKin screenshots)
- **Effort:** 4-6 hours
- **ROI:** High — organic traffic from Google, "full-stack .NET developer", "Blazor consultant"
- **Keyword targets:** "full-stack .NET developer", "Blazor consultant", "ERP systems", "Douglas Rosenberg"

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

### 7. **Services Page** 
- **Why:** "Services" nav link implies you offer services, but no clarity → confuses visitors
- **Scope:**
  - [ ] Create `/services` page or expand Hero CTA
  - [ ] Define 3-4 services:
    1. **ERP System Architecture** — design, data modeling, Azure deployment
    2. **Blazor Full-Stack Development** — from design to production
    3. **Consulting** — code review, performance optimization, mentoring
    4. **Contract / Retainer Work** — ongoing support, team augmentation
  - [ ] For each service: description, typical project scope, pricing/engagement model
  - [ ] Add CTA: "Schedule a free 30-min consultation → [Calendly link]"
  - [ ] Link from Homepage "About Me" section
- **Engagement Model:** Consider offering (pick 1-2):
  - [ ] Hourly rate ($X/hour)
  - [ ] Project-based (starting at $X)
  - [ ] Retainer ($X/month for 10 hours/week)
  - [ ] Equity / revenue share for early-stage startups
- **Effort:** 4-6 hours (page design, service definitions, pricing research)

### 8. **Calendly / Booking Integration** 
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

## ✨ Nice-to-Have / Phase 2 (Month 2+)

### 11. **Additional Case Studies** (Friars ERP, others)
- **Why:** One case study isn't enough; two projects = pattern, = credibility
- **Scope:** Add Friars ERP full breakdown (like ArborKin):
  - Problem solved (data synchronization, reporting)
  - Technical approach (Salesforce integration, CI/CD)
  - Hard problems + solutions
  - Screenshots or link to live site
  - Metrics: # of reports built, performance gains, user adoption
- **Effort:** 6-8 hours (write-up + screenshots + layout)

### 12. **GitHub Stats Optimization** 
- **Why:** Embedded GitHub Cards are slow; consider caching or static snapshot
- **Options:**
  - [ ] Remove GitHub stats entirely (you have code examples in case study)
  - [ ] Replace with link: "See my GitHub contributions →"
  - [ ] Cache GitHub card as static image (GitHub stats screenshot updated weekly)
- **Effort:** 1-2 hours
- **Priority:** Low; only if speed is an issue

### 13. **"Hire Me" / Availability Status** 
- **Why:** Clarify whether you're available for consulting (builds urgency)
- **Scope:**
  - [ ] Add availability badge to Hero or Header: "Available for Q3 2026 engagements"
  - [ ] Update as status changes (or remove if always available)
- **Effort:** 30 min

### 14. **Speaking Engagements / Awards** 
- **Why:** Social proof beyond testimonials
- **Scope:** If applicable, add section: "Speaking" or "Recognition"
  - Conference talks, podcasts, awards
  - Links to recordings or coverage
- **Effort:** 1-2 hours (if content exists)

### 15. **Dark Mode Refinement** 
- **Why:** Works now, but could be polished
- **Scope:**
  - [ ] Audit dark mode colors across all sections
  - [ ] Ensure sufficient contrast (WCAG AA minimum)
  - [ ] Test on real devices
- **Effort:** 1-2 hours

---

## 🔐 Security Hardening (Ongoing)

### 16. **Contact Form Security** 
- [ ] Validate email format server-side
- [ ] Rate limit (max 5 submissions per IP per hour)
- [ ] Add CAPTCHA (Cloudflare free tier) if spam becomes issue
- [ ] Log submissions to catch attacks
- [ ] Send confirmation email to visitor ("Thanks, I'll reply within 24 hours")

### 17. **Content Security Policy (CSP)** 
- [ ] Add CSP headers to prevent XSS
- [ ] Restrict script sources (self only)
- [ ] Restrict image sources (self + googleapis for fonts)

### 18. **Azure App Service Hardening**
- [ ] Enable HTTPS only (no HTTP redirect)
- [ ] Set `Strict-Transport-Security` header
- [ ] Check `AllowedHosts` in `appsettings.json`
- [ ] Review CORS settings (should be restrictive)

---

## 📋 Recommended Priority (Next Month)

### **Week 1-2 (Do Now)**
1. **#1** — Security audit (peace of mind)
2. **#2** — SEO foundation (meta tags, structured data, sitemap)

### **Week 3-4 (Lead Gen Setup)**
3. **#3** — Blog section + 3 launch posts (organic traffic pipeline)
4. **#4** — Blog preview on homepage (extends session)
5. **#5** — Newsletter signup (email capture)
6. **#6** — Testimonials (social proof)

### **Week 4-5 (Conversion)**
7. **#7** — Services page (clarity)
8. **#8** — Calendly integration (frictionless booking)

### **Week 5-6 (Measure)**
9. **#9** — Analytics (understand what's working)
10. **#10** — Core Web Vitals (speed optimization)

### **Month 2+ (Polish & Scale)**
- #11 (More case studies)
- #12 (GitHub stats optimization)
- #13-18 (Security, refinements, nice-to-haves)

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

**Last Updated:** 2026-08-09  
**Maintained by:** Douglas Rosenberg
