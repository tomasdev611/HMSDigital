import {Address} from './model.address';
import {PhoneNumber} from './model.phone-number';
export interface Patient {
  id: number;
  firstName: string;
  lastName: string;
  hospiceId: number;
  dateOfBirth: string;
  patientHeight: number;
  patientWeight: number;
  status: string;
  statusChangedDate: string;
  isPediatric: boolean;
  emergencyOptOut: boolean;
  emergencyContactName: string;
  isCovidPositive: boolean;
  patientAddress: Address[];
  phoneNumbers: PhoneNumber[];
  hospiceLocationId: number;
  facilityId: number;
  hospiceName?: string;
  name?: string;
  hospice?: string;
}
