import { Injectable } from '@angular/core';
import {
  distinctUntilChanged, shareReplay,
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
        notNullOrUndefined(),
        distinctUntilChanged(),
        switchMap((clanId) => {
          return api.Clans.getById(clanId)
        })
      )
    );
  }
}
