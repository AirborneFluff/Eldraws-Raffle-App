import { Entrant } from './entrant';

export interface RafflePrize {
  place: number,
  description: string,
  donationPercentage: number,
  winningTicketNumber: number | null,
  winner: Entrant | undefined
}
