import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../../../core/services/account.service';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm!: FormGroup;

  userName = new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9-._@+]+$/)])
  password = new FormControl('', [Validators.required, Validators.minLength(6)])

  invalidCredentials$ = new BehaviorSubject(false);

  constructor(private account: AccountService, router: Router) {
    this.account.currentUser$.subscribe(user => {
      if (!!user) router.navigate(['clans']);
    });
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = new FormGroup<any>({
      userName: this.userName,
      password: this.password
    })
  }

  login() {
    if (this.loginForm.invalid) return;

    this.account.login(this.loginForm.value).subscribe({
      error: () => {
        this.invalidCredentials$.next(true);
      }
    });
  }
}
