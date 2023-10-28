import { Component, OnDestroy } from '@angular/core';
import { map } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RaffleStream } from '../../../core/streams/raffle-stream';
import { Raffle } from '../../../data/models/raffle';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';

@Component({
  selector: 'app-raffle-details',
  templateUrl: './raffle-details.component.html',
  styleUrls: ['./raffle-details.component.scss']
})
export class RaffleDetailsComponent implements OnDestroy {

  constructor(private raffleUpdates$: RaffleStream, public raffle$: CurrentRaffleStream) {
  }

  editable$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      const date = new Date(raffle.closeDate);
      return date.getTime() > new Date().getTime();
    })
  )

  ngOnDestroy() {
    this.raffleUpdates$.next(undefined);
  }

  getDonations(raffle: Raffle): number {
    return raffle.entries.reduce((acc: number, curr) => acc + curr.donation, 0)
  }

  getTickets(raffle: Raffle): number {
    return raffle.entries.reduce((max, entry) => {
      const item2 = entry.tickets?.item2 ?? 0; // Use 0 as a default value if Item2 is missing
      return item2 > max ? item2 : max;
    }, 0);
  }
}
