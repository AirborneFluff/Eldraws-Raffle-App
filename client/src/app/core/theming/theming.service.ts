import { Injectable, OnDestroy } from '@angular/core';
import { Theme } from './theme';
import { BehaviorSubject, Observable, shareReplay, Subscription, tap } from 'rxjs';
import { Meta } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class ThemingService implements OnDestroy {
  private readonly themeClass = {
    [Theme.Light]: 'light',
    [Theme.Dark]: 'dark'
  }

  private readonly htmlBackgroundClass = {
    [Theme.Light]: 'bg-primary-0',
    [Theme.Dark]: 'bg-primary-1000'
  }

  private readonly _subscriptions = new Subscription();
  private _themeSubject: BehaviorSubject<Theme> = new BehaviorSubject<Theme>(this.clientPreferredTheme);

  theme$: Observable<Theme> = this._themeSubject.pipe(
    tap(theme => {
      localStorage.setItem('app-theme', this.themeClass[theme])
    }),
    shareReplay(1)
  )

  constructor(private meta: Meta) {
    this._subscriptions.add(
      this.theme$.subscribe(theme => this.setThemeClass(theme))
    );
  }

  ngOnDestroy() {
    this._subscriptions.unsubscribe();
  }

  public setTheme(theme: Theme) {
    this._themeSubject.next(theme)
  }

  private get clientPreferredTheme(): Theme {
    const previousTheme = localStorage.getItem('app-theme')?.replaceAll('"', '');

    if (previousTheme == this.themeClass[Theme.Light]) return Theme.Light;
    if (previousTheme == this.themeClass[Theme.Dark]) return Theme.Dark;

    const prefersDark: boolean = window.matchMedia(
      '(prefers-color-scheme: dark)'
    ).matches;

    return prefersDark ? Theme.Dark : Theme.Light;
  }

  private setThemeClass(theme: Theme): void {
    const themeClass = this.themeClass[theme]
    if (!themeClass) return

    const body = document.getElementsByTagName('body')[0];
    body.classList.remove(...Object.values(this.themeClass));
    body.classList.add(this.themeClass[theme]);

    this.setHTMLMeta(theme);
    this.setHTMLBackground(theme)

    const appTheme = document.getElementById('app-theme');

    if (!appTheme) {
      console.error('`app-theme` element not found, could not set theme');
      return;
    }

    appTheme.classList.remove(...Object.values(this.themeClass));
    appTheme.classList.add(themeClass);
  }

  private setHTMLMeta(theme: Theme) {
    let color: string = 'white'
    if (theme == Theme.Dark) color = '#000C11';
    this.meta.updateTag({ content: color }, 'name=theme-color');
  }

  private setHTMLBackground(theme: Theme) {
    const html = document.getElementsByTagName('html')[0];
    const bgClass = this.htmlBackgroundClass[theme]
    html.classList.remove(...Object.values(this.htmlBackgroundClass));
    html.classList.add(bgClass);
  }
}
