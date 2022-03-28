import {Injectable} from '@angular/core';
import {HttpParams} from '@angular/common/http';
import {CustomHttpUrlEncodingCodecService} from './custom-http-url-encoding-codec.service';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: CoreApiService) {}

  getAllUsers(usersRequest?: any) {
    const queryParams = setQueryParams(usersRequest);
    return this.http.get('users', queryParams);
  }
  getMyUser() {
    return this.http.get(`users/me`);
  }
  searchUser(userRequset?: any) {
    const query = setQueryParams(userRequset);
    return this.http.get(`users/search`, query);
  }
  getUserById(userId) {
    return this.http.get(`users/${userId}`);
  }
  createUser(user) {
    return this.http.post(`users`, user);
  }
  enableUser(userId) {
    return this.http.post(`users/${userId}/enable`, {});
  }
  disableUser(userId) {
    return this.http.post(`users/${userId}/disable`, {});
  }
  updateUserSites(userId, siteIds: any) {
    return this.http.put(`users/${userId}/sites`, siteIds);
  }
  getSites() {
    return this.http.get(`sites`);
  }
  getUserSites(userId) {
    return this.http.get(`users/${userId}/sites`);
  }
  getDrivers(user) {
    return this.http.post(`drivers/search`, user);
  }
  updateUser(userId, user) {
    return this.http.put(`users/${userId}`, user);
  }
  updateSelfUser(user) {
    const query = setQueryParams(user);
    return this.http.put(`users/me`, query);
  }
  resetPassword(userId, body) {
    return this.http.post(`users/${userId}/reset-password`, body);
  }
  changeSelfPassword(body) {
    return this.http.post(`users/me/change-password`, body);
  }
  sendResetPasswordLink(userId, body) {
    return this.http.post(`users/${userId}/reset-password-link`, body);
  }
  getUserRoles(userId) {
    return this.http.get(`users/${userId}/roles`);
  }
  addUserRole(userId, userRoleRequest) {
    return this.http.post(`users/${userId}/roles`, userRoleRequest);
  }
  removeUserRole(userId, userRoleId: number) {
    return this.http.delete(`users/${userId}/roles/${userRoleId}`);
  }
  sendVerificationCode(userId, attribute) {
    return this.http.post(`users/${userId}/send-confirmation-code`, attribute);
  }
  verifyCode(userId, body) {
    return this.http.post(`users/${userId}/verify-code`, body);
  }
  getUploadUrl(userId, body) {
    const query = setQueryParams(body);
    return this.http.post(`users/${userId}/profile-pic`, query);
  }
  confirmImageUpload(userId) {
    return this.http.post(`users/${userId}/profile-pic/confirm`, null);
  }
  getProfileUrl(userId) {
    return this.http.get(`users/${userId}/profile-pic`);
  }
  deleteProfileImage(userId) {
    return this.http.delete(`users/${userId}/profile-pic`);
  }
}
