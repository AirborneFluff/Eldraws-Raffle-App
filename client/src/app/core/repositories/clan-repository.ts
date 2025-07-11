import { BaseRepository } from './base-repository';
import { catchError, map, Observable, of } from 'rxjs';
import { NewClan, Entrant, Clan } from '../../data/data-models';
import { EntrantParams } from '../../data/params/entrant-params';
import { getPaginatedResult, getPaginationHeaders } from '../utils/pagination-helper';
import { PaginatedResult } from '../utils/pagination';
import { HttpParams } from '@angular/common/http';

export class ClanRepository extends BaseRepository {
  public exists(name: string | null): Observable<boolean> {
    if (name == null) return of(false);
    const params = {
      name: name
    }
    return this.http.get<any>(this.baseUrl + 'search', { params: params })
      .pipe(
        catchError(() => of(null)),
        map(val => !!val))
  }
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

  public getEntrants(clanId: number, entrantParams: EntrantParams): Observable<PaginatedResult<Entrant[]>> {
    let params = getPaginationHeaders(entrantParams.pageNumber, entrantParams.pageSize);
    if (entrantParams.orderBy) params = params.append("orderBy", entrantParams.orderBy);
    if (entrantParams.gamertag) params = params.append("gamertag", entrantParams.gamertag);

    return getPaginatedResult<Entrant[]>(this.baseUrl + `${clanId}/entrants`, params, this.http);
  }

  public addMember(clanId: number, memberId: string) {
    return this.http.post<Clan>(this.baseUrl + `${clanId}/members/${memberId}`, {})
  }

  public removeMember(clanId: number, memberId: string) {
    return this.http.delete<Clan>(this.baseUrl + `${clanId}/members/${memberId}`, {})
  }

  public setMemberActivity(clanId: number, entrantId: number, active: boolean) {
    const endpoint = active ? 'setActive' : 'setInactive'
    return this.http.post<Entrant>(this.baseUrl + `${clanId}/entrants/${entrantId}/${endpoint}`, {})
  }

  public searchEntrants(clanId: number, searchTerm: string): Observable<Entrant[]> {
    let params = new HttpParams;
    params = params.append('searchTerm', searchTerm);
    return this.http.get<Entrant[]>(this.baseUrl + `${clanId}/entrants/search`, { params: params })
  }
}
