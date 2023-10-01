import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { map, ReplaySubject } from 'rxjs';
import { AppUser, LoginDetails, RegisterDetails } from '../../data';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl + 'account/';
  currentUser$ = new ReplaySubject<AppUser | null>()
  constructor(private http: HttpClient) { }

  login(details: LoginDetails) {
    return this.http.post<AppUser>(this.baseUrl + 'login', details).pipe(
      map(response => {
        if (response) {
          this.setCurrentUser(response);
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser$.next(null);
  }

  register(details: RegisterDetails) {
    return this.http.post<AppUser>(this.baseUrl + 'register', details).pipe(
      map(response => {
        if (response) {
          this.setCurrentUser(response);
        }
      })
    )
  }

  private setCurrentUser(user: AppUser) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser$.next(user);
  }
}
