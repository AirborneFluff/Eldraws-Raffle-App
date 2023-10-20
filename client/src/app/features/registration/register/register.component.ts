import { Component } from '@angular/core';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

  constructor(account: AccountService, router: Router) {
    account.currentUser$.subscribe(user => {
      if (!!user) router.navigate(['clans']);
    });
  }

}
