import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {UserDetailComponent} from './user-detail.component';

import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {UserService} from 'src/app/services';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {MessageService} from 'primeng/api';
import {InputMaskModule} from 'primeng/inputmask';
import {DropdownModule} from 'primeng/dropdown';
import {MultiSelectModule} from 'primeng/multiselect';
import {InputSwitchModule} from 'primeng/inputswitch';
import {CheckboxModule} from 'primeng/checkbox';
import {TransferState} from '@angular/platform-browser';
import {BehaviorSubject} from 'rxjs';

describe('UserDetailComponent', () => {
  let component: UserDetailComponent;
  let fixture: ComponentFixture<UserDetailComponent>;
  let userService: any;

  beforeEach(
    waitForAsync(() => {
      const userServiceStub = jasmine.createSpyObj('UserService', [
        'getUserRoles',
        'getUserById',
        'updateUser',
        'enableUser',
        'disableUser',
        'resetPassword',
        'addUserRole',
        'removeUserRole',
        'sendVerificationCode',
      ]);
      userServiceStub.getUserRoles.and.returnValue(new BehaviorSubject(userRoleResponse));
      userServiceStub.getUserById.and.returnValue(new BehaviorSubject(user));
      userServiceStub.updateUser.and.returnValue(new BehaviorSubject(user));
      userServiceStub.enableUser.and.returnValue(new BehaviorSubject(user));
      userServiceStub.disableUser.and.returnValue(new BehaviorSubject(user));
      userServiceStub.resetPassword.and.returnValue(new BehaviorSubject(user));
      userServiceStub.addUserRole.and.returnValue(new BehaviorSubject(userRoleResponse));
      userServiceStub.removeUserRole.and.returnValue(new BehaviorSubject(userRoleResponse));
      userServiceStub.sendVerificationCode.and.returnValue(new BehaviorSubject(null));

      TestBed.configureTestingModule({
        declarations: [UserDetailComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          FormsModule,
          InputMaskModule,
          DropdownModule,
          MultiSelectModule,
          InputSwitchModule,
          CheckboxModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: UserService,
            useValue: userServiceStub,
          },
          OAuthService,
          MessageService,
          TransferState,
        ],
      }).compileComponents();
      userService = TestBed.inject(UserService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(UserDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'formatUserRoles').and.callThrough();
    spyOn(component, 'setPasswordForm').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('password form invalid when empty', () => {
    expect(component.passwordForm.valid).toBeFalsy();
  });

  it('password field validity', () => {
    const password = component.passwordForm.controls.password;
    expect(password.valid).toBeFalsy();
  });

  it('password field validity', () => {
    let errors: any = {};
    const password = component.passwordForm.controls.password;

    // Password field is required
    errors = password.errors || {};
    expect(errors.required).toBeTruthy();

    // Set password to something incorrect
    password.setValue('123456');
    errors = password.errors || {};
    expect(errors.required).toBeFalsy();
    expect(errors.minlength).toBeTruthy();
    expect(errors.hasNumber).toBeFalsy();
    expect(errors.hasUpperCase).toBeTruthy();
    expect(errors.hasSpecialCharacters).toBeTruthy();

    // Set password to something correct
    password.setValue('Aa1!1234');
    errors = password.errors || {};
    expect(errors.required).toBeFalsy();
    expect(errors.minlength).toBeFalsy();
    expect(errors.hasNumber).toBeFalsy();
    expect(errors.hasUpperCase).toBeFalsy();
    expect(errors.hasSpecialCharacters).toBeFalsy();
  });

  it('password form invalid when password and confirmPassword do not match', () => {
    expect(component.passwordForm.valid).toBeFalsy();
    // Set password and confirmPassword to something correct but different
    component.passwordForm.controls.password.setValue('Aa1!1234');
    component.passwordForm.controls.confirmPassword.setValue('1234Aa1!');
    expect(component.passwordForm.valid).toBeFalsy();
  });

  it('password form valid when valid values', () => {
    expect(component.passwordForm.valid).toBeFalsy();
    // Set password and confirmPassword to something correct and same
    component.passwordForm.controls.password.setValue('Aa1!1234');
    component.passwordForm.controls.confirmPassword.setValue('Aa1!1234');
    expect(component.passwordForm.valid).toBeTruthy();
  });

  it('user form invalid when empty', () => {
    component.userForm.patchValue({
      countryCode: '+1',
      email: '',
      firstName: '',
      lastName: '',
      phoneNumber: 0,
    });
    expect(component.userForm.valid).toBeFalsy();
  });

  it('user form valid when all required fields are set', () => {
    component.userForm.patchValue({
      countryCode: '+1',
      email: 'tloomis@hospicesource.net',
      firstName: 'Todd',
      lastName: 'Loomis',
      phoneNumber: 4439530628,
    });
    expect(component.userForm.valid).toBeTruthy();
  });

  it('should call `getUserRoles` of UserService on getUserRoles match the result', () => {
    component.userId = '2';
    component.getUserRoles();
    expect(userService.getUserRoles).toHaveBeenCalled();
    expect(component.formatUserRoles).toHaveBeenCalled();
  });

  it('should call `getUserById` of UserService on getUser match the result', () => {
    component.userId = '2';
    component.getUser();
    expect(userService.getUserById).toHaveBeenCalled();
    expect(component.user).toEqual(user);
  });

  it('should call `updateUser` of UserService on onSubmitUser', () => {
    component.userId = '2';
    component.userForm.patchValue({
      countryCode: '+1',
      email: 'tloomis@hospicesource.net',
      firstName: 'Todd',
      lastName: 'Loomis',
      phoneNumber: 4439530628,
    });
    const value = {
      countryCode: '+1',
      email: 'tloomis@hospicesource.net',
      firstName: 'Todd',
      lastName: 'Loomis',
      phoneNumber: 4439530628,
    };
    component.onSubmitUser(value);
    expect(userService.updateUser).toHaveBeenCalled();
  });

  it('should call `enableUser` of UserService on toggleUserStatus', () => {
    component.userId = '2';
    component.toggleUserStatus({
      checked: true,
    });
    expect(userService.enableUser).toHaveBeenCalled();
  });

  it('should call `disableUser` of UserService on toggleUserStatus', () => {
    component.userId = '2';
    component.toggleUserStatus({
      checked: false,
    });
    expect(userService.disableUser).toHaveBeenCalled();
  });

  it('should call `resetPassword` of UserService on onSubmitPassword', () => {
    component.userId = '2';
    component.user = user;
    const value = {
      password: 'Passwored1!',
      channels: ['Email'],
      confirmPassword: 'Passwored1!',
    };
    component.onSubmitPassword(value);
    expect(userService.resetPassword).toHaveBeenCalled();
    expect(component.setPasswordForm).toHaveBeenCalled();
  });

  it('should call `addUserRole` of UserService on addUserRole', () => {
    component.userId = '2';
    const role = {
      resource: {
        id: '*',
        name: 'All',
      },
      resourceType: 'Site',
      role: {
        id: 2,
        isStatic: true,
        level: 2,
        name: 'User Admin',
        permissions: ['User:Create'],
        roleType: 'Internal',
      },
    };
    component.addUserRole(role);
    expect(userService.addUserRole).toHaveBeenCalled();
    expect(component.formatUserRoles).toHaveBeenCalled();
  });

  it('should call `removeUserRole` of UserService on removeUserRole', () => {
    component.userId = '2';
    component.assignedUserRoles = [
      {
        id: 626,
        resource: {
          id: '*',
          name: 'All',
        },
        resourceId: '*',
        resourceType: 'Site',
        roleId: 2,
        roleLevel: 0,
        roleName: null,
      },
    ];
    component.removeUserRole(0);
    expect(userService.removeUserRole).toHaveBeenCalled();
    expect(component.formatUserRoles).toHaveBeenCalled();
  });

  it('should call `sendVerificationCode` of UserService on verify', () => {
    component.userId = '2';
    component.verify('verify');
    expect(userService.sendVerificationCode).toHaveBeenCalled();
    expect(component.displayCodeConfirmation).toEqual(true);
  });

  const userRole = {
    id: 197,
    resourceId: '*',
    resourceType: 'Site',
    roleId: 1,
    roleLevel: 1,
    roleName: 'Master Admin',
  };

  const user = {
    countryCode: 1,
    email: 'tloomis@hospicesource.net',
    enabled: true,
    firstName: 'Todd',
    id: 2,
    isEmailVerified: true,
    isPhoneNumberVerified: false,
    lastName: 'Loomis',
    name: 'Todd Loomis',
    phoneNumber: 4439530628,
    userId: '1469f2e5-6ac3-447b-b22b-b4aac55248c5',
    userRoles: [userRole],
    userStatus: null,
  };

  const userRoleResponse = [userRole];
});
