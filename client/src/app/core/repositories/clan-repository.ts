import { BaseRepository } from './base-repository';
import { Observable } from 'rxjs';
import { Clan } from '../../data/models/clan';

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
}
