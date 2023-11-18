import { Entrant } from './entrant';

export interface RollWinnerResponse {
  winner: Entrant,
  reroll: boolean,
  ticketNumber: number
}
