import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {UsersComponent} from './users.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {UserService} from 'src/app/services';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('UsersComponent', () => {
  let component: UsersComponent;
  let fixture: ComponentFixture<UsersComponent>;
  let userService: any;

  beforeEach(
    waitForAsync(() => {
      const userServiceStub = jasmine.createSpyObj('UserService', ['searchUser', 'getAllUsers']);
      userServiceStub.searchUser.and.returnValue(
        new BehaviorSubject<PaginationResponse>(userResponse)
      );
      userServiceStub.getAllUsers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(userResponse)
      );
      TestBed.configureTestingModule({
        declarations: [UsersComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: UserService,
            useValue: userServiceStub,
          },
          OAuthService,
          MessageService,
        ],
      }).compileComponents();
      userService = TestBed.inject(UserService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(UsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `searchUser` of UserService on getUsers and match the result', () => {
    const searchQuery = 'Hao';
    const userRequest = new SieveRequest();
    component.getUsers({userRequest, searchQuery});
    expect(userService.searchUser).toHaveBeenCalled();
    expect(component.users).toEqual(userResponse.records);
    expect(component.userResponse).toEqual(userResponse);
  });

  it('should call `getAllUsers` of UserService on getUsers and match the result', () => {
    const userRequest = new SieveRequest();
    component.getUsers({userRequest, searchQuery: null});
    component.getUsers({userRequest, searchQuery: null});
    expect(userService.getAllUsers).toHaveBeenCalled();
    expect(component.users).toEqual(userResponse.records);
    expect(component.userResponse).toEqual(userResponse);
  });

  const userRole = {
    id: 490,
    resourceId: '*',
    resourceType: 'Site',
    roleId: 1,
    roleLevel: 1,
    roleName: 'Master Admin',
  };

  const user = {
    countryCode: 0,
    email: 'hao.ying@nimbold.com',
    enabled: true,
    firstName: 'Hao',
    id: 250,
    isEmailVerified: false,
    isPhoneNumberVerified: false,
    lastName: 'Ying',
    name: 'Hao Ying',
    phoneNumber: 0,
    userId: 'ddca8d85-3b0e-4ee5-a272-28700457948e',
    userRoles: [userRole],
    userStatus: null,
  };

  const userResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [user],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
