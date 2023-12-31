import { Component } from '@angular/core';
import { map, startWith } from 'rxjs';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-raffle-list',
  templateUrl: './raffle-list.component.html',
  styleUrls: ['./raffle-list.component.scss']
})
export class RaffleListComponent {

  constructor(public clan$: CurrentClanStream) {}

  raffles$ = this.clan$.pipe(
    notNullOrUndefined(),
    map(clan => clan.raffles)
  )

  oldRaffles$ = this.raffles$.pipe(
    notNullOrUndefined(),
    map(raffles => {
      return raffles.filter(raffle => {
        const date = new Date(raffle.closeDate)
        const timeDiff = date.getTime() - new Date().getTime();
        return timeDiff <= 0;
      })
    })
  )

  currentRaffles$ = this.raffles$.pipe(
    notNullOrUndefined(),
    map(raffles => {
      return raffles.filter(raffle => {
        const date = new Date(raffle.closeDate)
        const timeDiff = date.getTime() - new Date().getTime();
        return timeDiff > 0;
      })
    })
  )
}
