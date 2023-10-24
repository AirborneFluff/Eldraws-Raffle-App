import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, take, throwError } from 'rxjs';
import { AccountService } from '../services/account.service';
import { AppUser } from '../../data/models/app-user';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private account: AccountService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser!: AppUser | null;

    this.account.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);

    if (currentUser) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`
        }
      });
    }
    return next.handle(request).pipe(
      catchError((requestError: HttpErrorResponse) => {
        if (requestError?.status === 401) {
          this.account.logout();
          this.router.navigate(['login'],{queryParams:{'redirectURL':this.router.url}});
        }
        return throwError(() => new Error(requestError.message));
      })
    );
  }
}
