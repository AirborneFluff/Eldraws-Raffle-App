import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { switchMap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-clan-details',
  templateUrl: './clan-details.component.html',
  styleUrls: ['./clan-details.component.scss']
})
export class ClanDetailsComponent {
  constructor(private clanId$: ClanIdStream, private api: ApiService) {
  }

  clan$ = this.clanId$.pipe(
    notNullOrUndefined(),
    switchMap(clanId => this.api.Clans.getById(clanId))
  )
}
