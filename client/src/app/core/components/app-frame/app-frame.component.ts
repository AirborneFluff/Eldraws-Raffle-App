import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-frame',
  templateUrl: './app-frame.component.html',
  styleUrls: ['./app-frame.component.scss']
})
export class AppFrameComponent {

  constructor(public account: AccountService) {
  }
}
