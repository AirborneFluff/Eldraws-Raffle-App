import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-frame',
  templateUrl: './app-frame.component.html',
  styleUrls: ['./app-frame.component.scss']
})
export class AppFrameComponent {

  constructor(public account: AccountService, public title: Title) {
  }
}
