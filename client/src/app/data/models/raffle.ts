import { Clan, Member, RafflePrize, RaffleEntry } from '../data-models';

export interface Raffle {
  id: number,
  clan: Clan
  host: Member,
  title: string,
  entryCost: number,
  discordMessageId: number,
  discordChannelId: string,
  description: string,
  totalTickets: number,
  totalDonations: number,
  createdDate: Date,
  openDate: Date,
  closeDate: Date,
  drawDate: Date,
  entries: RaffleEntry[],
  prizes: RafflePrize[]
}
