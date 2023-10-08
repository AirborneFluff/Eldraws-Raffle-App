import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { AccountService } from '../services/account.service';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = () => {
  const account: AccountService = inject(AccountService);

  return account.currentUser$.pipe(
    map(user => {
      return !!user;
    })
  );
};
