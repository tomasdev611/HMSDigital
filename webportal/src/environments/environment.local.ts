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
  apiServerURL: 'http://localhost:5000',
  patientApiServerUrl: 'http://localhost:5002',
  scaOrderingUrl: 'https://order-dev.hospicesource.net',
  routeOptimizerURL: 'http://localhost:5003',
  gaTrackingId: '',
  reportServerUrl: 'http://localhost:5004',
};
