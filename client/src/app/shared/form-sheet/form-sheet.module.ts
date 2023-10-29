import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SheetContainerComponent } from './sheet-container/sheet-container.component';
import { SheetHeaderComponent } from './sheet-header/sheet-header.component';
import { SheetFooterComponent } from './sheet-footer/sheet-footer.component';



@NgModule({
  declarations: [
    SheetContainerComponent,
    SheetHeaderComponent,
    SheetFooterComponent
  ],
  exports: [
    SheetContainerComponent,
    SheetHeaderComponent,
    SheetFooterComponent
  ],
  imports: [
    CommonModule
  ]
})
export class FormSheetModule { }
