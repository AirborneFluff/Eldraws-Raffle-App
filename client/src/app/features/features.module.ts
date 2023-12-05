import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginModule } from "./login/login.module";
import { RegistrationModule } from "./registration/registration.module";
import { ClansModule } from './clans/clans.module';
import { RafflesModule } from './raffles/raffles.module';
import { EntrantsModule } from './entrants/entrants.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RegistrationModule,
    LoginModule,
    ClansModule,
    RafflesModule,
    EntrantsModule
  ],
  exports: [
    LoginModule,
    RegistrationModule
  ]
})
export class FeaturesModule { }
