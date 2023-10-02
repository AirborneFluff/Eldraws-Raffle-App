import { BaseRepository } from './base-repository';
import { Observable } from 'rxjs';
import { Raffle, NewRaffle } from '../../data/data-models';

export class RaffleRepository extends BaseRepository {
    public getById(id: number): Observable<Raffle> {
        return this.http.get<Raffle>(this.baseUrl + id);
    }
    public getAll(): Observable<Raffle[]> {
        return this.http.get<Raffle[]>(this.baseUrl);
    }
    public addNew(clanId: number, newRaffle: NewRaffle): Observable<Raffle> {
        return this.http.post<Raffle>(this.baseUrl + `${clanId}/raffles`, newRaffle)
    }
}
