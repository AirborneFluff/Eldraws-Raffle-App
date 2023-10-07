import { Component, Input } from '@angular/core';
import { Raffle } from '../../../data/models/raffle';

@Component({
  selector: 'app-raffle-list-item',
  templateUrl: './raffle-list-item.component.html',
  styleUrls: ['./raffle-list-item.component.scss']
})
export class RaffleListItemComponent {
  @Input() raffle: Raffle | undefined = undefined;

}
