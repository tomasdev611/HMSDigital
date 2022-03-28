import {Injectable} from '@angular/core';
import {CoreApiService} from './http/core-api.service';
import {setQueryParams} from '../utils';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  constructor(private http: CoreApiService) {}

  getAllRoles(roleRequuest?: any) {
    const queryParams = setQueryParams(roleRequuest);
    return this.http.get('roles', queryParams);
  }

  getUsersForRole(roleId: number, usersRequest?: any) {
    const queryParams = setQueryParams(usersRequest);
    return this.http.get(`roles/${roleId}/users`, queryParams);
  }
}
