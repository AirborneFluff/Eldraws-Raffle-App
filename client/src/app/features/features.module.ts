import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginModule } from "./login/login.module";
import { RegistrationModule } from "./registration/registration.module";
import { ClansModule } from './clans/clans.module';
import { RafflesModule } from './raffles/raffles.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RegistrationModule,
    LoginModule,
    ClansModule,
    RafflesModule
  ],
  exports: [
    LoginModule,
    RegistrationModule
  ]
})
export class FeaturesModule { }
