import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, Validators } from '@angular/forms';
import { map, Observable, of } from 'rxjs';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements ControlValueAccessor, OnInit {
  @Input() label: string = '';
  @Input() textarea: boolean = false;
  @Input() password: boolean = false;


  get textType() {
    return !this.password ? 'text' : 'password';
  }

  requiredError$: Observable<boolean> = of(false);

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  get formControl(): FormControl<string> {
    return this.ngControl.control as FormControl<string>;
  }

  get isRequired(): boolean {
    return this.formControl.hasValidator(Validators.required);
  }

  ngOnInit(): void {
    this.requiredError$ = this.formControl.statusChanges.pipe(
      map(status => {
        return status != 'VALID';
      })
    )
  }

  registerOnChange(fn: any): void {}

  registerOnTouched(fn: any): void {}

  writeValue(obj: any): void {}
}
