import { Component } from '@angular/core';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { map, startWith } from 'rxjs';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { RaffleFormComponent } from '../raffle-form/raffle-form.component';

@Component({
  selector: 'app-raffle-details',
  templateUrl: './raffle-details.component.html',
  styleUrls: ['./raffle-details.component.scss']
})
export class RaffleDetailsComponent {

  constructor(public raffle$: CurrentRaffleStream, public bottomSheet: MatBottomSheet) {
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
        const item2 = entry.tickets?.item2 ?? 0;
        return item2 > max ? item2 : max;
        }, 0);
      }
    )
  )

  editRaffle() {
    this.bottomSheet.open(RaffleFormComponent);
  }
}
