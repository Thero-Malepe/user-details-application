import { Component, OnInit, signal } from '@angular/core';
import { AuthService } from './core/services/authService/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit{
  title = 'User Details Application';
  currentYear = new Date().getFullYear();
  angularVersion = '20.3.17';
  isLoggedIn = false;

  constructor(private authService: AuthService) { }
  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;
    });
  }

  logout() {
    if(confirm("Are you sure you want to log out?"))
    {
      this.authService.logout();
    };    
  }
}
