import { Injectable } from '@angular/core';
import {
  combineLatest,
  distinctUntilChanged, of,
  switchMap
} from 'rxjs';
import { RaffleIdStream } from './raffle-id-stream';
import { ClanIdStream } from './clan-id-stream';
import { ApiService } from '../services/api.service';
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
          if (!raffleId || !clanId) return of(undefined);
          return api.Raffles.getById(clanId, raffleId)
        })
      )
    );
  }
}
