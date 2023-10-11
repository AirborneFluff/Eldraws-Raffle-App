import { Injectable } from '@angular/core';
import { ModelStream } from '../observables/model-stream';
import { Clan } from '../../data/data-models';

@Injectable({
  providedIn: 'root',
})
export class ClanStream extends ModelStream<Clan> {}
