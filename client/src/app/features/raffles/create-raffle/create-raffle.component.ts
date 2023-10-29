import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { switchMap, take, withLatestFrom } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Router } from '@angular/router';
import { NewRaffle } from '../../../data/models/new-raffle';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { MatDialogRef } from '@angular/material/dialog';
import { parseNumericSuffix } from '../../../core/utils/parse-numeric-suffix';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';


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


  constructor(private api: ApiService, private router: Router, private clanId$: ClanIdStream, public dialogRef: MatDialogRef<CreateRaffleComponent>, private clan$: CurrentClanStream) {
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
    const raffle: NewRaffle = this.raffleForm.value;
    raffle.entryCost = parseNumericSuffix(raffle.entryCost.toString());

    this.clanId$.pipe(
      notNullOrUndefined(),
      take(1),
      switchMap(clanId => this.api.Raffles.addNew(clanId, raffle)),
      withLatestFrom(this.clan$.pipe(notNullOrUndefined()))
    ).subscribe({
        next: ([newRaffle, clan]) => {
          clan.raffles.push(newRaffle);
          this.clan$.next(clan);
          this.router.navigateByUrl('/clans/' + newRaffle.clan.id + '/raffles/' + newRaffle.id, { state: newRaffle });
          this.dialogRef.close();
        }
      }
    )
  }
}
