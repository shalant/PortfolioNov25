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

**Future post ideas (backlog, not part of the 3 launch posts above):**
- **"Angular → Blazor" before/after.** `/archive` and `/previous` (`PreviousPortfolio.razor`) already
  embed the old Angular-based site live via iframe (`/archive/dist/portfolio/index.html`) — it was
  built as a quick hack to keep the old site viewable, but it's already the exact asset a
  before/after post needs: a live, clickable comparison, not just screenshots. Route is live but
  unlinked from nav (intentionally, per the nav IA discussion — 2026-08-21).
- **The stack-decision story: Angular → Blazor WebAssembly → MudBlazor → dropping MudBlazor →
  (on haxbyte.com specifically) dropping Blazor WASM for Astro.** Doug's read: this is genuinely
  interesting content — a real decision trail with reasons at each step, not a tutorial. Positions
  well for the recruiter/engineer audience (haxbyte.com per the career-development repo's brand
  split) since it's about *judgment* (when to use a framework vs. when to leave it) rather than
  "how to." Pairs naturally with the before/after post above — the Angular→Blazor leg of the story
  has a live artifact to point to.

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

### 9. **Analytics Integration** — 🔥 relevant now (2026-08-21: Sept 2 FB launch approaching)
- **Why:** Can't optimize what you don't measure; need to track traffic, CTA clicks, lead source.
  Newly relevant: currently fixing GA4 + Meta Pixel tracking at the day job (with a runbook already
  written there), so doing this deliberately here doubles as a clean reference example — the same
  taxonomy discipline that fixes a messy account applies directly back to that work. Also directly
  answers the mobile-vs-desktop question from the nav/hero discussion (2026-08-21) with real data
  instead of inference, once the Sept 2 FB post starts sending traffic.

- **Analytics goals — what this is actually trying to answer (2026-08-21).** Written to stand alone
  without needing to remember the conversation that produced it. Going through the candidate
  questions honestly, not all of them cost the same:

  - **Answered for free, no extra setup:** device split (mobile vs. desktop — GA4's Tech reports)
    and page-level traffic (which pages got visited, engagement time — GA4's Pages report).

  - **"How clients funneled" — needs one thing, and it's the most important item on this whole
    list: tag every link before pasting it into a 1:1 DM reply.** Facebook's in-app browser (the one
    people are in when they read a Messenger DM) frequently strips the referrer, so GA4 can show
    this traffic as `(direct)` instead of attributing it to Facebook — even though it genuinely came
    from the DM. Without a tag on the link, "how did they funnel in" is unanswerable no matter how
    well everything else here is built. Fix: append UTM parameters to whichever `/webdesign/{slug}`
    (or other) link gets pasted, e.g. `?utm_source=facebook&utm_medium=dm&utm_campaign=sept2026`.
    **Also added as an action item to the Sept 2 runbook in the career-development repo**
    (`projects/HAXBYTE_BRAND_PLAN.md`) so it surfaces at the moment of actually sending the DM, not
    just buried here.

  - **"Light vs. dark theme" — not tracked by GA4 at all today.** Theme choice lives in
    `localStorage` (`dr-theme`), invisible to GA4 unless explicitly sent. Would need one small
    custom dimension (`theme: light|dark` as a parameter on `page_view` or a user property) — a
    real, deliberate decision to make, not yet built. Genuinely useful for knowing which theme is
    worth investing further polish in, but adds a piece to the taxonomy, so worth deciding on
    purpose rather than defaulting to "sure, why not."

  - **"CTA clicks vs. abandonment"** — exactly what `contact_dialog_open` vs.
    `contact_option_select` already answers (dialog opened but neither button clicked = abandoned).
    Already scoped above, blocked on the UI branch merge, nothing new to decide here.

  - **"Which page performed best" — answerable, but read it loosely.** You're the one choosing
    which link to send each person, so a page "performing better" may just mean that lead was more
    promising, not that the page itself is more persuasive. Real signal, but confounded — don't
    over-read it.

  - **Reality check on sample size.** ~2,000 friends, a realistic 5-10 responses expected — that's
    not enough volume for meaningful percentages; a single visitor swings any "rate" by 10-20
    points. Treat this round as directional signal plus a clean foundation for the higher-volume
    January window (per the brand plan), not a statistically rigorous read. What it *will* reliably
    tell you even at this scale: whether the mailto CTA silently breaks for anyone (a hard technical
    signal, not a rate), a rough device lean, and whether people engage with the case-study pages at
    all before replying.

- **GA4's own Setup Assistant (2026-08-22):** reviewed via screenshot — this is Google's generic
  template shown on every property (ads optimization, first-party/Customer Match data upload,
  Google signals cross-device tracking), not a custom checklist. Not worth itemizing here: most
  sections (Optimize your advertising, Add first-party data) don't apply to a portfolio site with
  no ad spend or CRM data. The two sections that do matter — data collection and key events — are
  already covered by the real checklist above. **Decision: user handles this section directly in
  the GA4 UI at their own pace, not tracked in this roadmap.** One item flagged as worth a
  deliberate choice rather than reflexively enabling: **Turn on Google signals** (cross-device/
  demographics tracking sourced from signed-in Google accounts) — optional, has privacy
  implications, not needed unless demographic reports become a priority.

- **Phase 0 — cleanup before adding anything new:**
  - [x] Audit the existing personal GA4 account: several old learning/demo properties present —
        archive or delete the ones with no ongoing purpose, so the account isn't cluttered before
        this gets added. (See resolution below — already handled.)
  - [x] Decide the fate of the existing "poorly built" GA4 implementation already on the old
        dougrosenbergdev.com — **decided 2026-08-21: start fresh, don't rescue it.** Confirmed via
        Admin > Data Streams that the old property ("DougRosenbergDev", 376622939) tracks
        `https://www.dougrosenbergdev.com` — the stale `www.` canonical the site's own SEO work
        already moved away from — and already has a week of real multi-country traffic (14 users,
        42 events), so its numbers aren't a clean baseline for measuring the Sept 2 launch anyway.
        Left untouched, not deleted — revisit later if it's worth archiving, not urgent.
  - [x] Remaining Phase 0 item: audit and archive/delete the other old learning/demo properties on
        the account (`DEMO`, `DRD.com2`, `ng-fitness-track...`, `ninja-firegram-1...`,
        `portfoliodec22`) — confirmed 2026-08-22: all show strikethrough in the property switcher,
        meaning they're already deleted and sitting in GA4's 35-day recovery/trash window before
        permanent purge. No action needed.

- **Phase 1 — GA4, new/cleaned property:**
  - [x] Create GA4 property + web data stream for dougrosenbergdev.com — **`G-3H1NB9ES0L`**,
        apex domain (`https://dougrosenbergdev.com`, not `www.`), confirmed zero inherited data.
        **Correction:** the ID first used here, `G-FFXHPXFCKJ`, was wrongly logged as "a fresh
        property" — it was actually the old www.-subdomain property described above, caught by
        checking Admin > Data Streams directly instead of trusting the assumption. `index.html` and
        this doc are both now on the correct ID.
  - [x] Add tracking code to `index.html` (gtag.js `<script>` tag) — done 2026-08-21 on
        `feature/ga4-meta-pixel`, builds clean
  - [x] **Don't duplicate GA4's own automatic events.** Enhanced measurement already fires a
        `file_download` event for PDF link clicks — including the résumé nav link built tonight.
        A custom `resume_download` event on top of that would double-count the same action; let
        GA4's automatic event cover it. (No custom event added for this — confirmed as the plan,
        nothing further to do here.)
  - [x] Custom events — **wired up 2026-08-21**, after `feature/nav-client-relabel` merged to
        `main` and was merged into this branch, bringing the contact dialog markup in. All four
        `gtag('event', ...)` calls applied to `WebDesignPage.razor` exactly as drafted below,
        builds clean. Kept the draft here for reference rather than deleting it:

        **1. The CTA that opens the dialog** — add `gtag(...)` before the existing `showModal()`:
        ```html
        <button type="button" class="hero-btn hero-btn--primary wd2-hero__cta"
                onclick="gtag('event','contact_dialog_open');document.getElementById('contactDialog').showModal()">
        ```

        **2. "Quick note" mailto link** — currently has no `onclick` at all, add one:
        ```html
        <a class="hero-btn hero-btn--ghost"
           onclick="gtag('event','contact_option_select',{option:'quick_note'})"
           href="mailto:doug.rosenberg@gmail.com?subject=Hey%20Doug">send a note</a>
        ```

        **3. "Tell me about your project" mailto link** — same idea, this is also the Key Event
        trigger (see below):
        ```html
        <a class="hero-btn hero-btn--primary"
           onclick="gtag('event','contact_option_select',{option:'project_inquiry'})"
           href="mailto:doug.rosenberg@gmail.com?subject=Website%20project&amp;body=...">
        ```

        **4. Copy-email fallback button** — add `gtag(...)` in front of the existing clipboard logic:
        ```html
        onclick="gtag('event','contact_email_copied');navigator.clipboard.writeText('doug.rosenberg@gmail.com').then(()=>{this.textContent='copied!';setTimeout(()=>this.textContent='doug.rosenberg@gmail.com',1500)})"
        ```

        What each answers:
        - `contact_dialog_open` — did the /webdesign CTA get someone to open the dialog?
        - `contact_option_select` (param `option = quick_note | project_inquiry`) — which path did
          they take?
        - `contact_email_copied` — did the mailto fallback get used (signals the mailto link itself
          may be failing for some visitors)?
  - [x] Mark exactly one **Key Event** — **done 2026-08-23:** `contact_option_select` (general
        event, both `quick_note` and `project_inquiry` options) starred as the sole Key Event.
        Resolves the conflict flagged below: went with the simpler general-event version rather
        than building a derived `project_inquiry`-only event, since at the realistic volume here
        (5-10 total responses expected) the quick_note/project_inquiry split isn't worth the extra
        GA4 config — that breakdown is just as easy to read manually later via the `option`
        parameter in Explore if it ever matters. `contact_dialog_open` and `contact_email_copied`
        correctly left unstarred.
        ~~**⚠️ Conflict flagged (2026-08-22):** in a live chat reply, before checking this doc, Claude
        told the user to mark all three contact events (`contact_dialog_open`,
        `contact_option_select`, `contact_email_copied`) as Key Events. That contradicts the
        deliberate "exactly one" decision above.~~ Resolved as above.
  - [x] Review enhanced measurement toggles deliberately — done 2026-08-21: page views, scrolls,
        outbound clicks, and file downloads kept on; site search and form interactions turned off
        (site has neither feature, so both toggles would just be noise). Video engagement left off
        too, no video on the site.
  - [x] **Internal traffic exclusion — done 2026-08-23.** Found both pieces already existed from an
        earlier session: an internal traffic rule ("Doug's IP") under Data streams > Configure tag
        settings > Define internal traffic, and a matching "Internal Traffic" data filter (Exclude,
        `traffic_type` = `internal`) under Admin > Data filters, sitting in Testing mode. Verified
        the IP rule still matches — fetched the current public IP live (`99.116.188.45`) and
        confirmed it's an exact match for the rule's IPv4 condition (plus an IPv6 /64 range already
        covered). Confirmed the filter has actually been catching real traffic (not just configured
        and untested): GA4's own dimension picker only offered one existing value for the
        `Traffic Type` custom dimension — `internal` — meaning matching events already exist in the
        property. Flipped the filter from Testing to Active (confirmed the "destructive and
        irreversible" dialog, matching the plan's own "verify before activating" caveat). Realtime
        report doesn't support custom-dimension comparisons and DebugView needs `debug_mode` the
        site doesn't set, so verification used the dimension-value-list approach above instead.
  - [x] **Dashboard — done 2026-08-23.** Applied GA4's "User behavior" template to the property's
        `Reports snapshot` (its landing dashboard), which already covered traffic source (Active
        users by first user source/medium) and the Key Event card (Key events by Platform) out of
        the box. Customized it further via the report editor to add two cards it was missing: Views
        by Page title and screen class (top pages) and Active users by Device category as a donut
        chart (device split — currently 85.7% desktop / 14.3% mobile over the last 28 days, small
        early sample). Saved to the current (shared) Reports snapshot rather than as a new private
        report, so it's what anyone opening the property lands on.
  - [x] **Lead-gen funnel exploration — built 2026-08-23.** Explore > "Lead-gen Funnel", five steps:
        Landing (`page_view`) → Scroll 75% (`scroll_75`) → Contact dialog open
        (`contact_dialog_open`) → Contact option selected (`contact_option_select`) → Contact email
        copied (`contact_email_copied`). Confirmed each event name against the property's actual
        recorded events while wiring it up (all four matched real data except `contact_email_copied`,
        which has genuinely never fired yet — the copy-to-clipboard fallback nobody's needed so far).
        Checked two other saved explorations first ("Launch Tracking - 2 Sept 26", "First
        Exploration") to make sure this wasn't a duplicate — neither was the funnel. Currently reads
        0% past step 1 on the last-28-days sample (7 users total), exactly the "not enough traffic
        yet" caveat this item already flagged — the exploration is built and will read correctly
        once real launch traffic arrives, nothing further to do here.
  - [x] **Audiences — built 2026-08-25.** Two custom GA4 audiences, both saved in the property:
        **"Engaged visitors"** (`scroll_75` fired at least once — 2 users, 8.33% of all-time users,
        matches the Page Read-Through finding below exactly) and **"Likely bot"** (Session medium
        contains `(none)` AND Country does not exactly match United States — 12 users, 50% of
        all-time users). No explicit "bouncers" audience built — that's just "not in Engaged
        visitors," doesn't need its own definition. "Likely bot" also resolves the "Decision
        needed" item from the 2026-08-25 bounce-rate follow-up below (it went with option 4, a
        standing segment, rather than waiting).
  - [x] **Link Search Console (2026-08-22):** Admin > Product links > Search Console — surfaces
        actual search queries driving traffic once the site is indexed; ties into the SEO items
        above (#2). **Done 2026-08-22:** linked the "Domain" property (`dougrosenbergdev.com`,
        covers all protocols/subdomains) rather than the narrower URL-prefix property — GA4 only
        needed the one link.
  - [x] **`scroll_75` custom event (2026-08-23):** added, mirroring one seen in a GA4 property at
        the user's day job — but motivated by the "did people engage with case-study pages before
        replying" question above, not copied reflexively. Fires once per page at 75% scroll depth
        (GA4's built-in `scroll` event only fires at 90%, too strict a bar at this site's expected
        traffic volume). See `docs/TASKS.md` (2026-08-23 entry) for implementation details. Feeds
        the "Audiences: engaged visitors vs. bouncers" item below once real traffic exists. Built
        on `feature/scroll-75-tracking`, verified via browser automation, merged to `main` 2026-08-23
        (PR #32).
  - [ ] Weekly review post-launch: which CTA path converts, which pages drive it

- **Follow-up from 2026-08-25 GA4 assessment (Claude-run walkthrough of the live property,
  28-day window):** confirmed 100% of traffic is `(direct) / (none)` and Google Search Console
  shows only 3 impressions / 1 click / avg. position 41.3 over the same window — the site isn't
  meaningfully indexed yet.
  - [x] **Tightened bottom-line conclusion (resolved below, via the bot-check exploration).** The
        original write-up's closing paragraph leaned on "grow visibility" without weighing it
        against bot contamination. Sharper version: **the 28-day sample is ~68% bot traffic; only
        6-8 of the 22 "active users" are real** (see breakdown below). SEO/indexing (3 impressions,
        1 click, avg. position 41.3) remains the one clean, real signal, independent of sample size
        — that's the actual thing worth acting on, not the bounce/engagement numbers.
  - [x] **Bounce rate — resolved with data, not guesswork.** Checked GA4's bot-filtering setting
        first (Admin > Data Streams > Data collection): **there is no user-facing toggle** — GA4
        applies the IAB/ABC known-bots list automatically and unconditionally to all data. So this
        traffic already passed that filter; it's not a "flip a setting" fix (rules out option 1
        from the original list below). Built a free-form Explore ("Bounce/Bot Check by City",
        saved in the property, 2026-08-25) crossing City against Engagement rate and Average
        engagement time per session to test the hypothesis directly (option 2 from the original
        list):

        | City | Active users | Events | Engagement rate | Avg. engagement time |
        |---|---|---|---|---|
        | Paris | 8 | 31 | 0% | 0s |
        | Oak Park | 4 | 349 | 86.36% | 1m 35s |
        | Council Bluffs | 3 | 10 | 0% | 0s |
        | (not set) | 2 | 6 | 50% | 8s |
        | Warsaw | 2 | 7 | 0% | 0s |
        | Amsterdam | 1 | 4 | 0% | 0s |
        | Chicago | 1 | 3 | 100% | 0s |
        | Deerfield | 1 | 6 | 100% | 3m 33s |
        | Reston | 1 | 5 | 0% | 3s |

        **Reading it:** Oak Park, Chicago, and Deerfield (6 users total) show real engagement —
        Oak Park alone accounts for 349 of the 421 total events (83%), almost certainly Doug's own
        dev/testing traffic plus real visits, not a stranger's session. Paris, Council Bluffs,
        Warsaw, Amsterdam, and Reston (15 users, 68% of the 22-user total) show **0% engagement
        and 0s (or near-0s) engagement time** — the signature of a scripted visit (fetch the page,
        fire a `page_view`, never actually engage), not a human who bounced after skimming. Paris
        alone is 8 of those 15 and is the single biggest contributor to the property's traffic —
        this isn't "Europe is noisy," it's specifically one Paris-based crawler/bot pattern
        dominating the sample. "(not set)" (2 users, 50% engagement) is genuinely ambiguous and
        not worth resolving further at this volume.
        - [x] Confirms Doug's read: realistically **6-8 real human visitors** in the 28-day window,
              not 22.
        - [x] **Decision made and built, 2026-08-25 (same day, work was slow — got ahead of it
              rather than waiting for Sept 2).** Built the standing "Likely bot" GA4 audience
              (Session medium `(none)` AND Country ≠ United States) instead of relying on re-running
              the saved exploration by hand — see the Audiences item above. 12 users / 50% of
              all-time users match, roughly double the 28-day-window bot share (68%) because this
              audience has no time bound (lifetime membership) while the exploration was scoped to
              the last 28 days — expected, not a discrepancy to chase.

- **Follow-up from 2026-08-25 "which pages are most read" question — Page Read-Through
  exploration.** Built a second saved Explore, **"Page Read-Through (scroll_75 vs page_view)"**,
  crossing Page path against `page_view` and `scroll_75` event counts to answer that question with
  actual read-depth data instead of just view counts:

  | Page | page_view | scroll_75 | Completion rate |
  |---|---|---|---|
  | `/` (homepage) | 111 | 0 | 0% |
  | `/webdesign` | 35 | 5 | 14.3% |
  | all other pages | 30 (combined) | 0 | 0% |

  **Investigated the 0-on-homepage result as a possible bug before trusting it** — GA4's own
  built-in 90%-scroll event fired for 16 of the homepage's 22 users, and 90% is a *harder* bar than
  our 75%, so on its face this looked like broken instrumentation. Live-tested `nav.js`'s
  `checkScroll75()` directly on production (hooked `gtag`, scrolled the real homepage from 0% to
  76% via CDP-simulated mouse wheel, watched `scroll_75` fire exactly at the 75% threshold) —
  **the code is correct, not a bug.** The likely real explanation: GA4's built-in scroll listener
  is bound once per full page load and isn't SPA-route-aware, so a scroll that happens after a
  client-side navigation (e.g. landing on `/`, then clicking into `/webdesign` without a full
  reload) can get misattributed back to whatever page was current when the listener first attached.
  Our custom `scroll_75` re-arms itself per Blazor route via `resetScroll75()` (`Header.razor`'s
  `OnLocationChanged`), so it doesn't have that problem — it's the more trustworthy of the two
  signals here, not the less trustworthy one.
  - [x] No code fix needed — confirmed via live production test, not just code review.
  - [x] Read-through conclusion: real visitors mostly aren't reading deep into the long homepage;
        they're heading to `/webdesign` and reading there. Consistent with, not contradicted by,
        the bounce-rate findings above.
  - [x] Feeds the "Engaged visitors" GA4 audience above (`scroll_75` fired at least once).

- **Phase 2 — Meta Pixel: deprioritized 2026-08-21, not "not yet touched."** Worked through this
  carefully rather than defaulting to "install it anyway since we're already in the neighborhood":
  Pixel's actual value is retargeting and ad-conversion optimization inside Meta's ad ecosystem —
  none of that applies here, since the entire Sept 2 strategy is organic/warm-network with zero ad
  spend anywhere in the brand plan. Everything Pixel could tell you about *this* launch specifically
  (on-site behavior after a click) is already covered by GA4 + the UTM-tagged DM links. Not "Pixel
  vs. GA4 redundant in general" — specifically "adds nothing GA4 doesn't already do, for this goal."
  **Real trigger to revisit, not a someday-maybe:** if paid Facebook/Instagram ads actually start.
  Until then, this stays parked — the standard-events plan below is kept for whenever that happens,
  not deleted, since the reasoning (use Meta's own standard event names, not custom ones) still
  holds whenever it's picked back up:
  - [ ] Add Meta Pixel base code to `index.html`
  - [ ] Use Meta's own **standard events**, not custom names — this is what actually unlocks their
        reporting, same "match the platform's taxonomy" discipline as GA4's Key Events:
        - `Contact` (standard) — on dialog-open, same trigger as GA4's `contact_dialog_open`
        - `Lead` (standard) — on the project-inquiry path, same trigger as GA4's
          `contact_option_select` filtered to `project_inquiry`
  - [ ] No custom conversions beyond that for now — start minimal, expand only if a real question
        comes up that these two don't answer

- **Effort:** 3-4 hours (cleanup + GA4 + Pixel setup, a bit more than the original GA4-only estimate
  since Phase 0 is new)
- **ROI:** Data-driven optimization; understand what works; real device-split data for the Sept 2
  cohort; a clean reference implementation for the day-job cleanup too

### 10. **Core Web Vitals Check** — ✅ Audited (2026-08-22), partially addressed
- **Why:** Google ranks fast sites higher; Blazor WASM apps are notoriously slow to load
- **Scope:**
  - [x] Run PageSpeed Insights on homepage (mobile, live production URL, Slow 4G)
  - [x] Check: LCP, TBT, CLS, Speed Index — **Performance score: 23/100.** LCP 19.4s, FCP 3.8s,
        TBT 1,420ms, CLS 0.252, Speed Index 7.0s — all failing. Total network payload 3,912 KiB.
        Accessibility 99/100 (good).
  - **Root cause (confirms what's already tracked as the "Phase 4: Astro migration" item):** this
    is the Blazor WASM runtime download/boot cost, not a fixable CSS/asset tweak — LCP at 19.4s
    under throttling is the runtime blocking first paint. Not something to attempt as a quick
    overnight fix; real fix is the tracked Astro migration, or at minimum server-side pre-rendering.
  - [x] **Two safe, no-content-decision fixes applied tonight** (separate from the big one above):
    - Fixed "Document does not have a main landmark" (Best Practices audit) — `MainLayout.razor`'s
      wrapping `<div id="main">` is now `<main id="main">` (plus the matching `div#main` →
      `main#main` CSS selector). Verified exactly one `<main>` renders, no visual regression.
    - Investigated "Image elements do not have explicit width/height" (CLS contributor) — **not
      applied**: most images on the site already use `aspect-ratio` in CSS (this session's
      case-study/hero/music work), which is the modern equivalent Lighthouse doesn't always credit
      the same way. Retrofitting explicit HTML `width`/`height` across every image site-wide
      without checking each one individually risked fighting existing responsive CSS — left as a
      real remaining task rather than guessing blindly.
  - [ ] Retest after the Astro migration (or after a real width/height audit) — re-run PageSpeed,
        expect the score to move meaningfully only once the WASM payload itself shrinks
  - [ ] Reduce unused JS (105 KiB est.) / unused CSS (44 KiB est.) / minify CSS (11 KiB est.) — real
        but modest savings next to the WASM bundle itself; worth doing alongside, not instead of,
        Phase 4
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

- [x] **Done (2026-08-23):** the `/webdesign` list-page project cards (`ProjectMediaFrame.razor`)
  already crossfade through each project's screenshots on hover — but there was no signal *before*
  the crossfade started that the thumbnail is interactive/animated at all. Went through a few
  iterations (a top-right cursor badge, then contrast fixes) before landing on the final design:
  a small glass "1/N" count badge (`.wd-project__count-chip`) sitting lower-third on the
  screenshot at rest, matching the glassmorphism already used elsewhere on the page
  (`.wd-case__ba-badge`, `.wd2-hero__glass`); on hover it fades as four small corner brackets
  (`.wd-project__corner`, `mix-blend-mode: difference` for guaranteed contrast against any
  screenshot) fly out from the frame's center to lock the whole frame, like a camera finding
  focus, then retract on mouse-leave. Applies automatically to every project card with more than
  one screenshot (`Images.Count > 1` in `ProjectMediaFrame.razor` — no per-card wiring needed).
  Corner brackets are hidden under `@media (hover: none)` since touch devices already autoplay the
  crossfade with no hover to hint at; the count badge stays visible there since it's information,
  not a motion cue.

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

**Parked idea (2026-08-18) — client-triggered, not a scheduled task:** if/when there's an actually-interested prospect (e.g. from the musician network), consider a `/start` route, not linked from main nav, holding: a lightweight client-side password gate (e.g. `AccessGate` component checking `sessionStorage` against a shared code like `YES123` — a soft deterrent against casual/bot traffic, not real security, which is fine since nothing sensitive sits behind it), a short 5-7 field intake form (name, email, business type, sites they like, budget range, timeline, free text), forwarded via a free-tier relay service (Web3Forms/Formspree) to email, and 2-3 starter template pages (musician one-pager, sole-practitioner site, SMB landing page) to point prospects at during the conversation. Explicitly do **not** build this pre-emptively — no real client data yet to design it from, and the actual next step for landing client #1 is a direct conversation, not new tooling. Revisit only once a real prospect exists.

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
