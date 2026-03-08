import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/authService/auth.service';
import { UserDto } from '../../core/models/userDto.model';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register implements OnInit{

  form!: FormGroup;
  registerDto: UserDto = new UserDto();

  constructor(
    private authService: AuthService,
    private fb: FormBuilder, 
    private router: Router
  ) {  }

  ngOnInit() {
    this.form = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
    }); 
  }

  toggle()
  {
    this.router.navigate(['/login']);
  }

  register()
  {
    if(!this.form.invalid)
    {      
      this.registerDto.email = this.form.get('email')?.value;
      this.registerDto.password = this.form.get('password')?.value;
      this.registerDto.firstName = this.form.get('firstName')?.value;
      this.registerDto.lastName = this.form.get('lastName')?.value;

      this.authService.register(this.registerDto).subscribe({
        next: () => { 
          alert('Successfully registered');
          this.router.navigate(['/login']);
        },
        error: () => {
          alert('Account already exists');
        }
      });
      
    }else{
      alert('Form Invalid');
    }    
  }
}
