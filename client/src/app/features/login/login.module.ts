import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RaffleFormsModule } from "../../shared/forms/raffle-forms.module";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';

@NgModule({
  declarations: [
    LoginComponent
  ],
  exports: [
    LoginComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RaffleFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterLink
  ]
})
export class LoginModule { }
