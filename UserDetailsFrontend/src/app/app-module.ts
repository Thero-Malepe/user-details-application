import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Login } from './components/login/login';
import { Home } from './components/home/home';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Register } from './components/register/register';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthInterceptor } from './core/services/interceptorService/Interceptor.service';
import { ResetPasswordComponent } from './components/reset-password-component/reset-password-component';
import { ForgotPassword } from './components/forgot-password/forgot-password';
import { TaskForm } from './components/task-form/task-form';
import { TaskList } from './components/task-list/task-list';
import { Loader } from './components/loader/loader';
import { DeniedAccess } from './components/denied-access/denied-access';
import { Pagination } from './components/pagination/pagination';

@NgModule({
  declarations: [
    App,
    Login,
    Home,
    Register,
    ResetPasswordComponent,
    ForgotPassword,
    TaskForm,
    TaskList,
    Loader,
    DeniedAccess,
    Pagination
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
