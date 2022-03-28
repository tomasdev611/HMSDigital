import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';

import {RoleService, UserService} from 'src/app/services';
import {PaginationResponse, FilterConfiguration} from 'src/app/models';
import {FilterTypes, SieveOperators} from 'src/app/enums';
import {deepCloneObject, getUniqArray, IsPermissionAssigned} from 'src/app/utils';
import {ActivatedRoute} from '@angular/router';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {UserAuditActionTypes} from 'src/app/constants';
import {UserListComponent} from 'src/app/common';
import {RolesPermissionComponent} from 'src/app/components';
import {TabView} from 'primeng/tabview';
import {Subscription} from 'rxjs';
import {Location} from '@angular/common';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit, OnDestroy {
  roleDetailsViewOpen = false;
  roleDetails: any;
  auditLogDetails: any;
  permissionGroups: any;
  actionTypes = deepCloneObject(UserAuditActionTypes).filter((x: any) => {
    x.auditAction = x.name || '*';
    x.name = x.label;
    return x;
  });
  filterConfiguration: FilterConfiguration[] = [];
  userFilterConfiguration: FilterConfiguration[] = [
    {
      label: 'Status',
      field: 'IsDisabled',
      type: FilterTypes.MultiSelect,
      value: [
        {label: 'Enabled', value: 'false'},
        {label: 'Disabled', value: 'true'},
      ],
      operator: SieveOperators.Equals,
    },
  ];

  filterString: string;
  allUsersCount = 0;
  searchQuery: string;
  users: any;
  userResponse: PaginationResponse;
  headers = [
    {label: 'Name', field: 'name', sortable: true},
    {label: 'Email Address', field: 'email', sortable: true},
    {
      label: 'Phone Number',
      field: 'phoneNumber',
      sortable: true,
      fieldType: 'Phone',
    },
  ];

  enableUserHeader = {
    label: 'Enabled',
    field: '',
    class: 'sm',
    inputSwitch: true,
    inputSwitchValue: 'enabled',
    readOnly: true,
  };

  editUserHeader = {
    label: '',
    field: '',
    class: 'xs',
    editBtn: 'Edit User',
    editBtnIcon: 'pi pi-user-edit',
    editBtnLink: '/users/edit',
    linkParams: 'id',
  };

  view = 'users';
  entities: any;
  subscriptions: Subscription[] = [];
  @ViewChild('filter', {static: false}) appFilter;
  @ViewChild('auditLog', {static: false}) auditLog;
  @ViewChild('userList', {static: false}) userList: UserListComponent;
  @ViewChild('rolesPermission', {static: false})
  rolesPermission: RolesPermissionComponent;
  @ViewChild('tabs', {static: false}) tabs: TabView;

  csvLoading = false;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private location: Location,
    private navbarSearchService: NavbarSearchService,
    private roleService: RoleService
  ) {}

  ngOnInit(): void {
    if (IsPermissionAssigned('User', 'Delete')) {
      this.enableUserHeader.readOnly = false;
    }
    this.headers.push(this.enableUserHeader as any);
    if (IsPermissionAssigned('User', 'Update')) {
      this.headers.push(this.editUserHeader as any);
    }
    this.route.queryParams.subscribe(params => {
      this.view = params.view || this.view || 'users';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.view}).toString()
      );
    });
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(text => this.searchUser(text))
    );
    this.getRoles();
  }

  getRoles() {
    this.roleService.getAllRoles().subscribe((response: any) => {
      response = response.map((r: any) => {
        return {label: r.name, value: r.id};
      });
      this.userFilterConfiguration = [
        ...this.userFilterConfiguration,
        {
          label: 'Role',
          field: 'roleId',
          type: FilterTypes.MultiSelect,
          value: response,
          operator: SieveOperators.Equals,
        },
      ];
      this.filterConfiguration = this.view === 'users' ? this.userFilterConfiguration : [];
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  getUsers({userRequest, searchQuery}) {
    if (searchQuery) {
      const params = {...userRequest, searchQuery};
      this.userService.searchUser(params).subscribe((res: any) => {
        this.users = res.records;
        this.userResponse = res;
      });
    } else {
      this.userService.getAllUsers(userRequest).subscribe((response: any) => {
        this.users = response.records;
        this.userResponse = response;
      });
    }
  }

  filterUsers({filterString}) {
    this.filterString = filterString;
  }

  filterChanged(event) {
    if (this.view === 'users') {
      this.filterUsers(event);
    }
    this.filterString = event.filterString;
  }

  searchUser(query) {
    this.searchQuery = query;
  }

  shouldShowTab(entity) {
    return IsPermissionAssigned(entity, 'Read');
  }

  tabChanged({index}) {
    const tabName = this.tabs.tabs[index].header;
    this.closeRoleDeatils();
    this.filterConfiguration = [];
    this.filterString = '';
    switch (tabName) {
      case 'Users':
        this.view = 'users';
        this.userList.dataTablesReset();
        this.filterConfiguration = [...this.userFilterConfiguration];
        break;
      case 'Roles':
        this.view = 'roles';
        this.rolesPermission?.dataTablesReset();
        break;
      case 'Audit Logs':
        this.view = 'audit';
        break;
    }
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.view}).toString()
    );
    this.appFilter?.clearFilter();
  }

  closeRoleDeatils() {
    this.roleDetailsViewOpen = false;
    this.roleDetails = null;
    this.auditLogDetails = null;
  }

  showRoleFlyout(roleDetails) {
    this.roleDetailsViewOpen = true;
    const permissions = getUniqArray(roleDetails.permissions);
    this.permissionGroups = permissions.reduce((accum, p) => {
      const [key, value] = p.split(':');
      const existingItemIndex = accum.findIndex(a => a.label === key);
      if (existingItemIndex >= 0) {
        accum[existingItemIndex].items.push({label: value});
        return accum;
      }
      accum.push({label: key, items: [{label: value}]});
      return accum;
    }, []);
    this.roleDetails = roleDetails;
  }

  getPermissionFor(group) {
    return this.entities[group];
  }
  exportUserAuditLogs() {
    this.auditLog.exportUserAuditLogs();
    this.csvLoading = true;
  }
  toggleCsvLoading(event) {
    this.csvLoading = false;
  }

  checkPermission(entity: string, action: string) {
    return IsPermissionAssigned(entity, action);
  }
}
