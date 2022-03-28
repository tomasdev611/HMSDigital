import {Address} from './model.address';
import {SitePhoneNumber} from './model.site-phone-number';
export interface Site {
  id: number;
  name: string;
  isDisable: boolean;
  sitePhoneNumber: SitePhoneNumber[];
  address: Address;
}
