import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  BehaviorSubject,
  combineLatest,
  map,
  of, shareReplay,
  startWith,
  switchMap, take, tap,
  withLatestFrom
} from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { parseNumericSuffix } from '../../../core/utils/parse-numeric-suffix';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';

@Component({
  selector: 'app-create-entry',
  templateUrl: './create-entry.component.html',
  styleUrls: ['./create-entry.component.scss']
})
export class CreateEntryComponent {
  gamertag = new FormControl('', Validators.required)
  donation = new FormControl('', [Validators.required, Validators.min(0)])
  constructor(private api: ApiService, private clanId: ClanIdStream, private clan$: CurrentClanStream, private raffleId: RaffleIdStream, private raffle$: CurrentRaffleStream) {
    this.selectedEntrant$.subscribe(val => console.log(val))
  }

  submitted$ = new BehaviorSubject<boolean>(false);

  entrants$ = this.clan$.pipe(
    notNullOrUndefined(),
    map(clan => clan.entrants),
    startWith([]),
    shareReplay({refCount: true, bufferSize: 1}));

  selectedEntrant$ = this.gamertag.valueChanges.pipe(
    notNullOrUndefined(),
    withLatestFrom(this.entrants$),
    switchMap(([gamertag, entrants]) => {
      const entrant = entrants.find(entrant => entrant.gamertag.toLowerCase() == gamertag.toLowerCase());
      return of(entrant);
    }),
    shareReplay(1)
  )

    // combineLatest([
    // this.entrantAdditionsSource$.pipe(
    //   scan((all: Entrant[], current) => [...all, current], []),
    //   startWith([])
    // ),
    // this.clanId.pipe(
    //   notNullOrUndefined(),
    //   switchMap((id) => this.api.Clans.getById(id).pipe(
    //     tap(() => this.entrantsLoading = false),
    //     map(clan => clan.entrants))))
    // ]).pipe(
    //   map(([arr1, arr2]) => [...arr1, ...arr2]),
    //   shareReplay({refCount: true, bufferSize: 1}))


  entryForm = new FormGroup<any>({
    gamertag: this.gamertag,
    donation: this.donation
  })

  filteredEntrants$ = combineLatest([
    this.entrants$,
    this.gamertag.valueChanges.pipe(notNullOrUndefined(), startWith(''))
  ]).pipe(map(([entrants, filter = '']) => {
    return entrants.filter(entrant => entrant.gamertag.toLowerCase().includes(filter.toLowerCase()))
  }))

  initializeForm() {
    this.gamertag.reset();
    this.donation.reset();
  }

  submit() {
    if (this.entryForm.invalid) return;

    const gamertag = this.gamertag.value;
    if (!gamertag) return;

    const donation = this.donation.value;
    if (!donation) return;

    this.selectedEntrant$.pipe(
      withLatestFrom(this.clanId.pipe(notNullOrUndefined())),
      switchMap(([entrant, clanId]) => {
        if (!entrant) return this.api.Clans.addEntrant(clanId, gamertag).pipe(
          withLatestFrom(this.clan$.pipe(notNullOrUndefined())),
          tap(([entrant, clan]) => {
            clan.entrants.push(entrant);
            this.clan$.next(clan)
          }),
          map(([entrant, clan]) => entrant)
        );
        return of(entrant)
      }))
      .pipe(
        withLatestFrom(this.clanId.pipe(notNullOrUndefined()), this.raffleId.pipe(notNullOrUndefined())),
        switchMap(([entrant, clanId, raffleId]) => {
          this.submitted$.next(true);
          return this.api.Raffles.addEntry(clanId, raffleId, {
            entrantId: entrant.id,
            donation: parseNumericSuffix(donation)
          })
        }),
        take(1)
      ).subscribe(updatedRaffle => {
        this.raffle$.next(updatedRaffle);
        this.submitted$.next(false);
        this.initializeForm();
    })
  }
}
