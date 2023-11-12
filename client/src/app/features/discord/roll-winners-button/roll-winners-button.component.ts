import { Component } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { RollWinnersFormComponent } from '../roll-winners-form/roll-winners-form.component';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { map } from 'rxjs';

@Component({
  selector: 'roll-winners-button',
  templateUrl: './roll-winners-button.component.html',
  styleUrls: ['./roll-winners-button.component.scss']
})
export class RollWinnersButtonComponent {

  constructor(private bottomSheet: MatBottomSheet, private raffle$: CurrentRaffleStream) {
  }

  disabled$ = this.raffle$.pipe(
      notNullOrUndefined(),
      map(raffle => !raffle.discordMessageId || !raffle.prizes.length))

  openRoll() {
    this.bottomSheet.open(RollWinnersFormComponent)
  }
}
