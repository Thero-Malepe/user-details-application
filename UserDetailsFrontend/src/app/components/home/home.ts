import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserDetailsService } from '../../core/services/userDetailsService/user-details.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../core/services/authService/auth.service';
import { Loader } from '../loader/loader';
import { LoaderService } from '../../core/services/loaderService/loader-service';


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
  isLoggedIn = false;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder, 
    private loader: LoaderService, 
    private modalService: NgbModal,
    private router: Router,
    private User: UserDetailsService
  ) {   }

  ngOnInit() { 
    this.loader.show();
    this.authService.isLoggedIn$.subscribe((status) =>{
      this.isLoggedIn = status;
    });

    if(this.isLoggedIn)
    {
      this.loadUserDetails();
      setTimeout(() => {
          this.loader.hide();
        }, 
        4000
      )
    }
    else{
      this.logout();
    }
    
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
    });
  }

  loadUserDetails()
  {
    this.User.getDetailsByEmail().subscribe({
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
