import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, switchMap } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Router } from '@angular/router';
import { NewRaffle } from '../../../data/models/new-raffle';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialogRef } from '@angular/material/dialog';


const INITIAL_OPEN_DATE = new Date(new Date().setMinutes(0));
const INITIAL_CLOSE_DATE = new Date(new Date().setMinutes(0));
const INITIAL_DRAW_DATE = new Date(new Date().setMinutes(0));

INITIAL_CLOSE_DATE.setTime(INITIAL_OPEN_DATE.getTime() + 7 * 86400000);
INITIAL_DRAW_DATE.setTime(INITIAL_CLOSE_DATE.getTime() + 3600000);

@Component({
  selector: 'app-create-raffle',
  templateUrl: './create-raffle.component.html',
  styleUrls: ['./create-raffle.component.scss']
})
export class CreateRaffleComponent {
  raffleForm!: FormGroup;

  name = new FormControl('', Validators.required)
  entryCost = new FormControl(5000, Validators.required)
  openDate = new FormControl(INITIAL_OPEN_DATE, Validators.required)
  closeDate = new FormControl(INITIAL_CLOSE_DATE, Validators.required)
  drawDate = new FormControl(INITIAL_DRAW_DATE, Validators.required)

  invalidForm$ = new Subject<boolean>();

  constructor(private api: ApiService, private router: Router, private clanId$: ClanIdStream, public dialogRef: MatDialogRef<CreateRaffleComponent>) {
    this.initializeForm();
  }

  initializeForm() {
    this.raffleForm = new FormGroup<any>({
      title: this.name,
      entryCost: this.entryCost,
      openDate: this.openDate,
      closeDate: this.closeDate,
      drawDate: this.drawDate
    })
  }

  submit() {
    if (this.raffleForm.invalid) return;

    this.clanId$.pipe(
      notNullOrUndefined(),
      switchMap(clanId => this.api.Raffles.addNew(clanId, this.raffleForm.value as NewRaffle))
    ).subscribe({
        next: newRaffle => {
          this.invalidForm$.next(false);
          this.router.navigateByUrl('/clans/' + newRaffle.clan.id + '/raffles/' + newRaffle.id, { state: newRaffle });
          this.dialogRef.close();
        },
        error: () => {
          this.invalidForm$.next(true);
        }
      }
    )
  }
}
