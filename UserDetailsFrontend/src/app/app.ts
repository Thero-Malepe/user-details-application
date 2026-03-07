import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {
  title = 'User Details Application';
  currentYear = new Date().getFullYear();
  angularVersion = '20.3.17';
}
