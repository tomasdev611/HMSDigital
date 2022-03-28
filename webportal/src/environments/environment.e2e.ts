export const environment = {
  production: false,
  aws: {
    clientId: '5it43qotdksr2duetikitp6bn2',
    oAuthHostURL: 'https://cognito-idp.us-east-1.amazonaws.com',
    userPoolId: 'us-east-1_9ViykHMDs',
    logoutUrl: 'https://hms-staging.auth.us-east-1.amazoncognito.com/logout',
  },
  appInsights: {
    instrumentationKey: 'e6591505-c252-444d-82cb-b6a39d4155fe',
  },
  azure: {
    mapSubscriptionKey: 'wMES8wte-szBigNUV8IGTGUrlvQtKQzsVZAhbrF6DF4',
  },
  apiServerURL: 'https://hmsd-api-e2e.azurewebsites.net',
  patientApiServerUrl: 'https://hmsd-patient-e2e.azurewebsites.net',
  scaOrderingUrl: 'https://order-e2e.hospicesource.net',
  routeOptimizerURL: 'https://hmsd-fulfillment-e2e.azurewebsites.net',
  gaTrackingId: 'G-X4HRKHW3LE',
  reportServerUrl: 'https://hmsd-reportcenter-e2e.azurewebsites.net',
};
