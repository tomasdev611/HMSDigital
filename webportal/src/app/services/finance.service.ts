import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';

@Injectable({
  providedIn: 'root',
})
export class FinanceService {
  constructor(private http: CoreApiService) {}

  fixHospice(patientUuid: string, fixBody: any) {
    return this.http.post(`finance/patients/${patientUuid}/hospice`, fixBody);
  }

  mergePatients(patientUuid: string, mergeBody: any) {
    return this.http.post(`finance/patients/${patientUuid}/merge`, mergeBody);
  }

  movePatientHospice(patientUuid: string, moveBody: any) {
    return this.http.post(
      `finance/patients/${patientUuid}/move-patient-to-hospice-location`,
      moveBody
    );
  }
}
