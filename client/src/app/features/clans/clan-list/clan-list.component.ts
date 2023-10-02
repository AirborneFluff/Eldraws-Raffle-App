import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { Clan } from '../../../data/models/clan';

@Component({
  selector: 'clan-list',
  templateUrl: './clan-list.component.html',
  styleUrls: ['./clan-list.component.scss']
})
export class ClanListComponent implements OnInit {
  clans$: Observable<Clan[]>;

  constructor(private api: ApiService) {
    this.clans$ = this.api.Clans.getAll();
  }
  ngOnInit(): void {
  }

}
