import { Injectable } from '@angular/core';
import {
  combineLatest,
  distinctUntilChanged, shareReplay,
  switchMap
} from 'rxjs';
import { InjectableStream } from '../observables/injectable-stream';
import { RaffleIdStream } from './raffle-id-stream';
import { ClanIdStream } from './clan-id-stream';
import { ApiService } from '../services/api.service';
import { notNullOrUndefined } from '../pipes/not-null';
import { Raffle } from '../../data/data-models';

@Injectable({
  providedIn: 'root',
})
export class CurrentRaffleStream extends InjectableStream<Raffle | undefined> {
  constructor(raffleId$: RaffleIdStream, clanId$: ClanIdStream, api: ApiService) {
    super(
      combineLatest([
        raffleId$.pipe(notNullOrUndefined(), distinctUntilChanged()),
        clanId$.pipe(notNullOrUndefined(), distinctUntilChanged())
      ]).pipe(
        switchMap(([raffleId, clanId]) => {
          return api.Raffles.getById(clanId, raffleId)
        }),
        shareReplay(1)
      )
    );
  }
}
