import { Component } from '@angular/core';
import { CreatePrizeComponent } from '../create-prize/create-prize.component';
import { MatBottomSheet } from '@angular/material/bottom-sheet';

@Component({
  selector: 'app-create-prize-button',
  templateUrl: './create-prize-button.component.html',
  styleUrls: ['./create-prize-button.component.scss']
})
export class CreatePrizeButtonComponent {
  constructor(private bottomSheet: MatBottomSheet) {
    this.open();
  }

  open() {
    this.bottomSheet.open(CreatePrizeComponent);
  }
}
