import { Component } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { CreateClanComponent } from '../create-clan/create-clan.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'clan-list',
  templateUrl: './clan-list.component.html',
  styleUrls: ['./clan-list.component.scss']
})
export class ClanListComponent {
  clans$ = this.api.Clans.getAll();

  constructor(private api: ApiService, private dialog: MatDialog) {
  }

  openCreateClan() {
    this.dialog.open(CreateClanComponent)
  }
}
