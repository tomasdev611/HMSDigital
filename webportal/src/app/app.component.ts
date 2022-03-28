import {Component, OnInit} from '@angular/core';
import {AuthConfig, OAuthService} from 'angular-oauth2-oidc';
import {environment} from 'src/environments/environment';
import {MonitoringService} from './services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'webportal';
  constructor(private oAuthService: OAuthService, private monitoringService: MonitoringService) {
    this.registerGoogleAnalyticScript();
    this.configure();
  }

  private configure() {
    this.oAuthService.setStorage(localStorage);
    this.oAuthService.configure(oAuthConfig);
  }
  ngOnInit() {}

  registerGoogleAnalyticScript() {
    if (environment.gaTrackingId) {
      // register google analytics
      const gaScript = document.createElement('script');
      gaScript.innerHTML = `
      (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
        (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
        m=s.getElementsByTagName(o)
        [0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
        })(window,document,'script','//www.google-analytics.com/analytics.js','ga');
        gtag('js', new Date());
        gtag('config', '${environment.gaTrackingId}');
      `;
      document.head.appendChild(gaScript);
    }
  }
}

export const oAuthConfig: AuthConfig = {
  issuer: `${environment.aws.oAuthHostURL}/${environment.aws.userPoolId}`,

  redirectUri: window.location.origin + '/login',
  logoutUrl: `${environment.aws.logoutUrl}?response_type=code&client_id=${environment.aws.clientId}&redirect_uri=${window.location.origin}/login`,
  postLogoutRedirectUri: window.location.origin + '/login',
  // The SPA's id. The SPA is registerd with this id at the auth-server
  // clientId: 'server.code',
  clientId: environment.aws.clientId,

  responseType: 'code',

  scope: 'openid phone email profile aws.cognito.signin.user.admin',

  showDebugInformation: true,

  // all urls in discovery document does not need to start with issuer url
  strictDiscoveryDocumentValidation: false,

  // Not recommented:
  // disablePKCE: true,
};
