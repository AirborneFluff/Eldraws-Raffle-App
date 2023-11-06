import { Injectable } from '@angular/core';
import {
  combineLatest,
  distinctUntilChanged, of,
  switchMap, take, withLatestFrom
} from 'rxjs';
import { RaffleIdStream } from './raffle-id-stream';
import { ClanIdStream } from './clan-id-stream';
import { ApiService } from '../services/api.service';
import { Raffle } from '../../data/data-models';
import { DataModelStream } from '../observables/data-model-stream';
import { CurrentClanStream } from './current-clan-stream';
import { notNullOrUndefined } from '../pipes/not-null';

@Injectable({
  providedIn: 'root',
})
export class CurrentRaffleStream extends DataModelStream<Raffle | undefined> {
  constructor(raffleId$: RaffleIdStream, clanId$: ClanIdStream, api: ApiService, private clan$: CurrentClanStream) {
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

  override next(value: Raffle | undefined) {
    this.clan$.pipe(
      notNullOrUndefined(),
      withLatestFrom(this),
      take(1)
    ).subscribe(([clan, currentRaffle]) => {
      const raffleIndex = clan.raffles.findIndex(r => r.id == currentRaffle?.id);
      if (raffleIndex == -1) return;

      if (value == undefined) {
        clan.raffles.splice(raffleIndex);
        this.clan$.next(clan);
        return;
      }

      clan.raffles[raffleIndex] = value;
      this.clan$.next(clan);
    });

    super.next(value);
  }
}
