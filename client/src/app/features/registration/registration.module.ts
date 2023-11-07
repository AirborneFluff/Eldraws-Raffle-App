import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { RaffleFormsModule } from '../../shared/forms/raffle-forms.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';



@NgModule({
  declarations: [
    RegisterComponent
  ],
  imports: [
    CommonModule,
    RaffleFormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    RouterLink
  ],
  exports: [
    RegisterComponent
  ]
})
export class RegistrationModule { }
