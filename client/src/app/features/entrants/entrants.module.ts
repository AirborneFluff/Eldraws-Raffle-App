import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EntrantListComponent } from './entrant-list/entrant-list.component';
import { CoreModule } from '../../core/core.module';
import { EntrantListItemComponent } from './entrant-list-item/entrant-list-item.component';
import { ClansModule } from '../clans/clans.module';
import { MatIconModule } from '@angular/material/icon';



@NgModule({
  declarations: [
    EntrantListComponent,
    EntrantListItemComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    ClansModule,
    MatIconModule
  ]
})
export class EntrantsModule { }
