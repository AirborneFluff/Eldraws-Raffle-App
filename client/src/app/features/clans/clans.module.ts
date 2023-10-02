import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClanListComponent } from './clan-list/clan-list.component';
import { ClanListItemComponent } from './clan-list-item/clan-list-item.component';
import { FormsModule } from '../../shared/forms/forms.module';



@NgModule({
  declarations: [
    ClanListComponent,
    ClanListItemComponent,
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    ClanListComponent,
    ClanListItemComponent,
  ]
})
export class ClansModule { }
