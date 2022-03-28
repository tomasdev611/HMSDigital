import {Component, OnInit} from '@angular/core';
import {OAuthService} from 'angular-oauth2-oidc';
import {NavigationEnd, NavigationStart, Router} from '@angular/router';
import {getAccessibleMenus, isFeatureEnabled, IsPermissionAssigned} from 'src/app/utils';
import {Menus} from 'src/app/constants';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
})
export class SidenavComponent implements OnInit {
  menuItems = [];
  reportCenterFeatureFlag = isFeatureEnabled('ReportPortal');
  bottomMenuItems = [
    {
      label: 'Log Out',
      icon: 'pi pi-sign-out',
      command: () => {
        this.logout();
      },
    },
  ];
  activeItem: any;
  permissions = [];
  menuList = [...Menus];

  constructor(private oAuthService: OAuthService, private router: Router) {}

  ngOnInit(): void {
    this.menuItems = getAccessibleMenus();
    if (!this.reportCenterFeatureFlag) {
      const id = this.menuItems.findIndex(x => x.label === 'Metrics');
      if (id >= 0) {
        this.menuItems.splice(id, 1);
      }
    }

    this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationStart) {
        if (event.url) {
          this.menuList.map(menu => {
            if (event.url.includes(menu.routerLink)) {
              this.activeItem = menu;
            }
          });
        }
      }
    });
  }

  public logout() {
    localStorage.clear();
    this.oAuthService.logOut();
  }

  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }
}
