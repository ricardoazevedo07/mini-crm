import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: false,
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected menuOpen = false;

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }
}
