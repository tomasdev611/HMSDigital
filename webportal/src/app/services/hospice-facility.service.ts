import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class HospiceFacilityService {
  constructor(private http: CoreApiService) {}

  getAllHospiceFacilities(hospiceId: number) {
    return this.http.get(`hospices/${hospiceId}/facilities`);
  }

  getHospiceFacilityById(hospiceId: number, hospiceFacilityId: number) {
    return this.http.get(`hospices/${hospiceId}/facilities/${hospiceFacilityId}`);
  }

  createHospiceFacility(hospiceId: number, hospiceFacility) {
    return this.http.post(`hospices/${hospiceId}/facilities`, hospiceFacility);
  }

  updateHospiceFacility(
    hospiceId: number,
    hospiceFacilityId: number,
    hospiceFacilityPatchDocument
  ) {
    return this.http.patch(
      `hospices/${hospiceId}/facilities/${hospiceFacilityId}`,
      hospiceFacilityPatchDocument
    );
  }

  assignPatientToFacility(hospiceId: number, body: any) {
    return this.http.put(`hospices/${hospiceId}/facilities/patients`, body);
  }

  getPatientsByFacilityId(hospiceId: number, facilityId: number) {
    return this.http.get(`hospices/${hospiceId}/facilities/${facilityId}/patients/search`);
  }

  getFacilityInputMappings(hospiceId: number) {
    const queryParams = setQueryParams({mappedItemType: 'Facility'});
    return this.http.get(`hospices/${hospiceId}/input-mappings`, queryParams);
  }

  createHospiceFacilitesFromCsv(
    hospiceId: number,
    file: File,
    parse: boolean = false,
    validate: boolean = false
  ) {
    const data = new FormData();
    if (file && file instanceof File) {
      data.append('facilities', file);
    }
    return this.http.post(
      `hospices/${hospiceId}/facilities.csv?parseOnly=${parse}&validateOnly=${validate}`,
      data
    );
  }

  getHospicePatientsFacilities(hospiceId: number, queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/${hospiceId}/facilities/patients`, query);
  }

  getFacilities(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`facilities`, query);
  }
}
