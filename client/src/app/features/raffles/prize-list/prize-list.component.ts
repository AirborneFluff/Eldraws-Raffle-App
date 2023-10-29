import { Component } from '@angular/core';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { map, startWith, tap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-prize-list',
  templateUrl: './prize-list.component.html',
  styleUrls: ['./prize-list.component.scss']
})
export class PrizeListComponent {

  constructor(public raffle$: CurrentRaffleStream) {
  }

  prizes$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => raffle.prizes.sort((a, b) => {
      return b.place - a.place;
    })),
    startWith([])
  )

  totalDonations$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      return raffle.entries.reduce((acc, curr) => {
        return acc + curr.donation
      }, 0)
    }),
    startWith(0)
  )
}
