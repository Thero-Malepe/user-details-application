import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserDetailsService } from '../../core/services/userDetailsService/user-details.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../core/services/authService/auth.service';


@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {

  isLogin: boolean = false;
  form!: FormGroup;
  details: any;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder, 
    private modalService: NgbModal,
    private router: Router,
    private User: UserDetailsService
  ) {   }

  ngOnInit() {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
    });

    var userEmail = localStorage.getItem('userEmail')!;
    if(userEmail)
    {
      this.loadUserDetails(userEmail);
    }
    else{
      this.logout();
    }     
  }

  loadUserDetails(email: string)
  {
    this.User.getDetailsByEmail(email).subscribe({
      next: (response) => { 
        this.form.patchValue({
          firstName: response.firstName,
          lastName: response.lastName,
          email: response.email
        });
      },
      error: () => {
        this.router.navigate(['/login']);
      }
    })
  }

  logout()
  {
    this.authService.logout();
  }

  openModal(content: any) {
    this.modalService.open(content);
  }
}
