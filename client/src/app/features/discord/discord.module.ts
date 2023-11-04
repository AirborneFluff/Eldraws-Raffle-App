import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiscordButtonComponent } from './discord-button/discord-button.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';



@NgModule({
  declarations: [
    DiscordButtonComponent
  ],
  exports: [
    DiscordButtonComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class DiscordModule { }
