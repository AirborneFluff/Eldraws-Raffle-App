import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-clans-list',
  templateUrl: './clans-list.component.html',
  styleUrls: ['./clans-list.component.scss']
})
export class ClansListComponent implements OnInit {

  constructor(private api: ApiService) {
  }
  ngOnInit(): void {
    this.api.Clans.getById(1).subscribe(clan => console.log(clan))
  }

}
