export const environment = {
  production: true,
  aws: {
    clientId: '85vcapvo4ajltfv3ucp0j0ocm',
    oAuthHostURL: 'https://cognito-idp.us-east-1.amazonaws.com',
    userPoolId: 'us-east-1_TBQXRrCsz',
    logoutUrl: 'https://hms3-dev.auth.us-east-1.amazoncognito.com/logout',
  },
  appInsights: {
    instrumentationKey: '7b5e23ae-2f5a-4e26-bbdc-f03d2af1893c',
  },
  azure: {
    mapSubscriptionKey: 'wMES8wte-szBigNUV8IGTGUrlvQtKQzsVZAhbrF6DF4',
  },
  apiServerURL: 'https://hmsd-api-prod.azurewebsites.net',
  patientApiServerUrl: 'https://hmsd-patient-prod.azurewebsites.net',
  scaOrderingUrl: 'https://order.hospicesource.net',
  routeOptimizerURL: 'https://hmsd-fulfillment-prod.azurewebsites.net',
  gaTrackingId: 'G-WXNZYG4CL9',
  reportServerUrl: 'https://hmsd-reportcenter-prod.azurewebsites.net',
};
