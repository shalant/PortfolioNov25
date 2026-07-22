# Security Policy

## Overview

This is a personal portfolio site with a static blog. Security is straightforward because there's no user authentication, database, or dynamic content generation on the live site.

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

## Known Risks & Mitigations

| Risk | Level | Mitigation |
|------|-------|-----------|
| XSS via blog content | Low | Claude API sanitizes; content displayed with MarkupString (safe) |
| JSON file corruption | Low | Versioned in git; can restore from history |
| Large JSON file | Low | Monitor file size; split by year if > 2MB |
| Repo access | Low | GitHub auth + branch protection rules |
| BlogPost-Generator API abuse | Low | Not deployed; internal-only |

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
dotnet package update --interactive
```
- Keep .NET, MudBlazor, and packages current
- Check `dotnet outdated` regularly

### Environment Variables
- `ANTHROPIC_API_KEY` - Never commit; use environment variables or Azure Key Vault
- Store in local `.env` or OS environment, not in code

## What's NOT Risky Here

- ✅ SQL Injection — no database used
- ✅ Unauthorized publishes — blog posts are just JSON, not executable
- ✅ User authentication — no users, no auth needed
- ✅ File upload abuse — images are limited in size/type, stored locally
- ✅ CSRF — Blazor has built-in protection

## If You Ever Deploy BlogPost-Generator Publicly

**Do not do this without:**
1. Adding authentication (Bearer token or OAuth)
2. Rate limiting per IP
3. API key in Azure Key Vault (not environment variable)
4. Monitoring for abuse

## Reporting Security Issues

This is a personal portfolio project. Security issues can be reported directly to doug.rosenberg@gmail.com.

---

**Last updated:** July 22, 2026  
**Status:** Blog feature with photo uploads, static JSON storage
