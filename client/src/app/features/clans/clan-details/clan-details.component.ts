import { Component } from '@angular/core';
import { CurrentClanStream } from '../../../core/streams/current-clan-stream';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ClanFormComponent } from '../clan-form/clan-form.component';

@Component({
  selector: 'app-clan-details',
  templateUrl: './clan-details.component.html',
  styleUrls: ['./clan-details.component.scss']
})
export class ClanDetailsComponent {
  constructor(public clan$: CurrentClanStream, private bottomSheet: MatBottomSheet) {
  }

  editClan() {
    this.bottomSheet.open(ClanFormComponent);
  }
}
