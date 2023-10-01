import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm!: FormGroup;

  userName = new FormControl('', Validators.required)
  password = new FormControl('', Validators.required)

  constructor(private fb: FormBuilder, private account: AccountService) {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = new FormGroup<any>({
      userName: this.userName,
      password: this.password
    })
  }

  login() {
    console.log(this.loginForm.value)
    if (this.loginForm.invalid) return;

    this.account.login(this.loginForm.value).subscribe(user => {
        console.log(user);
      })
  }
}
