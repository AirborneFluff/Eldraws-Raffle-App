import { Injectable } from '@angular/core';
import {
  distinctUntilChanged, of, shareReplay,
  switchMap
} from 'rxjs';
import { InjectableStream } from '../observables/injectable-stream';
import { ClanIdStream } from './clan-id-stream';
import { ApiService } from '../services/api.service';
import { notNullOrUndefined } from '../pipes/not-null';
import { Clan } from '../../data/data-models';
import { DataModelStream } from '../observables/data-model-stream';

@Injectable({
  providedIn: 'root',
})
export class CurrentClanStream extends DataModelStream<Clan | undefined> {
  constructor(clanId$: ClanIdStream, api: ApiService) {
    super(
      clanId$.pipe(
        distinctUntilChanged(),
        switchMap((clanId) => {
          if (!clanId) return of(undefined);
          return api.Clans.getById(clanId);
        })
      )
    );
  }
}
