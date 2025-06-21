import { PageParams } from './page-params';

export interface RafflesPageParams extends PageParams {
  clanId: number;
  endCloseDate?: string;
  startCloseDate?: string;
}
