import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import {
  BehaviorSubject,
  debounceTime,
  map,
  scan,
  shareReplay,
  switchMap,
  take,
  withLatestFrom
} from 'rxjs';
import { EntrantParams } from '../../../data/params/entrant-params';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

const INITIAL_SEARCH_PARAMS: EntrantParams = {
  pageSize: 3,
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

  private searchParams$ = new BehaviorSubject<EntrantParams>(INITIAL_SEARCH_PARAMS);

  private entrantSearch$ = this.searchParams$.pipe(
    debounceTime(500),
    withLatestFrom(this.clanId$.pipe(notNullOrUndefined())),
    switchMap(([params, clanId]) => this.api.Clans.getEntrants(clanId, params)),
    shareReplay(1)
  )

  entrantsList$ = this.entrantSearch$.pipe(
    map(result => result.result)
  )

  pagination$ = this.entrantSearch$.pipe(
    map(result => result.pagination)
  )

  searchUpdate(event: any) {
    let params = INITIAL_SEARCH_PARAMS;
    params.gamertag = event.target.value;
    this.searchParams$.next(params);
  }

  loadMore() {
    let params;
    this.searchParams$.pipe(take(1)).subscribe(val => params = val);

    params!.pageNumber += 1;
    this.searchParams$.next(params!);
  }
}
