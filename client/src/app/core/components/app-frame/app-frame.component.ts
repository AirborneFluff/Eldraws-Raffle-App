import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { NavigationEnd, Router } from '@angular/router';
import {filter, map, take } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../shared/dialog/confirm-dialog/confirm-dialog.component';
import { ClipboardService } from 'ngx-clipboard';
import { PageTitleService } from '../../services/page-title.service';
import {UrlStream} from "../../streams/url-stream";

@Component({
  selector: 'app-frame',
  templateUrl: './app-frame.component.html',
  styleUrls: ['./app-frame.component.scss']
})
export class AppFrameComponent {

  constructor(public account: AccountService,
              public title: PageTitleService,
              private router: Router,
              private dialog: MatDialog,
              private clipboard: ClipboardService,
              private url$: UrlStream) {
    this.url$.subscribe();
  }


  baseRoute$ = this.router.events.pipe(
    filter(event => event instanceof NavigationEnd),
    map(navEnd => {
      const event: NavigationEnd = navEnd as NavigationEnd;
      return event.url.split('/').length == 2;
    }))

  back() {
    this.url$.pipe(take(1)).subscribe(currentRoute => {
      console.log(currentRoute)
      const parentRoute = currentRoute.split('/').slice(0, -2).join('/');
      this.router.navigateByUrl(parentRoute);
    })
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
