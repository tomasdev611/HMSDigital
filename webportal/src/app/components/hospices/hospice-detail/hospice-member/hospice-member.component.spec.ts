import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {HospiceMemberComponent} from './hospice-member.component';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {ReactiveFormsModule} from '@angular/forms';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {HospiceLocationService, HospiceMemberService} from 'src/app/services';
import {BehaviorSubject, from, of} from 'rxjs';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {ConfirmationService, MessageService} from 'primeng/api';

describe('HospiceMemberComponent', () => {
  let component: HospiceMemberComponent;
  let fixture: ComponentFixture<HospiceMemberComponent>;
  let hospiceLocationService: any;
  let hospiceMemberService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getHospiceLocations',
      ]);
      hospiceLocationServiceStub.getHospiceLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hopspiceLocationResponse)
      );

      const hospiceMemberServiceStub = jasmine.createSpyObj('HospiceMemberService', [
        'getAllHospiceMembers',
      ]);
      hospiceMemberServiceStub.getAllHospiceMembers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hopspiceMemberResponse)
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
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          {
            provide: HospiceMemberService,
            useValue: hospiceMemberServiceStub,
          },
          OAuthService,
          MessageService,
          ConfirmationService,
        ],
        declarations: [HospiceMemberComponent],
      }).compileComponents();
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      hospiceMemberService = TestBed.inject(HospiceMemberService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospiceMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getHospiceLocations` of HospiceLocationService on getHospiceLocations', () => {
    component.hospiceId = 2;
    component.getHospiceLocations();
    expect(hospiceLocationService.getHospiceLocations).toHaveBeenCalled();
  });

  it('should call `getAllHospiceMembers` of HospiceMemberService on getMembers', () => {
    component.hospiceId = 2;
    component.membersRequest = new SieveRequest();
    component.getMembers();
    expect(hospiceMemberService.getAllHospiceMembers).toHaveBeenCalled();
    expect(component.membersResponse).toEqual(hopspiceMemberResponse);
  });

  const hospiceLocation = {
    address: {},
    hospiceId: 1,
    id: 2,
    name: '1st Quality Hospice, LLC : 1st Quality - Woodville, TX',
    netSuiteCustomerId: 2027,
    phoneNumber: null,
    site: null,
    siteId: null,
  };

  const hospiceMember = {
    canAccessWebStore: true,
    canApproveOrder: false,
    cognitoUserId: '74ade2d3-455d-4297-85d3-9bc6197d2c26',
    countryCode: 1,
    designation: null,
    email: 'hospice1@suitecentric.com',
    enabled: false,
    firstName: 'SuiteCentric',
    hospiceId: 1,
    hospiceLocationMembers: [],
    hospiceLocations: [],
    id: 107,
    isEmailVerified: false,
    isPhoneNumberVerified: false,
    lastName: 'Account1',
    name: 'SuiteCentric Account1',
    netSuiteContactId: 41117,
    phoneNumber: 5555555555,
    userId: 44,
    userRoles: [],
    userStatus: null,
  };

  const hopspiceLocationResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const hopspiceMemberResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceMember],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
