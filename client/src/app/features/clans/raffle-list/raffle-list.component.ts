import { Component, Input } from '@angular/core';
import { Raffle } from '../../../data/models/raffle';

@Component({
  selector: 'app-raffle-list',
  templateUrl: './raffle-list.component.html',
  styleUrls: ['./raffle-list.component.scss']
})
export class RaffleListComponent {
  @Input() raffles: Raffle[] = [];
}
