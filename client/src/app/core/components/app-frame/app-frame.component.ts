import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { ClipboardService } from 'ngx-clipboard';
import { Title } from '@angular/platform-browser';
import { NavigationService } from '../../services/navigation.service';

@Component({
  selector: 'app-frame',
  templateUrl: './app-frame.component.html',
  styleUrls: ['./app-frame.component.scss']
})
export class AppFrameComponent {

  constructor(public account: AccountService,
              public title: Title,
              private router: Router,
              private dialog: MatDialog,
              private clipboard: ClipboardService,
              private navigation: NavigationService) {
  }


  baseRoute$ = this.router.events.pipe(
    filter(event => event instanceof NavigationEnd),
    map(navEnd => {
      const event: NavigationEnd = navEnd as NavigationEnd;
      return event.url.split('/').length == 2;
    }))

  back() {
    this.navigation.navigateDown();
  }

  showMemberId() {
    let id: any;
    this.account.currentUser$.subscribe(user => id = user?.id);

    if (!id) return;

    this.dialog.open(ConfirmDialogComponent, {
      data : {
        title: 'Member Id',
        message: `Your unique Member Id is: <br> ${id}`,
        btnOkText: 'Copy',
        btnCancelText: 'Close'
      }
    }).afterClosed().subscribe(value => {
      if (value) this.clipboard.copy(id);
    })
  }
}
