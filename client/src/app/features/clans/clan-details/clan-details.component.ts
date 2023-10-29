import { Component, OnDestroy } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { switchMap, tap, combineLatest, map, of, shareReplay } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialog } from '@angular/material/dialog';
import { CreateRaffleComponent } from '../../raffles/create-raffle/create-raffle.component';
import { AccountService } from '../../../core/services/account.service';
import { ClanStream } from '../../../core/streams/clan-stream';
import { PageTitleService } from '../../../core/services/page-title.service';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';

@Component({
  selector: 'app-clan-details',
  templateUrl: './clan-details.component.html',
  styleUrls: ['./clan-details.component.scss']
})
export class ClanDetailsComponent {
  constructor(public clan$: CurrentClanStream) {
  }
}
