import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { UserDto } from '../../models/userDto.model';
import { User } from '../../models/user.model';
import { LoginDto } from '../../models/loginDto.model';
import { TokenResponseDto } from '../../models/tokenResponseDto.model';
import { RefreshTokenDto } from '../../models/refreshTokenDto.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  register(dto: UserDto): Observable<User> {
    return this.http.post<User>(
      `${this.apiUrl}/Auth/register`, dto
    );
  }

  login(dto: LoginDto): Observable<TokenResponseDto> {
    return this.http.post<TokenResponseDto>(
      `${this.apiUrl}/Auth/login`, dto
    );
  }

  refreshToken(dto: RefreshTokenDto): Observable<TokenResponseDto> {
    return this.http.post<TokenResponseDto>(
      `${this.apiUrl}/Auth/refresh-token`, dto
    );
  }

  isLoggedIn()
  {
    if(!!localStorage.getItem('token'))
    {
      return true;
    }

    return false;
  }
}
export { User };

