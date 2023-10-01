import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-form-link',
  templateUrl: './form-link.component.html',
  styleUrls: ['./form-link.component.scss']
})
export class FormLinkComponent {
  @Input() link!: string;

}
