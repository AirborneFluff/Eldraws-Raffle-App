import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { combineLatest, map, switchMap, take } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { NewRafflePrize } from '../../../data/models/new-prize';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { GreaterThanValidator } from '../../../core/validators/greater-than-validator';


@Component({
  selector: 'app-create-prize',
  templateUrl: './create-prize.component.html',
  styleUrls: ['./create-prize.component.scss']
})
export class CreatePrizeComponent {
  private _usePercentage = true;

  nextPosition$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      if (!raffle.prizes.length) return 1;
      return raffle.prizes.reduce((max, curr) => curr > max ? curr : max).place + 1;
    })
  )

  position: FormControl = new FormControl<number>(1, Validators.min(1));
  donationPercentage: FormControl = new FormControl<number | null>(0, [GreaterThanValidator(0), Validators.max(100)]);
  description: FormControl = new FormControl<string>('');

  prizeForm: FormGroup = new FormGroup<any>({
    place: this.position,
    donationPercentage: this.donationPercentage,
    description: this.description
  })

  get usePercentage() { return this._usePercentage }
  set usePercentage(val: boolean) {
    this._usePercentage = val;
    if (!val) {
      this.donationPercentage.setValue(0);
      this.description.setValue('');
      return;
    }
  }
  constructor(public bottomSheet: MatBottomSheet, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private raffle$: CurrentRaffleStream) {
    this.nextPosition$.pipe(
      take(1)
    ).subscribe(val => {
      this.position.setValue(val)
    })
  }

  submit() {
    if (this.prizeForm.invalid) return;
    const newPrize: NewRafflePrize = this.prizeForm.value;

    newPrize.donationPercentage = this.donationPercentage.value / 100;
    if (newPrize.donationPercentage) newPrize.description = '';

    combineLatest([
        this.clanId$.pipe(notNullOrUndefined()),
        this.raffleId$.pipe(notNullOrUndefined())
    ]).pipe(
      take(1),
      switchMap(([clanId, raffleId]) => {
        return this.api.Raffles.addPrize(clanId, raffleId, newPrize)
    })).subscribe(raffle => {
      this.raffle$.next(raffle);
      this.bottomSheet.dismiss();
    })
  }
}
