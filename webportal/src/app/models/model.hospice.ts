import {HospiceLocation} from './model.hospiceLocation';
export interface Hospice {
  id: number;
  name: string;
  hospiceLocations: HospiceLocation[];
}
