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
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { ClanMemberListComponent } from './clan-member-list/clan-member-list.component';
import { AddClanMemberButtonComponent } from './add-clan-member-button/add-clan-member-button.component';
import { AddClanMemberComponent } from './add-clan-member/add-clan-member.component';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ClanListPlaceholderComponent } from './clan-list-placeholder/clan-list-placeholder.component';
import { ClanFormComponent } from './clan-form/clan-form.component';
import { FormSheetModule } from '../../shared/form-sheet/form-sheet.module';



@NgModule({
  declarations: [
    ClanListComponent,
    ClanListItemComponent,
    CreateClanComponent,
    ClanDetailsComponent,
    CreateClanButtonComponent,
    ClanMemberListComponent,
    AddClanMemberButtonComponent,
    AddClanMemberComponent,
    ClanListPlaceholderComponent,
    ClanFormComponent
  ],
  imports: [
    CommonModule,
    RaffleFormsModule,
    ReactiveFormsModule,
    CoreModule,
    RouterLink,
    RafflesModule,
    MatDialogModule,
    MatExpansionModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormSheetModule
  ],
  exports: [
    ClanListComponent,
    ClanListItemComponent,
    CreateClanComponent,
  ]
})
export class ClansModule { }
