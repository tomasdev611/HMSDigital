import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class HospiceMemberService {
  constructor(private http: CoreApiService) {}

  createHospiceMembersFromCsv(
    hospiceId: number,
    file: File,
    parse: boolean = false,
    validate: boolean = false
  ) {
    const data = new FormData();
    if (file && file instanceof File) {
      data.append('members', file);
    }
    return this.http.post(
      `hospices/${hospiceId}/members.csv?parseOnly=${parse}&validateOnly=${validate}`,
      data
    );
  }

  getAllHospiceMembers(hospiceId: number, queryParams?) {
    const query = setQueryParams(queryParams);
    return this.http.get(`hospices/${hospiceId}/members`, query);
  }

  getHospiceMemberById(hospiceId: number, hospiceMemberId: number) {
    return this.http.get(`hospices/${hospiceId}/members/${hospiceMemberId}`);
  }

  createHospiceMember(hospiceId: number, hospiceMember) {
    return this.http.post(`hospices/${hospiceId}/members`, hospiceMember);
  }

  deleteHospiceMember(hospiceId: number, memberId: number) {
    return this.http.delete(`hospices/${hospiceId}/members/${memberId}`);
  }

  updateHospiceMember(hospiceId: number, hospiceMemberId: number, hospiceMember: any) {
    return this.http.put(`hospices/${hospiceId}/members/${hospiceMemberId}`, hospiceMember);
  }

  getApproverList() {
    return this.http.get(`hospices/approver-contacts`);
  }

  resetPassword(hospiceId, memberId, body) {
    return this.http.post(`hospices/${hospiceId}/members/${memberId}/reset-password`, body);
  }

  sendResetPasswordLink(hospiceId, memberId, body) {
    return this.http.post(`hospices/${hospiceId}/members/${memberId}/reset-password-link`, body);
  }
}
