# Component Documentation

Detailed reference for all Blazor components in the DR Codeworks portfolio.

## Overview

Components are the building blocks of the Blazor app. Each component is a `.razor` file containing HTML, CSS, and C# code.

### Component Locations

- **Layout Components:** `src/BlazorApp/Layout/`
- **Page Components:** `src/BlazorApp/Pages/`
- **Reusable Components:** `src/BlazorApp/Components/`

---

## Layout Components

### Header.razor

**Purpose:** Navigation bar with DR Codeworks logo and site navigation

**Location:** `src/BlazorApp/Layout/Header.razor`

**Features:**
- Sticky navbar (stays at top while scrolling)
- 3D hexagon logo (navy + teal)
- Navigation links with smooth scrolling
- Glassmorphic background with blur effect
- Responsive mobile menu via Bootstrap

**Parameters:** None (used directly in MainLayout)

**Styling:**
- Navy background: `rgba(255, 255, 255, 0.4)` with blur
- Logo height: 45px
- Navigation links: `#2c3e50` (navy)

**Navigation Links:**
- Home → `#home`
- About → `about`
- Experience → `#experience`
- More Info (dropdown)
  - Technical Skills → `#technicalskills`
  - Casual → `#casual`
  - Consulting → `#consulting`
  - Web Design → `#webdesign`

**Dependencies:**
- Bootstrap 5 navbar component
- `logos/drdev-logo.png`

### MainLayout.razor

**Purpose:** Main page layout wrapper

**Location:** `src/BlazorApp/Layout/MainLayout.razor`

**Features:**
- Wraps all pages
- Includes Header component
- Defines overall page structure

**Parameters:** None

---

## Page Components

### Index.razor

**Purpose:** Home page - main entry point

**Location:** `src/BlazorApp/Pages/Index.razor`

**Features:**
- Composes all major sections
- Passes HttpClient and services to child components

**Includes:**
1. Hero section (heading/intro)
2. Experience section
3. Skills/Technical section
4. Casual/Creative section
5. Consulting section
6. Todo list section

**Parameters:**
- `HttpClient` — For data fetching
- `HeroImageService` — For hero images

---

## Reusable Components

### Experience.razor

**Purpose:** Displays professional experience in a carousel

**Location:** `src/BlazorApp/Components/Experience.razor`

**Features:**
- MudBlazor carousel component
- Loads experience data from JSON
- Shows images, titles, descriptions, tech stacks
- Navigation arrows and bullet indicators
- Auto-cycling carousel
- Swipe gesture support on mobile

**Data Source:**
```json
// sample-data/experience.json
[
  {
    "title": "Company Name",
    "url": "https://company.com",
    "image": "/images/logo.png",
    "description": "Detailed description",
    "bulletpoints": ["Point 1", "Point 2"],
    "icons": "React,Node.js,PostgreSQL"
  }
]
```

**Parameters:**
```csharp
[Parameter, EditorRequired] public required HttpClient Http { get; set; }
[Parameter, EditorRequired] public required HeroImageService HeroImageService { get; set; }
```

**Styling:**
- `.custom-carousel` — Main carousel container
- `.carousel-card` — Individual items
- `.custom-chip` — Tech stack tags (gradient bg, white text, teal accent)
- Glassmorphism effect with backdrop blur

**Section ID:** `#experience`

**Key Styling Details:**
```css
.custom-chip {
  background: linear-gradient(90deg, rgba(70,75,77,1) 0%, rgba(157,176,165,0.6) 86%...);
  color: whitesmoke;
  border-radius: 16px;
  transition: background 0.4s ease-in-out, box-shadow 0.4s ease-in-out;
}

.custom-chip:hover {
  background: linear-gradient(90deg, rgba(70,75,77,1) 0%, rgba(157,176,165,0.7) 76%...);
  box-shadow: 3px 3px 3px 3px rgba(151,158,165,0.3);
}
```

---

### ToDo.razor

**Purpose:** Interactive checklist of upcoming projects

**Location:** `src/BlazorApp/Components/ToDo.razor`

**Features:**
- Clickable checkboxes for each item
- Toggle completion state
- Strikethrough text when completed
- Glassmorphic card styling
- Smooth animations
- Session-persisted state (resets on page reload)

**Todo Items:**
1. Reporting
2. Dashboards
3. Design
4. Leadership
5. Backend
6. Consulting
7. Website Design
8. Portfolio

**Parameters:**
```csharp
[Parameter, EditorRequired] public required HttpClient Http { get; set; }
[Parameter, EditorRequired] public required HeroImageService HeroImageService { get; set; }
```

**Component State:**
```csharp
private List<TodoItem> todoItems = new();

private class TodoItem
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public bool Completed { get; set; }
}
```

**Methods:**
- `ToggleTodo(int id, bool completed)` — Toggle completion state

**Styling:**
- `.todo-container` — Wrapper, max-width 600px
- `.todo-item` — Individual item card
  - Background: `rgba(255,255,255,0.3)`
  - Hover: `rgba(255,255,255,0.5)` with slide animation
  - Border-radius: 8px
- `.todo-checkbox` — Native checkbox with teal accent (`#1abc9c`)
- `.todo-label` — Item text
  - Completed state: `text-decoration: line-through`, opacity 0.6

**Animations:**
```css
@keyframes slideIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
```

**Section ID:** `#todo`

---

## Other Components

### Skills Component (if exists)

**Purpose:** Display technical skills

**Data Source:** `sample-data/technicalskills.json`

**Section ID:** `#technicalskills`

### About Component (if exists)

**Purpose:** Personal introduction and bio

**Section ID:** `#about`

---

## Data Models

### TodoItem

```csharp
private class TodoItem
{
    public int Id { get; set; }           // Unique identifier
    public string Title { get; set; }     // Display text
    public bool Completed { get; set; }   // Completion state
}
```

### Experience (from JSON)

```json
{
  "title": "string",
  "url": "string",
  "image": "string",
  "description": "string",
  "bulletpoints": ["string"],
  "icons": "string (comma-separated)"
}
```

---

## Common Patterns

### Loading Data from JSON

```csharp
protected override async Task OnInitializedAsync()
{
    items = await Http.GetFromJsonAsync<List<Item>>("sample-data/items.json");
    hero = await HeroImageService.GetHeroAsync(img => img.Name is "experience");
}
```

### Binding Checkbox State

```html
<input type="checkbox"
       id="item-@id"
       class="todo-checkbox"
       @onchange="@((ChangeEventArgs e) => Toggle(id, (bool)e.Value!))"
       checked="@completed" />
```

### Conditional CSS Classes

```html
<div class="@(completed ? "completed" : "")">Item</div>
```

### Loop with @key

```html
@foreach (var item in items)
{
    <div @key="item.Id">@item.Title</div>
}
```

---

## Styling Guidelines

### Color Palette

| Color | Hex | Usage |
|-------|-----|-------|
| Navy | #2c3e50 | Primary text, headers |
| Teal | #1abc9c | Accents, interactive elements |
| Light Navy | #1a252f | Dark backgrounds |
| Charcoal | #0f1620 | 3D hexagon shadow |
| Light Grey | #f5f5f5 | Backgrounds |
| White | #ffffff | Text, highlights |

### Effects

**Glassmorphism:**
```css
background: rgba(255, 255, 255, 0.3);
backdrop-filter: blur(10px);
border: 1px solid rgba(255, 255, 255, 0.2);
border-radius: 5px;
```

**Smooth Transitions:**
```css
transition: all 0.3s ease-in-out;
```

**Hover Effects:**
```css
:hover {
  background: rgba(255, 255, 255, 0.5);
  transform: translateX(8px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}
```

---

## Dependencies

### External Libraries

- **MudBlazor** — UI components (carousel, grids)
  - Installed via NuGet
  - CSS: `_content/MudBlazor/MudBlazor.min.css`
  - JS: `/MudBlazor/MudBlazor.min.js`
  
- **Bootstrap 5** — Layout framework
  - CSS: CDN (with local fallback)
  - JS: CDN bundle
  - Grid, utilities, responsive design

### Custom Services

- **HeroImageService** — Manages hero image selection
  - Injects into components
  - Loads images based on section name

---

## Lifecycle & Best Practices

### Component Initialization

1. Component loads → `OnInitialized()` or `OnInitializedAsync()`
2. Data loaded from JSON via HttpClient
3. Component renders with data
4. Event handlers attached to elements
5. User interaction triggers state changes → re-render

### State Management

- State is component-scoped
- Use `StateHasChanged()` to force re-render if needed
- Parent components pass state via `[Parameter]`
- Child components notify parent via callbacks

### Performance Tips

1. Use `@key` in `@foreach` for large lists
2. Lazy load images
3. Minimize re-renders
4. Cache data locally
5. Use event delegation for many items

---

## Testing Components

### Manual Testing Checklist

- [ ] Component renders without errors
- [ ] Data loads correctly
- [ ] All interactive elements work (buttons, checkboxes, dropdowns)
- [ ] Responsive on mobile (< 420px), tablet (420-1024px), desktop (> 1024px)
- [ ] Styling matches navy + teal theme
- [ ] Animations are smooth
- [ ] No console errors (F12 → Console tab)

---

## Common Issues & Solutions

| Issue | Cause | Solution |
|-------|-------|----------|
| Data not loading | JSON path incorrect | Check `sample-data/` folder, verify path in code |
| Checkbox not toggling | Event handler not bound | Check `@onchange` attribute, parameter types |
| Styles not applying | CSS class typo or specificity | Check DevTools, inspect element, verify cascade |
| Component not rendering | Missing `[Parameter]` | Ensure all `EditorRequired` params are passed |
| Carousel not working | MudBlazor not loaded | Check `index.html` for MudBlazor CSS/JS references |

---

## Adding New Components

### Template

```razor
<section class="light" id="newsection">
    <h2 class="mt-5">Section Title</h2>
    
    <div class="section-container">
        <!-- Your content here -->
    </div>
</section>

@code {
    [Parameter, EditorRequired]
    public required HttpClient Http { get; set; }
    
    [Parameter, EditorRequired]
    public required HeroImageService HeroImageService { get; set; }
    
    private List<Item>? items;
    
    protected override async Task OnInitializedAsync()
    {
        items = await Http.GetFromJsonAsync<List<Item>>("sample-data/items.json");
    }
}

<style>
    .section-container {
        /* Your styles */
    }
</style>
```

### Steps

1. Create `Components/YourComponent.razor`
2. Add parameters and initialization
3. Add HTML template
4. Add CSS styling
5. Reference in `Index.razor`
6. Create JSON data file in `sample-data/` (if needed)
7. Test in browser

---

**Last Updated:** July 22, 2026  
**Component Count:** 8+ (Layout, Pages, Reusable)
