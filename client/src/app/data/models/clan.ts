import { Entrant, Member, Raffle } from '../data-models';

export interface Clan {
  id: number,
  name: string,
  owner: Member,
  members: Member[],
  entrants: Entrant[],
  raffles: Raffle
}
