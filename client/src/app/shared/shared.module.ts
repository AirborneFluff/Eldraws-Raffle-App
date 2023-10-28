import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogConfig } from '@angular/material/dialog';
import { DialogModule } from './dialog/dialog.module';
import { FormSheetModule } from './form-sheet/form-sheet.module';
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatBottomSheetModule
  ],
  exports: [
    FormsModule,
    DialogModule,
    FormSheetModule
  ],
  providers: [
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: {
        ...new MatDialogConfig(),
        panelClass: ['!w-full', '!max-w-lg'],
        maxHeight: '85vh',
      } as MatDialogConfig,
    }
  ]
})
export class SharedModule { }
