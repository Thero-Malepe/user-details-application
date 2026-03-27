import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Home } from './components/home/home';
import { AuthGuard } from './core/services/authGuardService/auth-guard.service';
import { Register } from './components/register/register';
import { ResetPasswordComponent } from './components/reset-password-component/reset-password-component';
import { ForgotPassword } from './components/forgot-password/forgot-password';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'register',
    component: Register,
    title: 'Register'
  },
  {
    path: 'login',
    component: Login,
    title: 'Login'
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
    title: 'Reset Password'
  },
  {
    path: 'forgot-password',
    component: ForgotPassword,
    title: 'Forgot Password'
  },
  {
    path: 'home',
    canActivate: [AuthGuard],
    component: Home,
    title: 'home'
  },
  {
    path: '**',
    redirectTo: '/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
