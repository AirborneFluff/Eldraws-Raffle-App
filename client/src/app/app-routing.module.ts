import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClanListComponent } from './features/clans/clan-list/clan-list.component';
import { authGuard } from './core/guards/auth.guard';
import { CreateClanComponent } from './features/clans/create-clan/create-clan.component';
import { CreateRaffleComponent } from './features/raffles/create-raffle/create-raffle.component';
import { RaffleDetailsComponent } from './features/raffles/raffle-details/raffle-details.component';

const routes: Routes = [
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [authGuard],
    children: [
      { path: "",
        redirectTo: 'clans',
        pathMatch: 'full'
      },
      { title: "Clans", path: "clans", component: ClanListComponent },
      { title: "Clans", path: "clans/:clanId", component: ClanListComponent },
      { title: "Create Clan", path: "clans/create", component: CreateClanComponent },
      { title: "Raffle", path: "clans/:clanId/raffles/:raffleId", component: RaffleDetailsComponent },
      { title: "Create Raffle", path: "raffles/create", component: CreateRaffleComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
