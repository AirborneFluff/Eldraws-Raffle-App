import { Component, Input } from '@angular/core';
import { RaffleEntry } from '../../../data/models/raffle-entry';

@Component({
  selector: 'app-entry-list',
  templateUrl: './entry-list.component.html',
  styleUrls: ['./entry-list.component.scss']
})
export class EntryListComponent {
  @Input() entries: RaffleEntry[] = [];
}
