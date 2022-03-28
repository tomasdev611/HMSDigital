import {Injectable} from '@angular/core';
declare const ga: (...args: any[]) => () => void;

const has = Object.prototype.hasOwnProperty;

@Injectable({
  providedIn: 'root',
})
export class GoogleAnalyticsService {
  constructor() {}

  logPageView(url: string) {
    ga(() => {
      if (has.call(window, 'ga')) {
        ga('set', 'page', url);
        ga('send', 'pageview');
      }
    });
  }
}
