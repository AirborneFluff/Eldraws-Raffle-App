import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { switchMap } from 'rxjs';
import { EntrantParams } from '../../../data/params/entrant-params';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-entrant-list',
  templateUrl: './entrant-list.component.html',
  styleUrls: ['./entrant-list.component.scss']
})
export class EntrantListComponent {

  constructor(private clanId$: ClanIdStream, private api: ApiService) {
    let params: EntrantParams = {
      pageSize: 10,
      pageNumber: 1,
      orderBy: 'totalDonations'
    }

    this.clanId$.pipe(
      notNullOrUndefined(),
      switchMap(clanId => this.api.Clans.getEntrants(clanId,params))
    ).subscribe(val => console.log(val))
  }
}
