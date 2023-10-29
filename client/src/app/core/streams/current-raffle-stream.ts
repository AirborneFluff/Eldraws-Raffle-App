import { Injectable } from '@angular/core';
import {
  combineLatest,
  distinctUntilChanged, of, shareReplay,
  switchMap
} from 'rxjs';
import { RaffleIdStream } from './raffle-id-stream';
import { ClanIdStream } from './clan-id-stream';
import { ApiService } from '../services/api.service';
import { notNullOrUndefined } from '../pipes/not-null';
import { Raffle } from '../../data/data-models';
import { DataModelStream } from '../observables/data-model-stream';

@Injectable({
  providedIn: 'root',
})
export class CurrentRaffleStream extends DataModelStream<Raffle | undefined> {
  constructor(raffleId$: RaffleIdStream, clanId$: ClanIdStream, api: ApiService) {
    super(
      combineLatest([
        raffleId$.pipe(distinctUntilChanged()),
        clanId$.pipe(distinctUntilChanged())
      ]).pipe(
        switchMap(([raffleId, clanId]) => {
          if (!raffleId || !clanId) return of();
          return api.Raffles.getById(clanId, raffleId)
        })
      )
    );
  }
}
