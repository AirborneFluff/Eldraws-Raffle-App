import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClansListComponent } from './features/clans/clans-list/clans-list.component';
import { authGuard } from './core/guards/auth.guard';

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
      { title: "Clans", path: "clans", component: ClansListComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
