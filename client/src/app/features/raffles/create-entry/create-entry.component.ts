import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  combineLatest,
  map,
  of,
  shareReplay,
  Subject,
  switchMap,
  withLatestFrom
} from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Router } from '@angular/router';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { Raffle } from '../../../data/models/raffle';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';

@Component({
  selector: 'app-create-entry',
  templateUrl: './create-entry.component.html',
  styleUrls: ['./create-entry.component.scss']
})
export class CreateEntryComponent {
  @Output() raffle = new EventEmitter<Raffle>();

  entryForm!: FormGroup;
  entrants$ = this.clanId.pipe(
    notNullOrUndefined(),
    switchMap(id => this.api.Clans.getById(id).pipe(
      map(clan => clan.entrants))), shareReplay(1))

  gamertag = new FormControl('', Validators.required)
  donation = new FormControl(5000, Validators.required)

  filteredEntrants$ = combineLatest([
    this.entrants$,
    this.gamertag.valueChanges.pipe(notNullOrUndefined())
  ]).pipe(map(([entrants, filter]) => {
    return entrants.filter(entrant => entrant.gamertag.toLowerCase().includes(filter))
  }))


  invalidForm$ = new Subject<boolean>();

  constructor(private api: ApiService, private router: Router, private clanId: ClanIdStream, private raffleId: RaffleIdStream) {
    this.initializeForm();
  }

  initializeForm() {
    this.entryForm = new FormGroup<any>({
      gamertag: this.gamertag,
      donation: this.donation
    })
  }

  submit() {
    if (this.entryForm.invalid) return;

    const gamertag = this.gamertag.value;
    if (!gamertag) return;

    const donation = this.donation.value;
    if (!donation) return;

    this.entrants$.pipe(
      map(arr => arr.find(entrant => entrant.gamertag.toLowerCase() == gamertag.toLowerCase())),
      withLatestFrom(this.clanId.pipe(notNullOrUndefined())),
      switchMap(([ent, clanId]) => {
        if (!ent) return this.api.Clans.addEntrant(clanId, gamertag);
        return of(ent)
      }))
      .pipe(
        withLatestFrom(this.clanId.pipe(notNullOrUndefined()), this.raffleId.pipe(notNullOrUndefined())),
        switchMap(([entrant, clanId, raffleId]) => {
          return this.api.Raffles.addEntry(clanId, raffleId, {
            entrantId: entrant.id,
            donation: donation
          })
        })
      ).subscribe(x => console.log(x))
  }
}
