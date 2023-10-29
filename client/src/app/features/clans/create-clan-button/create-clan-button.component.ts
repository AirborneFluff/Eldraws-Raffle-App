import { Component } from '@angular/core';
import { CreateClanComponent } from '../create-clan/create-clan.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-create-clan-button',
  templateUrl: './create-clan-button.component.html',
  styleUrls: ['./create-clan-button.component.scss']
})
export class CreateClanButtonComponent {
  constructor(private dialog: MatDialog) {
  }

  openCreateClan() {
    this.dialog.open(CreateClanComponent)
  }
}
