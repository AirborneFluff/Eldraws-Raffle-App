import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';

@Component({
  selector: 'app-frame',
  templateUrl: './app-frame.component.html',
  styleUrls: ['./app-frame.component.scss']
})
export class AppFrameComponent {

  constructor(public account: AccountService, public title: Title, private route: ActivatedRoute, private router: Router) {
  }

  baseRoute$ = this.router.events.pipe(
    filter(event => event instanceof NavigationEnd),
    map(navEnd => {
      const event: NavigationEnd = navEnd as NavigationEnd;
      return event.url.split('/').length == 2;
    }))

  back() {
    this.router.navigate(['../'], {relativeTo: this.route})
  }
}
