import { Component } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'clan-list',
  templateUrl: './clan-list.component.html',
  styleUrls: ['./clan-list.component.scss']
})
export class ClanListComponent {
  clans$ = this.api.Clans.getAll();

  constructor(private api: ApiService) {
  }
}
