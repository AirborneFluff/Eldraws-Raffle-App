import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../../../core/services/account.service';
import { BehaviorSubject, finalize, Subject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm!: FormGroup;

  userName = new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9-._@+]+$/)])
  password = new FormControl('', [Validators.required, Validators.minLength(6)])

  error$ = new Subject<string>();
  submitted$ = new BehaviorSubject(false);

  constructor(private account: AccountService, router: Router, route: ActivatedRoute) {
    const redirectParams = route.snapshot.queryParams['redirectURL'] ?? 'clans';
    this.account.currentUser$.subscribe(user => {
      if (!!user) router.navigate([redirectParams]);
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

    this.submitted$.next(true);
    this.account.login(this.loginForm.value).pipe(
      finalize(() => this.submitted$.next(false))
    ).subscribe({
      error: err => {
        if (err.status == 401) {
          this.error$.next("Invalid login credentials");
          return;
        }
        this.error$.next("There was a problem connecting to the server");
      }
    });
  }
}
