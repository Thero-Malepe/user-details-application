import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../authService/auth.service";
import { Router } from "@angular/router";
import { catchError, Observable, switchMap, throwError } from "rxjs";
import { TokenResponseDto } from "../../models/tokenResponseDto.model";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService, private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>  {
        const token = localStorage.getItem('token');

        let authReq = req;
        if (token) {
            authReq = req.clone({
                setHeaders: { Authorization: `Bearer ${token}` }
            });
        }

        return next.handle(authReq).pipe(
            catchError(err => {
                if (err.status === 401) {

                    const accToken = localStorage.getItem('token');
                    const refreshToken = localStorage.getItem('refreshToken');

                    if(accToken && refreshToken)
                    {
                        var tokenObj = new TokenResponseDto();
                        tokenObj.accessToken = accToken;
                        tokenObj.refreshToken = refreshToken;

                        return this.auth.refreshToken(tokenObj).pipe(
                            switchMap((response: any) => {

                                localStorage.setItem('token', response.accessToken);
                                localStorage.setItem('refreshToken', response.refreshToken);

                                const newReq = req.clone({
                                    setHeaders: { Authorization: `Bearer ${response.accessToken}` }
                                });

                                return next.handle(newReq);
                            }),
                            catchError(() => {
                                this.router.navigate(['/login']);
                                return throwError(() => err);
                            })
                        );
                    }
                
                }

                return throwError(() => err);
            })
        );
    }
}