import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ReactiveFormsModule} from '@angular/forms';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {SiteMembersService} from 'src/app/services';
import {SiteMembersComponent} from './site-members.component';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('SiteMembersComponent', () => {
  let component: SiteMembersComponent;
  let fixture: ComponentFixture<SiteMembersComponent>;
  let siteMemberService: any;

  beforeEach(
    waitForAsync(() => {
      const siteMemberServiceStub = jasmine.createSpyObj('SiteMembersService', [
        'getAllSiteMembers',
      ]);
      siteMemberServiceStub.getAllSiteMembers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(siteMembersResponse)
      );
      TestBed.configureTestingModule({
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: SiteMembersService,
            useValue: siteMemberServiceStub,
          },
        ],
        declarations: [SiteMembersComponent],
      }).compileComponents();
      siteMemberService = TestBed.inject(SiteMembersService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SiteMembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllSiteMembers` of SiteMembersService and match the result', () => {
    component.siteId = 1;
    component.membersRequest = new SieveRequest();
    component.getMembers();
    expect(siteMemberService.getAllSiteMembers).toHaveBeenCalled();
    expect(component.membersResponse).toEqual(siteMembersResponse);
  });

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

  const siteMembersResponse = {
    records: [siteMember],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
