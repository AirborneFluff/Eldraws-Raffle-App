import { Component, Input } from '@angular/core';
import { RafflePrize } from '../../../data/models/raffle-prize';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { combineLatest, map, Observable, of, startWith, switchMap, take, withLatestFrom } from 'rxjs';
import { Observe } from '../../../core/decorators/observe';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { NumericPositionPipe } from '../../../core/pipes/numeric-position';
import { ClanIdStream } from '../../../core/streams/clan-id-stream';
import { RaffleIdStream } from '../../../core/streams/raffle-id-stream';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'prize-list-item',
  templateUrl: './prize-list-item.component.html',
  styleUrls: ['./prize-list-item.component.scss']
})
export class PrizeListItemComponent {
  @Input() prize!: RafflePrize;
  @Input() showFullQuantity: boolean = false;
  @Observe("prize") public prize$!: Observable<RafflePrize>;

  constructor(private raffle$: CurrentRaffleStream, private clanId$: ClanIdStream, private raffleId$: RaffleIdStream, private dialog: MatDialog, private numericPipe: NumericPositionPipe, private api: ApiService) {
  }

  totalDonations$ = this.raffle$.pipe(
    notNullOrUndefined(),
    map(raffle => {
      return raffle.entries.reduce((acc, curr) => {
        return acc + curr.donation
      }, 0)
    }),
    startWith(0)
  )

  winningTicketNumber$ = this.prize$.pipe(
    map(prize => prize.winningTicketNumber)
  )

  winner$ = this.winningTicketNumber$.pipe(
    notNullOrUndefined(),
    withLatestFrom(this.raffle$.pipe(notNullOrUndefined())),
    map(([ticket, raffle]) => {
      return raffle.entries.find(entry => entry.tickets.item1 <= ticket && entry.tickets.item2 >= ticket)?.entrant
    })
  )

  removePrize() {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Are you sure?',
        message: `You will be removing the ${this.numericPipe.transform(this.prize.place)} prize`,
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
          switchMap(([clanId, raffleId]) => this.api.Raffles.removePrize(clanId, raffleId, this.prize.place))
        )
      })).subscribe(updatedRaffle => {
        this.raffle$.next(updatedRaffle)
    })
  }
}
