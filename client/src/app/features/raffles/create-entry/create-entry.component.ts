import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  combineLatest,
  map,
  of, scan,
  shareReplay, startWith,
  Subject,
  switchMap, tap,
  withLatestFrom
} from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { RaffleStream } from '../../../core/streams/raffle-stream';
import { Entrant } from '../../../data/models/entrant';

@Component({
  selector: 'app-create-entry',
  templateUrl: './create-entry.component.html',
  styleUrls: ['./create-entry.component.scss']
})
export class CreateEntryComponent {
  entryForm!: FormGroup;
  entrantAdditionsSource$ = new Subject<Entrant>();

  entrants$ = combineLatest([
    this.entrantAdditionsSource$.pipe(
      scan((all: Entrant[], current) => [...all, current], []),
      startWith([])
    ),
    this.clanId.pipe(
      notNullOrUndefined(),
      switchMap((id) => this.api.Clans.getById(id).pipe(
        map(clan => clan.entrants))))
    ]).pipe(map(([arr1, arr2]) => [...arr1, ...arr2]))

  gamertag = new FormControl('', Validators.required)
  donation = new FormControl(5000, [Validators.required, Validators.min(0)])

  filteredEntrants$ = combineLatest([
    this.entrants$,
    this.gamertag.valueChanges.pipe(notNullOrUndefined())
  ]).pipe(map(([entrants, filter]) => {
    return entrants.filter(entrant => entrant.gamertag.toLowerCase().includes(filter))
  }))

  constructor(private api: ApiService, private clanId: ClanIdStream, private raffleId: RaffleIdStream, private raffleUpdates: RaffleStream) {
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
        if (!ent) return this.api.Clans.addEntrant(clanId, gamertag).pipe(tap(entrant => this.addEntrantToList(entrant)));
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
      ).subscribe(x => {
        this.raffleUpdates.next(x);
    })
  }

  addEntrantToList(entrant: Entrant) {
    this.entrantAdditionsSource$.next(entrant)
  }
}
