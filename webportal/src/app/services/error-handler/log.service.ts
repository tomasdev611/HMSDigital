import {ApplicationInsights} from '@microsoft/applicationinsights-web';
import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MonitoringService {
  appInsights: ApplicationInsights;
  constructor() {
    this.appInsights = new ApplicationInsights({
      config: {
        instrumentationKey: environment.appInsights.instrumentationKey,
        enableAutoRouteTracking: true, // option to log all route changes,
        autoTrackPageVisitTime: true,
      },
    });
    this.appInsights.loadAppInsights();
    this.appInsights.trackPageView();
  }

  logPageView(name?: string, url?: string) {
    // option to call manually
    this.appInsights.trackPageView({
      name,
      uri: url,
    });
  }

  logEvent(name: string, properties?: {[key: string]: any}) {
    this.appInsights.trackEvent({name}, properties);
  }

  logMetric(name: string, average: number, properties?: {[key: string]: any}) {
    this.appInsights.trackMetric({name, average}, properties);
  }

  logException(exception: Error, severityLevel?: number) {
    console.log('LogService: ' + exception.message);
    this.appInsights.trackException({exception, severityLevel});
  }

  logTrace(message: string, properties?: {[key: string]: any}) {
    this.appInsights.trackTrace({message}, properties);
  }
}
