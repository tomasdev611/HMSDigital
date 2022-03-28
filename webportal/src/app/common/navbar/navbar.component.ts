import {Component, OnInit} from '@angular/core';
import {OAuthService} from 'angular-oauth2-oidc';
import {NavigationEnd, NavigationStart, Router} from '@angular/router';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {ViewChild} from '@angular/core';
import {SearchBarComponent} from '../search-bar/search-bar.component';
import {Subscription} from 'rxjs';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  @ViewChild('searchBar', {static: true}) searchBar: SearchBarComponent;
  user: any;
  contactViewOpen = false;
  showSearch = false;
  routesWithSearch = [
    '/dashboard',
    '/patients',
    '/users',
    '/inventory',
    '/vehicles',
    '/sites',
    '/drivers',
    '/hospice',
  ];
  searchPreservedRoutes = ['patients'];
  subscriptions: Subscription[] = [];
  userImageUrl = '';
  constructor(
    private oAuthService: OAuthService,
    private router: Router,
    private navbarSearchService: NavbarSearchService
  ) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart || event instanceof NavigationEnd) {
        this.showSearch = this.routesWithSearch.some(route => {
          const subString = event.url.replace(route, '');
          return subString.indexOf('/') === -1;
        });
      }
    });
  }

  ngOnInit(): void {
    const myInfo: any = localStorage.getItem('me');
    if (myInfo) {
      this.user = JSON.parse(myInfo);
      this.userImageUrl = this.user.profilePictureUrl;
    }

    this.subscriptions.push(
      this.navbarSearchService.image.subscribe(text => (this.userImageUrl = text ?? ''))
    );
    this.subscriptions.push(
      this.navbarSearchService.userInfo.subscribe(userInfo => (this.user = userInfo))
    );
  }
  closeFlyout() {
    this.contactViewOpen = false;
  }
  openFlyout() {
    this.contactViewOpen = true;
  }
  search(text) {
    this.navbarSearchService.searchTextChanged(text);
  }
  openSideNav() {
    const element = document.getElementsByClassName(
      'sidenav-wrapper'
    ) as HTMLCollectionOf<HTMLElement>;
    const wrapper = element[0];
    if (wrapper.style.display === '' || wrapper.style.display === 'none') {
      wrapper.style.display = 'block';
    } else {
      wrapper.style.display = 'none';
    }
  }
}
