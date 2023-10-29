import { Component } from '@angular/core';
import { Raffle } from '../../../data/models/raffle';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { map, startWith } from 'rxjs';

@Component({
  selector: 'app-raffle-details',
  templateUrl: './raffle-details.component.html',
  styleUrls: ['./raffle-details.component.scss']
})
export class RaffleDetailsComponent {

  constructor(public raffle$: CurrentRaffleStream) {
  }

  totalDonations$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      return raffle.entries.reduce((acc, curr) => {
        return acc + curr.donation
      }, 0)
    }),
    startWith(0)
  )

  totalTickets$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      return raffle.entries.reduce((max, entry) => {
        const item2 = entry.tickets?.item2 ?? 0; // Use 0 as a default value if Item2 is missing
        return item2 > max ? item2 : max;
        }, 0);
      }
    )
  )

  getTickets(raffle: Raffle): number {
    return raffle.entries.reduce((max, entry) => {
      const item2 = entry.tickets?.item2 ?? 0; // Use 0 as a default value if Item2 is missing
      return item2 > max ? item2 : max;
    }, 0);
  }
}
