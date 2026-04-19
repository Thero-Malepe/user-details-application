import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/authService/auth.service';
import { LoginDto } from '../../core/models/loginDto.model';
import { LoaderService } from '../../core/services/loaderService/loader-service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit{

  form!: FormGroup;
  loginDto: LoginDto = new LoginDto();

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private loader: LoaderService, 
    private router: Router
  ) {  }

  ngOnInit() {
    this.loader.show();
    this.form = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.email]]
    }); 

    setTimeout(() => {
      this.loader.hide();
    }, 4000);
  }

  login()
  {
    if(!this.form.invalid)
    {
      this.loginDto.email = this.form.get('email')?.value;
      this.loginDto.password = this.form.get('password')?.value;

      this.authService.login(this.loginDto).subscribe({
        next: (response) => {
          localStorage.setItem('token', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          this.authService.setLoggedIn();
          
          this.router.navigate(['/home']);
        },
        error: (error: HttpErrorResponse) => {
          if(error.status === 400 )
            alert("Incorrect Email or Password"); 
        }
      });
      
    }   
  }
}
