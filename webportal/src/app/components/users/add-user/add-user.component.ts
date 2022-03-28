import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {UserService, ToastService} from 'src/app/services';
import {User} from 'src/app/models';
import {finalize} from 'rxjs/operators';
import {Router} from '@angular/router';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
})
export class AddUserComponent implements OnInit {
  userForm: FormGroup;
  submitted: boolean;
  loading = false;
  user: User;
  assignedUserRoles = [];
  roles = [];
  sites = [];
  selectedRoles = [];
  formSubmit = false;
  item: string;
  userType = '';

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastService: ToastService,
    private router: Router
  ) {
    this.setUserForm();
  }

  ngOnInit(): void {}

  setUserForm() {
    this.userForm = this.fb.group({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.required),
      phoneNumber: new FormControl(null),
      userRoles: new FormControl(null),
    });
  }

  onSubmitUser(value: any) {
    const controls = this.userForm.controls;
    controls.phoneNumber.setValue(value.phoneNumber ? parseInt(value.phoneNumber, 10) : 0);
    value.userRoles = [];
    this.assignedUserRoles.map(ur => {
      if (ur.resource.id) {
        if (ur.resource.id === '*') {
          const obj = {
            roleId: ur.roleId,
            resourceType: ur.resourceType,
            resourceId: ur.resource.id,
          };
          value.userRoles = [...value.userRoles, obj];
        } else if (ur.resource.id === 'Selected') {
          ur.resource[ur.resourceType.toLowerCase()].map(res => {
            const obj = {
              roleId: ur.roleId,
              resourceType: ur.resourceType,
              resourceId: res.id,
            };
            value.userRoles = [...value.userRoles, obj];
          });
        }
      }
    });
    controls.userRoles.setValue(value.userRoles ? value.userRoles : []);
    this.formSubmit = true;
    this.userService
      .createUser(this.userForm.value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.user = {...this.user, ...this.userForm.value};
        this.toastService.showSuccess(`User Created successfully`);
        this.router.navigate([`/users/edit/${response.id}`]);
      });
  }

  addUserRole(role: any) {
    this.assignedUserRoles = [
      {resourceType: 'Site', resource: {id: '', site: []}},
      {resourceType: 'Hospice', resource: {id: '', hospice: []}},
    ];
    this.assignedUserRoles = this.assignedUserRoles.map(userRole => {
      userRole.roleId = role.id;
      userRole.roleName = role.name;
      return userRole;
    });
  }

  removeUserRole(index: number) {
    this.assignedUserRoles.splice(index, 1);
  }
}
