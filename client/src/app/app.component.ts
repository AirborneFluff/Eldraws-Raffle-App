import { Component } from '@angular/core';
import { AccountService } from './core/services/account.service';
import { ThemingService } from './core/theming/theming.service';
import { Theme } from './core/theming/theme';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  currentTheme!: Theme;
  constructor(public account: AccountService, private theme: ThemingService) {
    this.theme.theme$.subscribe(x => this.currentTheme = x);
  }

  toggle() {
    if (this.currentTheme == Theme.Dark) {
      this.theme.setTheme(Theme.Light);
      return;
    }
    this.theme.setTheme(Theme.Dark)
  }
}
