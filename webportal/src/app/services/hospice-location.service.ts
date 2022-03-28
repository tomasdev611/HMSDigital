import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';
import {SieveRequest} from '../models';

@Injectable({
  providedIn: 'root',
})
export class HospiceLocationService {
  constructor(private http: CoreApiService) {}

  getHospiceLocations(hospiceId, params?) {
    const path = hospiceId ? `hospices/${hospiceId}/hospice-locations` : `hospice-locations`;
    const queryParams = setQueryParams(params);
    return this.http.get(path, queryParams);
  }

  getHospiceLocationById(hospiceId: number, hospiceLocationId: number) {
    return this.http.get(`hospices/${hospiceId}/hospice-locations/${hospiceLocationId}`);
  }

  createHospiceLocation(hospiceId: number, hospiceLocation) {
    return this.http.post(`hospices/${hospiceId}/hospice-locations`, hospiceLocation);
  }

  updateHospiceLocation(
    hospiceId: number,
    hospiceLocationId: number,
    hospiceLocationPatchDocument
  ) {
    return this.http.patch(
      `hospices/${hospiceId}/hospice-locations/${hospiceLocationId}`,
      hospiceLocationPatchDocument
    );
  }

  deleteHospiceLocation(hospiceId: number, hospiceLocationId: number) {
    return this.http.delete(`hospices/${hospiceId}/hospice-locations/${hospiceLocationId}`);
  }

  searchLocations(searchRequest: any) {
    const queryParams = setQueryParams(searchRequest);
    return this.http.get(
      `hospices/${searchRequest.hospiceId}/hospice-locations/search`,
      queryParams
    );
  }

  getLocations(queryParams?: any) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospice-locations`, query);
  }
}
