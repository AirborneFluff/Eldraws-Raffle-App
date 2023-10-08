import { Component, Input } from '@angular/core';
import { Raffle } from '../../../data/models/raffle';
import { BehaviorSubject, map } from 'rxjs';

@Component({
  selector: 'app-raffle-list',
  templateUrl: './raffle-list.component.html',
  styleUrls: ['./raffle-list.component.scss']
})
export class RaffleListComponent {
  rafflesSource$ = new BehaviorSubject<Raffle[]>([]);
  @Input()
  set raffles(val: Raffle[]) {
    this.rafflesSource$.next(val);
  }
  @Input() closed: boolean = false;

  constructor() {
    this.currentRaffles$.subscribe();
  }

  oldRaffles$ = this.rafflesSource$.pipe(
    map(raffles => {
      return raffles.filter(raffle => {
        const date = new Date(raffle.closeDate)
        const timeDiff = date.getTime() - new Date().getTime();
        return timeDiff <= 0;
      })
    })
  )

  currentRaffles$ = this.rafflesSource$.pipe(
    map(raffles => {
      return raffles.filter(raffle => {
        const date = new Date(raffle.closeDate)
        const timeDiff = date.getTime() - new Date().getTime();
        return timeDiff > 0;
      })
    })
  )
}
