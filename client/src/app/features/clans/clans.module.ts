import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClansListComponent } from './clans-list/clans-list.component';



@NgModule({
  declarations: [
    ClansListComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ClansListComponent
  ]
})
export class ClansModule { }
