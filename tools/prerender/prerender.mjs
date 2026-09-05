// Prerenders every known route of the published Blazor WASM app to static HTML.
//
// Why: this is a client-rendered SPA — a plain HTTP GET (which is what most AI/search
// crawlers issue, since they don't boot the WASM runtime) only ever sees the root
// index.html's <head> and a loading spinner. This script serves the published build
// locally, drives a real headless browser to each route, waits for Blazor to finish
// rendering, and writes the resulting DOM back as that route's index.html. Blazor still
// boots and takes over for real visitors — same swap-not-merge behavior already used for
// the hand-authored homepage hero preview, just generated for every route instead of
// hand-maintained for one.
//
// Order matters: '/' is prerendered LAST so every other route's SPA-fallback request
// (see startServer below) still gets the ORIGINAL pristine build output, not an
// already-mutated home-page snapshot, while this script is running.

import { chromium } from 'playwright';
import http from 'node:http';
import fs from 'node:fs';
import path from 'node:path';

const MIME = {
  '.html': 'text/html; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.mjs': 'text/javascript; charset=utf-8',
  '.css': 'text/css; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.wasm': 'application/wasm',
  '.dll': 'application/octet-stream',
  '.blat': 'application/octet-stream',
  '.dat': 'application/octet-stream',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.jpeg': 'image/jpeg',
  '.gif': 'image/gif',
  '.svg': 'image/svg+xml',
  '.webp': 'image/webp',
  '.woff': 'font/woff',
  '.woff2': 'font/woff2',
  '.ico': 'image/x-icon',
  '.xml': 'application/xml; charset=utf-8',
  '.txt': 'text/plain; charset=utf-8',
  '.pdf': 'application/pdf',
  '.map': 'application/json; charset=utf-8',
};

function contentType(filePath) {
  return MIME[path.extname(filePath).toLowerCase()] || 'application/octet-stream';
}

// Minimal static server with SPA fallback: serves a real file if one exists at the
// requested path, otherwise serves the root index.html (mirrors client-side routing —
// Blazor's router decides what to render from the URL, not from the served markup).
function startServer(rootDir) {
  const server = http.createServer((req, res) => {
    const reqPath = decodeURIComponent(req.url.split('?')[0]);
    let filePath = path.join(rootDir, reqPath);

    if (fs.existsSync(filePath) && fs.statSync(filePath).isDirectory()) {
      filePath = path.join(filePath, 'index.html');
    }
    if (!fs.existsSync(filePath) || fs.statSync(filePath).isDirectory()) {
      filePath = path.join(rootDir, 'index.html');
    }

    fs.readFile(filePath, (err, data) => {
      if (err) {
        res.writeHead(500);
        res.end('Server error');
        return;
      }
      res.writeHead(200, { 'Content-Type': contentType(filePath) });
      res.end(data);
    });
  });

  return new Promise((resolve) => {
    server.listen(0, '127.0.0.1', () => resolve(server));
  });
}

function buildRouteList(publishDir) {
  const routes = ['/webdesign', '/services', '/consulting', '/blog', '/blog/archive'];

  const webdesignPath = path.join(publishDir, 'sample-data', 'webdesign.json');
  if (fs.existsSync(webdesignPath)) {
    const data = JSON.parse(fs.readFileSync(webdesignPath, 'utf8'));
    for (const project of data.projects ?? []) {
      if (project.slug) routes.push(`/webdesign/${project.slug}`);
    }
  }

  routes.push('/'); // last, deliberately — see file header
  return routes;
}

function outputPathFor(publishDir, route) {
  if (route === '/') return path.join(publishDir, 'index.html');
  const segments = route.split('/').filter(Boolean);
  return path.join(publishDir, ...segments, 'index.html');
}

async function main() {
  const publishDirArg = process.argv[2];
  if (!publishDirArg) {
    console.error('Usage: node prerender.mjs <path-to-published-wwwroot>');
    process.exit(1);
  }
  const publishDir = path.resolve(publishDirArg);
  if (!fs.existsSync(publishDir)) {
    console.error(`Publish directory not found: ${publishDir}`);
    process.exit(1);
  }

  const routes = buildRouteList(publishDir);
  const server = await startServer(publishDir);
  const { port } = server.address();
  const baseUrl = `http://127.0.0.1:${port}`;

  const browser = await chromium.launch();
  const page = await browser.newPage({ viewport: { width: 1280, height: 900 } });

  try {
    for (const route of routes) {
      process.stdout.write(`Prerendering ${route} ... `);
      await page.goto(`${baseUrl}${route}`, { waitUntil: 'networkidle', timeout: 60000 });
      await page.waitForSelector('footer', { timeout: 15000 }).catch(() => {
        console.warn(`\n  warning: no <footer> found for ${route} within 15s, capturing anyway`);
      });
      await page.waitForTimeout(300); // let any post-render CSS/entrance state settle

      const html = await page.content();
      const outPath = outputPathFor(publishDir, route);
      fs.mkdirSync(path.dirname(outPath), { recursive: true });
      fs.writeFileSync(outPath, html, 'utf8');
      console.log(`wrote ${path.relative(publishDir, outPath)}`);
    }
  } finally {
    await browser.close();
    server.close();
  }
}

main().catch((err) => {
  console.error(err);
  process.exit(1);
});
