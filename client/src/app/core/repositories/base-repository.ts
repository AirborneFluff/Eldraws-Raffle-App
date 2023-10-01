import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export abstract class BaseRepository {
  protected readonly baseUrl;

  constructor(protected http: HttpClient, private readonly urlRoute: string) {
    this.baseUrl = environment.apiUrl + this.urlRoute + '/';
  }
}
