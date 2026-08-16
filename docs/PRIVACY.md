# Privacy Policy

**Last updated:** 2026-08-16

This is a personal portfolio site for Douglas Rosenberg (dougrosenbergdev.com). It's a static
Blazor WebAssembly application with no backend server, no database, and no user accounts.

## What this site does *not* do

- **No cookies set by this site.** Nothing here writes a cookie to your browser.
- **No analytics or tracking.** There's no Google Analytics, no ad pixels, no visitor tracking
  of any kind.
- **No forms that submit anywhere.** The "contact" buttons throughout the site are `mailto:`
  links — clicking one opens *your own* email client, addressed to `doug.rosenberg@gmail.com`.
  Nothing is transmitted to this site or stored by it; the message goes directly from your email
  client to Doug's inbox, the same as any other email.
- **No data collection.** This site doesn't ask for or store your name, email, IP address, or
  any other personal information.

## What does happen

- **Hosting logs.** This site is hosted on GitHub Pages. Like any web host, GitHub's servers log
  basic request data (IP address, browser user-agent, requested page) as part of normal
  operation. That's covered by
  [GitHub's own Privacy Statement](https://docs.github.com/en/site-policy/privacy-policies/github-general-privacy-statement),
  not by anything this site controls.
- **Third-party resources.** The site loads Bootstrap's JavaScript bundle from jsDelivr
  (`cdn.jsdelivr.net`) and fonts from Google Fonts (`fonts.googleapis.com`,
  `fonts.gstatic.com`). Loading a resource from a CDN causes your browser to make a direct
  request to that provider, which may log the request per their own privacy policies — this is
  standard for essentially any website that uses a CDN or web font, not something specific to
  this site.
- **Session storage (technical, not tracking).** A small script in `index.html` uses
  `sessionStorage` to remember which page you were trying to reach if you land on a 404 during
  navigation (a standard workaround for single-page apps hosted on GitHub Pages). It stores a
  URL path, nothing else, and it's cleared immediately after use. It isn't used for tracking and
  never leaves your browser.

## Third-party links

Pages like `/webdesign` and the case studies link out to external sites (client projects,
GitHub, LinkedIn, Haxbyte, etc.). Once you click through, you're subject to that site's own
privacy policy, not this one.

## Questions

Reach out via [doug.rosenberg@gmail.com](mailto:doug.rosenberg@gmail.com).

---

*This is a plain-language description of what this site actually does, written to be accurate
for a simple static portfolio with no data collection — not a substitute for legal review if
this site's scope changes (e.g., adding a real contact form, analytics, or a newsletter signup
in the future would each need this doc revisited).*
