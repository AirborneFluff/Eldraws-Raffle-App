import { Component } from '@angular/core';
import { Member } from '../../../data/models/member';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { map, of, switchMap, withLatestFrom } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'clan-member-list',
  templateUrl: './clan-member-list.component.html',
  styleUrls: ['./clan-member-list.component.scss']
})
export class ClanMemberListComponent {
  constructor(public dialog: MatDialog, private api: ApiService, private clanId$: ClanIdStream, private clan$: CurrentClanStream, private account: AccountService) {}

  isOwner$ = this.clan$.pipe(
    notNullOrUndefined(),
    withLatestFrom(this.account.currentUser$.pipe(notNullOrUndefined())),
    map(([clan, user]) => {
      return clan.owner.id == user.id
    })
  )

  ownerId$ = this.clan$.pipe(
    notNullOrUndefined(),
    map(clan => clan.owner.id)
  )

  members$ = this.clan$.pipe(
    notNullOrUndefined(),
    map(clan => clan.members)
  )

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
