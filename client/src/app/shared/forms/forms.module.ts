import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TextInputComponent } from './text-input/text-input.component';
import { FormHeaderComponent } from './form-header/form-header.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormFieldComponent } from './form-field/form-field.component';
import { FormDialogComponent } from './form-dialog/form-dialog.component';
import { FormButtonComponent } from './form-button/form-button.component';
import { FormActionsComponent } from './form-actions/form-actions.component';
import { FormErrorComponent } from './form-error/form-error.component';
import { FormLinkComponent } from './form-link/form-link.component';
import { RouterLink } from '@angular/router';



@NgModule({
  declarations: [
    TextInputComponent,
    FormHeaderComponent,
    FormFieldComponent,
    FormDialogComponent,
    FormButtonComponent,
    FormActionsComponent,
    FormErrorComponent,
    FormLinkComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  exports: [
    TextInputComponent,
    FormHeaderComponent,
    FormFieldComponent,
    FormDialogComponent,
    FormButtonComponent,
    FormActionsComponent,
    FormErrorComponent,
    FormLinkComponent
  ]
})
export class FormsModule { }
