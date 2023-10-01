import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { map, ReplaySubject } from 'rxjs';
import { AppUser, LoginDetails, RegisterDetails } from '../../data/data-models';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl + 'account/';

  private currentUserSource$ = new ReplaySubject<AppUser | null>();
  currentUser$ = this.currentUserSource$.asObservable();
  constructor(private http: HttpClient) {
    const val = localStorage.getItem('user');
    if (!val) return;
    const user: AppUser = JSON.parse(val);

    this.setCurrentUser(user);
  }

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
    this.currentUserSource$.next(null);
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
    this.currentUserSource$.next(user);
  }
}
