import { BaseRepository } from './base-repository';
import { map, Observable } from 'rxjs';
import { Raffle, NewRaffle, NewRafflePrize, RaffleEntry } from '../../data/data-models';
import { NewRaffleEntry } from '../../data/models/new-entry';
import { RollParams } from '../../data/models/roll-params';
import { HttpParams } from '@angular/common/http';
import { RollWinnerResponse } from '../../data/models/roll-winner-response';
import { RaffleEntryParams } from '../../data/params/raffle-entry-params';
import { PaginatedResult } from '../utils/pagination';
import { getPaginatedResult, getPaginationHeaders } from '../utils/pagination-helper';
import { RafflesPageParams } from '../../data/params/raffles-page-params';

export class RaffleRepository extends BaseRepository {

  public getById(clanId: number, id: number): Observable<Raffle> {
      return this.http.get<Raffle>(this.baseUrl + `${clanId}/raffles/` + id).pipe(
        map(response => ({
          ...response,
          createdDate: new Date(response.createdDate),
          openDate: new Date(response.openDate),
          closeDate: new Date(response.closeDate),
          drawDate: new Date(response.drawDate),
        }))
      );
  }
  public getRaffles(pageParams: RafflesPageParams): Observable<PaginatedResult<Raffle[]>> {
    let params = getPaginationHeaders(pageParams.pageNumber, pageParams.pageSize);
    if (pageParams.endCloseDate) {
      params = params.append('endCloseDate', pageParams.endCloseDate);
    }
    if (pageParams.startCloseDate) {
      params = params.append('startCloseDate', pageParams.startCloseDate);
    }

    return getPaginatedResult<Raffle[]>(this.baseUrl + `${pageParams.clanId}/raffles/list`, params, this.http);
  }
  public addNew(clanId: number, newRaffle: NewRaffle): Observable<Raffle> {
      return this.http.post<Raffle>(this.baseUrl + `${clanId}/raffles`, newRaffle)
  }
  public delete(clanId: number, raffleId: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + `${clanId}/raffles/${raffleId}`);
  }
  public updateRaffle(clanId: number, raffleId: number, raffle: NewRaffle): Observable<Raffle> {
    return this.http.put<Raffle>(this.baseUrl + `${clanId}/raffles/${raffleId}`, raffle)
  }
  public getEntries(clanId: number, raffleId: number, entryParams: RaffleEntryParams): Observable<PaginatedResult<RaffleEntry[]>> {
    let params = getPaginationHeaders(entryParams.pageNumber, entryParams.pageSize);
    if (entryParams.orderBy) params = params.append("orderBy", entryParams.orderBy);

    return getPaginatedResult<RaffleEntry[]>(this.baseUrl + `${clanId}/raffles/${raffleId}/entries`, params, this.http);
  }
  public addEntry(clanId: number, raffleId: number, entry: NewRaffleEntry) {
    return this.http.post<Raffle>(this.baseUrl + `${clanId}/raffles/${raffleId}/entries`, entry);
  }
  public removeEntry(clanId: number, raffleId: number, entryId: number) {
    return this.http.delete<Raffle>(this.baseUrl + `${clanId}/raffles/${raffleId}/entries/${entryId}`);
  }

  public addPrize(clanId: number, raffleId: number, prize: NewRafflePrize) {
    return this.http.post<Raffle>(this.baseUrl + `${clanId}/raffles/${raffleId}/prizes`, prize)
  }

  public removePrize(clanId: number, raffleId: number, prizePlace: number) {
    return this.http.delete<Raffle>(this.baseUrl + `${clanId}/raffles/${raffleId}/prizes/${prizePlace}`);
  }

  public createDiscordPost(clanId: number, raffleId: number) {
    return this.http.post<number>(this.baseUrl + `${clanId}/raffles/${raffleId}/discord`, {})
  }

  public rollWinner(clanId: number, raffleId: number, prizePlace: number) {
    let params = new HttpParams();
    params = params.append('preventMultiWin', true);

    return this.http.post<RollWinnerResponse>(this.baseUrl + `${clanId}/raffles/${raffleId}/prizes/${prizePlace}/roll-winner`, {})
  }

  public removeWinner(clanId: number, raffleId: number, prizePlace: number) {
    return this.http.delete(this.baseUrl + `${clanId}/raffles/${raffleId}/prizes/${prizePlace}/roll-winner`, {})
  }

  public rollWinnersDiscord(clanId: number, raffleId: number, options: RollParams) {
    let params = new HttpParams;

    params = params.append('preventMultipleWins', options.preventMultipleWins ? 'true' : 'false');
    if (!!options.delay) params = params.append('delay', options.delay.toString());
    if (!!options.maxRerolls) params = params.append('maxRerolls', options.maxRerolls.toString());

    return this.http.post(this.baseUrl + `${clanId}/raffles/${raffleId}/discord/roll`, {}, {params: params})
  }
}
