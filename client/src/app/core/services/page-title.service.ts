import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PageTitleService {
  loading$ = new BehaviorSubject<boolean>(false);

  constructor(private title: Title) { }

  setTitle(value: string) {
    this.title.setTitle(value);
    this.loading$.next(false);
  }

  getTitle(): string {
    return this.title.getTitle();
  }

  busy() {
    this.loading$.next(true);
  }
}
