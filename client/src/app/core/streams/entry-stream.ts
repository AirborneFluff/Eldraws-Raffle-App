import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import {
  distinctUntilChanged,
  filter,
  map,
  shareReplay,
  startWith,
} from 'rxjs';
import { getRouteParamsFromSnapshot } from 'src/app/core/utils/get-route-params';
import { InjectableStream } from '../observables/injectable-stream';
import { NavigationParams } from '../config/navigation';
import { ModelStream } from '../observables/model-stream';
import { Raffle } from '../../data/models/raffle';
import { RaffleEntry } from '../../data/models/raffle-entry';

@Injectable({
  providedIn: 'root',
})
export class EntryStream extends ModelStream<RaffleEntry> {}
