import { Component } from '@angular/core';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormControl,
  FormGroup, ValidationErrors,
  Validators
} from '@angular/forms';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import {
  BehaviorSubject,
  finalize,
  map,
  Observable,
  switchMap,
  take,
  takeUntil,
  timer
} from 'rxjs';
import { Clan } from '../../../data/models/clan';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { NewClan } from '../../../data/models/new-clan';
import { Router } from '@angular/router';

@Component({
  selector: 'app-clan-form',
  templateUrl: './clan-form.component.html',
  styleUrls: ['./clan-form.component.scss']
})
export class ClanFormComponent {

  name = new FormControl('', Validators.required, this.clanExistsValidator());
  discordChannelId = new FormControl('', Validators.pattern('^\\d{17}$|^\\d{18}$|^\\d{19}$'));

  clanForm = new FormGroup({
    name: this.name,
    discordChannelId: this.discordChannelId
  })

  checkingName$ = new BehaviorSubject(false);

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
          console.log(clan)
          this.clan$.next(clan);
          this.bottomSheet.dismiss();
        }
      }
    )
  }


  clanExistsValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      this.checkingName$.next(true);

      return timer(500).pipe(
        switchMap(() => this.api.Clans.exists(control.value)
        .pipe(
          map((result: boolean) =>
            result ? {exists: true} : null
          ),
          finalize(() => this.checkingName$.next(false)),
          takeUntil(timer(5000))
        )));
    };
  }
}
