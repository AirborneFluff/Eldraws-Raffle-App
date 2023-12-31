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
import { RollWinnersFormComponent } from './roll-winners-form/roll-winners-form.component';
import { RollWinnersButtonComponent } from './roll-winners-button/roll-winners-button.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SharedModule } from '../../shared/shared.module';



@NgModule({
  declarations: [
    DiscordButtonComponent,
    DiscordFormComponent,
    RollWinnersFormComponent,
    RollWinnersButtonComponent
  ],
  exports: [
    DiscordButtonComponent,
    RollWinnersButtonComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    FormSheetModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    SharedModule
  ]
})
export class DiscordModule { }
