import {Injectable} from '@angular/core';
import {Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {OAuthService} from 'angular-oauth2-oidc';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private oAuthService: OAuthService) {}

  getAccessToken() {
    return this.oAuthService.hasValidAccessToken() && this.oAuthService.hasValidIdToken();
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.getAccessToken()) {
      // logged in so return true
      return true;
    }
    // not logged in so redirect to login page with the return url
    localStorage.clear();
    this.router.navigate(['/login'], {queryParams: {returnUrl: state.url}});
    return false;
  }
}
