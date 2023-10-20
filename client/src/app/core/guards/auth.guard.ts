import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AccountService } from '../services/account.service';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = () => {
  const account: AccountService = inject(AccountService);
  const router: Router = inject(Router);

  return account.currentUser$.pipe(
    map(user => {
      const isAuthenticated = !!user;
      if (!isAuthenticated) {
        return router.createUrlTree(['/login']);
      }

      return isAuthenticated;
    })
  );
};
