import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { switchMap, take } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialogRef } from '@angular/material/dialog';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';

@Component({
  selector: 'app-add-clan-member',
  templateUrl: './add-clan-member.component.html',
  styleUrls: ['./add-clan-member.component.scss']
})
export class AddClanMemberComponent {

  memberId = new FormControl('', Validators.required);

  constructor(private api: ApiService, private clanId$: ClanIdStream, private clan$: CurrentClanStream, public dialogRef: MatDialogRef<AddClanMemberComponent>) {
  }

  addMember() {
    if (this.memberId.invalid) return;

    this.clanId$.pipe(
      take(1),
      notNullOrUndefined(),
      switchMap(clanId => this.api.Clans.addMember(clanId, this.memberId.value as string))
    ).subscribe(clan => {
      this.clan$.next(clan);
      this.dialogRef.close();
    })
  }

}
