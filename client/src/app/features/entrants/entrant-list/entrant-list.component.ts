import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import {
  BehaviorSubject,
  debounceTime, filter,
  map,
  scan,
  shareReplay,
  switchMap,
  take,
  withLatestFrom
} from 'rxjs';
import { EntrantParams } from '../../../data/params/entrant-params';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { Entrant } from '../../../data/models/entrant';

const INITIAL_SEARCH_PARAMS: EntrantParams = {
  pageSize: 20,
  pageNumber: 1,
  orderBy: 'totalDonations'
}

@Component({
  selector: 'app-entrant-list',
  templateUrl: './entrant-list.component.html',
  styleUrls: ['./entrant-list.component.scss']
})
export class EntrantListComponent {
  constructor(private clanId$: ClanIdStream, private api: ApiService) {
  }

  private searchParams$ = new BehaviorSubject<EntrantParams>(Object.create(INITIAL_SEARCH_PARAMS));

  private entrantSearch$ = this.searchParams$.pipe(
    debounceTime(200),
    withLatestFrom(this.clanId$.pipe(notNullOrUndefined())),
    switchMap(([params, clanId]) => this.api.Clans.getEntrants(clanId, params)),
    shareReplay(1)
  )

  entrantsList$ = this.entrantSearch$.pipe(
    notNullOrUndefined(),
    scan((acc: Entrant[], curr) =>
      curr.pagination.currentPage == 1 ? curr.result : acc.concat(curr.result), [])
  )

  pagination$ = this.entrantSearch$.pipe(
    map(result => result.pagination)
  )

  searchUpdate(event: any) {
    const params: EntrantParams = Object.create(INITIAL_SEARCH_PARAMS);
    params.gamertag = event.target.value;

    this.searchParams$.next(params!);
  }

  loadMore() {
    let params: EntrantParams;
    let currentPage: number;

    this.searchParams$.pipe(take(1)).subscribe(val => params = val);

    this.pagination$.pipe(take(1)).pipe(
      take(1),
      map(pagination => pagination.currentPage)
    ).subscribe(val => currentPage = val);

    params!.pageNumber = currentPage! + 1;
    this.searchParams$.next(params!);
  }
}
