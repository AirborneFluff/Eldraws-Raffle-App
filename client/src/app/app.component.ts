import { Component } from '@angular/core';
import { AccountService } from "./core/services/account.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  username: string = '';
  password: string = '';

  constructor(public account: AccountService) {
  }

  login() {
    this.account.login({
      userName : this.username,
      password : this.password
    }).subscribe()
  }

}
