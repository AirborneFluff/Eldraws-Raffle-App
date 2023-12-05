import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { BehaviorSubject, map, scan, switchMap, take, withLatestFrom } from 'rxjs';
import { EntrantParams } from '../../../data/params/entrant-params';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { Entrant } from '../../../data/models/entrant';

@Component({
  selector: 'app-entrant-list',
  templateUrl: './entrant-list.component.html',
  styleUrls: ['./entrant-list.component.scss']
})
export class EntrantListComponent {

  constructor(private clanId$: ClanIdStream, private api: ApiService) {
  }

  searchParams$ = new BehaviorSubject<EntrantParams>({
    pageSize: 20,
    pageNumber: 1,
    orderBy: 'totalDonations'
  });

  entrantsResult$ = this.searchParams$.pipe(
    withLatestFrom(this.clanId$.pipe(notNullOrUndefined())),
    switchMap(([params, clanId]) => this.api.Clans.getEntrants(clanId, params)))

  entrants$ = this.entrantsResult$.pipe(
    map(result => result.result),
    notNullOrUndefined(),
    scan((acc: Entrant[], curr) => [...acc, ...curr], [])
  )

  pagination$ = this.entrantsResult$.pipe(
    map(result => result.pagination),
    notNullOrUndefined()
  )

  loadMore() {
    let nextParams: EntrantParams;
    this.searchParams$.pipe(take(1)).subscribe(params => {
      params.pageNumber += 1;
      nextParams = params;
    });
    this.searchParams$.next(nextParams!);
  }
}
