import { Component, Input } from '@angular/core';
import { RaffleEntry } from '../../../data/models/raffle-entry';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { combineLatest, of, switchMap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RaffleStream } from '../../../core/streams/raffle-stream';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-entry-list',
  templateUrl: './entry-list.component.html',
  styleUrls: ['./entry-list.component.scss']
})
export class EntryListComponent {
  @Input() entries: RaffleEntry[] = [];

  constructor(private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private raffleUpdates$: RaffleStream, private dialog: MatDialog) {
  }

  removeEntry(entry: RaffleEntry) {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: `Removing this entry will re-assign all ticket assignments beyond the deleted entry <br>${entry.entrant.gamertag} : ${entry.tickets.item1} - ${entry.tickets.item2}`,
        btnOkText: 'Yes',
        btnCancelText: 'No',
      }
    }).afterClosed().pipe(switchMap(confirm => {
      if (!confirm) return of();
      return combineLatest([
        this.clanId$.pipe(notNullOrUndefined()),
        this.raffleId$.pipe(notNullOrUndefined())
      ]).pipe(
        switchMap(([clanId, raffleId]) => this.api.Raffles.removeEntry(clanId, raffleId, entry.id))
      )
    })).subscribe(value => {
      this.raffleUpdates$.next(value)
    })
  }
}
