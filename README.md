# DR Codeworks Portfolio

A modern, interactive portfolio website built with Blazor WebAssembly showcasing professional work, skills, experience, and upcoming projects.

![Status](https://img.shields.io/badge/status-active-brightgreen) ![Built with Blazor](https://img.shields.io/badge/built%20with-Blazor-512bd4) ![License](https://img.shields.io/badge/license-MIT-blue)

## 🌟 Features

- **3D Hexagon Logo Branding** — Custom DR Codeworks logo with navy + teal gradient
- **Interactive Navigation** — Sticky navbar with smooth scrolling navigation
- **Experience Carousel** — MudBlazor carousel displaying professional experience
- **Skills Section** — Organized technical skills with custom chip styling
- **Interactive Todo List** — Checkable tasks for upcoming projects:
  - Reporting
  - Dashboards
  - Design
  - Leadership
  - Backend
  - Consulting
  - Website Design
  - Portfolio
- **Responsive Design** — Mobile-first approach with glassmorphism effects
- **Smooth Animations** — CSS transitions and Blazor component lifecycle animations
- **Bootstrap 5 Integration** — CDN with local fallback support

## 📁 Project Structure

```
portfolioNov25/
├── src/BlazorApp/
│   ├── Components/           # Reusable Razor components
│   │   ├── Experience.razor   # Experience carousel
│   │   ├── ToDo.razor        # Interactive todo list
│   │   └── ...
│   ├── Layout/
│   │   ├── Header.razor      # Navigation bar with logo
│   │   └── MainLayout.razor
│   ├── Pages/
│   │   └── Index.razor       # Home page (main entry)
│   ├── wwwroot/
│   │   ├── css/
│   │   │   └── app.css       # Global styling + component styles
│   │   ├── logos/
│   │   │   └── drdev-logo.png # Favicon + navbar branding
│   │   └── sample-data/      # JSON data files
│   └── BlazorApp.csproj
├── MyPortfolio.sln           # Solution file
├── README.md                 # This file
├── CLAUDE.md                 # Claude Code development guidelines
└── COMPONENTS.md             # Component documentation
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 or higher
- Node.js 18+ (optional, for npm packages)
- Visual Studio Code or Visual Studio

### Local Development

1. **Clone the repository**
   ```bash
   git clone <repo-url>
   cd portfolioNov25
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the development server**
   ```bash
   dotnet run --project src/BlazorApp/BlazorApp.csproj
   ```

4. **Open in browser**
   - Navigate to `http://localhost:5000`
   - The app will automatically reload on file changes

### Using Claude Code

For AI-assisted development with Claude Code:

```bash
claude-code .
```

See [CLAUDE.md](./CLAUDE.md) for detailed Claude Code guidelines.

## 🎨 Customization

### Update Personal Information

Edit `src/BlazorApp/wwwroot/sample-data/siteproperties.json`:
```json
{
  "name": "Your Name",
  "title": "Your Title",
  "email": "your.email@example.com",
  "socialLinks": {
    "linkedin": "https://linkedin.com/in/...",
    "github": "https://github.com/...",
    "twitter": "https://twitter.com/..."
  }
}
```

### Modify Experience Items

Edit `src/BlazorApp/wwwroot/sample-data/experience.json` to add/remove experience entries.

### Update Todo Items

Edit the `todoItems` list in `src/BlazorApp/Components/ToDo.razor`:
```csharp
todoItems = new List<TodoItem>
{
    new TodoItem { Id = 1, Title = "Your Item", Completed = false },
    // ...
};
```

### Styling

- Global styles: `src/BlazorApp/wwwroot/css/app.css`
- Component styles: Embedded `<style>` blocks in `.razor` files
- Theme colors:
  - Primary Navy: `#2c3e50`
  - Accent Teal: `#1abc9c`
  - Background: Light semi-transparent with glassmorphism effects

## 🏗️ Architecture

### Components

- **Header.razor** — Navigation with DR Codeworks logo
- **Experience.razor** — Carousel of professional experience
- **ToDo.razor** — Interactive checklist of upcoming projects
- **Layout components** — Main layout structure

### Data Flow

1. Components receive data via parameters (HttpClient, services)
2. Data loaded from JSON files in `wwwroot/sample-data/`
3. State managed at component level
4. Styling applied via CSS classes

### Key Technologies

- **Blazor WebAssembly** — SPA framework
- **MudBlazor** — UI component library (carousel, grids, etc.)
- **Bootstrap 5** — Layout and utilities (CDN + local fallback)
- **Custom CSS** — Glassmorphism effects, animations

## 📱 Responsive Design

The site is mobile-first with breakpoints:
- Mobile: < 420px
- Tablet: 420px - 1024px
- Desktop: > 1024px

Media queries ensure proper display across all devices.

## 🔒 Browser Support

- Chrome/Edge 90+
- Firefox 88+
- Safari 14+
- Requires JavaScript enabled

## 📦 Build & Deploy

### Build for Production

```bash
dotnet publish -c Release -o publish/
```

### Deploy to Azure

Using Azure Static Web Apps:

```bash
swa start
```

For detailed deployment instructions, see [DEPLOYMENT.md](./DEPLOYMENT.md).

## 📝 Todo List Features

- ✅ Click checkboxes to mark items complete
- ✅ State persists during session
- ✅ Visual feedback (strikethrough, opacity fade)
- ✅ Glassmorphic styling matching site design
- ✅ Teal accent color for checkboxes

Current items:
1. Reporting
2. Dashboards
3. Design
4. Leadership
5. Backend
6. Consulting
7. Website Design
8. Portfolio

## 🎯 Recent Updates (May 2026)

- ✅ Integrated DR Codeworks 3D hexagon logo
- ✅ Fixed CSS compatibility (webkit prefixes)
- ✅ Implemented interactive todo list
- ✅ Added Bootstrap CDN with local fallback
- ✅ Cleaned up navigation code
- ✅ Standardized navigation IDs across components

## 🐛 Known Issues

None currently. Report issues via GitHub Issues.

## 📞 Support

For questions or issues:
- Check [COMPONENTS.md](./COMPONENTS.md) for component details
- Review [CLAUDE.md](./CLAUDE.md) for development practices
- Check existing GitHub Issues

## 📄 License

MIT License - feel free to use this template for your own portfolio.

## 🙏 Acknowledgments

- Built with [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- UI components from [MudBlazor](https://mudblazor.com/)
- Layout framework [Bootstrap 5](https://getbootstrap.com/)
- Icons and assets from [Bootstrap Icons](https://icons.getbootstrap.com/)

---

**Last Updated:** May 29, 2026  
**Maintainer:** Douglas Rosenberg
