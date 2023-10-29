import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function GreaterThanValidator(value: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (control.value > value) return null;
    return { greaterThan : true }
  }
}
