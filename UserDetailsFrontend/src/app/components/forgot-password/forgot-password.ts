import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/authService/auth.service';
import { Router } from '@angular/router';
import { LoaderService } from '../../core/services/loaderService/loader-service';

@Component({
  selector: 'app-forgot-password',
  standalone: false,
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css',
})
export class ForgotPassword implements OnInit {

  form!: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder, 
    private loader: LoaderService, 
    private router: Router
  ) {  }

  ngOnInit() {
    this.loader.show();
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    }); 

    setTimeout(() => {
        this.loader.hide();
      }, 
      4000
    );
  }

  toggle()
  {
    this.router.navigate(['/register']);
  }

  SendEmail()
  {
    if(!this.form.invalid)
    {
      var email = this.form.get('email')?.value;

      this.authService.forgotPassword(email).subscribe({
        next: (response: string) => { 
          alert('If the email exists, a reset link has been sent');          
          this.router.navigate(['/login']);
        },
        error: () => {
          alert('Email does not exist');
          this.router.navigate(['/register']);
        }
      });
      
    }else{
      alert('Form Invalid');
    }    
  }
}
