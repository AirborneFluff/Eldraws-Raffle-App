import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  BehaviorSubject,
  combineLatest, debounceTime, distinctUntilChanged,
  map,
  of, shareReplay,
  startWith, Subscription,
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
import { EntryStream } from '../../../core/streams/entry-stream';

@Component({
  selector: 'app-create-entry',
  templateUrl: './create-entry.component.html',
  styleUrls: ['./create-entry.component.scss']
})
export class CreateEntryComponent implements OnDestroy {
  gamertag = new FormControl('', Validators.required)
  donation = new FormControl('', [Validators.required, Validators.min(0)])
  complimentary = new FormControl(false, [Validators.required])
  subscriptions = new Subscription();
  constructor(private api: ApiService, private clanId: ClanIdStream, private clan$: CurrentClanStream, private raffleId: RaffleIdStream, private raffle$: CurrentRaffleStream, private entryUpdates$: EntryStream) {
    this.subscriptions.add(this.selectedEntrant$.subscribe());
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  submitted$ = new BehaviorSubject<boolean>(false);

  filteredEntrants$ = combineLatest([
    this.clanId.pipe(notNullOrUndefined()),
    this.gamertag.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      distinctUntilChanged(),
      notNullOrUndefined()
    )
  ]).pipe(
    switchMap(([clanId, filter = '']) => {
      if (!filter || filter.length < 2) return of([]);
      return this.api.Clans.searchEntrants(clanId, filter).pipe(
        map(entrants => entrants.sort((a, b) => {
          if (a.active === b.active) return 0;
          return a.active ? -1 : 1;
        }))
      );
    }),
    shareReplay({ refCount: true, bufferSize: 1 })
  );

  selectedEntrant$ = this.gamertag.valueChanges.pipe(
    notNullOrUndefined(),
    withLatestFrom(this.filteredEntrants$),
    switchMap(([gamertag, entrants]) => {
      const entrant = entrants.find(entrant =>
        entrant.gamertag.toLowerCase() === gamertag.toLowerCase()
      );
      return of(entrant);
    }),
    tap(entrant => {
      const currentErrors = this.gamertag.errors || {};

      if (entrant && !entrant.active) {
        this.gamertag.setErrors({ ...currentErrors, inactiveEntrant: true });
        return;
      }
      delete currentErrors['inactiveEntrant'];
      this.gamertag.setErrors(Object.keys(currentErrors).length ? currentErrors : null);
    }),
    shareReplay(1)
  );

  selectedEntrantValid$ = this.selectedEntrant$.pipe(
    map(entrant => entrant == undefined ? true : entrant.active),
    startWith(true)
  );

  entryForm = new FormGroup({
    gamertag: this.gamertag,
    donation: this.donation,
    complimentary: this.complimentary
  });

  initializeForm() {
    this.gamertag.setValue('');
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
      take(1),
      withLatestFrom(this.clanId.pipe(notNullOrUndefined())),
      switchMap(([entrant, clanId]) => {
        this.submitted$.next(true);
        if (!entrant) return this.api.Clans.addEntrant(clanId, gamertag);
        return of(entrant)
      })).pipe(
        take(1),
        withLatestFrom(this.clanId.pipe(notNullOrUndefined()), this.raffleId.pipe(notNullOrUndefined())),
        switchMap(([entrant, clanId, raffleId]) => {
          return this.api.Raffles.addEntry(clanId, raffleId, {
            entrantId: entrant.id,
            donation: parseNumericSuffix(donation),
            complimentary: this.complimentary.value!
          })
        })
      ).subscribe(updatedRaffle => {
        this.raffle$.next(updatedRaffle);
        this.submitted$.next(false);
        this.entryUpdates$.next(undefined);
        this.initializeForm();
    })
  }
}
