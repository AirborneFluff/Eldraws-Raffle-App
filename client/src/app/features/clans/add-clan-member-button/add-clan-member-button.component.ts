import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddClanMemberComponent } from '../add-clan-member/add-clan-member.component';

@Component({
  selector: 'add-clan-member-button',
  templateUrl: './add-clan-member-button.component.html',
  styleUrls: ['./add-clan-member-button.component.scss']
})
export class AddClanMemberButtonComponent {

  constructor(private dialog: MatDialog) {
  }
  openAddMember() {
    this.dialog.open(AddClanMemberComponent)
  }
}
