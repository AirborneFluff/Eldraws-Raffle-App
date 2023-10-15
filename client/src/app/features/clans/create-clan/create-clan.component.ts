import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, Subscription } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Router } from '@angular/router';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-clan',
  templateUrl: './create-clan.component.html',
  styleUrls: ['./create-clan.component.scss']
})
export class CreateClanComponent implements OnDestroy {
  clanForm!: FormGroup;

  name = new FormControl('', Validators.required)
  invalidForm$ = new Subject<boolean>();

  subscription = new Subscription();

  constructor(private api: ApiService, private router: Router, public dialogRef: MatDialogRef<CreateClanComponent>) {
    this.initializeForm();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  initializeForm() {
    this.clanForm = new FormGroup<any>({
      name: this.name
    })
  }

  submit() {
    if (this.clanForm.invalid) return;
    if (this.name.value == null) return;

    const subscription = this.api.Clans.addNew(this.name.value)
      .subscribe({
          next: newClan => {
            this.invalidForm$.next(false);
            this.router.navigateByUrl('/clans/' + newClan.id, { state: newClan });
            this.dialogRef.close();
          },
          error: () => {
            this.invalidForm$.next(true);
          }
        }
      )

    this.subscription.add(subscription)
  }
}
