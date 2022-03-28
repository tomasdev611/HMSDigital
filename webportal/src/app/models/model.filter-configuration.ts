import {FilterTypes, SieveOperators} from '../enums';

export interface FilterConfiguration {
  label: string;
  field: string;
  fields?: string[];
  type: FilterTypes;
  value?: any;
  dependent?: any;
  operator: SieveOperators;
  class?: string;
  datePickerConfig?: any;
}
