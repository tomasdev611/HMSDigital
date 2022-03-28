import {Injectable} from '@angular/core';
import {setQueryParams} from '../utils';
import {Patient} from '../models';
import {PatientApiService} from './http/patient-api.service';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  constructor(private http: PatientApiService) {}

  getPatients(patientRequest?: any) {
    const queryParams = setQueryParams(patientRequest);
    return this.http.get(`patients`, queryParams);
  }

  createPatient(patient: Patient) {
    return this.http.post(`patients`, patient);
  }

  searchPatient(searchRequest) {
    return this.http.post(`patients/search`, searchRequest);
  }

  getPatientById(patientId: number) {
    return this.http.get(`patients/${patientId}`);
  }

  getPatientNotes(patientId: number) {
    return this.http.get(`patients/${patientId}/notes`);
  }

  updatePatient(patientId: number, patientPatch, queryParams?) {
    return this.http.patch(`patients/${patientId}`, patientPatch, queryParams);
  }

  searchPatientsBySearchQuery(params?: any) {
    const query = setQueryParams(params);
    return this.http.get(`patients/search`, query);
  }

  updateStatus(patientId: number, statusBody) {
    return this.http.post(`patients/${patientId}/status`, statusBody);
  }

  getPatientMergeHistory(historyRequest) {
    const requestQuery = setQueryParams(historyRequest);
    return this.http.get(`patients/merge/history`, requestQuery);
  }
}
