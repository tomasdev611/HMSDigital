import {Injectable} from '@angular/core';
import {setQueryParams} from '../utils';
import {CoreApiService} from './http/core-api.service';

@Injectable({
  providedIn: 'root',
})
export class SiteMembersService {
  constructor(private http: CoreApiService) {}

  getAllSiteMembers(siteId: number, params?: any) {
    const queryParams = setQueryParams(params);
    return this.http.get(`sites/${siteId}/members`, queryParams);
  }

  getSiteMemberById(siteId: number, siteMemberId: number) {
    return this.http.get(`sites/${siteId}/members/${siteMemberId}`);
  }

  createSiteMember(siteId: number, siteMember) {
    return this.http.post(`sites/${siteId}/members`, siteMember);
  }

  updateSiteMember(siteId: number, siteMemberId: number, siteMember: any) {
    return this.http.put(`sites/${siteId}/members/${siteMemberId}`, siteMember);
  }
}
