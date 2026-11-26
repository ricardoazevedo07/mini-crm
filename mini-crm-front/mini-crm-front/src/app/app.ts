import { Component, inject } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

type Theme = 'light' | 'dark';

@Component({
  selector: 'app-root',
  standalone: false,
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  protected menuOpen = false;
  protected theme: Theme = 'light';
  protected pageTitle = 'Gestão';

  constructor() {
    const saved = this.read('crm.theme');
    this.setTheme(saved === 'dark' ? 'dark' : 'light');
    this.router.events.pipe(filter((e) => e instanceof NavigationEnd)).subscribe(() => {
      let r = this.route;
      while (r.firstChild) r = r.firstChild;
      const title = r.snapshot.data['title'] ?? r.snapshot.title;
      this.pageTitle = title ? String(title) : 'Gestão';
    });
  }

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }

  setTheme(theme: Theme): void {
    this.theme = theme;
    document.documentElement.setAttribute('data-theme', theme);
    try { localStorage.setItem('crm.theme', theme); } catch { /* armazenamento indisponível */ }
  }

  private read(key: string): string | null {
    try { return localStorage.getItem(key); } catch { return null; }
  }
}
