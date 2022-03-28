import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {InputMaskModule} from 'primeng/inputmask';
import {MultiSelectModule} from 'primeng/multiselect';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {
  HospiceMemberService,
  HospiceService,
  HospiceLocationService,
  ToastService,
} from 'src/app/services';
import {ConfirmationService, MessageService} from 'primeng/api';
import {DropdownModule} from 'primeng/dropdown';
import {AddEditHospiceMemberComponent} from './add-edit-hospice-member.component';
import {InputSwitchModule} from 'primeng/inputswitch';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {CheckboxModule} from 'primeng/checkbox';
import {BehaviorSubject, of} from 'rxjs';

describe('AddEditHospiceMemberComponent', () => {
  let component: AddEditHospiceMemberComponent;
  let fixture: ComponentFixture<AddEditHospiceMemberComponent>;
  let hospiceService: any;
  let hospiceLocationService: any;
  let hospiceMemberService: any;
  let toastService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', [
        'getHospiceById',
        'getHospiceRoles',
      ]);
      hospiceServiceStub.getHospiceById.and.returnValue(of(hospice));
      hospiceServiceStub.getHospiceRoles.and.returnValue(of(hospiceRoles));

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getHospiceLocations',
      ]);
      hospiceLocationServiceStub.getHospiceLocations.and.returnValue(of(hospiceLocationsResponse));

      const hospiceMemberServiceStub = jasmine.createSpyObj('HospiceMemberService', [
        'resetPassword',
        'createHospiceMember',
        'updateHospiceMember',
        'deleteHospiceMember',
      ]);
      hospiceMemberServiceStub.resetPassword.and.returnValue(new BehaviorSubject(null));
      hospiceMemberServiceStub.createHospiceMember.and.returnValue(new BehaviorSubject(null));
      hospiceMemberServiceStub.updateHospiceMember.and.returnValue(new BehaviorSubject(null));
      hospiceMemberServiceStub.deleteHospiceMember.and.returnValue(new BehaviorSubject(null));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      TestBed.configureTestingModule({
        imports: [
          RouterTestingModule.withRoutes([{path: 'hospice/:hospiceId', redirectTo: ''}]),
          HttpClientTestingModule,
          ReactiveFormsModule,
          InputMaskModule,
          DropdownModule,
          MultiSelectModule,
          ConfirmDialogModule,
          CheckboxModule,
          InputSwitchModule,
          FormsModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          {
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          {
            provide: HospiceMemberService,
            useValue: hospiceMemberServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          MessageService,
          OAuthService,
          ConfirmationService,
        ],
        declarations: [AddEditHospiceMemberComponent],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      hospiceMemberService = TestBed.inject(HospiceMemberService);
      toastService = TestBed.inject(ToastService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditHospiceMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'handleHospiceResponse').and.callThrough();
    spyOn(component, 'handleHospiceLocationResponse').and.callThrough();
    spyOn(component, 'handleHospiceRolesListResponse').and.callThrough();
    spyOn(component, 'setLocationTableData').and.callThrough();
    spyOn(component, 'setPasswordForm').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('memberForm form invalid when empty', () => {
    expect(component.memberForm.valid).toBeFalsy();
  });

  it('memberForm form valid when all required fileds are set', () => {
    component.memberForm.patchValue({
      firstName: 'test_firstName',
      lastName: 'test_lastName',
      email: 'test@test.com',
    });
    expect(component.memberForm.valid).toBeTruthy();
  });

  it('should call services on getPageDetails and match the result', () => {
    component.hospiceId = 1;
    component.getPageDetails();
    expect(hospiceService.getHospiceById).toHaveBeenCalled();
    expect(hospiceLocationService.getHospiceLocations).toHaveBeenCalled();
    expect(hospiceService.getHospiceRoles).toHaveBeenCalled();
    expect(component.handleHospiceResponse).toHaveBeenCalled();
    expect(component.handleHospiceLocationResponse).toHaveBeenCalled();
    expect(component.handleHospiceRolesListResponse).toHaveBeenCalled();
    expect(component.setLocationTableData).toHaveBeenCalled();
  });

  it('should call resetPassword of HospiceMemberService on onSubmitPassword and match the result', () => {
    const submitFormValue = {
      channels: [],
      confirmPassword: 'P@ssw0rd1',
      password: 'P@ssw0rd1',
    };
    component.hospiceId = 1;
    component.memberId = 1;
    component.member = member;
    component.setPasswordForm();
    component.onSubmitPassword(submitFormValue);
    expect(hospiceMemberService.resetPassword).toHaveBeenCalled();
    expect(component.setPasswordForm).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call createHospiceMember of HospiceMemberService on saveMember and match the result', () => {
    const submitFormValue = {
      canAccessWebStore: true,
      countryCode: '+1',
      email: 'test@test.com',
      firstName: 'firstName',
      hospiceLocations: [],
      lastName: 'lastNamee',
      phoneNumber: 1111111111,
      userRoles: [],
    };
    component.hospiceId = 1;
    component.saveMember(submitFormValue);
    expect(hospiceMemberService.createHospiceMember).toHaveBeenCalled();
    expect(component.member).toEqual(submitFormValue);
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call updateHospiceMember of HospiceMemberService on updateMember and match the result', () => {
    const submitFormValue = {
      canAccessWebStore: false,
      countryCode: 1,
      email: 'testing@hospicesource.net',
      firstName: 'Account',
      hospiceLocations: [],
      lastName: 'Holder',
      phoneNumber: 1111111111,
      userRoles: [],
    };
    component.hospiceId = 1;
    component.memberId = member.id;
    component.updateMember(submitFormValue);
    expect(hospiceMemberService.updateHospiceMember).toHaveBeenCalled();
    expect(component.member).toEqual(submitFormValue);
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call deleteHospiceMember of HospiceMemberService on deleteConfirmed and match the result', () => {
    component.hospiceId = 1;
    component.memberId = member.id;
    component.deleteConfirmed();
    expect(hospiceMemberService.deleteHospiceMember).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  const hospice = {
    id: 1,
    name: 'Apple Hospices',
    hospiceLocations: [],
  };

  const hospiceRole = {
    id: 6,
    isStatic: true,
    level: 11,
    name: 'Hospice Admin',
    permissions: [],
    roleType: 'Hospice',
  };

  const hospiceRoles = [hospiceRole];

  const hospiceLocation = {
    address: null,
    hospiceId: 1,
    id: 2,
    name: '1st Quality Hospice, LLC : 1st Quality - Woodville, TX',
    netSuiteCustomerId: 2027,
    phoneNumber: null,
    site: null,
    siteId: null,
  };

  const hospiceLocationsResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const member = {
    canAccessWebStore: false,
    canApproveOrder: false,
    cognitoUserId: '6d080f26-1cf1-49fb-9522-f8c29a757766',
    countryCode: 0,
    designation: 'Admin',
    email: 'testing@hospicesource.net',
    enabled: false,
    firstName: 'Account',
    hospiceId: 1,
    hospiceLocationMembers: [],
    hospiceLocations: [],
    id: 1,
    isEmailVerified: false,
    isPhoneNumberVerified: false,
    lastName: 'Holder',
    name: 'Account Holder',
    netSuiteContactId: 33439,
    phoneNumber: null,
    userId: 5,
    userRoles: [],
    userStatus: null,
  };
});
