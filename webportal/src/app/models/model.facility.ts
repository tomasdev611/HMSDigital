import {Address} from './model.address';
export interface Facility {
  id: number;
  name: string;
  hospiceId: number;
  facilityPhoneNumber: any;
  address: Address;
  hospiceLocationId: number;
  isDisable: boolean;
  site: any;
}
