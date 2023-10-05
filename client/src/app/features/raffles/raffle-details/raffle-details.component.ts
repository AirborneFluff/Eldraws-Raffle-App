import { Component } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import {
  combineLatest, of,
  switchMap
} from 'rxjs';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RaffleStream } from '../../../core/streams/raffle-stream';

@Component({
  selector: 'app-raffle-details',
  templateUrl: './raffle-details.component.html',
  styleUrls: ['./raffle-details.component.scss']
})
export class RaffleDetailsComponent {
  raffle$ = combineLatest([
        this.raffleId$.pipe(notNullOrUndefined()),
        this.clanId$.pipe(notNullOrUndefined()),
        this.raffleUpdates$])
    .pipe(
      switchMap(([raffleId, clanId, raffle]) => {
        if (!raffle) return this.api.Raffles.getById(clanId, raffleId);
        return of(raffle);
      }))

  constructor(private api: ApiService, private raffleId$: RaffleIdStream, private clanId$: ClanIdStream, private raffleUpdates$: RaffleStream) {
  }
}
