import { Component } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import {
  combineLatest,
  switchMap
} from 'rxjs';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-raffle-details',
  templateUrl: './raffle-details.component.html',
  styleUrls: ['./raffle-details.component.scss']
})
export class RaffleDetailsComponent {
  raffle$ = combineLatest([
        this.raffleId$.pipe(notNullOrUndefined()),
        this.clanId$.pipe(notNullOrUndefined())])
    .pipe(
      switchMap(([raffleId, clanId]) => {
        return this.api.Raffles.getById(clanId, raffleId)}))

  constructor(private api: ApiService, private raffleId$: RaffleIdStream, private clanId$: ClanIdStream) {
  }
}
