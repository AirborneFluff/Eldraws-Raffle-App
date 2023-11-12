import { Component } from '@angular/core';
import { BehaviorSubject, combineLatest, finalize, map, switchMap, take, tap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { ApiService } from '../../../core/services/api.service';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RollParams } from '../../../data/models/roll-params';

@Component({
  selector: 'app-roll-winners-form',
  templateUrl: './roll-winners-form.component.html',
  styleUrls: ['./roll-winners-form.component.scss']
})
export class RollWinnersFormComponent {

  constructor(private raffleId$: RaffleIdStream,
              private clanId$: ClanIdStream,
              private clan$: CurrentClanStream,
              private api: ApiService,
              public bottomSheet: MatBottomSheet) {
  }

  discordChannelId$ = this.clan$.pipe(
    notNullOrUndefined(),
    map(clan => clan.discordChannelId)
  )

  postError$ = new BehaviorSubject<string | null>(null);
  submitted$ = new BehaviorSubject<boolean>(false);

  delay = new FormControl(null, [Validators.pattern('[0-9]+'), Validators.max(30)]);
  maxRerolls = new FormControl(null, [Validators.pattern('[0-9]+'), Validators.max(50)]);
  preventMultipleWins = true;

  paramsForm = new FormGroup({
    delay: this.delay,
    maxRerolls: this.maxRerolls
  })

  rollWinners() {
    const options: RollParams = {
      delay: this.delay.value,
      preventMultipleWins: this.preventMultipleWins,
      maxRerolls: this.maxRerolls.value
    }

    combineLatest([
      this.clanId$.pipe(notNullOrUndefined()),
      this.raffleId$.pipe(notNullOrUndefined())
    ]).pipe(
      tap(() => {
        this.postError$.next(null);
        this.submitted$.next(true);
      }),
      take(1),
      switchMap(([clanId, raffleId]) => {
        return this.api.Raffles.rollWinnersDiscord(clanId, raffleId, options);
      }),
      finalize(() => this.submitted$.next(false))
    ).subscribe({
      next: () => {
        this.bottomSheet.dismiss();
      },
      error: e => this.handleError(e)
    })
  }

  handleError(e: any) {
    const errorMessage: string = e.error;

    if (errorMessage.includes('50013') || errorMessage.includes('50001'))
      return this.postError$.next('Missing Permissions');

    this.postError$.next(errorMessage);
  }

}
