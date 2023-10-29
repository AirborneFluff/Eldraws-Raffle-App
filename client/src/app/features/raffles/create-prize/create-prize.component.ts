import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { BehaviorSubject, combineLatest, map, switchMap, take } from 'rxjs';
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
  private _dynamicPrize = true;

  nextPosition$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      if (!raffle.prizes.length) return 1;
      return raffle.prizes.reduce((max, curr) => curr > max ? curr : max).place + 1;
    })
  )

  position: FormControl = new FormControl<number>(1, Validators.min(1));
  donationPercentage: FormControl = new FormControl<number | null>(0, [GreaterThanValidator(0), Validators.max(100)]);
  description: FormControl = new FormControl<string>('', Validators.required);

  prizeForm: FormGroup = new FormGroup<any>({
    place: this.position,
    description: this.description
  })

  dynamicPrizeForm: FormGroup = new FormGroup<any>({
    place: this.position,
    donationPercentage: this.donationPercentage
  })

  currentForm$ = new BehaviorSubject<FormGroup>(this.dynamicPrizeForm);

  get dynamicPrize() { return this._dynamicPrize }
  set dynamicPrize(val: boolean) {
    this._dynamicPrize = val;
    this.donationPercentage.reset();
    this.description.reset();

    if (val) {
      this.currentForm$.next(this.dynamicPrizeForm)
      return;
    }

    this.currentForm$.next(this.prizeForm)
  }
  constructor(public bottomSheet: MatBottomSheet, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private raffle$: CurrentRaffleStream) {
    this.nextPosition$.pipe(
      take(1)
    ).subscribe(val => {
      this.position.setValue(val)
    })
  }

  submit() {
    const form = this._dynamicPrize ? this.dynamicPrizeForm : this.prizeForm;

    if (form.invalid) return;
    const newPrize: NewRafflePrize = form.value;

    if (this._dynamicPrize) {
      newPrize.donationPercentage = this.donationPercentage.value / 100;
    }

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
