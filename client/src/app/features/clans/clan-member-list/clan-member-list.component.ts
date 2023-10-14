import { Component, Input } from '@angular/core';
import { Member } from '../../../data/models/member';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { of, switchMap, withLatestFrom } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { ClanStream } from '../../../core/streams/clan-stream';

@Component({
  selector: 'clan-member-list',
  templateUrl: './clan-member-list.component.html',
  styleUrls: ['./clan-member-list.component.scss']
})
export class ClanMemberListComponent {
  @Input() members: Member[] = [];

  constructor(public dialog: MatDialog, private api: ApiService, private clanId$: ClanIdStream, private clan$: ClanStream) {}

  openConfirmDialog(member: Member) {
    this.dialog.open(ConfirmDialogComponent, {
      data : {
        title: 'Are you sure?',
        message: `This will remove ${member.userName}'s permissions to edit raffles in your clan.`,
        btnOkText: 'Yes',
        btnCancelText: 'Cancel'
      }
    }).afterClosed()
      .pipe(
        withLatestFrom(this.clanId$.pipe(notNullOrUndefined())),
        switchMap(([confirm, clanId]) => {
          if (confirm) return this.api.Clans.removeMember(clanId, member.id)
          return of();
      })).subscribe(clan => {
        if (clan) this.clan$.next(clan);
    })
  }
}
