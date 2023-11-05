import { Component } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { BehaviorSubject, combineLatest, finalize, map, switchMap, take, tap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { FormControl, FormGroup, Validators } from '@angular/forms';

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

  postError$ = new BehaviorSubject<boolean>(false);
  submitted$ = new BehaviorSubject<boolean>(false);

  constructor(public bottomSheet: MatBottomSheet, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private clan$: CurrentClanStream) {
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
        this.postError$.next(false);
        this.submitted$.next(true);
      }),
      take(1),
      switchMap(([clanId, raffleId]) => {
        return this.api.Raffles.createDiscordPost(clanId, raffleId);
      }),
      finalize(() => this.submitted$.next(false))
    ).subscribe({
      next: () => {
        this.bottomSheet.dismiss();
      },
      error: e => {
        this.postError$.next(true);
      }
    })
  }
}
