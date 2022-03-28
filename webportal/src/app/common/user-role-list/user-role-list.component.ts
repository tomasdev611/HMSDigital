import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {finalize} from 'rxjs/operators';
import {RoleService, HospiceService, SitesService, HospiceLocationService} from 'src/app/services';
import {PaginationResponse} from 'src/app/models';
import {SieveOperators} from 'src/app/enums';
import {buildFilterString} from 'src/app/utils/filter.utils';

@Component({
  selector: 'app-user-role-list',
  templateUrl: './user-role-list.component.html',
  styleUrls: ['./user-role-list.component.scss'],
})
export class UserRoleListComponent implements OnInit {
  roles = [];
  backUpRoles = [];
  resourceTypes = [
    {label: 'Site', value: 'Site'},
    {label: 'Hospice', value: 'Hospice'},
    {label: 'HospiceLocation', value: 'HospiceLocation'},
  ];
  filteredResourceTypes = [];
  resourceOptions = [];
  resourceList = [];
  siteList = [];
  hospiceList = [];
  userRole: any = {};
  headers = [
    {label: 'Role Name', field: 'roleName'},
    {label: 'Resource', field: 'resourceType'},
    {label: 'Scope', field: 'resource.name'},
    {
      label: '',
      field: '',
      class: 'xs',
      deleteBtn: 'Remove Resource',
      deleteBtnIcon: 'pi pi-trash',
    },
  ];

  @Input() loading = false;
  @Input() editmode = false;
  selectedRoleType = '';
  rolesObject: any;
  @Input() set roleType(type: string) {
    if (type !== this.selectedRoleType) {
      this.userRole = {};
    }
    this.selectedRoleType = type;
    this.filteredResourceTypes = [
      ...this.resourceTypes.filter(
        x =>
          this.selectedRoleType === '*' ||
          this.selectedRoleType === 'Internal' ||
          x.value === 'Hospice'
      ),
    ];
    this.assignedUserRoles = [];
  }
  @Input() assignedUserRoles: any[] = [];
  @Output() addUserRole = new EventEmitter<any>();
  @Output() deleteUserRole = new EventEmitter<any>();

  constructor(
    private roleService: RoleService,
    private hospiceService: HospiceService,
    private siteService: SitesService,
    private hospiceLocationService: HospiceLocationService
  ) {}

  ngOnInit(): void {
    this.getAllRoles();
  }
  searchResource({query}) {
    if (!this.userRole.role || !this.userRole.resourceType) {
      return;
    }
    if (query.length < 2) {
      this.resourceList = [{id: '*', name: 'All'}];
      return;
    }
    let getResourceList;
    switch (this.userRole.resourceType) {
      case 'Site':
        getResourceList = this.siteService.searchSites({searchQuery: query});
        break;
      case 'Hospice':
        getResourceList = this.hospiceService.searchHospices({
          searchQuery: query,
        });
        break;
      case 'HospiceLocation': {
        const filterValues = [
          {
            field: 'name',
            operator: SieveOperators.Contains,
            value: [query],
          },
        ];
        const locationReq = {filters: buildFilterString(filterValues)};
        getResourceList = this.hospiceLocationService.getHospiceLocations(null, locationReq);
      }
    }
    getResourceList.subscribe((res: PaginationResponse) => {
      this.resourceList = res.records.map(r => ({id: r.id, name: r.name}));
    });
  }
  fetchList(resourceType) {
    resourceType = resourceType.toLowerCase();
    let getResourceList;
    switch (resourceType) {
      case 'site':
        getResourceList = this.siteService.getAllSites();
        break;
      case 'hospice':
        getResourceList = this.hospiceService.getAllhospices();
        break;
    }
    getResourceList.subscribe((res: PaginationResponse) => {
      this[`${resourceType}List`] = res.records.map(r => ({
        id: r.id,
        name: r.name,
      }));
    });
  }
  getAllRoles() {
    this.roleService
      .getAllRoles()
      .pipe(finalize(() => {}))
      .subscribe((response: any) => {
        this.backUpRoles = response;
      });
  }

  AddUserRole() {
    this.addUserRole.emit(this.userRole);
    this.userRole = {};
  }

  removeUserRole(index: number) {
    this.deleteUserRole.emit(index);
  }

  IsUserRoleValid(): boolean {
    return !(this.userRole.role && this.userRole.resourceType && this.userRole.resource);
  }

  checkRoles(event) {
    if (this.roles.length === 0) {
      this.getAllRoles();
    }
  }
  rolesOpened() {
    this.roles = [
      ...this.backUpRoles.filter(
        x => this.selectedRoleType === '*' || x.roleType === this.selectedRoleType
      ),
    ];
  }
  rolesChanged(event) {
    this.userRole.resourceType =
      this.selectedRoleType === '*' || this.selectedRoleType === 'Internal' ? 'Site' : 'Hospice';
    if (!this.editmode) {
      this.addUserRole.emit(event.value);
    }
  }

  updateResource(role, multiselect?) {
    if (!multiselect) {
      role.resource[role?.resourceType?.toLowerCase()] = [];
      this[`${role?.resourceType?.toLowerCase()}List`] = [];
    }
    this.assignedUserRoles = [...this.assignedUserRoles];
  }
}
