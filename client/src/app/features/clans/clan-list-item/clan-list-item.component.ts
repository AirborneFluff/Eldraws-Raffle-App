import { Component, Input } from '@angular/core';
import { Clan } from '../../../data/models/clan';

@Component({
  selector: 'clan-list-item',
  templateUrl: './clan-list-item.component.html',
  styleUrls: ['./clan-list-item.component.scss']
})
export class ClanListItemComponent {
  @Input() clan!: Clan;

}
