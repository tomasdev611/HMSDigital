import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {convertObjectToArray} from '../utils';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const permissions = JSON.parse(localStorage.getItem('userPermissions'));
    const isAllowed = convertObjectToArray(route.data).reduce((allowed: boolean, obj: any) => {
      const p = permissions?.find((x: any) => x === `${obj.name}:${obj.access}`);
      return allowed && p;
    }, true);

    if (isAllowed) {
      return true;
    }
    this.router.navigate(['/forbidden']);
  }
}
