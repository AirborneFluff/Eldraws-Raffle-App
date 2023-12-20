import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialogData } from '../confirm-dialog-data';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent {
  title: string;
  message: string;
  btnOkText: string;
  btnCancelText: string;

  constructor(@Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData, public dialogRef: MatDialogRef<ConfirmDialogComponent>) {
    this.title = data.title;
    this.message = data.message;
    this.btnOkText = data.btnOkText;
    this.btnCancelText = data.btnCancelText;
  }

  confirm() {
    this.dialogRef.close(true);
  }

  decline() {
    this.dialogRef.close(false);
  }
}
