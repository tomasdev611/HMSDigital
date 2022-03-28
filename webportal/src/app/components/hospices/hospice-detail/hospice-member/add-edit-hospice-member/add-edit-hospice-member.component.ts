import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {
  HospiceService,
  ToastService,
  HospiceMemberService,
  HospiceLocationService,
} from 'src/app/services';
import {ConfirmDialog} from 'src/app/models';
import {forkJoin} from 'rxjs';
import {CustomValidators} from 'src/app/directives/custom-validators';
import {Location} from '@angular/common';

@Component({
  selector: 'app-add-edit-hospice-member',
  templateUrl: './add-edit-hospice-member.component.html',
  styleUrls: ['./add-edit-hospice-member.component.scss'],
})
export class AddEditHospiceMemberComponent implements OnInit {
  hospiceId: number;
  memberId: number;
  memberForm: FormGroup;
  passwordForm: FormGroup;
  passwordSubmit = false;
  resetLinkSubmit = false;
  resetLinkChannels = [];
  submitted: boolean;
  loading = false;
  member: any;
  hospiceRoles = [];
  formSubmit = false;
  editmode: boolean;
  locations = [];
  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  deleteData = new ConfirmDialog();
  tableData: any[];
  hospiceDetails: any;
  locationColumns = [
    {
      header: 'Location',
      key: 'name',
    },
    {
      header: 'Role',
      key: 'roleId',
    },
  ];

  backUrlParams = {view: 'members'};
  locationRoleData = [];
  hospiceRole = {role: null, selectedValue: 0};
  scaPath: any;
  scaHash: any;
  orderType: any;
  patientId: any;
  orderAction: any;
  netSuiteOrderId: any;
  options = {
    floor: 0,
    ceil: 2,
    showTicksValues: true,
    translate: (value: number): string => {
      return value === 0 ? 'None' : value === 1 ? 'Standard' : 'Admin';
    },
    getPointerColor: (value: number): string => {
      return '#007ad9';
    },
  };
  constructor(
    private fb: FormBuilder,
    private hospiceService: HospiceService,
    private hospiceMemberService: HospiceMemberService,
    private toastService: ToastService,
    private router: Router,
    private route: ActivatedRoute,
    private hospiceLocationService: HospiceLocationService,
    private location: Location
  ) {
    const {url} = this.route.snapshot;
    this.route.params.subscribe((params: any) => {
      this.hospiceId = Number(params.hospiceId);
      this.memberId = Number(params.memberId);
    });

    const urlLength = url && url.length;
    this.editmode = url[urlLength - 2]?.path === 'edit';

    this.setMemberForm();
    if (this.editmode) {
      this.setPasswordForm();
    }
  }

  ngOnInit(): void {
    this.getPageDetails();
  }

  setMemberForm() {
    this.memberForm = this.fb.group({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.required),
      phoneNumber: new FormControl(null),
      countryCode: new FormControl('+1'),
      userRoles: new FormControl([]),
      hospiceLocations: new FormControl([]),
      canAccessWebStore: new FormControl(true),
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

  onSendResetLink() {
    const body = {
      email: this.member.email ?? '',
      phoneNumber: this.member.phoneNumber ?? 0,
      countryCode: this.member.countryCode ?? 0,
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
    this.hospiceMemberService
      .sendResetPasswordLink(this.hospiceId, this.memberId, body)
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

  onSubmitPassword(value: any) {
    const body = {
      password: value.password,
      permanent: true,
      email: this.member.email ?? '',
      phoneNumber: this.member.phoneNumber ?? 0,
      countryCode: this.member.countryCode ?? 0,
      channels: value.channels,
    };
    this.passwordSubmit = true;
    this.hospiceMemberService
      .resetPassword(this.hospiceId, this.memberId, body)
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

  getPageDetails() {
    const requests = [
      this.hospiceService.getHospiceById(this.hospiceId),
      this.hospiceLocationService.getHospiceLocations(this.hospiceId),
      this.hospiceService.getHospiceRoles(this.hospiceId),
    ];
    if (this.editmode) {
      requests.push(this.hospiceMemberService.getHospiceMemberById(this.hospiceId, this.memberId));
    }

    forkJoin(requests).subscribe(res => {
      const [hospiceDetails, locationDetails, roles, memberDetails] = res;
      this.handleHospiceResponse(hospiceDetails);
      this.handleHospiceLocationResponse(locationDetails);
      this.handleHospiceRolesListResponse(roles);
      if (this.editmode && memberDetails) {
        this.handleHospiceMemberResponse(memberDetails);
      }
      this.setLocationTableData();
    });
  }

  handleHospiceResponse(res) {
    this.hospiceDetails = res;
  }

  setLocationTableData() {
    this.locationRoleData = this.locations.map((hl: any) => {
      const assigned = this.member?.hospiceLocations.find(x => x.id === hl.id);
      let roleAssigned = {
        role: null,
        selectedValue: 0,
      };
      if (assigned) {
        const hospiceRole = this.member.userRoles.find(
          hr => hr.resourceType === 'HospiceLocation' && hl.id === Number(hr.resourceId)
        );
        if (hospiceRole) {
          roleAssigned.role = this.hospiceRoles.find(
            x => x?.value?.id === hospiceRole.roleId
          )?.value;
          if (roleAssigned.role) {
            roleAssigned.selectedValue = roleAssigned.role.name.includes('Standard') ? 1 : 2;
          }
        }
        roleAssigned = roleAssigned?.role ? roleAssigned : {role: null, selectedValue: 0};
      }
      const locationMember = this.member?.hospiceLocationMembers?.find(
        (hm: any) => hm.hospiceLocationId === hl.id
      );
      return {
        id: hl.id,
        location: hl,
        assigned,
        name: hl.name,
        role: roleAssigned?.role || null,
        selectedValue: roleAssigned?.selectedValue || null,
        canApprove: locationMember ? locationMember?.canApproveOrder : false,
      };
    });
  }

  handleHospiceRolesListResponse(response) {
    this.hospiceRoles = response
      .map(r => {
        const obj = {
          selectedValue: r.name.includes('Standard') ? 1 : 2,
          value: r,
          label: r.name.includes('Standard')
            ? 'Standard'
            : r.name.includes('Admin')
            ? 'Admin'
            : r.name,
        };
        return obj;
      })
      .reverse();
    this.hospiceRoles.splice(0, 0, {
      selectedValue: 0,
      value: null,
      label: 'None',
    });
  }

  handleHospiceMemberResponse(response) {
    this.member = response;
    this.member.phoneNumber = this.member.phoneNumber ? this.member.phoneNumber : null;
    this.memberForm.patchValue(this.member);
    this.memberForm.controls.countryCode.setValue(
      this.member.phoneNumber ? `+${this.member.countryCode}` : 1
    );
    const hospiceRole = this.member.userRoles.find(hr => hr.resourceType === 'Hospice');
    if (hospiceRole) {
      this.hospiceRole.role = this.hospiceRoles.find(
        x => x?.value?.id === hospiceRole.roleId
      )?.value;
      if (this.hospiceRole.role) {
        this.hospiceRole.selectedValue = this.hospiceRole.role.name.includes('Standard') ? 1 : 2;
      }
    }
    this.hospiceRole = this.hospiceRole || {role: null, selectedValue: 0};
  }

  onSubmitMember(value: any) {
    const body = this.formatValues(value);
    if (body) {
      if (this.editmode) {
        this.updateMember(body);
        return;
      }
      this.saveMember(body);
    }
  }

  saveMember(value: any) {
    this.formSubmit = true;
    this.hospiceMemberService
      .createHospiceMember(this.hospiceId, value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe(() => {
        this.member = {...this.member, ...value};
        this.toastService.showSuccess(`Member Created successfully`);
        this.location.back();
      });
  }

  updateMember(value: any) {
    this.formSubmit = true;
    this.hospiceMemberService
      .updateHospiceMember(this.hospiceId, this.memberId, value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.member = {...this.member, ...value};
        this.toastService.showSuccess(`Member updated successfully`);
      });
  }

  handleHospiceLocationResponse(res) {
    this.locations = res.records;
  }

  formatValues(value) {
    const {controls} = this.memberForm;
    if (this.memberId) {
      controls.countryCode.setValue(value.countryCode ? value.countryCode : '+1');
      value.countryCode = controls.countryCode.value;
    }

    controls.phoneNumber.setValue(value.phoneNumber ? parseInt(value.phoneNumber, 10) : 0);
    value.phoneNumber = controls.phoneNumber.value;
    value.userRoles = value.userRoles.map((ur: any) => {
      return {
        roleId: ur?.role?.id ? ur.role.id : ur?.roleId ? ur.roleId : null,
        resourceType: ur.resourceType,
        resourceId: ur.resourceId?.toString(),
      };
    });
    return value;
  }

  deleteMember() {
    this.deleteData.message = `Do you want to delete Member ${this.member.name || ''}?`;
    this.confirmDialog.showDialog(this.deleteData);
  }

  deleteConfirmed() {
    this.hospiceMemberService.deleteHospiceMember(this.hospiceId, this.memberId).subscribe(() => {
      this.toastService.showSuccess(`Member Deleted successfully`);
      this.location.back();
    });
  }

  assignRole(event, resourceType, resourceId) {
    const selectedValue = event.value === 0 ? 'None' : event.value === 1 ? 'Standard' : 'Admin'; // selected value from slider
    const value = this.hospiceRoles.find(x => x.label === selectedValue).value; // value via hospiceRoles compared to selected slider value
    let rolesAssigned = this.memberForm.get('userRoles').value;
    if (resourceType === 'Hospice' && !value) {
      rolesAssigned = rolesAssigned.filter(x => x.resourceType !== 'Hospice');
      this.hospiceRole.role = null;
      this.hospiceRole.selectedValue = 0;
    } else {
      const roleIndex = rolesAssigned.findIndex(
        ra => ra.resourceType === resourceType && Number(ra.resourceId) === resourceId
      );
      if (roleIndex >= 0) {
        if (value) {
          rolesAssigned[roleIndex].role = value;
          rolesAssigned[roleIndex].selectedValue = event.value;
        } else {
          rolesAssigned.splice(roleIndex, 1);
        }
      } else if (value) {
        rolesAssigned.push({
          resourceType,
          resourceId,
          role: value,
          selectedValue: event.value,
        });
      }
      if (resourceType === 'Hospice' && !this.hospiceRole.role) {
        this.hospiceRole.role = value;
        this.hospiceRole.selectedValue = event.value;
      }
    }
    this.memberForm.patchValue({userRoles: rolesAssigned});
    this.memberForm.markAsDirty();
  }
}
