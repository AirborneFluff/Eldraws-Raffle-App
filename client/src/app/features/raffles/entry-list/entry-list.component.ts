import { Component, Input, OnDestroy } from '@angular/core';
import { RaffleEntry } from '../../../data/models/raffle-entry';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { combineLatest, map, Observable, of, startWith, Subscription, switchMap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RaffleStream } from '../../../core/streams/raffle-stream';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';

@Component({
  selector: 'app-entry-list',
  templateUrl: './entry-list.component.html',
  styleUrls: ['./entry-list.component.scss']
})
export class EntryListComponent implements OnDestroy {
  subscription = new Subscription();

  constructor(private raffle$: CurrentRaffleStream, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private dialog: MatDialog) {
  }

  entries$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => raffle.entries),
    startWith([])
  )

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  removeEntry(entry: RaffleEntry) {
    const subscription = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: `Removing this entry will re-assign all ticket assignments beyond the deleted entry <br>${entry.entrant.gamertag} : ${entry.tickets.item1} - ${entry.tickets.item2}`,
        btnOkText: 'Yes',
        btnCancelText: 'No',
      }
    }).afterClosed().pipe(
      switchMap(confirm => {
        if (!confirm) return of();

        return combineLatest([
          this.clanId$.pipe(notNullOrUndefined()),
          this.raffleId$.pipe(notNullOrUndefined())
        ]).pipe(
          switchMap(([clanId, raffleId]) => this.api.Raffles.removeEntry(clanId, raffleId, entry.id))
        )
    })).subscribe(value => {
      this.raffle$.next(value)
    })

    this.subscription.add(subscription);
  }
}
