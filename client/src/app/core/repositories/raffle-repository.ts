import { BaseRepository } from './base-repository';
import { Observable } from 'rxjs';
import { Raffle, NewRaffle } from '../../data/data-models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

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
}
