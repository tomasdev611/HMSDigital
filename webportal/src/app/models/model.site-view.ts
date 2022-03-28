import {Site} from './model.site';
export interface SiteView extends Site {
  phoneNumber: number;
  fax: number;
  isActive: boolean;
  siteCode: number;
  locationType: string;
  parent: any;
  parentNetSuiteLocationId: number;
  netSuiteLocationId: number;
}
