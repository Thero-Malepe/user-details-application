import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { User } from '../../models/user.model';
import { UserDetails } from '../../models/userDetails.model';

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {

  public apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  getDetailsByEmail(email: string): Observable<UserDetails> {
    console.log('request sent')
    return this.http.get<UserDetails>(
      `${this.apiUrl}/UserDetails`,
      {
        params: {email: email}
      }
    );
  }
}
export { User };

