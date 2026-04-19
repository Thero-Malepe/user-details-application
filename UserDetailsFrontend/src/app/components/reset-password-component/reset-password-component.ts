import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginDto } from '../../core/models/loginDto.model';
import { AuthService } from '../../core/services/authService/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetDto } from '../../core/models/resetDto.model';
import { LoaderService } from '../../core/services/loaderService/loader-service';

@Component({
  selector: 'app-reset-password-component',
  standalone: false,
  templateUrl: './reset-password-component.html',
  styleUrl: './reset-password-component.css',
})
export class ResetPasswordComponent implements OnInit{

  form!: FormGroup;
  email!: string;
  token!: string;
  resetDto: ResetDto = new ResetDto();

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private loader: LoaderService, 
    private route: ActivatedRoute, 
    private router: Router
  ) {  }

  ngOnInit() {
    this.loader.show();
    this.email = this.route.snapshot.queryParamMap.get('email')!;
    this.token = this.route.snapshot.queryParamMap.get('token')!;

    if(this.email == null || this.token == null)
    {
      this.router.navigate(['/forbidden']);
    }
    else{
      this.authService.validateToken(this.token).subscribe({
        next: () => {           
            
          },
        error: () => {
          this.router.navigate(['/forbidden']);
        }
      });
    }

    this.form = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(15)]]
    }); 

    setTimeout(() => {
        this.loader.hide();
      }, 
      4000
    );
  }

  toggle()
  {
    this.router.navigate(['/login']);
  }

  logout()
  {
    this.authService.logout();
  }

  reset()
  {
    if(!this.form.invalid)
    {
      this.resetDto.email = this.email;
      this.resetDto.password = this.form.get('password')?.value;
      this.resetDto.token = this.token;

      this.authService.resetPassword(this.resetDto).subscribe({
        next: (response) => { 
          alert('Password successfully changed');
          
          this.router.navigate(['/login']);
        },
        error: () => {
          alert('Something went wrong, please try again');
        }
      });
      
    }else{
      alert('Form Invalid');
    }    
  }
}
