import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateRaffleComponent } from './create-raffle/create-raffle.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RaffleFormsModule } from '../../shared/forms/raffle-forms.module';
import { RaffleDetailsComponent } from './raffle-details/raffle-details.component';



@NgModule({
  declarations: [
    CreateRaffleComponent,
    RaffleDetailsComponent
  ],
  imports: [
      CommonModule,
      ReactiveFormsModule,
      RaffleFormsModule
  ]
})
export class RafflesModule { }
