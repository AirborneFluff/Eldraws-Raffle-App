import { Component, Input } from '@angular/core';
import { Entrant } from '../../../data/models/entrant';

@Component({
  selector: 'app-entrant-list-item',
  templateUrl: './entrant-list-item.component.html',
  styleUrls: ['./entrant-list-item.component.scss']
})
export class EntrantListItemComponent {
  @Input() entrant!: Entrant;

}
