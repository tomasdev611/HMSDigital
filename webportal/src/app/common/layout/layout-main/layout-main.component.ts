import {Component, OnInit, HostListener, Inject, PLATFORM_ID} from '@angular/core';
import {Router, NavigationEnd} from '@angular/router';
import {isPlatformBrowser} from '@angular/common';
import {GoogleAnalyticsService} from 'src/app/services';
import {environment} from 'src/environments/environment';
@Component({
  selector: 'app-layout-main',
  templateUrl: './layout-main.component.html',
  styleUrls: ['./layout-main.component.scss'],
})
export class LayoutMainComponent implements OnInit {
  dispatchView = false;
  screenWidth: number;

  @HostListener('document:click', ['$event']) onDocumentClick(event) {
    const element = document.getElementsByClassName(
      'sidenav-wrapper'
    ) as HTMLCollectionOf<HTMLElement>;
    const wrapper = element[0];
    if (
      !wrapper.contains(event.target) &&
      event.target.className !== 'pi pi-bars sidenav-open-icon' &&
      this.screenWidth <= 1024
    ) {
      wrapper.style.display = 'none';
    }
  }

  @HostListener('window:resize', ['$event'])
  getScreenSize(event?) {
    this.screenWidth = window.innerWidth;
    if (this.screenWidth > 1024) {
      const element = document.getElementsByClassName(
        'sidenav-wrapper'
      ) as HTMLCollectionOf<HTMLElement>;
      const wrapper = element[0];
      if (wrapper) {
        wrapper.style.display = 'block';
      }
    }
  }

  constructor(
    private router: Router,
    private service: GoogleAnalyticsService,
    @Inject(PLATFORM_ID) private platformId: any
  ) {
    this.getScreenSize();
    this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
        this.dispatchView = event.url.includes('/dispatch');
        if (environment.gaTrackingId && isPlatformBrowser(this.platformId)) {
          this.service.logPageView(event.urlAfterRedirects);
        }
      }
    });
  }

  ngOnInit(): void {}
}
