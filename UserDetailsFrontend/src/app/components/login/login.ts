import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../core/services/authService/auth.service';
import { LoginDto } from '../../core/models/loginDto.model';
import { UserDto } from '../../core/models/userDto.model';

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
    private router: Router
  ) {  }

  ngOnInit() {
    this.form = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.email]]
    }); 
  }

  toggle()
  {
    this.router.navigate(['/register']);
  }

  login()
  {
    if(!this.form.invalid)
    {
      this.loginDto.email = this.form.get('email')?.value;
      this.loginDto.password = this.form.get('password')?.value;

      this.authService.login(this.loginDto).subscribe({
        next: (response) => { 
          alert('Successfully logged in');

          localStorage.setItem('token', response.accessToken);
          localStorage.setItem('userEmail', this.loginDto.email);
          localStorage.setItem('refreshToken', response.refreshToken);
          
          this.router.navigate(['/home']);
        },
        error: () => {
          alert('Email or password is incorrect');
        }
      });
      
    }else{
      alert('Form Invalid');
    }    
  }
}
