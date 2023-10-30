import { Component } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { RaffleFormComponent } from '../raffle-form/raffle-form.component';

@Component({
  selector: 'create-raffle-button',
  templateUrl: './create-raffle-button.component.html',
  styleUrls: ['./create-raffle-button.component.scss']
})
export class CreateRaffleButtonComponent {
  constructor(private bottomSheet: MatBottomSheet) {}

  openCreateRaffle() {
    this.bottomSheet.open(RaffleFormComponent);
  }
}
