import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import {
  distinctUntilChanged,
  filter,
  map,
  shareReplay,
} from 'rxjs';
import { InjectableStream } from '../observables/injectable-stream';

@Injectable({
  providedIn: 'root',
})
export class UrlStream extends InjectableStream<string> {
  constructor(router: Router) {
    super(
      router.events.pipe(
        filter(event => event instanceof NavigationEnd),
        map(value => (value as NavigationEnd).url),
        distinctUntilChanged(),
        shareReplay(1)
      )
    );
  }
}
