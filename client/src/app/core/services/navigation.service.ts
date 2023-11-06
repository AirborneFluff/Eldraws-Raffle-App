import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { UrlStream } from '../streams/url-stream';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  constructor(private url$: UrlStream, private router: Router) {
    this.url$.subscribe();
  }

  navigateDown() {
    this.url$.pipe(
      take(1)
    ).subscribe(currentRoute => {
      const parentRoute = currentRoute.split('/').slice(0, -2).join('/');
      this.router.navigateByUrl(parentRoute);
    })
  }
}
