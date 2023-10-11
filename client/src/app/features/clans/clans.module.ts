import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClanListComponent } from './clan-list/clan-list.component';
import { ClanListItemComponent } from './clan-list-item/clan-list-item.component';
import { RaffleFormsModule } from '../../shared/forms/raffle-forms.module';
import { CreateClanComponent } from './create-clan/create-clan.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ClanDetailsComponent } from './clan-details/clan-details.component';
import { CoreModule } from '../../core/core.module';
import { RouterLink } from '@angular/router';
import { RafflesModule } from '../raffles/raffles.module';
import { CreateClanButtonComponent } from './create-clan-button/create-clan-button.component';
import { MatDialogModule } from '@angular/material/dialog';



@NgModule({
  declarations: [
    ClanListComponent,
    ClanListItemComponent,
    CreateClanComponent,
    ClanDetailsComponent,
    CreateClanButtonComponent
  ],
  imports: [
    CommonModule,
    RaffleFormsModule,
    ReactiveFormsModule,
    CoreModule,
    RouterLink,
    RafflesModule,
    MatDialogModule
  ],
  exports: [
    ClanListComponent,
    ClanListItemComponent,
    CreateClanComponent,
  ]
})
export class ClansModule { }
