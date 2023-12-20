import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { GreaterThanValidator } from '../../../core/validators/greater-than-validator';
import { IntegerValidator } from '../../../core/validators/integer-validator';
import { Raffle } from '../../../data/models/raffle';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { switchMap, take, withLatestFrom, combineLatest, filter } from 'rxjs';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { ApiService } from '../../../core/services/api.service';
import { NewRaffle } from '../../../data/models/new-raffle';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { Router } from '@angular/router';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { NavigationService } from '../../../core/services/navigation.service';
import { MatDialog } from "@angular/material/dialog";
import { ConfirmDialogComponent } from "../../../shared/dialog/confirm-dialog/confirm-dialog.component";


const INITIAL_OPEN_DATE = new Date(new Date().setMinutes(0));
const INITIAL_CLOSE_DATE = new Date(new Date().setMinutes(0));
const INITIAL_DRAW_DATE = new Date(new Date().setMinutes(0));

INITIAL_CLOSE_DATE.setTime(INITIAL_OPEN_DATE.getTime() + 7 * 86400000);
INITIAL_DRAW_DATE.setTime(INITIAL_CLOSE_DATE.getTime() + 3600000);

@Component({
  selector: 'app-raffle-form',
  templateUrl: './raffle-form.component.html',
  styleUrls: ['./raffle-form.component.scss']
})
export class RaffleFormComponent {
  title = new FormControl('', Validators.required)
  entryCost = new FormControl(5000, [GreaterThanValidator(0), IntegerValidator()])
  openDate = new FormControl(INITIAL_OPEN_DATE, Validators.required)
  closeDate = new FormControl(INITIAL_CLOSE_DATE, Validators.required)
  drawDate = new FormControl(INITIAL_DRAW_DATE, Validators.required)

  raffleForm = new FormGroup({
    title: this.title,
    entryCost: this.entryCost,
    openDate: this.openDate,
    closeDate: this.closeDate,
    drawDate: this.drawDate
  })

  constructor(public raffle$: CurrentRaffleStream,
              public bottomSheet: MatBottomSheet,
              private clanId$: ClanIdStream,
              private api: ApiService,
              private clan$: CurrentClanStream,
              private router: Router,
              private raffleId$: RaffleIdStream,
              private navigation: NavigationService,
              private dialog: MatDialog) {
    this.raffle$.pipe(notNullOrUndefined(), take(1)).subscribe(raffle => {
      this.patchValues(raffle);
    })
  }

  patchValues(raffle: Raffle) {
    this.raffleForm.patchValue(raffle);
  }

  createRaffle() {
    this.clanId$.pipe(
      notNullOrUndefined(),
      take(1),
      switchMap(clanId => this.api.Raffles.addNew(clanId, this.raffleForm.value as NewRaffle)),
      withLatestFrom(this.clan$.pipe(notNullOrUndefined()))
    ).subscribe({
        next: ([newRaffle, clan]) => {
          clan.raffles.push(newRaffle);
          this.clan$.next(clan);
          this.router.navigate(['clans', clan.id, 'raffles', newRaffle.id]);
          this.bottomSheet.dismiss();
        }
      }
    )
  }

  updateRaffle() {
    combineLatest([
      this.clanId$.pipe(notNullOrUndefined()),
      this.raffleId$.pipe(notNullOrUndefined())
    ]).pipe(
      take(1),
      switchMap(([clanId, raffleId]) => this.api.Raffles.updateRaffle(clanId, raffleId, this.raffleForm.value as NewRaffle))
    ).subscribe(updatedRaffled => {
        this.raffle$.next(updatedRaffled);
        this.bottomSheet.dismiss();
      }
    )
  }

  deleteRaffle() {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: `This will remove the raffle. <br>You will have to manually remove any Discord posts. <br>Entrant donations will still be shown in their total donations.`,
        btnOkText: 'Yes',
        btnCancelText: 'No',
      }
    }).afterClosed().pipe(
      filter(confirm => confirm),
      switchMap(() => this.deleteRaffle$))
      .subscribe(() => {
        this.raffle$.next(undefined);
        this.navigation.navigateDown();
        this.bottomSheet.dismiss();
      }
    )
  }

  private deleteRaffle$ = combineLatest([
    this.clanId$.pipe(notNullOrUndefined()),
    this.raffleId$.pipe(notNullOrUndefined())
  ]).pipe(
    take(1),
    switchMap(([clanId, raffleId]) => this.api.Raffles.delete(clanId, raffleId))
  )
}
