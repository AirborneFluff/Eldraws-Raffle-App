import { Component} from '@angular/core';
import { RaffleEntry } from '../../../data/models/raffle-entry';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import {
  BehaviorSubject,
  combineLatest, distinctUntilChanged,
  map,
  of,
  scan,
  shareReplay, skip,
  startWith,
  switchMap,
  take, tap,
  withLatestFrom
} from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { RaffleEntryParams } from '../../../data/params/raffle-entry-params';
import { EntryStream } from '../../../core/streams/entry-stream';

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
  constructor(private raffle$: CurrentRaffleStream, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private dialog: MatDialog, private entryUpdates$: EntryStream) {
    this.entryUpdates$.pipe(skip(1)).subscribe(changes => {
      this.loadMore(1)
    })
  }

  private searchParams$ = new BehaviorSubject<RaffleEntryParams>(Object.create(INITIAL_SEARCH_PARAMS));
  private entryRemove$ = new BehaviorSubject<RaffleEntry | undefined>(undefined);

  private entryStream$ = this.searchParams$.pipe(
    notNullOrUndefined(),
    withLatestFrom(
      this.raffleId$.pipe(notNullOrUndefined()),
      this.clanId$.pipe(notNullOrUndefined())),
    distinctUntilChanged(),
    tap(obj => console.log(obj)),
    switchMap(([params, raffleId, clanId]) => this.api.Raffles.getEntries(clanId, raffleId, params)),
    shareReplay(1)
  )

  entries$ = combineLatest([
    this.entryRemove$.pipe(notNullOrUndefined(), startWith(undefined)),
    this.entryStream$.pipe(notNullOrUndefined())
  ]).pipe(
    map(([update, stream]) => {
      // if (!!update) {
      //   let index = stream.result.findIndex(ent => ent.id == update.id);
      //   if (index != -1) stream.result.splice(index, 1)
      //   this.entryRemove$.next(undefined);
      // }
      return stream;
    }),
    scan((acc: RaffleEntry[], curr) =>
      curr.pagination.currentPage == 1 ? curr.result : acc.concat(curr.result), [])
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
      this.raffle$.next(updatedRaffle);
      this.loadMore(1);
    })
  }

  loadMore(pageNumber?: number) {
    let params: RaffleEntryParams;
    let currentPage: number;

    this.searchParams$.pipe(take(1)).subscribe(val => params = val);

    this.pagination$.pipe(
      take(1),
      map(pagination => pagination.currentPage)
    ).subscribe(val => currentPage = val);

    params!.pageNumber = pageNumber ?? currentPage! + 1;
    this.searchParams$.next(params!);
  }
}
