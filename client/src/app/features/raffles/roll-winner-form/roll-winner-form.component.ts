import { Component, Inject } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheet, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { Entrant, RafflePrize } from '../../../data/data-models';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import {
  BehaviorSubject,
  combineLatest,
  finalize,
  map, of,
  switchMap,
  take,
  tap,
  withLatestFrom
} from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-roll-winner-form',
  templateUrl: './roll-winner-form.component.html',
  styleUrls: ['./roll-winner-form.component.scss']
})
export class RollWinnerFormComponent {
  constructor(@Inject(MAT_BOTTOM_SHEET_DATA) public data: any,
              private api: ApiService,
              private clanId$: ClanIdStream,
              private raffleId$: RaffleIdStream,
              public bottomSheet: MatBottomSheet,
              private bottomSheetRef: MatBottomSheetRef<RollWinnerFormComponent>) {
    this.prize$.next(data.prize);
    this.bottomSheetRef.afterDismissed().pipe(
      switchMap(() => this.winnerConfirmed ? of() :this.removeWinner$)
    ).subscribe();
  }

  winnerConfirmed: boolean = false;

  prize$ = new BehaviorSubject<RafflePrize | undefined>(undefined);
  winner$ = new BehaviorSubject<Entrant | undefined>(undefined);
  submitted$ = new BehaviorSubject<boolean>(false);

  removeWinner$ = combineLatest([
    this.clanId$.pipe(notNullOrUndefined()),
    this.raffleId$.pipe(notNullOrUndefined()),
    this.prize$.pipe(notNullOrUndefined(), map(prize => prize.place))
    ]).pipe(
      take(1),
      tap(() => this.submitted$.next(true)),
      switchMap(([clanId, raffleId, place]) => this.api.Raffles.removeWinner(clanId, raffleId, place)),
      withLatestFrom(this.prize$.pipe(notNullOrUndefined())),
      map(([_, currentPrize]) => {
        currentPrize.winningTicketNumber = null;
        this.prize$.next(currentPrize);
      }),
      finalize(() => this.submitted$.next(false)))

  rollWinner$ = combineLatest([
    this.clanId$.pipe(notNullOrUndefined()),
    this.raffleId$.pipe(notNullOrUndefined()),
    this.prize$.pipe(notNullOrUndefined(), map(prize => prize.place))
    ]).pipe(
      take(1),
      tap(() => this.submitted$.next(true)),
      switchMap(([clanId, raffleId, place]) => this.api.Raffles.rollWinner(clanId, raffleId, place)),
      withLatestFrom(this.prize$.pipe(notNullOrUndefined())),
      tap(([rollResponse, currentPrize]) => {
        currentPrize.winningTicketNumber = rollResponse.ticketNumber;

        this.prize$.next(currentPrize);
        this.winner$.next(rollResponse.winner);
      }),
      finalize(() => this.submitted$.next(false)))

  confirmWinner$ = combineLatest([
      this.clanId$.pipe(notNullOrUndefined()),
      this.raffleId$.pipe(notNullOrUndefined())
      ]).pipe(
        take(1),
        tap(() => this.submitted$.next(true)),
        switchMap(([clanId, raffleId]) => this.api.Raffles.createDiscordPost(clanId, raffleId)),
        finalize(() => {
          this.winnerConfirmed = true;
          this.bottomSheet.dismiss()
        }))

  updateRaffleStream() {
    /* UPDATE CURRENT RAFFLE */
  }

  rollWinner() {
    this.rollWinner$.subscribe({
      error: e => console.log(e)
    })
  }

  confirmWinner() {
    this.confirmWinner$.subscribe({
      error: e => console.log(e)
    })
  }
}
