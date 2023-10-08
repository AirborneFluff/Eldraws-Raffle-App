import { Component } from '@angular/core';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';

@Component({
  selector: 'create-raffle-button',
  templateUrl: './create-raffle-button.component.html',
  styleUrls: ['./create-raffle-button.component.scss']
})
export class CreateRaffleButtonComponent {
  constructor(public clanId$: ClanIdStream) {}

}
