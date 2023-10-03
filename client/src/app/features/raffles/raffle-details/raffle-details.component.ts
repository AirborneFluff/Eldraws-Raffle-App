import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Raffle } from '../../../data/data-models';
import { ApiService } from '../../../core/services/api.service';
import { BehaviorSubject, combineLatest, map, Observable, ReplaySubject, Subject, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-raffle-details',
  templateUrl: './raffle-details.component.html',
  styleUrls: ['./raffle-details.component.scss']
})
export class RaffleDetailsComponent {
  clanId!: number;
  raffleId$ = new ReplaySubject<number>(1);
  // raffle!: Raffle;
  raffle$ = this.raffleId$.pipe(
    switchMap(raffleId => {
      return this.api.Raffles.getById(this.clanId, raffleId).pipe(tap(x => console.log(x)))
    })
  )

  constructor(private route: ActivatedRoute, private router: Router, private api: ApiService) {
    const id = this.route.snapshot.paramMap.get('clanId');
    if (!id) return;
    this.clanId = Number.parseInt(id);

    this.getRaffleInfo();
  }


  private getRaffleInfo() {
    this.getRaffleIdFromRoute();
    if (this.getRaffleDataFromRoute()) return;
  }

  private getRaffleDataFromRoute(): boolean {
    // const routeData = this.router.getCurrentNavigation()?.extras.state as Raffle;
    // if (!routeData) return false;
    // if (routeData.id != this.raffleId) return false;
    //
    // this.raffle = routeData;
    return true;
  }

  private getRaffleIdFromRoute() {
    const id = this.route.snapshot.paramMap.get('raffleId');
    if (!id) return;

    const value = Number.parseInt(id);
    if (!Number.isInteger(value)) return;

    this.raffleId$.next(value);
  }
}
