import { Component, OnInit, signal } from '@angular/core';
import { AuthService } from './core/services/authService/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { UserDetailsService } from './core/services/userDetailsService/user-details.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit{
  title = 'Task Manager Application';
  currentYear = new Date().getFullYear();
  angularVersion = '20.3.17';
  isLoggedIn = false;
  userInitials: string = '';

  constructor(private authService: AuthService, private userService: UserDetailsService) { }

  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;
      if( this.isLoggedIn )
      {
        this.loadUserDetails();
      }
    });    
  }

  loadUserDetails()
  {
    this.userService.getDetails().subscribe({
      next: (response) => { 
        this.userInitials = this.getInitials(response.firstName, response.lastName);
      },
      error: () => { }
    })
  }

  getInitials(firstName: string, lastName: string): string
  {
    const init = firstName[0].toUpperCase() + lastName[0].toUpperCase();
    return init;
  }

  logout() {
    if(confirm("Do you want to log out?"))
    {
      this.authService.logout();
    };    
  }

  deleteAccount()
  {
    if(confirm("Are you sure you want to DELETE Your account?"))
    {
      this.authService.deleteAccount().subscribe({
        next: () => { 
          alert("Your account has been permanently deleted!");
          this.authService.logout();
        },
        error: () => { }
      });
    }; 
  }
}
