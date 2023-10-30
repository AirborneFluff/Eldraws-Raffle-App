import { Component } from '@angular/core';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';

@Component({
  selector: 'app-clan-details',
  templateUrl: './clan-details.component.html',
  styleUrls: ['./clan-details.component.scss']
})
export class ClanDetailsComponent {
  constructor(public clan$: CurrentClanStream) {
  }
}
