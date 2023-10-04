import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import {
  distinctUntilChanged,
  filter,
  map,
  shareReplay,
  startWith,
} from 'rxjs';
import { getRouteParamsFromSnapshot } from 'src/app/core/utils/get-route-params';
import { InjectableStream } from '../observables/injectable-stream';
import { NavigationParams } from '../config/navigation';

@Injectable({
  providedIn: 'root',
})
export class ClanIdStream extends InjectableStream<number | undefined> {
  constructor(router: Router) {
    super(
      router.events.pipe(
        filter(event => event instanceof NavigationEnd),
        startWith(undefined),
        map(() =>
          getRouteParamsFromSnapshot<number>(
            router.routerState.root.snapshot,
            NavigationParams.CLAN_ID
          )
        ),
        distinctUntilChanged(),
        shareReplay(1)
      )
    );
  }
}
