import { Injectable } from '@angular/core';
import {
  BehaviorSubject, Observable,
  Subject, combineLatest, merge, shareReplay
} from 'rxjs';
import { InjectableStream } from './injectable-stream';

@Injectable({
  providedIn: 'root',
})
export abstract class DataModelStream<T> extends InjectableStream<T> {
  private modelSource$: Subject<T>;
  protected constructor(stream$: Observable<T>) {
    const source$ = new Subject<T>();
    super(merge(source$, stream$).pipe(shareReplay(1)));

    this.modelSource$ = source$;
  }

  next(value: T) {
    this.modelSource$.next(value);
  }
}
