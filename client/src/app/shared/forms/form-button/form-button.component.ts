import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-form-button',
  templateUrl: './form-button.component.html',
  styleUrls: ['./form-button.component.scss']
})
export class FormButtonComponent {
  @Input() type: 'primary' | 'secondary' | 'warn' | 'danger' = 'primary';
  @Input() disabled: boolean = false;
  @Output() onClick: EventEmitter<any> = new EventEmitter<any>();
}
