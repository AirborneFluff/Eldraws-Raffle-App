import { Injectable } from '@angular/core';
import { ClanRepository } from '../repositories/repositories';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public Clans: ClanRepository = new ClanRepository(this.http, 'clans');

  constructor(private http: HttpClient) { }
}
