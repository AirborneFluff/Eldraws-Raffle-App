import { Injectable } from '@angular/core';
import { ClanRepository, RaffleRepository } from '../repositories/repositories';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public Clans: ClanRepository = new ClanRepository(this.http, 'clans');
  public Raffles: RaffleRepository = new RaffleRepository(this.http, 'clans');

  constructor(private http: HttpClient) { }
}
