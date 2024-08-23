import { Component, Input } from '@angular/core';
import { Entrant } from '../../../data/models/entrant';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { map, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-entrant-list-item',
  templateUrl: './entrant-list-item.component.html',
  styleUrls: ['./entrant-list-item.component.scss']
})
export class EntrantListItemComponent {
  @Input() entrant!: Entrant;

  constructor(private api: ApiService, private clanId$: ClanIdStream) {}

  toggleEntrantActivity() {
    this.clanId$.pipe(
      notNullOrUndefined(),
      take(1),
      switchMap(clanId => this.api.Clans.setMemberActivity(clanId, this.entrant.id, !this.entrant.active))
    ).pipe(take(1))
      .subscribe(entrant => this.entrant = entrant);
  }
}
