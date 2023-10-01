import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { AccountService } from '../services/account.service';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const account = inject(AccountService);

  return account.currentUser$.pipe(
    map(user => {
      return !!user;
    })
  );
};
