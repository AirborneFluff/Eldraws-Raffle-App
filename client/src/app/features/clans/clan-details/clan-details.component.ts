import { Component, OnDestroy } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { switchMap, tap, combineLatest, map, of, shareReplay } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialog } from '@angular/material/dialog';
import { CreateRaffleComponent } from '../../raffles/create-raffle/create-raffle.component';
import { Title } from '@angular/platform-browser';
import { AccountService } from '../../../core/services/account.service';
import { ClanStream } from '../../../core/streams/clan-stream';

@Component({
  selector: 'app-clan-details',
  templateUrl: './clan-details.component.html',
  styleUrls: ['./clan-details.component.scss']
})
export class ClanDetailsComponent implements OnDestroy {
  constructor(private clanId$: ClanIdStream, private api: ApiService, private dialog: MatDialog, private title: Title, private account: AccountService, private clanUpdates$: ClanStream) {
  }

  ngOnDestroy(): void {
    this.clanUpdates$.next(undefined);
  }

  clan$ = combineLatest([
    this.clanId$.pipe(notNullOrUndefined()),
    this.clanUpdates$])
    .pipe(
      switchMap(([clanId, clan]) => {
        if (!clan) return this.api.Clans.getById(clanId);
        return of(clan);
      }),
    tap(clan => this.title.setTitle(clan.name)),
    shareReplay({refCount: true, bufferSize: 1}))

  isOwner$ = combineLatest([
    this.clan$,
    this.account.currentUser$
  ]).pipe(
    map(([clan, user]) => {
      return clan.owner.id === user?.id
    }))

  openCreateRaffle() {
    this.dialog.open(CreateRaffleComponent);
  }
}
