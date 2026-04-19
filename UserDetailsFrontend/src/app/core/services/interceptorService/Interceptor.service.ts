import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from "../authService/auth.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  private isRefreshing = false;

  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    //Before request is sent
    const accessToken = localStorage.getItem('token');

    // Attach token to request
    let authReq = req;
    if (accessToken) {
      authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`
        }
      });
    }

    //After request is sent with api response
    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {

        // If token expired → try refresh
        if (error.status === 401 && !this.isRefreshing) {
          this.isRefreshing = true;

          const refreshToken = localStorage.getItem('refreshToken');

          if (!refreshToken) {
            this.isRefreshing = false;
            this.authService.logout();
            return throwError(() => error);
          }

          return this.authService.refreshToken({
            accessToken: accessToken!,
            refreshToken: refreshToken
          }).pipe(
            switchMap((response: any) => {
              this.isRefreshing = false;

              // Handle null or invalid refresh response
              if (!response || !response.accessToken || !response.refreshToken) {
                this.authService.logout();
                return throwError(() => new Error("Invalid refresh token"));
              }

              // Save new tokens
              localStorage.setItem('token', response.accessToken);
              localStorage.setItem('refreshToken', response.refreshToken);

              // Retry original request with new token
              const newReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${response.accessToken}`
                }
              });

              return next.handle(newReq);
            }),
            catchError(refreshError => {
              this.isRefreshing = false;
              this.authService.logout();
              return throwError(() => refreshError);
            })
          );
        }

        if (error.status === 500)
        {
          alert("Something Went wrong, please try again later.");
        }

        return throwError(() => error);
      })
    );
  }
}