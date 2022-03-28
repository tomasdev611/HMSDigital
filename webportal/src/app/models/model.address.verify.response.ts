export interface VerifiedAddress {
  addressLine1: string;
  addressLine2: string;
  addressLine3: string;
  addressUuid: string;
  country: string;
  state: string;
  county: string;
  city: string;
  zipCode: number;
  plus4Code: number;
  id: number;
  isVerified: boolean;
  isValid: boolean;
  latitude: number;
  longitude: number;
  results: string;
  verifiedBy: string;
}
