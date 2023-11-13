import { Component, Inject } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheet } from '@angular/material/bottom-sheet';
import { Entrant, RafflePrize } from '../../../data/data-models';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { BehaviorSubject, combineLatest, finalize, map, switchMap, take, tap, withLatestFrom } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-roll-winner-form',
  templateUrl: './roll-winner-form.component.html',
  styleUrls: ['./roll-winner-form.component.scss']
})
export class RollWinnerFormComponent {
  prize$ = new BehaviorSubject<RafflePrize | undefined>(undefined);
  winner$ = new BehaviorSubject<Entrant | undefined>(undefined);

  submitted$ = new BehaviorSubject<boolean>(false);

  constructor(@Inject(MAT_BOTTOM_SHEET_DATA) public data: any, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private bottomSheet: MatBottomSheet) {
    this.prize$.next(data.prize);
  }

  rollWinner() {
    combineLatest([
      this.clanId$.pipe(notNullOrUndefined()),
      this.raffleId$.pipe(notNullOrUndefined()),
      this.prize$.pipe(notNullOrUndefined(), map(prize => prize.place))
    ]).pipe(
      take(1),
      tap(() => this.submitted$.next(true)),
      switchMap(([clanId, raffleId, place]) => this.api.Raffles.rollWinner(clanId, raffleId, place)),
      withLatestFrom(this.prize$.pipe(notNullOrUndefined())),
      map(([rollResponse, currentPrize]) => {
        currentPrize.winningTicketNumber = rollResponse.ticketNumber;

        this.prize$.next(currentPrize);
        this.winner$.next(rollResponse.winner);
      }),
      finalize(() => this.submitted$.next(false))
    ).subscribe({
      error: e => console.log(e)
    })
  }

  confirmWinner() {
    combineLatest([
      this.clanId$.pipe(notNullOrUndefined()),
      this.raffleId$.pipe(notNullOrUndefined())
    ]).pipe(
      take(1),
      tap(() => this.submitted$.next(true)),
      switchMap(([clanId, raffleId]) => this.api.Raffles.createDiscordPost(clanId, raffleId)),
      finalize(() => this.bottomSheet.dismiss())
    ).subscribe({
      error: e => console.log(e)
    })
  }
}
