import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { combineLatest, switchMap, take } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { NewRafflePrize } from '../../../data/models/new-prize';
import { RaffleStream } from '../../../core/streams/raffle-stream';


@Component({
  selector: 'app-create-prize',
  templateUrl: './create-prize.component.html',
  styleUrls: ['./create-prize.component.scss']
})
export class CreatePrizeComponent {
  private _usePercentage = true;

  position: FormControl = new FormControl<number>(1, Validators.min(1));
  donationPercentage: FormControl = new FormControl<number | null>(0, [Validators.min(0), Validators.max(100)]);
  description: FormControl = new FormControl<any>('');

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
  constructor(public bottomSheet: MatBottomSheet, private api: ApiService, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private raffleUpdates: RaffleStream) {
  }

  submit() {
    if (this.prizeForm.invalid) return;
    const newPrize: NewRafflePrize = this.prizeForm.value;

    newPrize.donationPercentage = this.donationPercentage.value / 100;
    if (this.donationPercentage) newPrize.description = '';

    combineLatest([
        this.clanId$.pipe(notNullOrUndefined()),
        this.raffleId$.pipe(notNullOrUndefined())
    ]).pipe(
      take(1),
      switchMap(([clanId, raffleId]) => {
        return this.api.Raffles.addPrize(clanId, raffleId, newPrize)
    })).subscribe(raffle => {
      this.raffleUpdates.next(raffle);
      this.bottomSheet.dismiss();
    })
  }
}
