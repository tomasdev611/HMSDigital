import {Injectable} from '@angular/core';
import {ReportApiService} from './http/report-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  constructor(private http: ReportApiService) {}

  getOrdersMetric(reportRequest: any) {
    const queryParams = setQueryParams(reportRequest);
    return this.http.get('ordersMetric', queryParams);
  }

  getPatientsMetric(reportRequest: any) {
    const queryParams = setQueryParams(reportRequest);
    return this.http.get('patientsMetric', queryParams);
  }
}
