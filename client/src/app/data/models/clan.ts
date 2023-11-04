import { Entrant, Member, Raffle } from '../data-models';

export interface Clan {
  id: number,
  name: string,
  owner: Member,
  discordChannelId: string,
  members: Member[],
  entrants: Entrant[],
  raffles: Raffle[]
}
