# Portfolio Self-Assessment — Awwwards Evaluation System

**Date:** 2026-08-19
**Branch reviewed:** `feature/stunning-design-pass`
**Method:** Live walkthrough (dev server, both themes) of the homepage, `/webdesign`, and
`/services`, scored against [Awwwards' public judging criteria](https://www.awwwards.com/about-evaluation/):
Design (40%), Usability (30%), Creativity (20%), Content (10%).

Awwwards judges reward experimental, visually daring agency/art sites more than credibility-first
business portfolios, so this isn't a perfect lens for the site's actual goal (convert recruiters,
companies, and $1-2k freelance clients) — see [`PORTFOLIO_TODO.md`](./PORTFOLIO_TODO.md) /
[`TASKS.md`](./TASKS.md) for that framing. Useful here as a structured, third-party-referenced
gut-check rather than an internal opinion.

## Scores

| Category | Weight | Score | Weighted |
|---|---|---|---|
| Design | 40% | 7.5 / 10 | 3.00 |
| Usability | 30% | 7.0 / 10 | 2.10 |
| Creativity | 20% | 6.5 / 10 | 1.30 |
| Content | 10% | 7.5 / 10 | 0.75 |
| **Total** | | | **≈7.25 / 10** |

## Design — 7.5/10

**Working:**
- Cohesive navy/teal/cream system that holds up in both light and dark themes
- Cormorant Garamond (display) + JetBrains Mono (labels/tags) pairing gives real typographic
  personality instead of default system fonts
- The Bauhaus tile texture and the brass waveform section-divider are genuine signature elements,
  not stock decoration
- Tech-chip system (theme-aware icon tinting, wordmark backing plates for Syncfusion/DevExpress)
  shows a level of polish most portfolios skip
- `/webdesign`'s dual-speed image marquee behind a frosted glass panel is a strong hero moment

**Costing points:**
- `/services` uses a different portrait (moody cybersecurity/hacker backdrop, glowing lock icon)
  that doesn't match the warm, personal photography used everywhere else — a visual-identity break
  between pages a judge would flag immediately
- Emoji bullet icons (🛠️🔄💡) in Experience bullets read casual against the otherwise refined
  serif/mono typography
- Visual interest isn't evenly distributed — About and Casual read plainer than the hero/skills
  sections

## Usability — 7/10

**Working:**
- Sticky nav with a "more" dropdown keeps the top-level link count sane
- Clear primary CTAs ("get in touch," "view work")
- Working dark/light toggle, accessible focus rings, reduced-motion handling honored site-wide
- Mobile touch-target and nav-scroll-lock fixes already shipped in earlier phases

**Costing points:**
- Every subpage that actually matters for conversion — `/webdesign`, `/services`, `/consulting` —
  is buried one click deep inside "more," with nothing on the homepage distinguishing "I'm a
  recruiter" from "I want to hire you for $1,500"
- One visitor path currently serves three different audiences with three different
  jobs-to-be-done

## Creativity — 6.5/10

**Working:**
- The waveform divider pulled from an actual saxophone, the swing motion easing echoing jazz
  phrasing, and the theme-aware icon system are real, subject-specific ideas, not templates

**Costing points:**
- Structurally this is a conventional single-page scroll (hero → about → experience → skills →
  personal → contact) — nothing takes a structural or interactive risk the way Awwwards-tier sites
  do. Legitimate choice for a credibility-first portfolio, not necessarily a flaw, but it caps this
  category honestly under Awwwards' actual rubric

## Content — 7.5/10

**Working:**
- Specific, non-generic bullets throughout Experience
- Real, transparent pricing on `/services` ($750–$3,000/project, $125–175/hr, $300–800/mo)
- Named real clients on `/webdesign` (Hardware Etc LLC, Sonus Construction Group, etc.)
- The personal "why" (the jazz-musician background) genuinely differentiates the voice from a
  generic dev-portfolio

**Costing points:**
- No social proof/testimonials anywhere (known gap — see Phase 3L discussion in `TASKS.md`;
  deliberately not solved with fabricated quotes)
- Minor data-hygiene note, not visitor-facing: `experience.json`'s `description` field duplicates
  the first bullet verbatim on 3 of 4 entries and isn't rendered anywhere in `Experience.razor` —
  dead data, not an actual content bug

## Highest-leverage next moves

Design and Usability are 70% of the Awwwards score, so that's where further effort pays most:

1. **Design, quick fix:** reconcile the `/services` hero portrait/backdrop with the rest of the
   site's photography style
2. **Usability, bigger fix:** give the three audiences (recruiter / business client / casual
   visitor) distinct, visible paths instead of one shared homepage scroll
