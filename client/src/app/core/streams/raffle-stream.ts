import { Injectable } from '@angular/core';
import { ModelStream } from '../observables/model-stream';
import { Raffle } from '../../data/models/raffle';

@Injectable({
  providedIn: 'root',
})
export class RaffleStream extends ModelStream<Raffle> {}
