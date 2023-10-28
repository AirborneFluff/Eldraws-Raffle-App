import { Component, Input } from '@angular/core';
import { RafflePrize } from '../../../data/models/raffle-prize';

@Component({
  selector: 'app-prize-list',
  templateUrl: './prize-list.component.html',
  styleUrls: ['./prize-list.component.scss']
})
export class PrizeListComponent {
  @Input() prizes: RafflePrize[] = [];
  @Input() totalDonations: number = 0;

}
