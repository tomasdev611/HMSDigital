import {Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {RoleService, ToastService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {TableVirtualScrollComponent} from 'src/app/common';
import {SieveRequest} from 'src/app/models';

@Component({
  selector: 'app-roles-permission',
  templateUrl: './roles-permission.component.html',
  styleUrls: ['./roles-permission.component.scss'],
})
export class RolesPermissionComponent implements OnInit {
  @Output() openFlyout = new EventEmitter<any>();
  roles = [];
  loading = false;
  showAddRoleModal = false;
  newRole: any = {};
  roleRequest = new SieveRequest();
  headers = [
    {label: 'Name', field: 'name', sortable: true},
    {label: 'Permissions', field: 'permissionsLength', sortable: true},
    {label: 'Type', field: 'roleType', sortable: true},
  ];
  @ViewChild('rolesPermissionTable ', {static: false})
  rolesPermissionTable: TableVirtualScrollComponent;

  constructor(private roleService: RoleService, private toastService: ToastService) {}

  ngOnInit(): void {
    this.roleRequest.pageSize = 100;
    this.getRoles();
  }
  getRoles() {
    this.loading = true;
    this.roleService
      .getAllRoles(this.roleRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: any) => {
        this.roles = response;
      });
  }

  showRoleModal() {
    this.showAddRoleModal = true;
  }

  roleSelected(event) {
    this.openFlyout.emit(event.currentRow);
  }

  sortRoles(event) {
    switch (event.order) {
      case 0:
        this.roleRequest.sorts = '';
        break;
      case 1:
        this.roleRequest.sorts = event.field;
        break;
      case -1:
        this.roleRequest.sorts = '-' + event.field;
        break;
    }
    this.dataTablesReset();
    this.getRoles();
  }

  dataTablesReset() {
    if (this.rolesPermissionTable) {
      this.rolesPermissionTable.reset();
    }
  }
}
