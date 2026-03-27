import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/authService/auth.service';
import { Router } from '@angular/router';

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
    private router: Router
  ) {  }

  ngOnInit() {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    }); 
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
        }
      });
      
    }else{
      alert('Form Invalid');
    }    
  }
}
