import { Injectable } from '@angular/core';
import {
  BehaviorSubject,
  Subject
} from 'rxjs';
import { InjectableStream } from './injectable-stream';

@Injectable({
  providedIn: 'root',
})
export abstract class ModelStream<T> extends InjectableStream<T | undefined> {
  modelSource$: Subject<T | undefined>;
  protected constructor() {
    const source = new BehaviorSubject<T | undefined>(undefined);
    super(source);

    this.modelSource$ = source;
  }

  next(value: T) {
    this.modelSource$.next(value);
  }
}
