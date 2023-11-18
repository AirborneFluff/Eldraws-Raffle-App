import { Component } from '@angular/core';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { map, startWith } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-prize-list',
  templateUrl: './prize-list.component.html',
  styleUrls: ['./prize-list.component.scss']
})
export class PrizeListComponent {
  showFullQuantity: boolean = false;

  constructor(public raffle$: CurrentRaffleStream) {
  }

  prizes$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => raffle.prizes.sort((a, b) => {
      return b.place - a.place;
    })),
    startWith([])
  )
}
