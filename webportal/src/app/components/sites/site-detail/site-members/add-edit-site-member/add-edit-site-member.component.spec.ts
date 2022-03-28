import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {AddEditSiteMemberComponent} from './add-edit-site-member.component';
import {MessageService} from 'primeng/api';
import {SiteMembersService} from '../../../../../services/site-members.service';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MultiSelectModule} from 'primeng/multiselect';
import {DropdownModule} from 'primeng/dropdown';
import {InputMaskModule} from 'primeng/inputmask';
import {ReactiveFormsModule} from '@angular/forms';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {TransferState} from '@angular/platform-browser';
import {SitesService, UserService} from 'src/app/services';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse} from 'src/app/models';

describe('AddEditSiteMemberComponent', () => {
  let component: AddEditSiteMemberComponent;
  let fixture: ComponentFixture<AddEditSiteMemberComponent>;
  let siteService: any;
  let siteMemberService: any;
  let userService: any;

  beforeEach(
    waitForAsync(() => {
      const siteServiceStub = jasmine.createSpyObj('SitesService', ['getSiteRoles']);
      siteServiceStub.getSiteRoles.and.returnValue(new BehaviorSubject(siteRolesResponse));
      const siteMemberServiceStub = jasmine.createSpyObj('SiteMembersService', [
        'getSiteMemberById',
        'createSiteMember',
        'updateSiteMember',
      ]);
      siteMemberServiceStub.getSiteMemberById.and.returnValue(new BehaviorSubject(siteMember));
      siteMemberServiceStub.createSiteMember.and.returnValue(new BehaviorSubject(siteMember));
      siteMemberServiceStub.updateSiteMember.and.returnValue(new BehaviorSubject(siteMember));
      const userServiceStub = jasmine.createSpyObj('UserService', ['searchUser']);
      userServiceStub.searchUser.and.returnValue(
        new BehaviorSubject<PaginationResponse>(userResponse)
      );
      TestBed.configureTestingModule({
        imports: [
          RouterTestingModule.withRoutes([{path: 'sites/info/:siteId', redirectTo: ''}]),
          HttpClientTestingModule,
          ReactiveFormsModule,
          InputMaskModule,
          DropdownModule,
          MultiSelectModule,
          AutoCompleteModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: SiteMembersService,
            useValue: siteMemberServiceStub,
          },
          {
            provide: SitesService,
            useValue: siteServiceStub,
          },
          {
            provide: UserService,
            useValue: userServiceStub,
          },
          MessageService,
          TransferState,
          OAuthService,
        ],
        declarations: [AddEditSiteMemberComponent],
      }).compileComponents();
      siteService = TestBed.inject(SitesService);
      siteMemberService = TestBed.inject(SiteMembersService);
      userService = TestBed.inject(UserService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditSiteMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('memberForm form invalid when empty', () => {
    expect(component.memberForm.valid).toBeFalsy();
  });

  it('memberForm form valid when all required fileds are set', () => {
    component.memberForm.controls.firstName.setValue('Jose');
    component.memberForm.controls.lastName.setValue('Olvera');
    component.memberForm.controls.user.setValue({
      email: 'jolvera@hospicesource.net',
      firstName: 'Jose',
      lastName: 'Olvera',
      phoneNumber: 5555555555,
    });

    expect(component.memberForm.valid).toBeTruthy();
  });

  it('should call `getSiteRoles` method of SiteService on getSiteRoles and match the result', () => {
    component.siteId = 1;
    component.getSiteRoles();
    expect(siteService.getSiteRoles).toHaveBeenCalled();
    expect(component.siteRoles).toEqual([
      {
        value: siteRole.id,
        label: siteRole.name,
      },
    ]);
  });

  it('should call `getSiteMemberById` method of SiteMemberService on getUser and match the result', () => {
    component.siteId = 1;
    component.memberId = 4;
    component.getUser();
    expect(siteMemberService.getSiteMemberById).toHaveBeenCalled();
    expect(component.member).toEqual(siteMember);
  });

  it('should call `createSiteMember` method of SiteMemberService on saveMember', () => {
    component.siteId = 1;
    component.memberId = 4;
    component.saveMember({
      countryCode: '+1',
      designation: 'a',
      email: 'cjhoward@hospicesource.net',
      firstName: 'Carrie Jo',
      lastName: 'Howard',
      phoneNumber: 5555555555,
      roleIds: [2],
    });
    expect(siteMemberService.createSiteMember).toHaveBeenCalled();
  });

  it('should call `updateSiteMember` method of SiteMemberService on updateMember', () => {
    component.siteId = 1;
    component.memberId = 4;
    component.updateMember({
      countryCode: '+1',
      designation: 'a',
      email: 'cjhoward@hospicesource.net',
      firstName: 'Carrie Jo',
      lastName: 'Howard',
      phoneNumber: 5555555555,
      roleIds: null,
    });
    expect(siteMemberService.updateSiteMember).toHaveBeenCalled();
  });

  it('should call `searchUser` method of userService on searchUsers and match the result', () => {
    component.searchUsers({
      query: 'test',
    });
    expect(userService.searchUser).toHaveBeenCalled();
    expect(component.searchedUsers).toEqual([
      {
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        phoneNumber: user.phoneNumber,
      },
    ]);
  });

  const siteRole = {
    id: 1,
    isStatic: true,
    level: 1,
    name: 'Master Admin',
    permissions: ['System:Create', 'System:Read'],
    roleType: 'Internal',
  };

  const siteRolesResponse = [siteRole];

  const siteMember = {
    cognitoUserId: '198ab6a3-519a-46ce-9c19-b526c0b21317',
    countryCode: 1,
    designation: 'Nurse',
    email: 'jake.peralta@example.com',
    enabled: false,
    firstName: 'Jake',
    id: 4,
    isEmailVerified: false,
    isPhoneNumberVerified: false,
    lastName: 'Peralta',
    name: 'Jake Peralta',
    phoneNumber: 5550102985,
    roles: [],
    site: {},
    siteId: 1,
    userId: 186,
    userRoles: [],
    userStatus: null,
  };

  const user = {
    countryCode: 1,
    email: 'abhay.saraf@bushel.co.in',
    enabled: true,
    firstName: 'Abhay',
    id: 23,
    isEmailVerified: true,
    isPhoneNumberVerified: false,
    lastName: 'Saraf',
    name: 'Abhay Saraf',
    phoneNumber: 5555555555,
    userId: '72ded20e-f379-4c31-8913-a56eac68f376',
    userRoles: [],
    userStatus: null,
  };

  const userResponse = {
    records: [user],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
