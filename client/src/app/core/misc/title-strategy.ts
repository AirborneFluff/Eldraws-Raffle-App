import { RouterStateSnapshot, TitleStrategy } from '@angular/router';
import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Injectable()
export class CustomTitleStrategy extends TitleStrategy {
  constructor(private readonly title: Title) {
    super();
  }
  override updateTitle(snapshot: RouterStateSnapshot): void {
    const title = this.buildTitle(snapshot);
    if (title !== undefined) {
      this.title.setTitle(title);
      return;
    }
    this.title.setTitle('Raffler');
  }
}
