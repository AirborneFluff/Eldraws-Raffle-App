import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { combineLatest, map, startWith, Subject, switchMap } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Router } from '@angular/router';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';

@Component({
  selector: 'app-create-entry',
  templateUrl: './create-entry.component.html',
  styleUrls: ['./create-entry.component.scss']
})
export class CreateEntryComponent {
  raffleForm!: FormGroup;
  entrants$ = this.clanId.pipe(
    notNullOrUndefined(),
    switchMap(id => this.api.Clans.getById(id).pipe(
      map(clan => clan.entrants))))

  gamertag = new FormControl('', Validators.required)
  donation = new FormControl(5000, Validators.required)

  filteredEntrants$ = combineLatest([
    this.entrants$,
    this.gamertag.valueChanges.pipe(notNullOrUndefined())
  ]).pipe(map(([entrants, filter]) => {
    return entrants.filter(entrant => entrant.gamertag.toLowerCase().includes(filter))
  }))


  invalidForm$ = new Subject<boolean>();

  constructor(private api: ApiService, private router: Router, private clanId: ClanIdStream) {
    this.initializeForm();
  }

  initializeForm() {
    this.raffleForm = new FormGroup<any>({
      gamertag: this.gamertag,
      donation: this.donation
    })
  }
}
