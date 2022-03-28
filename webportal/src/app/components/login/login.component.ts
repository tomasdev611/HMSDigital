import {Component, OnInit, OnDestroy} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {OAuthService, OAuthSuccessEvent} from 'angular-oauth2-oidc';
import {getDefaultRoute, getAccessibleMenus, storeCache} from 'src/app/utils';
import {hms} from 'src/app/constants';
import {CacheService} from 'src/app/services';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit, OnDestroy {
  loading = false;
  returnUrl = '';
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private oAuthService: OAuthService,
    private cacheService: CacheService
  ) {}

  ngOnInit(): void {
    const defaultRoute = getDefaultRoute(hms.defaultRoute);
    const returnUrl = this.route.snapshot.queryParams.returnUrl;
    const storedUrl = sessionStorage.getItem('returnUrl');
    this.returnUrl = returnUrl || storedUrl || defaultRoute;
    sessionStorage.setItem('returnUrl', this.returnUrl);
    this.checkPermissions();
  }

  ngOnDestroy() {
    if (localStorage.getItem('userPermissions')) {
      sessionStorage.removeItem('returnUrl');
    }
  }

  checkPermissions() {
    const user: any = this.oAuthService.getIdentityClaims();
    if (user && this.oAuthService.hasValidAccessToken() && this.oAuthService.hasValidIdToken()) {
      this.cacheService.loadCache().subscribe((response: any) => {
        storeCache(response);
        this.navigateToRoute(this.returnUrl);
      });
    } else {
      this.oAuthService.loadDiscoveryDocumentAndLogin();
      this.oAuthService.events.subscribe(event => {
        if (event instanceof OAuthSuccessEvent && event.type === 'token_received') {
          this.loading = true;
          this.checkPermissions();
        }
      });
    }
  }

  navigateToRoute(path) {
    const route = getDefaultRoute(path);
    this.router.navigate([route]);
  }
}
