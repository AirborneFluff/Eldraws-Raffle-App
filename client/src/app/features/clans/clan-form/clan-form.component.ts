import { Component } from '@angular/core';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import {
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { switchMap, take } from 'rxjs';
import { Clan } from '../../../data/models/clan';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { NewClan } from '../../../data/models/new-clan';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-clan-form',
  templateUrl: './clan-form.component.html',
  styleUrls: ['./clan-form.component.scss']
})
export class ClanFormComponent {

  name = new FormControl('', Validators.required);
  discordChannelId = new FormControl('', Validators.pattern('^\\d{17}$|^\\d{18}$|^\\d{19}$'));

  clanForm = new FormGroup({
    name: this.name,
    discordChannelId: this.discordChannelId
  })
  constructor(public clan$: CurrentClanStream, public bottomSheet: MatBottomSheet, private clanId$: ClanIdStream, private api: ApiService, private router: Router) {
    this.clan$.pipe(notNullOrUndefined(), take(1)).subscribe(clan => {
      this.patchValues(clan);
      this.name.disable();
    })
  }

  patchValues(clan: Clan) {
    this.clanForm.patchValue(clan);
  }

  createClan() {
    this.api.Clans.addNew(this.clanForm.value as NewClan)
      .subscribe({
          next: newClan => {
            this.clan$.next(newClan);
            this.router.navigate(['clans', newClan.id]);
            this.bottomSheet.dismiss();
          },
          error: (requestError: HttpErrorResponse) => {
            this.handleResponseError(requestError);
          }
        }
      )
  }

  updateClan() {
    this.clanId$.pipe(
      take(1),
      notNullOrUndefined(),
      switchMap(clanId => {
        return this.api.Clans.update(clanId, this.clanForm.value as NewClan)
      })
    ).subscribe({
        next: clan => {
          this.clan$.next(clan);
          this.bottomSheet.dismiss();
        },
        error: (requestError: HttpErrorResponse) => {
          this.handleResponseError(requestError);
        }
      }
    )
  }

  handleResponseError(error: HttpErrorResponse) {
  }
}
