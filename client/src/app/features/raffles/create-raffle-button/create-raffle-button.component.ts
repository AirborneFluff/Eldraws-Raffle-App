import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { CreateRaffleComponent } from '../create-raffle/create-raffle.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'create-raffle-button',
  templateUrl: './create-raffle-button.component.html',
  styleUrls: ['./create-raffle-button.component.scss']
})
export class CreateRaffleButtonComponent {
  constructor(public clanId$: ClanIdStream, private dialog: MatDialog) {}



  openCreateRaffle() {
    this.dialog.open(CreateRaffleComponent);
  }

}
