import { Component } from '@angular/core';
import {
  BehaviorSubject,
  map,
  scan,
  shareReplay,
  switchMap, take,
  withLatestFrom
} from 'rxjs';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { PageParams } from '../../../data/params/page-params';
import { Raffle } from '../../../data/models/raffle';

const INITIAL_QUERY_PARAMS: PageParams = {
  pageSize: 5,
  pageNumber: 1
}

@Component({
  selector: 'app-raffle-list',
  templateUrl: './raffle-list.component.html',
  styleUrls: ['./raffle-list.component.scss']
})
export class RaffleListComponent {

  constructor(public clan$: CurrentClanStream, private api: ApiService, private clanId$: ClanIdStream) {}

  private queryParams$ = new BehaviorSubject<PageParams>(INITIAL_QUERY_PARAMS);

  private oldRafflesResult$ = this.queryParams$.pipe(
    notNullOrUndefined(),
    withLatestFrom(this.clanId$.pipe(notNullOrUndefined())),
    switchMap(([params, clanId]) => this.api.Raffles.getRaffles({
      ...params,
      clanId: clanId,
      endCloseDate: new Date().toISOString()
    })),
    shareReplay(1)
  )

  currentRaffles$ = this.clanId$.pipe(
    notNullOrUndefined(),
    switchMap((clanId) => this.api.Raffles.getRaffles({
      pageNumber: 1,
      pageSize: 50,
      clanId: clanId,
      startCloseDate: new Date().toISOString()
    })),
    map(result => result.result),
    shareReplay(1)
  )

  oldRafflesList$ = this.oldRafflesResult$.pipe(
    notNullOrUndefined(),
    scan((acc: Raffle[], curr) =>
      curr.pagination.currentPage == 1 ? curr.result : acc.concat(curr.result), []),
    shareReplay(1)
  )

  pagination$ = this.oldRafflesResult$.pipe(
    notNullOrUndefined(),
    map(result => result.pagination)
  )

  loadMore() {
    let params: PageParams;
    let currentPage: number;

    this.queryParams$.pipe(take(1)).subscribe(val => params = val);

    this.pagination$.pipe(take(1)).pipe(
      take(1),
      map(pagination => pagination.currentPage)
    ).subscribe(val => currentPage = val);

    params!.pageNumber = currentPage! + 1;
    this.queryParams$.next(params!);
  }
}
