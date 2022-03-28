export const environment = {
  production: false,
  aws: {
    clientId: '5it43qotdksr2duetikitp6bn2',
    oAuthHostURL: 'https://cognito-idp.us-east-1.amazonaws.com',
    userPoolId: 'us-east-1_9ViykHMDs',
    logoutUrl: 'https://hms-staging.auth.us-east-1.amazoncognito.com/logout',
  },
  appInsights: {
    instrumentationKey: '327e2be3-2ac3-4799-8896-f1c0f6ce2e0a',
  },
  azure: {
    mapSubscriptionKey: 'wMES8wte-szBigNUV8IGTGUrlvQtKQzsVZAhbrF6DF4',
  },
  apiServerURL: 'https://hmsd-api-dev.azurewebsites.net',
  patientApiServerUrl: 'https://hmsd-patient-dev.azurewebsites.net',
  scaOrderingUrl: 'https://order-dev.hospicesource.net',
  routeOptimizerURL: 'https://hmsd-fulfillment-dev.azurewebsites.net',
  gaTrackingId: 'G-90BR7YTTPG',
  reportServerUrl: 'https://hmsd-reportcenter-dev.azurewebsites.net',
};
