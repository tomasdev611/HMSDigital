export interface User {
  defaultSiteId: number;
  driverId: number;
  email: string;
  enabled: boolean;
  firstName: string;
  lastName: string;
  name: string;
  phoneNumber: number;
  countryCode: number;
  sites: [number];
  userId: string;
  userStatus: string;
  isEmailVerified: boolean;
  isPhoneNumberVerified: boolean;
}
