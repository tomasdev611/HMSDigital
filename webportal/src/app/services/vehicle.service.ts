import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  constructor(private http: CoreApiService) {}

  getAllVehicles(vehicleRequest?: any) {
    const queryParams = setQueryParams(vehicleRequest);
    return this.http.get(`vehicles`, queryParams);
  }

  getVehicleDetail(vehicleId: number) {
    return this.http.get(`vehicles/${vehicleId}`);
  }

  searchVehicles(searchParam: any) {
    const query = setQueryParams(searchParam);
    return this.http.get('vehicles/search', query);
  }
}
