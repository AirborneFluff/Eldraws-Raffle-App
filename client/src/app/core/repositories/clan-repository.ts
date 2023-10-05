import { BaseRepository } from './base-repository';
import { Observable } from 'rxjs';
import { Clan } from '../../data/models/clan';
import { Entrant } from '../../data/models/entrant';

export class ClanRepository extends BaseRepository {
  public getById(id: number): Observable<Clan> {
    return this.http.get<Clan>(this.baseUrl + id);
  }
  public getAll(): Observable<Clan[]> {
    return this.http.get<Clan[]>(this.baseUrl);
  }

  public addNew(clanName: string): Observable<Clan> {
    return this.http.post<Clan>(this.baseUrl, {
      name: clanName
    })
  }

  public addEntrant(clanId: number, gamertag: string) {
    return this.http.post<Entrant>(this.baseUrl + `${clanId}/entrants`, {
      gamertag: gamertag
    })
  }
}
