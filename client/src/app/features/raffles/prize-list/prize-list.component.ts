import { Component, Input } from '@angular/core';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { combineLatest, map, of, startWith, switchMap, take } from 'rxjs';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { RafflePrize } from '../../../data/models/raffle-prize';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { NumericPositionPipe } from '../../../core/pipes/numeric-position';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-prize-list',
  templateUrl: './prize-list.component.html',
  styleUrls: ['./prize-list.component.scss']
})
export class PrizeListComponent {
  @Input() showRollingUI: boolean = false;
  showFullQuantity: boolean = false;

  constructor(public raffle$: CurrentRaffleStream, private dialog: MatDialog, private numericPipe: NumericPositionPipe, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private api: ApiService) {
  }

  prizes$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => raffle.prizes.sort((a, b) => {
      return b.place - a.place;
    })),
    startWith([])
  )

  totalDonations$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      return raffle.entries.reduce((acc, curr) => {
        return acc + curr.donation
      }, 0)
    }),
    startWith(0)
  )

  removePrize(prize: RafflePrize) {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: `You will be removing the ${this.numericPipe.transform(prize.place)} prize`,
        btnOkText: 'Yes',
        btnCancelText: 'No',
      }
    }).afterClosed().pipe(
      switchMap(confirm => {
        if (!confirm) return of();

        return combineLatest([
          this.clanId$.pipe(notNullOrUndefined()),
          this.raffleId$.pipe(notNullOrUndefined())
        ]).pipe(
          take(1),
          switchMap(([clanId, raffleId]) => this.api.Raffles.removePrize(clanId, raffleId, prize.place))
        )
      })).subscribe(updatedRaffle => {
        this.raffle$.next(updatedRaffle)
    })
  }
}
