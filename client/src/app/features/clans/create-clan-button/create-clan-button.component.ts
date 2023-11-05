import { Component } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ClanFormComponent } from '../clan-form/clan-form.component';

@Component({
  selector: 'app-create-clan-button',
  templateUrl: './create-clan-button.component.html',
  styleUrls: ['./create-clan-button.component.scss']
})
export class CreateClanButtonComponent {
  constructor(private bottomSheet: MatBottomSheet) {
  }

  openCreateClan() {
    this.bottomSheet.open(ClanFormComponent);
  }
}
