import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function IntegerValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const intValue = Number.parseInt(control.value);
    const floatValue = Number.parseFloat(control.value);
    if (intValue === floatValue) return null;

    return { integer : true }
  }
}
