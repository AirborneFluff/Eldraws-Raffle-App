import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiscordButtonComponent } from './discord-button/discord-button.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DiscordFormComponent } from './discord-form/discord-form.component';
import { FormSheetModule } from '../../shared/form-sheet/form-sheet.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    DiscordButtonComponent,
    DiscordFormComponent
  ],
  exports: [
    DiscordButtonComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    FormSheetModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule
  ]
})
export class DiscordModule { }
