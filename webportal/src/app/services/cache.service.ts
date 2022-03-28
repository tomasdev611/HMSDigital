import {Injectable} from '@angular/core';
import {forkJoin} from 'rxjs';
import {EnumService} from './enum.service';
import {RoleService} from './role.service';
import {UserService} from './user.service';
import {SystemService} from './system.service';

@Injectable({
  providedIn: 'root',
})
export class CacheService {
  constructor(
    private userService: UserService,
    private enumService: EnumService,
    private roleService: RoleService,
    private systemService: SystemService
  ) {}

  loadCache() {
    const requests = [];
    requests.push(this.userService.getMyUser());
    requests.push(this.enumService.getEnumerations());
    requests.push(this.roleService.getAllRoles());
    requests.push(this.systemService.getAllSystemFeatures());
    return forkJoin(requests);
  }
}
