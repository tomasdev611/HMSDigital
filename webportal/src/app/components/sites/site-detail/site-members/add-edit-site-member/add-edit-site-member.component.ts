import {Component, OnInit} from '@angular/core';
import {SitesService, SiteMembersService, ToastService, UserService} from 'src/app/services';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {Location} from '@angular/common';

@Component({
  selector: 'app-add-edit-site-member',
  templateUrl: './add-edit-site-member.component.html',
  styleUrls: ['./add-edit-site-member.component.scss'],
})
export class AddEditSiteMemberComponent implements OnInit {
  siteId: number;
  memberForm: FormGroup;
  submitted: boolean;
  loading = false;
  member: any;
  selectedRoles = [];
  siteRoles = [];
  formSubmit = false;
  memberId: number;
  editmode: boolean;
  searchedUsers = [];

  constructor(
    private fb: FormBuilder,
    private siteService: SitesService,
    private siteMemberService: SiteMembersService,
    private toastService: ToastService,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private location: Location
  ) {
    const {url, paramMap} = this.route.snapshot;

    this.memberId = Number(paramMap.get('memberId'));
    this.siteId = Number(paramMap.get('siteId'));

    const urlLength = url && url.length;
    this.editmode = url[urlLength - 2]?.path === 'edit';
  }

  ngOnInit(): void {
    if (this.editmode) {
      this.getUser();
    }
    this.getSiteRoles();
    this.setMemberForm();
  }

  setMemberForm() {
    this.memberForm = this.fb.group({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      designation: new FormControl(null),
      user: new FormControl(null, Validators.required),
      email: new FormControl(null),
      phoneNumber: new FormControl(null),
      countryCode: new FormControl('+1'),
      roleIds: new FormControl(null),
    });
  }

  getSiteRoles() {
    this.siteService.getSiteRoles(this.siteId).subscribe((response: any) => {
      this.siteRoles = response.map(r => {
        const obj = {
          value: r.id,
          label: r.name,
        };
        return obj;
      });
    });
  }

  getUser() {
    this.siteMemberService
      .getSiteMemberById(this.siteId, this.memberId)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: any) => {
        this.member = response;
        this.member.phoneNumber = this.member.phoneNumber ?? null;
        this.memberForm.patchValue({
          ...response,
          user: {
            email: response.email,
            firstName: response.firstName,
            lastName: response.lastName,
            phoneNumber: response.phoneNumber,
          },
        });
        this.memberForm.controls.countryCode.setValue(
          this.member.phoneNumber ? `+${this.member.countryCode}` : 0
        );
      });
  }

  onSubmitMember(value: any) {
    value.email = value?.user?.email || value.user || value.email;
    delete value.user;
    if (this.editmode) {
      this.updateMember(value);
      return;
    }
    this.saveMember(value);
  }

  saveMember(value: any) {
    value = this.formatValue(value);
    this.formSubmit = true;
    this.siteMemberService
      .createSiteMember(this.siteId, value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.member = {...this.member, value};
        this.toastService.showSuccess(`Member Created successfully`);
        this.location.back();
      });
  }

  updateMember(value: any) {
    value = this.formatValue(value);
    this.formSubmit = true;
    this.siteMemberService
      .updateSiteMember(this.siteId, this.memberId, value)
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

  formatValue(value) {
    const {controls} = this.memberForm;
    controls.phoneNumber.setValue(value.phoneNumber ? parseInt(value.phoneNumber, 10) : 0);
    if (this.editmode) {
      controls.countryCode.setValue(parseInt(value.countryCode, 10) ?? 0);
    } else {
      value.email = value.email || value?.user?.email || value.user;
      value.phoneNumber = controls.phoneNumber.value;
      controls.roleIds.setValue(value.roleIds ? value.roleIds : []);
    }
    delete value.user;
    return value;
  }

  searchUsers({query}) {
    if (this.editmode) {
      this.searchedUsers = [];
      return;
    }
    this.userService.searchUser({searchQuery: query}).subscribe((res: any) => {
      this.searchedUsers = res.records.map((usr: any) => {
        return {
          email: usr.email,
          firstName: usr.firstName,
          lastName: usr.lastName,
          phoneNumber: usr.phoneNumber,
        };
      });
    });
  }

  setUserFields(event) {
    this.memberForm.patchValue(event);
  }

  clearEmail() {
    this.memberForm.get('email').reset();
    this.memberForm.get('user').reset();
  }
}
