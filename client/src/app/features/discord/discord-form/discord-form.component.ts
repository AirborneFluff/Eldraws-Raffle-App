import { Component } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import {
  BehaviorSubject,
  combineLatest,
  finalize,
  map,
  startWith,
  switchMap,
  take,
  tap,
  withLatestFrom
} from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';

@Component({
  selector: 'app-discord-form',
  templateUrl: './discord-form.component.html',
  styleUrls: ['./discord-form.component.scss']
})
export class DiscordFormComponent {
  discordChannelId = new FormControl({value: '', disabled: true}, Validators.pattern('^\\d{17}$|^\\d{18}$|^\\d{19}$'));

  discordMessageForm = new FormGroup({
    discordChannelId: this.discordChannelId
  })

  discordChannelId$ = this.clan$.pipe(
    notNullOrUndefined(),
    map(clan => clan.discordChannelId),
    notNullOrUndefined()
  )

  discordMessageId$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => raffle.discordMessageId)
  )

  submitButtonText$ = this.discordMessageId$.pipe(
    map(val => !!val ? 'Update' : 'Post'),
    startWith('Post')
  )

  showRollButton$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => raffle.prizes.length > 0),
    withLatestFrom(this.discordMessageId$.pipe(map(val => val > 0))),
    map(([prizes, messageId]) => prizes && messageId)
  )

  postError$ = new BehaviorSubject<string | null>(null);
  submitted$ = new BehaviorSubject<boolean>(false);

  constructor(public bottomSheet: MatBottomSheet, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private clan$: CurrentClanStream, private raffle$: CurrentRaffleStream) {
    this.clan$.pipe(
      notNullOrUndefined(),
      take(1),
    ).subscribe(clan => {
      this.discordChannelId.setValue(clan.discordChannelId);
    })
  }

  post() {
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
        return this.api.Raffles.createDiscordPost(clanId, raffleId);
      }),
      finalize(() => this.submitted$.next(false))
    ).subscribe({
      next: messageId => {
        this.raffle$.pipe(
          take(1),
          notNullOrUndefined(),
          map(raffle => {
            raffle.discordMessageId = messageId;
            return raffle;
          })).subscribe(updatedRaffle => this.raffle$.next(updatedRaffle))
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
