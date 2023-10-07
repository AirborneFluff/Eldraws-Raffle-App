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
import { RaffleListComponent } from './raffle-list/raffle-list.component';
import { RaffleListItemComponent } from './raffle-list-item/raffle-list-item.component';



@NgModule({
  declarations: [
    ClanListComponent,
    ClanListItemComponent,
    CreateClanComponent,
    ClanDetailsComponent,
    RaffleListComponent,
    RaffleListItemComponent
  ],
  imports: [
    CommonModule,
    RaffleFormsModule,
    ReactiveFormsModule,
    CoreModule,
    RouterLink,
  ],
  exports: [
    ClanListComponent,
    ClanListItemComponent,
    CreateClanComponent,
  ]
})
export class ClansModule { }
