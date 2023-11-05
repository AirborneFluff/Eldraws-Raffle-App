import { BaseRepository } from './base-repository';
import { Observable } from 'rxjs';
import { NewClan, Entrant, Clan } from '../../data/data-models';

export class ClanRepository extends BaseRepository {
  public getById(id: number): Observable<Clan> {
    return this.http.get<Clan>(this.baseUrl + id);
  }
  public getAll(): Observable<Clan[]> {
    return this.http.get<Clan[]>(this.baseUrl);
  }

  public addNew(clan: NewClan): Observable<Clan> {
    return this.http.post<Clan>(this.baseUrl, clan)
  }

  public update(clanId: number, clan: NewClan): Observable<Clan> {
    return this.http.put<Clan>(this.baseUrl + clanId, clan)
  }

  public addEntrant(clanId: number, gamertag: string) {
    return this.http.post<Entrant>(this.baseUrl + `${clanId}/entrants`, {
      gamertag: gamertag
    })
  }

  public addMember(clanId: number, memberId: string) {
    return this.http.post<Clan>(this.baseUrl + `${clanId}/members/${memberId}`, {})
  }

  public removeMember(clanId: number, memberId: string) {
    return this.http.delete<Clan>(this.baseUrl + `${clanId}/members/${memberId}`, {})
  }
}
