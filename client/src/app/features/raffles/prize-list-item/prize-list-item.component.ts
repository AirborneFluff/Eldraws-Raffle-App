import { Component, Input } from '@angular/core';
import { RafflePrize } from '../../../data/models/raffle-prize';
import { CurrentRaffleStream } from '../../../core/streams/current-raffle-stream';
import { notNullOrUndefined } from '../../../core/pipes/not-null';
import { map, Observable, withLatestFrom } from 'rxjs';
import { Observe } from '../../../core/decorators/observe';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { RollWinnerFormComponent } from '../roll-winner-form/roll-winner-form.component';

@Component({
  selector: 'prize-list-item',
  templateUrl: './prize-list-item.component.html',
  styleUrls: ['./prize-list-item.component.scss']
})
export class PrizeListItemComponent {
  @Input() prize!: RafflePrize;
  @Observe("prize") public prize$!: Observable<RafflePrize>;

  constructor(private raffle$: CurrentRaffleStream, private bottomSheet: MatBottomSheet) {
  }

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

  showRollWinnerDialog() {
    this.bottomSheet.open(RollWinnerFormComponent, {
      data: {
        prize: this.prize
      },
    });
  }
}
