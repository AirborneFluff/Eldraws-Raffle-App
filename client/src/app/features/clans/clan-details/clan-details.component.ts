import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { switchMap, tap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialog } from '@angular/material/dialog';
import { CreateRaffleComponent } from '../../raffles/create-raffle/create-raffle.component';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-clan-details',
  templateUrl: './clan-details.component.html',
  styleUrls: ['./clan-details.component.scss']
})
export class ClanDetailsComponent {
  constructor(private clanId$: ClanIdStream, private api: ApiService, private dialog: MatDialog, private title: Title) {
  }

  clan$ = this.clanId$.pipe(
    notNullOrUndefined(),
    switchMap(clanId => this.api.Clans.getById(clanId)),
    tap(clan => this.title.setTitle(clan.name))
  )

  openCreateRaffle() {
    this.dialog.open(CreateRaffleComponent);
  }
}
