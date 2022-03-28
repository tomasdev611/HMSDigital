// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  aws: {
    clientId: '5it43qotdksr2duetikitp6bn2',
    oAuthHostURL: 'https://cognito-idp.us-east-1.amazonaws.com',
    userPoolId: 'us-east-1_9ViykHMDs',
    logoutUrl: 'https://hms-staging.auth.us-east-1.amazoncognito.com/logout',
  },
  appInsights: {
    instrumentationKey: '44dab033-3d2f-41b3-bf6d-23e3434b81bd',
  },
  azure: {
    mapSubscriptionKey: 'wMES8wte-szBigNUV8IGTGUrlvQtKQzsVZAhbrF6DF4',
  },
  apiServerURL: 'https://hmsd-api-dev.azurewebsites.net',
  patientApiServerUrl: 'https://hmsd-patient-dev.azurewebsites.net',
  scaOrderingUrl: 'https://order-dev.hospicesource.net',
  routeOptimizerURL: 'https://hmsd-fulfillment-dev.azurewebsites.net',
  gaTrackingId: '',
  reportServerUrl: 'https://hmsd-reportcenter-dev.azurewebsites.net',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
