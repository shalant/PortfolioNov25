// Nav bar behavior (scroll state, active-link highlighting, mobile toggle, hash scrolling).
// Loaded once in index.html and driven from Header.razor via JS interop — Blazor tells us
// when it actually rendered instead of us guessing with setTimeout.
window.DrNav = (function () {
    let initialized = false;
    let hashSettleObserver = null;

    function scrollToHash() {
        const hash = window.location.hash;
        if (!hash) return;
        const el = document.querySelector(hash);
        if (!el) return;
        const navbar = document.getElementById('navbar');
        const offset = (navbar ? navbar.offsetHeight : 80) + 20;
        window.scrollTo({ top: el.getBoundingClientRect().top + window.scrollY - offset, behavior: 'smooth' });
    }

    // Async sections can still grow/shrink after the first scroll (their JSON hasn't
    // arrived yet), which would land short. Instead of guessing a delay, re-snap once
    // the DOM actually stops changing, with a ceiling so this can't run forever.
    function scrollToHashAndSettle() {
        scrollToHash();
        if (!window.location.hash) return;

        hashSettleObserver?.disconnect();
        let debounce;
        hashSettleObserver = new MutationObserver(() => {
            clearTimeout(debounce);
            debounce = setTimeout(() => {
                scrollToHash();
                hashSettleObserver?.disconnect();
                hashSettleObserver = null;
            }, 150);
        });
        hashSettleObserver.observe(document.body, { childList: true, subtree: true });
        setTimeout(() => { hashSettleObserver?.disconnect(); hashSettleObserver = null; }, 3000);
    }

    function highlightActive() {
        const navbar = document.getElementById('navbar');
        const sections = Array.from(document.querySelectorAll('section[id]'));
        const threshold = (navbar ? navbar.offsetHeight : 80) + 40;
        let currentSection = sections.length ? sections[0].id : null;
        for (const s of sections) {
            if (s.getBoundingClientRect().top <= threshold) currentSection = s.id;
        }

        const path = window.location.pathname.replace(/\/$/, '') || '/';

        document.querySelectorAll('.nav-link[href]').forEach(link => {
            const href = link.getAttribute('href') ?? '';
            if (href.startsWith('/#') || href.startsWith('#')) {
                const hashId = href.split('#')[1];
                link.classList.toggle('active', currentSection !== null && hashId === currentSection);
            } else {
                const linkPath = href.replace(/\/$/, '') || '/';
                const isActive = linkPath !== '/' && (path === linkPath || path.startsWith(linkPath + '/'));
                link.classList.toggle('active', isActive);
            }
        });
    }

    // Scroll handling, combined into one rAF-throttled pass. Previously this was two
    // separate raw `scroll` listeners, each re-running on every event the browser fired
    // (which can be dozens per second on a trackpad) — doing a full getBoundingClientRect
    // pass over every section on each one. That was the source of "twitchy" navbar motion:
    // the .scrolled toggle used a single scrollY > 50 threshold with no hysteresis, so any
    // momentum/rubber-band micro-scroll right around that one pixel line flipped the class
    // on and off repeatedly, re-triggering the 0.5s background transition and 0.3s logo
    // resize transition each time. Fix is two-fold: only do the work once per animation
    // frame (rAF), and give the threshold a dead zone (enter "scrolled" past 60px, only
    // leave once back under 40px) so hovering near the line doesn't flicker.
    let scrolled = false;
    let scrollTicking = false;

    function handleScroll() {
        const y = window.scrollY;
        if (!scrolled && y > 60) scrolled = true;
        else if (scrolled && y < 40) scrolled = false;
        document.getElementById('navbar')?.classList.toggle('scrolled', scrolled);
        highlightActive();
        scrollTicking = false;
    }

    function onScroll() {
        if (scrollTicking) return;
        scrollTicking = true;
        requestAnimationFrame(handleScroll);
    }

    function init() {
        if (initialized) return;
        initialized = true;

        window.addEventListener('scroll', onScroll, { passive: true });

        // Delegated on document so this keeps working across Blazor SPA navigation,
        // which swaps in brand-new #navToggle/#navItems/.nav-link elements each time.
        document.addEventListener('click', (e) => {
            const toggle = e.target.closest('#navToggle');
            if (toggle) {
                const navItems = document.getElementById('navItems');
                const open = navItems?.classList.toggle('active');
                toggle.classList.toggle('open', !!open);
                // Lock background scroll while the mobile menu is open. Without this,
                // scrolling the page behind the open menu caused visible clipping/
                // ghosting on real devices: the sticky header and the dropdown both
                // use backdrop-filter, and the header's blur radius also changes
                // mid-scroll via .scrolled — recompositing two stacked blur layers,
                // one of them animating, while the page moves underneath is what a
                // mobile GPU couldn't keep up with.
                document.body.classList.toggle('nav-open', !!open);
                return;
            }
            // Only <a> nav links close the mobile menu on tap. The "more" dropdown
            // trigger is a <button> (no href — its items are already always-expanded
            // below it at mobile widths via the @media(max-width:820px) block in
            // app.css), so it must NOT match here or tapping it would just close the
            // whole menu instead of doing nothing/toggling.
            const link = e.target.closest('a.nav-link, a.nav-link--page, a.nav-link--archive, a.nav-cta');
            if (link) {
                document.getElementById('navItems')?.classList.remove('active');
                document.getElementById('navToggle')?.classList.remove('open');
                document.body.classList.remove('nav-open');
            }
        });
    }

    return { init, highlightActive, scrollToHash: scrollToHashAndSettle };
})();
