import { Component } from '@angular/core';
import { MatBottomSheet, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { RafflePrize } from '../../../data/data-models';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import {
  combineLatest, filter,
  finalize,
  map, Subject,
  switchMap,
  take,
  tap,
  withLatestFrom,
  merge, from
} from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { RollWinnerResponse } from '../../../data/models/roll-winner-response';

@Component({
  selector: 'app-roll-winner-form',
  templateUrl: './roll-winner-form.component.html',
  styleUrls: ['./roll-winner-form.component.scss']
})
export class RollWinnerFormComponent {
  prizeCount = 0;
  rollResponse: RollWinnerResponse | undefined;
  submitted: boolean = false;
  error: string | null = null;

  constructor(private raffle$: CurrentRaffleStream,
              private api: ApiService,
              private clanId$: ClanIdStream,
              private raffleId$: RaffleIdStream,
              public bottomSheet: MatBottomSheet,
              private bottomSheetRef: MatBottomSheetRef<RollWinnerFormComponent>) {
    this.bottomSheetRef.afterDismissed().pipe(
      withLatestFrom(
        this.clanId$.pipe(notNullOrUndefined()),
        this.raffleId$.pipe(notNullOrUndefined())),
      switchMap(([_, clanId, raffleId]) => this.api.Raffles.createDiscordPost(clanId, raffleId))
    ).subscribe();
  }

  private prizeSource$ = new Subject<RafflePrize>();
  prizes$ = this.raffle$.pipe(
    notNullOrUndefined(),
    filter(raffle => raffle.prizes?.length > 0),
    switchMap(raffle => {
      let arr = raffle.prizes.filter(prize => prize.winningTicketNumber == null);
      arr.sort((a, b) => a.place - b.place);
      this.prizeCount = arr.length;
      return from(arr)
    }))

  prize$ = merge(this.prizes$.pipe(take(1)), this.prizeSource$);

  rollWinner() {
    this.error = null;
    this.rollResponse = undefined;

    combineLatest([
      this.clanId$.pipe(notNullOrUndefined()),
      this.raffleId$.pipe(notNullOrUndefined()),
      this.prize$.pipe(notNullOrUndefined(), map(prize => prize.place))
    ]).pipe(
      take(1),
      tap(() => this.submitted = true),
      switchMap(([clanId, raffleId, prizePlace]) => this.api.Raffles.rollWinner(clanId, raffleId, prizePlace)),
      withLatestFrom(this.prize$.pipe(notNullOrUndefined())),
      finalize(() => this.submitted = false)
    ).subscribe({
      next: ([response, prize]) => {
        this.rollResponse = response;
        if (!response.reroll) {
          prize.winningTicketNumber = response.ticketNumber;
          prize.winner = response.winner;
        }
      },
      error: e => this.handleError(e)
    })
  }

  nextPrize() {
    this.prizes$.pipe(
      take(1)
    ).subscribe(nextPrize => {
      this.prizeSource$.next(nextPrize);
      this.rollResponse = undefined;
      this.rollWinner();
    })
  }

  handleError(e: any) {
    const errorMessage: string = e.error;
    if (errorMessage.includes('50013') || errorMessage.includes('50001')) {
      this.error = 'Missing Permissions';
      return;
    }
    this.error = errorMessage;
  }
}
