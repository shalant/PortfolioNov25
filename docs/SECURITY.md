# Security Policy & Audit

**Date:** 2026-08-09  
**Status:** Reasonably secure. No critical issues found. Minor hardening recommended.

---

## Overview

This is a personal portfolio site with a static blog. Security is straightforward because there's no user authentication, database, or dynamic content generation on the live site.

## Security Audit Summary ✅

### What's Secure
- ✅ **HTTPS/TLS** — Azure App Service enforces HTTPS
- ✅ **No secrets in code** — API keys, passwords, tokens not exposed
- ✅ **XSS prevention** — No inline handlers or innerHTML usage
- ✅ **Blazor WebAssembly** — Client-only, no server-side logic exposed
- ✅ **CSRF protection** — Blazor has built-in protection
- ✅ **File upload safety** — Images limited to 5MB, validated formats

### What Needs Attention
- ⚠️ **Content Security Policy (CSP)** — Not configured (low priority for static site)
- ⚠️ **Security headers** — Missing X-Content-Type-Options, X-Frame-Options, Strict-Transport-Security
- 🟡 **Contact form validation** — Server-side validation needed
- 🟡 **Rate limiting** — No rate limiting on form submissions (spam risk)

---

## Infrastructure Security

### Blog Storage
- **Where:** `src/BlazorApp/wwwroot/sample-data/blog-posts.json`
- **Format:** Static JSON with HTML content + base64-encoded images
- **Risk level:** Low (static file, versioned in git)
- **Protection:** GitHub repository access control

### Blog Post Generation
- **Tool:** `BlogPost-Generator/` (Blazor Server app)
- **Status:** Internal-only, never deployed publicly
- **Runs on:** Localhost only
- **API:** Uses Anthropic Claude API (credentials stored in environment variables)
- **Risk:** None if kept internal; would need authentication if deployed publicly

### Image Storage
- **Format:** Base64-encoded in JSON (self-contained)
- **Size limit:** 5MB per upload, validated formats (jpg/png/gif/webp)
- **Risk level:** Low
- **Note:** Images make JSON file larger. Keep total JSON < 2MB for good performance.

---

## Known Risks & Mitigations

| Risk | Level | Mitigation | Status |
|------|-------|-----------|--------|
| XSS via blog content | Low | Claude API sanitizes; content displayed with MarkupString (safe) | ✅ Mitigated |
| JSON file corruption | Low | Versioned in git; can restore from history | ✅ Mitigated |
| Large JSON file | Low | Monitor file size; split by year if > 2MB | ✅ Mitigated |
| Repo access | Low | GitHub auth + branch protection rules | ✅ Mitigated |
| BlogPost-Generator API abuse | Low | Not deployed; internal-only | ✅ Mitigated |
| Contact form spam | Medium | Rate limiting + CAPTCHA (future) | 🟡 Needs work |
| Missing security headers | Low | Add via Azure or Web.config | 🟡 Needs work |

---

## Security Recommendations (Priority Order)

### 1. **Add Security Headers** — 30 min (Recommended)
Azure App Service → Configuration → Custom Headers:

```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Referrer-Policy: no-referrer-when-downgrade
Strict-Transport-Security: max-age=31536000; includeSubDomains
```

**Or via Web.config:**
```xml
<configuration>
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <add name="X-Content-Type-Options" value="nosniff" />
        <add name="X-Frame-Options" value="DENY" />
        <add name="Referrer-Policy" value="no-referrer-when-downgrade" />
        <add name="Strict-Transport-Security" value="max-age=31536000; includeSubDomains" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>
```

### 2. **Contact Form Security** — 1-2 hours (If contact form exists)
- [ ] **Server-side validation** — validate email format, length
- [ ] **Input sanitization** — strip HTML tags, escape output
- [ ] **Rate limiting** — max 5 submissions per IP per hour
- [ ] **CAPTCHA** — consider hCaptcha (free, privacy-respecting) to prevent spam
- [ ] **Email verification** — confirm form sends securely (HTTPS only)

### 3. **Content Security Policy (CSP)** — 2 hours (Low Priority)
Add CSP headers to prevent injection attacks:

```
Content-Security-Policy: default-src 'self'; script-src 'self' https://cdn.jsdelivr.net; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com
```

### 4. **Verify Azure App Service Settings** — 30 min
- [ ] HTTPS Only: Enabled (redirects HTTP → HTTPS)
- [ ] Minimum TLS Version: 1.2 or higher
- [ ] CORS: Restrictive (only allow necessary origins)

### 5. **Dependency Updates** — 1 hour (Monthly)
```bash
dotnet outdated
dotnet package update --interactive
```
Keep .NET, Bootstrap, MudBlazor, and all packages current.

### 6. **Email Security (If Contact Form)** — 1 hour
- [ ] Use SMTP over TLS (port 587 or 465, not 25)
- [ ] Store email credentials in Azure Key Vault, not appsettings
- [ ] Log submissions for audit trail
- [ ] Implement CAPTCHA to prevent spam

---

## What's NOT Risky Here

- ✅ SQL Injection — no database used
- ✅ Unauthorized publishes — blog posts are just JSON, not executable
- ✅ User authentication — no users, no auth needed
- ✅ File upload abuse — images are limited in size/type, stored locally
- ✅ CSRF — Blazor has built-in protection

---

## Best Practices

### When Adding Blog Posts
1. Use `BlogPost-Generator` locally to create posts
2. Export JSON, copy to `wwwroot/sample-data/blog-posts.json`
3. Commit to git with descriptive message
4. Deploy as normal

### Backups
- Blog posts are in git history (free backup)
- For additional safety: Azure backup (if deployed to App Service)

### Updating Dependencies
```bash
dotnet outdated
dotnet package update --interactive
```
- Keep .NET, MudBlazor, and packages current
- Check monthly for security patches

### Environment Variables
- `ANTHROPIC_API_KEY` — Never commit; use environment variables or Azure Key Vault
- Store in local `.env` or OS environment, not in code

---

## If You Ever Deploy BlogPost-Generator Publicly

**Do not do this without:**
1. Adding authentication (Bearer token or OAuth)
2. Rate limiting per IP
3. API key in Azure Key Vault (not environment variable)
4. Monitoring for abuse
5. HTTPS only
6. Request logging for security audit trail

---

## GDPR / Privacy (If Contact Form Collects Data)

- [ ] **Privacy Policy** — explain you store email, reply within 24h, don't spam
- [ ] **Cookie Notice** — if using analytics (GA4), inform users
- [ ] **Data Retention** — decide: keep submissions 30 days? 1 year?
- [ ] **CCPA Compliance** — if targeting California users, add "Do Not Sell My Info" link

---

## Reporting Security Issues

This is a personal portfolio project. Security issues can be reported directly to doug.rosenberg@gmail.com.

---

**Last updated:** 2026-08-09  
**Status:** Blog feature with photo uploads, static JSON storage  
**Next review:** 2026-09-09
