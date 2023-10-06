import { BaseRepository } from './base-repository';
import { Observable } from 'rxjs';
import { Raffle, NewRaffle } from '../../data/data-models';
import { NewRaffleEntry } from '../../data/models/new-entry';

export class RaffleRepository extends BaseRepository {

    public getById(clanId: number, id: number): Observable<Raffle> {
        return this.http.get<Raffle>(this.baseUrl + `${clanId}/raffles/` + id);
    }
    public getAll(clanId: number): Observable<Raffle[]> {
        return this.http.get<Raffle[]>(this.baseUrl + `${clanId}/raffles/`);
    }
    public addNew(clanId: number, newRaffle: NewRaffle): Observable<Raffle> {
        return this.http.post<Raffle>(this.baseUrl + `${clanId}/raffles`, newRaffle)
    }

    public addEntry(clanId: number, raffleId: number, entry: NewRaffleEntry) {
      return this.http.post<Raffle>(this.baseUrl + `${clanId}/raffles/${raffleId}/entries`, entry);
    }
}
