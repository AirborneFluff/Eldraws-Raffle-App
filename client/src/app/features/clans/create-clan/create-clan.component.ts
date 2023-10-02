import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-clan',
  templateUrl: './create-clan.component.html',
  styleUrls: ['./create-clan.component.scss']
})
export class CreateClanComponent {
  clanForm!: FormGroup;

  name = new FormControl('', Validators.required)
  invalidForm$ = new Subject<boolean>();

  constructor(private api: ApiService, private router: Router) {
    this.initializeForm();
  }

  initializeForm() {
    this.clanForm = new FormGroup<any>({
      name: this.name
    })
  }

  submit() {
    if (this.clanForm.invalid) return;
    if (this.name.value == null) return;

    this.api.Clans.addNew(this.name.value)
      .subscribe({
          next: newClan => {
            this.invalidForm$.next(false);
            this.router.navigateByUrl('/clans/' + newClan.id, { state: newClan });
          },
          error: () => {
            this.invalidForm$.next(true);
          }
        }
      )
  }
}
