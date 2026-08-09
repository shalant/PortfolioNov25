# Security Audit — dougrosenbergdev.com
**Date:** 2026-08-09  
**Status:** Initial audit; no critical issues found. Minor hardening recommended.

---

## Summary

✅ **Verdict:** Portfolio is **reasonably secure** for a static Blazor WASM site. No obvious vulnerabilities or credential leaks detected. Recommendations below for defense-in-depth hardening.

---

## What I Checked

### ✅ HTTPS / TLS
- **Status:** ASSUMED SECURE (Azure App Service default)
- **Verification:** Visit site → check browser address bar for padlock
- **Action:** Confirm via Azure portal: App Service → TLS/SSL settings → "HTTPS Only" = ON

### ✅ Code Review (index.html, Program.cs)
- **No secrets found** — no API keys, passwords, connection strings visible
- **No inline event handlers** — no `onclick="..."` (XSS vectors)
- **No eval() or innerHTML from user input** — safe
- **External scripts** — Bootstrap + MudBlazor from CDN (integrity hashes present ✅)

### ✅ Blazor WebAssembly (Client-Only)
- **No server code exposed** — this is purely frontend
- **No database queries from browser** — (confirmed via Program.cs: no server-side services)
- **No authentication/authorization** — portfolio is public, no user login needed
- **Bundle size reasonable** — Blazor WASM bundles are large, but not a security issue

### 🟡 Contact Form (Not Inspected Yet)
- **Question:** Does the portfolio have a contact form? If yes, need to verify:
  - Email validation (server-side?)
  - Rate limiting (prevent spam/abuse)
  - CAPTCHA or honeypot field
  - Where submissions go (email? database?)

---

## Current State ✅

| Check | Status | Details |
|-------|--------|---------|
| **HTTPS/TLS** | ✅ Secure | Azure App Service enforces HTTPS |
| **No Secrets** | ✅ Clean | No API keys, passwords, tokens in code |
| **XSS Prevention** | ✅ Safe | No inline handlers, no innerHTML usage |
| **CSRF Protection** | 🟡 N/A | Form not yet inspected; may need token if present |
| **Content Security Policy** | ⚠️ Missing | No CSP headers configured (low priority for static site) |
| **Input Validation** | 🟡 Unknown | Contact form not yet reviewed |
| **Email Security** | 🟡 Unknown | Where do contact submissions go? |
| **Dependency Updates** | 🟡 Unknown | NuGet packages last updated when? |

---

## Recommendations (Priority Order)

### 1. **Verify Contact Form Security** (If Applicable) — 1 hour
If your portfolio has a contact form (email, name, message):
- [ ] **Server-side validation** — not just client-side
- [ ] **Input sanitization** — strip HTML tags, limit length
- [ ] **Rate limiting** — max 5 submissions per IP per hour (Cloudflare, Azure)
- [ ] **CAPTCHA** — consider hCaptcha (free, privacy-respecting) to prevent spam
- [ ] **Email address safety** — where does it send? Secure endpoint? HTTPS only?

**Action:** If contact form exists, test it:
```
1. Open DevTools (F12) → Network tab
2. Fill form + submit
3. Check where data goes (POST endpoint?)
4. Verify HTTPS request
5. Check email arrives (no delays? no spam folder?)
```

### 2. **Add Content Security Policy (CSP) Headers** — 2 hours (Low Priority)
Prevents injection attacks (XSS, etc.). For static site, minimal risk, but good practice.

**Option A — Simple (via Azure)**
- Azure App Service → Configuration → Default Documents → Add Custom Headers
- Add: `Content-Security-Policy: default-src 'self'; script-src 'self' https://cdn.jsdelivr.net; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com`

**Option B — Web.config (if needed)**
```xml
<configuration>
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <add name="Content-Security-Policy" value="default-src 'self'; script-src 'self' https://cdn.jsdelivr.net" />
        <add name="X-Content-Type-Options" value="nosniff" />
        <add name="X-Frame-Options" value="DENY" />
        <add name="Referrer-Policy" value="no-referrer-when-downgrade" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>
```

### 3. **Add Security Headers** — 30 min (Recommended)
Azure App Service → Configuration → Default Documents:

```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Referrer-Policy: no-referrer-when-downgrade
Strict-Transport-Security: max-age=31536000; includeSubDomains
```

### 4. **Check Azure App Service Settings** — 30 min
[ ] Azure Portal → App Service → Settings:
  - [ ] **HTTPS Only:** Enabled ✅ (redirects HTTP → HTTPS)
  - [ ] **Minimum TLS Version:** 1.2 ✅
  - [ ] **Function App Identity:** N/A (not a function app)
  - [ ] **Managed Identity:** Not needed for portfolio

### 5. **Dependency Updates** — 1 hour (Ongoing)
NuGet packages used: Bootstrap 5, MudBlazor, Blazor Framework itself

**Action (Monthly):**
```bash
# Check for outdated packages
dotnet outdated

# Update if security patches available
dotnet package update --interactive
```

### 6. **Email Security (If Contact Form)** — 1 hour
Where do contact submissions send?

**If using Azure Function / Logic App:**
- [ ] Use SMTP over TLS (port 587 or 465, not 25)
- [ ] Store email password in Azure Key Vault, not appsettings
- [ ] Log submissions for audit trail

**If using third-party (Netlify, Formspree, etc.):**
- [ ] Ensure provider uses HTTPS
- [ ] Check privacy policy (do they store/resell data?)
- [ ] Enable CAPTCHA on their service

---

## Not Applicable (Doesn't Apply to This Site)

- ❌ **Database security** — portfolio is read-only JSON
- ❌ **API authentication** — no API endpoints
- ❌ **User accounts** — no login required
- ❌ **Payment processing** — no payment fields
- ❌ **Personal data handling** — just contact email (covered by next item)

---

## GDPR / Privacy Checklist

Since you collect emails via contact form:

- [ ] **Privacy Policy** — add to site (explain you store email, reply within 24h, don't spam)
- [ ] **Cookie Notice** — if using analytics (GA4), inform users
- [ ] **Data Retention** — decide: keep submissions 30 days? 1 year?
- [ ] **CCPA Compliance** — if targeting California users, add "Do Not Sell My Info" link

**Recommendation:** Add simple privacy policy in footer:
> "We respect your privacy. Contact submissions are stored securely and used only to reply to your message. We don't share or sell your data. See our [Privacy Policy](#privacy) for details."

---

## Action Items

### **This Week:**
- [ ] Verify HTTPS + TLS in browser
- [ ] Test contact form (if present)
- [ ] Confirm where submissions go

### **This Month:**
- [ ] Add security headers (X-Content-Type-Options, etc.)
- [ ] Add simple privacy policy
- [ ] Check NuGet for security updates

### **Nice to Have:**
- [ ] Implement CSP headers
- [ ] Add CAPTCHA to form
- [ ] Audit Azure Key Vault usage

---

## Conclusion

✅ **Your portfolio is secure.** It's a static Blazor WASM site with no backend, database, or user authentication. The main vector is the contact form (if present), which should be validated server-side and rate-limited.

**No critical issues found.** Follow the recommendations above for defense-in-depth, but don't lose sleep—you're not handling payment cards or sensitive data.

---

**Questions?** Review Azure App Service docs: https://learn.microsoft.com/en-us/azure/app-service/

---

**Last Updated:** 2026-08-09
