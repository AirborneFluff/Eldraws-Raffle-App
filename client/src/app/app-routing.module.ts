import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClanListComponent } from './features/clans/clan-list/clan-list.component';
import { authGuard } from './core/guards/auth.guard';
import { RaffleDetailsComponent } from './features/raffles/raffle-details/raffle-details.component';
import { ClanDetailsComponent } from './features/clans/clan-details/clan-details.component';
import { LoginComponent } from './features/login/login/login.component';
import { RegisterComponent } from './features/registration/register/register.component';

const routes: Routes = [
  { path: "", redirectTo: "login", pathMatch: 'full' },
  { title: "Login", path: "login", component: LoginComponent },
  { title: "Register", path: "register", component: RegisterComponent },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [authGuard],
    children: [
      { title: "Clans", path: "clans", component: ClanListComponent },
      { title: "Clans", path: "clans/:clanId", component: ClanDetailsComponent },
      { title: "Raffle", path: "clans/:clanId/raffles/:raffleId", component: RaffleDetailsComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
