import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, Validators } from '@angular/forms';
import { Observable, of, ReplaySubject, startWith } from 'rxjs';

@Component({
  selector: 'app-form-date-input',
  templateUrl: './form-date-input.component.html',
  styleUrls: ['./form-date-input.component.scss']
})
export class FormDateInputComponent implements ControlValueAccessor, OnInit {
  private _inputValue: string = '';
  private readonly timezoneAbbreviationSource$: ReplaySubject<string> = new ReplaySubject<string>(1);

  @Input() label: string = '';

  inputFocussed: boolean = false;
  requiredError$: Observable<boolean> = of(false);
  timezoneAbbreviation$: Observable<string> = this.timezoneAbbreviationSource$.asObservable().pipe(startWith('UTC'));

  constructor(@Self() public ngControl: NgControl) {
    ngControl.valueAccessor = this;
  }

  get requiredError() {
    if (this.formControl.errors == null) return false;
    return 'required' in this.formControl.errors
  }

  ngOnInit(): void {
    if (!this.ngControl.control) { return; }
    const date = new Date(this.ngControl.control.value as string);
    this._inputValue = this.getDateAsInputValue(date);
    this.updateTimezoneAbbreviation(date);

    // this.requiredError$ = this.formControl.errors.pipe(
    //   withLatestFrom(this.formControl.touch$),
    //   map(([errors, touched]) => {
    //     if (!errors || !touched) { return false; }
    //
    //     return 'required' in errors;
    //   }));
  }

  get dateValid(): boolean {
    const date = new Date(this._inputValue);

    return !isNaN(date.getTime());
  }

  get isRequired(): boolean {
    return this.formControl.hasValidator(Validators.required);
  }

  get inputValue(): string {
    return this._inputValue;
  }

  set inputValue(val: string) {
    this._inputValue = val;
    if (!this.dateValid) { return; }

    const date = new Date(val);
    this.updateTimezoneAbbreviation(date);

    this.ngControl.control?.setValue(date);
  }

  updateTimezoneAbbreviation(date: Date): void {
    if (!date.getTime()) {
      date = new Date();
    }
    const locale = Intl.DateTimeFormat().resolvedOptions().locale;

    const abbreviation = date
      .toLocaleDateString(locale, {
        day: '2-digit',
        timeZoneName: 'short',
      })
      .slice(4);

    this.timezoneAbbreviationSource$.next(abbreviation);
  }

  get formControl(): FormControl<Date> {
    return this.ngControl.control as FormControl<Date>;
  }

  private getDateAsInputValue(date: Date): string {
    const year = date.getFullYear().toString().padStart(4, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');

    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}
  writeValue(obj: any): void {}
}
