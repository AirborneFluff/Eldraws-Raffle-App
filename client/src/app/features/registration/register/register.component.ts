import { Component, OnDestroy } from '@angular/core';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { BehaviorSubject, finalize, Subscription } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnDestroy {
  registerForm!: FormGroup;

  userName = new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9-._@+]+$/)])
  password = new FormControl('', [Validators.required, Validators.minLength(6)])
  confirmPassword = new FormControl('', [Validators.required, Validators.minLength(6), this.matchValues('password')])

  registrationError$ = new BehaviorSubject<string | null>(null);
  submitted$ = new BehaviorSubject(false);

  subscriptions = new Subscription();

  constructor(private account: AccountService, router: Router) {
    this.account.currentUser$.subscribe(user => {
      if (!!user) router.navigate(['clans']);
    });

    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup<any>({
      userName: this.userName,
      password: this.password,
      confirmPassword: this.confirmPassword
    })

    this.subscriptions.add(
      this.password.valueChanges.subscribe(() => {
        this.confirmPassword.updateValueAndValidity();
      })
    )
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  matchValues(matchTo: string): ValidatorFn {
    return(control: AbstractControl) => {
      const matchedControl = control?.parent?.get(matchTo);
      return control?.value === matchedControl?.value ? null : { isMatching: true }
    }
  }

  register() {
    if (this.registerForm.invalid) return;

    this.submitted$.next(true);
    this.account.register(this.registerForm.value).pipe(
      finalize(() => this.submitted$.next(false))
    ).subscribe({
      error: err => {
        this.registrationError$.next(err.error)
      }
    })
  }
}
