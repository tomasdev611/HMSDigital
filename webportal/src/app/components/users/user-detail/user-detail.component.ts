import {Component, OnInit, ViewChild} from '@angular/core';
import {FormGroup, FormBuilder, FormControl, Validators} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {
  UserService,
  ToastService,
  SitesService,
  HospiceService,
  HospiceLocationService,
} from 'src/app/services';
import {finalize, mergeMap} from 'rxjs/operators';
import {CustomValidators} from '../../../directives/custom-validators';
import {Dropdown} from 'primeng/dropdown';
import {buildFilterString, getRoleById} from 'src/app/utils';
import {SieveOperators} from 'src/app/enums';
import {forkJoin, of} from 'rxjs';
import {Location} from '@angular/common';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss'],
})
export class UserDetailComponent implements OnInit {
  userForm: FormGroup;
  passwordForm: FormGroup;
  driverSearchForm: FormGroup;
  submitted: boolean;
  loading = false;
  passwordSubmit = false;
  resetLinkSubmit = false;
  user: any;
  userId: string = this.route.snapshot.paramMap.get('userId');
  roles = [];
  userRoles = [];
  userRolesLoading = false;
  selectedRoles = [];
  selectedSites = [];
  assignedUserRoles = [];
  drivers = [];
  initialDrivers = [];
  formSubmit = false;
  item: string;
  resetLinkChannels = [];
  displayCodeConfirmation = false;
  attributeToVerify = '';
  verificationCode = '';
  verifyReqLoading = false;
  verificationLoading = false;
  driverSearchEnabled = false;
  driverSearchLoading = false;
  view = 'general';
  @ViewChild('userDriver') userDriver: Dropdown;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private userService: UserService,
    private toastService: ToastService,
    private siteService: SitesService,
    private hospiceService: HospiceService,
    private hospiceLocationService: HospiceLocationService,
    private location: Location
  ) {
    this.route.queryParams.subscribe(params => {
      this.view = params.view || this.view || 'general';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.view}).toString()
      );
    });
    this.setUserForm();
    this.setPasswordForm();
  }

  setUserForm() {
    this.userForm = this.fb.group({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.required),
      phoneNumber: new FormControl(0),
      countryCode: new FormControl('+1'),
    });
  }

  setPasswordForm() {
    this.passwordForm = this.fb.group(
      {
        password: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/\d/, {hasNumber: true}),
            CustomValidators.patternValidator(/[A-Z]/, {hasUpperCase: true}),
            CustomValidators.patternValidator(/[a-z]/, {hasLowerCase: true}),
            CustomValidators.patternValidator(/[~!@#$%^&*()_[\]{};':"\\|,.<>\/?]/, {
              hasSpecialCharacters: true,
            }),
            Validators.minLength(8),
          ]),
        ],
        channels: new FormControl([]),
        confirmPassword: [null, Validators.compose([Validators.required])],
      },
      {
        validators: CustomValidators.passwordMatchValidator,
      }
    );
  }

  ngOnInit(): void {
    if (this.view === 'general') {
      this.getUser();
    } else {
      this.getUserRoles();
    }
  }

  getUserRoles() {
    this.userRolesLoading = true;
    this.userService
      .getUserRoles(this.userId)
      .pipe(
        finalize(() => {
          this.userRolesLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.formatUserRoles(response);
      });
  }

  formatUserRoles(response) {
    let siteIds = [];
    let hospiceIds = [];
    let hospcieLocationIds = [];
    this.assignedUserRoles = response.map(x => {
      if (x.resourceId === '*') {
        x.resource = {id: '*', name: 'All'};
      } else if (x.resourceType === 'Site') {
        siteIds = [...siteIds, x.resourceId];
      } else if (x.resourceType === 'Hospice') {
        hospiceIds = [...hospiceIds, x.resourceId];
      } else if (x.resourceType === 'HospiceLocation') {
        hospcieLocationIds = [...hospcieLocationIds, x.resourceId];
      }
      if (!x.roleName) {
        const role = getRoleById(x.roleId);
        x.roleName = role.name;
      }
      return x;
    });
    const requests = [];
    if (siteIds.length > 0) {
      requests.push(
        this.siteService.getAllSites().pipe(
          mergeMap((sites: any) => {
            sites.records = sites.records.filter(site => siteIds.includes(site.id));
            sites.totalRecordCount = sites.records.length;
            return of(sites);
          })
        )
      );
    }
    if (hospiceIds.length > 0) {
      const filterValues = [
        {
          field: 'id',
          operator: SieveOperators.Equals,
          value: [...hospiceIds],
        },
      ];
      const hospiceReq = {filters: buildFilterString(filterValues)};
      requests.push(this.hospiceService.getAllhospices(hospiceReq));
    }
    if (hospcieLocationIds.length > 0) {
      const filterValues = [
        {
          field: 'id',
          operator: SieveOperators.Equals,
          value: [...hospcieLocationIds],
        },
      ];
      const locationReq = {filters: buildFilterString(filterValues)};
      requests.push(this.hospiceLocationService.getHospiceLocations(null, locationReq));
    }
    forkJoin(requests).subscribe((res: any) => {
      res.map(x => {
        x.records.map(resource => {
          this.assignedUserRoles.map(y => {
            if (
              (y.resourceType === 'Site' ||
                (resource.hospiceLocations && y.resourceType === 'Hospice') ||
                y.resourceType === 'HospiceLocation') &&
              Number(y.resourceId) === resource.id
            ) {
              y.resource = {id: y.id, name: resource.name};
            }
            return y;
          });
        });
      });
    });
  }

  onSubmitUser(value: any) {
    this.userForm.controls.phoneNumber.setValue(parseInt(value.phoneNumber, 10) ?? 0);
    this.userForm.controls.countryCode.setValue(parseInt(value.countryCode, 10) ?? 0);
    this.formSubmit = true;
    this.userService
      .updateUser(this.userId, this.userForm.value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.user = {...this.user, ...this.userForm.value};
        this.toastService.showSuccess(`User updated successfully`);
      });
  }

  getUser() {
    this.userService
      .getUserById(this.userId)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: any) => {
        this.user = response;
        this.user.phoneNumber = this.user.phoneNumber || 0;
        this.userForm.patchValue(this.user);
        this.userForm.controls.countryCode.setValue(
          this.user.phoneNumber ? `+${this.user.countryCode}` : 0
        );
      });
  }

  toggleUserStatus(event) {
    this.formSubmit = true;
    (event.checked
      ? this.userService.enableUser(this.userId)
      : this.userService.disableUser(this.userId)
    )
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe(
        (response: any) => {
          this.toastService.showSuccess(
            `${this.user.firstName ? this.user.firstName : this.user.email} ${
              event.checked ? 'enabled' : 'disabled'
            } successfully`
          );
        },
        error => {
          this.user.enabled = !event.checked;
          throw error;
        }
      );
  }

  onSubmitPassword(value: any) {
    const body = {
      password: value.password,
      permanent: true,
      email: this.user.email ?? '',
      phoneNumber: this.user.phoneNumber ?? 0,
      countryCode: this.user.countryCode ?? 0,
      channels: value.channels,
    };
    this.passwordSubmit = true;
    this.userService
      .resetPassword(this.userId, body)
      .pipe(
        finalize(() => {
          this.passwordSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.passwordForm.reset();
        this.setPasswordForm();
        this.toastService.showSuccess(`Password changed successfully`);
      });
  }

  onSendResetLink() {
    const body = {
      email: this.user.email ?? '',
      phoneNumber: this.user.phoneNumber ?? 0,
      countryCode: this.user.countryCode ?? 0,
      channels: this.resetLinkChannels,
    };
    if (body.channels.includes('Email') && body.email === '') {
      this.toastService.showError(`Email should be configured first`);
      return;
    }
    if (body.channels.includes('Sms') && body.phoneNumber === 0) {
      this.toastService.showError(`Mobile Number should be configured first`);
      return;
    }
    this.resetLinkSubmit = true;
    this.userService
      .sendResetPasswordLink(this.userId, body)
      .pipe(
        finalize(() => {
          this.resetLinkSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.resetLinkChannels = [];
        this.toastService.showSuccess(`Reset Link send successfully`);
      });
  }

  addUserRole(userRole: any) {
    userRole.roleId = userRole.role.id;
    userRole.resourceId = userRole.resource.id + '';
    this.userRolesLoading = true;
    this.userService
      .addUserRole(this.userId, userRole)
      .pipe(
        finalize(() => {
          this.userRolesLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.toastService.showSuccess(`Added Resource permission successfully`);
        this.formatUserRoles(response);
      });
  }

  removeUserRole(index: number) {
    const userRole = this.assignedUserRoles[index];
    this.userRolesLoading = true;
    this.userService
      .removeUserRole(this.userId, userRole.id)
      .pipe(
        finalize(() => {
          this.userRolesLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.toastService.showSuccess(`Removed Resource permission successfully`);
        this.formatUserRoles(response);
      });
  }

  verify(attribute) {
    this.verifyReqLoading = true;
    this.attributeToVerify = attribute;
    const body = {
      verifyAttribute: this.attributeToVerify,
    };
    this.userService
      .sendVerificationCode(this.userId, body)
      .pipe(
        finalize(() => {
          this.verifyReqLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.displayCodeConfirmation = true;
        this.toastService.showSuccess(
          `Verification Code sent to ${
            this.attributeToVerify === 'email_verify' ? 'Email' : 'Mobile'
          }`
        );
      });
  }

  verifyConfirmation() {
    this.verificationLoading = true;
    const body = {
      code: this.verificationCode,
      verifyAttribute: this.attributeToVerify,
    };
    this.userService
      .verifyCode(this.userId, body)
      .pipe(
        finalize(() => {
          this.verificationLoading = false;
        })
      )
      .subscribe((response: any) => {
        if (this.attributeToVerify === 'email_verify') {
          this.user.isEmailVerified = true;
        } else {
          this.user.isPhoneNumberVerified = true;
        }
        this.toastService.showSuccess(
          `${this.attributeToVerify === 'email_verify' ? 'Email' : 'Mobile'} verified successfully`
        );
        this.closeVerificationDialog();
      });
  }

  closeVerificationDialog(event?) {
    this.displayCodeConfirmation = false;
  }

  onTabChange({index}) {
    switch (index) {
      case 0:
        this.view = 'general';
        if (!this.user?.userId) {
          this.getUser();
        }
        break;
      case 1:
        this.view = 'accessControl';
        if (this.assignedUserRoles.length === 0) {
          this.getUserRoles();
        }
        break;
    }
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.view}).toString()
    );
  }
}
