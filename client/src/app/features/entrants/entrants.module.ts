import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EntrantListComponent } from './entrant-list/entrant-list.component';
import { CoreModule } from '../../core/core.module';
import { EntrantListItemComponent } from './entrant-list-item/entrant-list-item.component';



@NgModule({
  declarations: [
    EntrantListComponent,
    EntrantListItemComponent
  ],
  imports: [
    CommonModule,
    CoreModule
  ]
})
export class EntrantsModule { }
