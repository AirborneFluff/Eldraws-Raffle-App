import { Entrant, Tickets } from '../data-models';

export interface RaffleEntry {
  id: number,
  entrant: Entrant,
  donation: number,
  inputDate: Date,
  tickets: Tickets
}
