import { Component, Input } from '@angular/core';
import { Clan } from '../../../data/models/clan';
import { MatDialog } from '@angular/material/dialog';
import { CreateRaffleComponent } from '../../raffles/create-raffle/create-raffle.component';

@Component({
  selector: 'clan-list-item',
  templateUrl: './clan-list-item.component.html',
  styleUrls: ['./clan-list-item.component.scss']
})
export class ClanListItemComponent {
  @Input() clan!: Clan;

  constructor(private dialog: MatDialog) {
  }

  openCreateRaffle() {
    this.dialog.open(CreateRaffleComponent)
  }
}
