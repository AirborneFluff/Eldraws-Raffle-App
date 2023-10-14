import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { switchMap } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { ClanStream } from '../../../core/streams/clan-stream';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-clan-member',
  templateUrl: './add-clan-member.component.html',
  styleUrls: ['./add-clan-member.component.scss']
})
export class AddClanMemberComponent {

  memberId = new FormControl('', Validators.required);

  constructor(private api: ApiService, private clanId$: ClanIdStream, private clan$: ClanStream, public dialogRef: MatDialogRef<AddClanMemberComponent>) {
  }

  addMember() {
    if (this.memberId.invalid) return;

    this.clanId$.pipe(
      notNullOrUndefined(),
      switchMap(clanId => this.api.Clans.addMember(clanId, this.memberId.value as string))
    ).subscribe(clan => {
      this.clan$.next(clan);
      this.dialogRef.close();
    })
  }

}
