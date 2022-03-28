import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {InputMaskModule} from 'primeng/inputmask';
import {UserService, ToastService} from 'src/app/services';
import {ProfileComponent} from './profile.component';
import {BehaviorSubject} from 'rxjs';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let userService: any;
  let toastService: any;

  beforeEach(
    waitForAsync(() => {
      const userServiceStub = jasmine.createSpyObj('UserService', [
        'getProfileUrl',
        'updateSelfUser',
        'sendVerificationCode',
        'changeSelfPassword',
        'getUploadUrl',
        'deleteProfileImage',
      ]);
      userServiceStub.getProfileUrl.and.returnValue(new BehaviorSubject(profileUrlResponse));
      userServiceStub.updateSelfUser.and.returnValue(new BehaviorSubject(user));
      userServiceStub.sendVerificationCode.and.returnValue(new BehaviorSubject(null));
      userServiceStub.changeSelfPassword.and.returnValue(new BehaviorSubject(null));
      userServiceStub.getUploadUrl.and.returnValue(new BehaviorSubject(null));
      userServiceStub.deleteProfileImage.and.returnValue(new BehaviorSubject(null));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showSuccess']);

      toastServiceStub.showSuccess.and.callThrough();

      TestBed.configureTestingModule({
        declarations: [ProfileComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          OAuthModule.forRoot(),
          InputMaskModule,
        ],
        providers: [
          {
            provide: UserService,
            useValue: userServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          OAuthService,
          MessageService,
        ],
      }).compileComponents();
      userService = TestBed.inject(UserService);
      toastService = TestBed.inject(ToastService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    component.user = user;
    fixture.detectChanges();
    spyOn(component, 'removeFile').and.callThrough();
    spyOn(component, 'setPasswordForm').and.callThrough();
    spyOn(component, 'updateUserCache').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('userForm invalid when empty', () => {
    component.setUserForm();
    expect(component.userForm.valid).toBeFalsy();
  });

  it('userForm valid when all required fileds are set', () => {
    component.userForm.controls.firstName.setValue('first_name');
    component.userForm.controls.lastName.setValue('last_name');
    component.userForm.controls.email.setValue('a@a.com');

    expect(component.userForm.valid).toBeTruthy();
  });

  it('passwordForm invalid when empty', () => {
    component.setPasswordForm();
    expect(component.passwordForm.valid).toBeFalsy();
  });

  it('passwordForm invalid when validate is failed', () => {
    component.passwordForm.controls.oldPassword.setValue('old_password');
    component.passwordForm.controls.password.setValue('password');

    expect(component.passwordForm.valid).toBeFalsy();
  });

  it('passwordForm valid when all required fileds are set', () => {
    component.passwordForm.controls.oldPassword.setValue('old_password');
    component.passwordForm.controls.password.setValue('P@ssw0rd123');
    component.passwordForm.controls.confirmPassword.setValue('P@ssw0rd123');

    expect(component.passwordForm.valid).toBeTruthy();
  });

  it('should call `getProfileUrl` of UserService on getProfileImage and match the result', () => {
    component.user = user;
    component.getProfileImage();
    expect(userService.getProfileUrl).toHaveBeenCalled();
    expect(component.userImageUrl).toEqual(profileUrlResponse.downloadUrl);
    expect(component.removeFile).toHaveBeenCalled();
  });

  it('should call `updateSelfUser` of UserService on getProfileImage and match the result', () => {
    const value = {
      countryCode: 0,
      email: 'a@a.com',
      firstName: 'first_name',
      lastName: 'last_name',
      phoneNumber: 0,
    };
    component.userForm.patchValue(value);
    component.onSubmit(value);
    expect(userService.updateSelfUser).toHaveBeenCalled();
    expect(component.user).toEqual(user);
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call `sendVerificationCode` of UserService on verify and match the result', () => {
    component.user = user;
    component.verify('email_verify');
    expect(userService.sendVerificationCode).toHaveBeenCalled();
    expect(component.displayCodeConfirmation).toEqual(true);
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call `changeSelfPassword` of UserService on onSubmitPassword and match the result', () => {
    component.onSubmitPassword({
      oldPassword: 'P@ssw0rd123',
      newPassword: 'P@ssw0rd123',
    });
    component.verify('email_verify');
    expect(userService.changeSelfPassword).toHaveBeenCalled();
    expect(component.setPasswordForm).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call `deleteProfileImage` of UserService on deleteProfileImage and match the result', () => {
    component.user = user;
    component.deleteProfileImage();
    expect(userService.deleteProfileImage).toHaveBeenCalled();
    expect(component.updateUserCache).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  const user = {
    countryCode: 0,
    email: 'a@a.com',
    enabled: true,
    firstName: 'a',
    id: 218,
    isEmailVerified: true,
    isPhoneNumberVerified: false,
    lastName: 'a',
    name: 'a a',
    phoneNumber: 0,
    profilePictureUrl: '',
    userId: 'ddca8d85-3b0e-4ee5-a272-28700457948e',
    userRoles: [],
    userStatus: null,
  };

  const profileUrlResponse = {
    contentType: 'jpeg',
    description: null,
    downloadUrl:
      'https://hmsdstoragedev.blob.core.windows.net/user-picture/face.jpg_8cf25a32ac0c49948c3d43ce764a20d2.jpeg?sv=2019-07-07&sr=b&sig=SkVYc69pQTiVv9%2BpihgNPNwdYli1GPs6Q3HicyQntOI%3D&se=2021-05-03T22%3A09%3A03Z&sp=r',
    id: 13,
    isPublic: false,
    isUploaded: true,
    name: 'face.jpg',
    sizeInBytes: 111624,
    storageFilePath: 'face.jpg_8cf25a32ac0c49948c3d43ce764a20d2.jpeg',
    storageRoot: "user'picture",
    storageTypeId: 1,
    uploadUrl: null,
  };
});
