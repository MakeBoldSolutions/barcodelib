# Make Bold Solutions — Website Implementation Kit

Plain HTML/CSS package to implement the Make Bold Solutions brand on your site. No build step, no framework required.

## What's inside
- `styles.css` — link this first. Imports fonts, color/type/spacing/effects tokens, and base element styles (headings, links, `.mbs-container`, `.mbs-eyebrow`, `.mbs-display`).
- `components.css` — link this second. Plain CSS classes for buttons, cards, badges, eyebrows and form inputs.
- `tokens/` — the underlying CSS custom properties (`--brand`, `--fs-h1`, `--space-8`, etc.), split by concern. Referenced by `styles.css`; you don't need to link these individually.
- `assets/fonts/` — Be Vietnam Pro (weights 400–900) and Inter Tight (variable, roman + italic) TTFs, already wired up in `tokens/fonts.css`.
- `assets/logos/` — logo mark, wordmark lockups, and PNG exports.
- `brand-guidelines.md` — condensed brand reference (voice, color, type usage) for anyone writing or designing pages.
- `index.html` — a working demo page. Open it in a browser to see everything rendered together.

## Setup
1. Copy this whole folder into your site (e.g. `/brand/`).
2. In your page `<head>`:
   ```html
   <link rel="stylesheet" href="/brand/styles.css">
   <link rel="stylesheet" href="/brand/components.css">
   ```
3. Use the tokens and classes below.

## Colors & type (CSS variables, from `tokens/colors.css` + `tokens/typography.css`)
```css
background: var(--surface-page);   /* cream page background */
color: var(--text-body);
h1 { font-size: var(--fs-h1); }    /* Be Vietnam Pro is wired to h1–h4 automatically */
```
Key variables: `--brand` (rust `#982407`), `--accent` (ember `#C6620C`), `--text-strong`, `--surface-card`, `--border-default`. Full list in `tokens/colors.css`.

## Components

**Button**
```html
<button class="mbs-btn mbs-btn--primary">Get started</button>
<a class="mbs-btn mbs-btn--secondary mbs-btn--lg" href="#">Learn more</a>
```
Variants: `--primary --accent --secondary --ghost --dark`. Sizes: `--sm` (default) `--lg`. Add `--full` for full width.

**Card**
```html
<div class="mbs-card mbs-card--accent">
  <h3>Cash Flow Snapshot</h3>
  <p>A clear picture of where your money is going.</p>
</div>
```
Padding modifiers: `--none --sm --md --xl` (default is the JSX "lg" padding, built into `.mbs-card`). Add `--interactive` for hover lift.

**Badge**
```html
<span class="mbs-badge mbs-badge--brand">Fractional CFO</span>
```
Tones: `--brand --accent --positive --caution --critical --info` (default is neutral).

**Eyebrow** (signature spaced-caps label)
```html
<div class="mbs-eyebrow">Services</div>
```
Modifiers: `--brand --muted --on-dark`.

**Input**
```html
<div class="mbs-field">
  <label for="email">Email</label>
  <input class="mbs-input" id="email" type="email">
  <span class="mbs-field-hint">We'll never share this.</span>
</div>
```
Add `mbs-input--error` + `mbs-field-hint--error` for validation states.

## Icons
No icon set ships with the brand. The design system uses [Lucide](https://lucide.dev) (2px stroke, outline-only) as a substitute:
```html
<script src="https://unpkg.com/lucide@latest"></script>
<script>lucide.createIcons();</script>
```

## Logo usage
Keep clear space around the mark equal to the height of its tallest peak. Never recolor, distort, or rotate it. Use `assets/logos/logo-mark.svg` as a favicon/loader/bullet, and a wordmark lockup for headers.
