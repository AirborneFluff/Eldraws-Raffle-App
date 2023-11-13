import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RaffleFormsModule } from '../../shared/forms/raffle-forms.module';
import { RaffleDetailsComponent } from './raffle-details/raffle-details.component';
import { EntryListComponent } from './entry-list/entry-list.component';
import { CoreModule } from '../../core/core.module';
import { CreateEntryComponent } from './create-entry/create-entry.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RaffleListComponent } from './raffle-list/raffle-list.component';
import { RouterLink } from '@angular/router';
import { CreateRaffleButtonComponent } from './create-raffle-button/create-raffle-button.component';
import { MatIconModule } from '@angular/material/icon';
import { PrizeListComponent } from './prize-list/prize-list.component';
import { CreatePrizeComponent } from './create-prize/create-prize.component';
import { CreatePrizeButtonComponent } from './create-prize-button/create-prize-button.component';
import { SharedModule } from '../../shared/shared.module';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { RaffleFormComponent } from './raffle-form/raffle-form.component';
import { DiscordModule } from '../discord/discord.module';
import { PrizeListItemComponent } from './prize-list-item/prize-list-item.component';
import { RollWinnerFormComponent } from './roll-winner-form/roll-winner-form.component';



@NgModule({
  declarations: [
    RaffleDetailsComponent,
    EntryListComponent,
    CreateEntryComponent,
    RaffleListComponent,
    CreateRaffleButtonComponent,
    PrizeListComponent,
    CreatePrizeComponent,
    CreatePrizeButtonComponent,
    RaffleFormComponent,
    PrizeListItemComponent,
    RollWinnerFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RaffleFormsModule,
    CoreModule,
    MatAutocompleteModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    RouterLink,
    MatIconModule,
    SharedModule,
    MatCheckboxModule,
    DiscordModule
  ],
  exports: [
    RaffleListComponent,
    CreateRaffleButtonComponent
  ]
})
export class RafflesModule { }
