import { Component} from '@angular/core';
import { RaffleEntry } from '../../../data/models/raffle-entry';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  map,
  of,
  scan,
  shareReplay,
  startWith, Subject,
  switchMap,
  take, tap,
  withLatestFrom
} from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { Entrant } from '../../../data/models/entrant';
import { RaffleEntryParams } from '../../../data/params/raffle-entry-params';
import { EntrantParams } from '../../../data/params/entrant-params';

const INITIAL_SEARCH_PARAMS: RaffleEntryParams = {
  pageSize: 50,
  pageNumber: 1,
  orderBy: 'descending'
}

@Component({
  selector: 'app-entry-list',
  templateUrl: './entry-list.component.html',
  styleUrls: ['./entry-list.component.scss']
})
export class EntryListComponent {
  constructor(private raffle$: CurrentRaffleStream, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private dialog: MatDialog) {
  }

  private searchParams$ = new BehaviorSubject<RaffleEntryParams>(Object.create(INITIAL_SEARCH_PARAMS));

  private entryStream$ = this.searchParams$.pipe(
    notNullOrUndefined(),
    withLatestFrom(
      this.raffleId$.pipe(notNullOrUndefined()),
      this.clanId$.pipe(notNullOrUndefined())),
    switchMap(([params, raffleId, clanId]) => this.api.Raffles.getEntries(clanId, raffleId, params)),
    shareReplay(1)
  )

  entries$ = this.entryStream$.pipe(
    notNullOrUndefined(),
    scan((acc: RaffleEntry[], curr) =>
      curr.pagination.currentPage == 1 ? curr.result : acc.concat(curr.result), []),
    tap(val => console.log(val))
  )

  pagination$ = this.entryStream$.pipe(
    map(result => result.pagination)
  )

  removeEntry(entry: RaffleEntry) {
    this.dialog.open(ConfirmDialogComponent, {
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
          take(1),
          switchMap(([clanId, raffleId]) => this.api.Raffles.removeEntry(clanId, raffleId, entry.id))
        )
    })).subscribe(updatedRaffle => {
      this.raffle$.next(updatedRaffle)
    })
  }

  loadMore() {
    let params: RaffleEntryParams;
    let currentPage: number;

    this.searchParams$.pipe(take(1)).subscribe(val => params = val);

    this.pagination$.pipe(
      take(1),
      map(pagination => pagination.currentPage)
    ).subscribe(val => currentPage = val);

    params!.pageNumber = currentPage! + 1;
    this.searchParams$.next(params!);
  }
}
