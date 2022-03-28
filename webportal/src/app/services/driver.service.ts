import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class DriverService {
  constructor(private http: CoreApiService) {}

  getAllDrivers(driverRequest: any) {
    const queryParams = setQueryParams(driverRequest);
    return this.http.get('drivers', queryParams);
  }

  searchDrivers(driverRequest: any) {
    const queryParams = setQueryParams(driverRequest);
    return this.http.get('drivers/search', queryParams);
  }

  getDriverById(driverId: number) {
    return this.http.get(`drivers/${driverId}`);
  }

  saveDriver(driver) {
    return this.http.post(`drivers`, driver);
  }

  updateDriver(driverId: number, driverPatch) {
    return this.http.put(`drivers/${driverId}`, driverPatch);
  }

  deleteDriver(driverId: number) {
    return this.http.delete(`drivers/${driverId}`);
  }
}
