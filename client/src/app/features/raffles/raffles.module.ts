import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateRaffleComponent } from './create-raffle/create-raffle.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RaffleFormsModule } from '../../shared/forms/raffle-forms.module';



@NgModule({
  declarations: [
    CreateRaffleComponent
  ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RaffleFormsModule
    ]
})
export class RafflesModule { }
