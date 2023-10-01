import { BaseRepository } from './base-repository';
import { Observable } from 'rxjs';
import { Clan } from '../../data/models/clan';

export class ClanRepository extends BaseRepository {
  public getById(id: number): Observable<Clan> {
    return this.http.get<Clan>(this.baseUrl + id);
  }
}
