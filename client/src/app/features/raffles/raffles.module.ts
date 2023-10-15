import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateRaffleComponent } from './create-raffle/create-raffle.component';
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
import { RaffleListItemComponent } from './raffle-list-item/raffle-list-item.component';
import { RaffleListComponent } from './raffle-list/raffle-list.component';
import { RouterLink } from '@angular/router';
import { CreateRaffleButtonComponent } from './create-raffle-button/create-raffle-button.component';
import { MatIconModule } from '@angular/material/icon';



@NgModule({
  declarations: [
    CreateRaffleComponent,
    RaffleDetailsComponent,
    EntryListComponent,
    CreateEntryComponent,
    RaffleListItemComponent,
    RaffleListComponent,
    CreateRaffleButtonComponent
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
    MatIconModule
  ],
  exports: [
    RaffleListItemComponent,
    RaffleListComponent,
    CreateRaffleButtonComponent
  ]
})
export class RafflesModule { }
