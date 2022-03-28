export const environment = {
  production: true,
  aws: {
    clientId: '5it43qotdksr2duetikitp6bn2',
    oAuthHostURL: 'https://cognito-idp.us-east-1.amazonaws.com',
    userPoolId: 'us-east-1_9ViykHMDs',
    logoutUrl: 'https://hms-staging.auth.us-east-1.amazoncognito.com/logout',
  },
  appInsights: {
    instrumentationKey: '3c206314-4b82-4b65-9ca8-63e3ca8ebdf6',
  },
  azure: {
    mapSubscriptionKey: 'wMES8wte-szBigNUV8IGTGUrlvQtKQzsVZAhbrF6DF4',
  },
  apiServerURL: 'https://hmsd-api-training.azurewebsites.net',
  patientApiServerUrl: 'https://hmsd-patient-training.azurewebsites.net',
  scaOrderingUrl: 'https://order-training.hospicesource.net',
  routeOptimizerURL: 'https://hmsd-fulfillment-training.azurewebsites.net',
  gaTrackingId: 'G-ZSP8DMT696',
  reportServerUrl: 'https://hmsd-reportcenter-training.azurewebsites.net',
};
