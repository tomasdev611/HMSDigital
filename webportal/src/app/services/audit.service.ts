import {Injectable} from '@angular/core';
import {setQueryParams} from '../utils';
import {CoreApiService} from './http/core-api.service';

@Injectable({
  providedIn: 'root',
})
export class AuditService {
  constructor(private http: CoreApiService) {}

  getAuditLogs(auditRequest) {
    return this.http.post(`audit`, auditRequest);
  }

  getDispatchAuditLogs(auditRequest) {
    const queryParams = setQueryParams(auditRequest);
    return this.http.get(`audit/dispatch-update`, queryParams);
  }

  getAuditLogsAsCsv(auditRequest) {
    const queryParams = setQueryParams(auditRequest);
    return this.http.getCsv(`audit.csv`, queryParams);
  }

  getAuditLogsById(id: number, auditType: string) {
    const queryParams = setQueryParams({auditType});
    return this.http.get(`audit/${id}`, queryParams);
  }
}
