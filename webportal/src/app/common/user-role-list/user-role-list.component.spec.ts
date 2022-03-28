import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {UserRoleListComponent} from './user-role-list.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {DropdownModule} from 'primeng/dropdown';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {FormsModule} from '@angular/forms';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {TransferState} from '@angular/platform-browser';
import {SitesService, HospiceService, HospiceLocationService, RoleService} from 'src/app/services';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('UserRoleListComponent', () => {
  let component: UserRoleListComponent;
  let fixture: ComponentFixture<UserRoleListComponent>;
  let siteService: any;
  let hospiceService: any;
  let hospiceLocationService: any;
  let roleService: any;

  beforeEach(
    waitForAsync(() => {
      const siteServiceStub = jasmine.createSpyObj('SitesService', ['searchSites']);
      siteServiceStub.searchSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(siteResponse)
      );

      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', ['searchHospices']);
      hospiceServiceStub.searchHospices.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceResponse)
      );

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getHospiceLocations',
      ]);
      hospiceLocationServiceStub.getHospiceLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceLocationsResponse)
      );

      const roleServiceStub = jasmine.createSpyObj('RoleService', ['getAllRoles']);
      roleServiceStub.getAllRoles.and.returnValue(new BehaviorSubject<Role[]>([new Role()]));

      TestBed.configureTestingModule({
        declarations: [UserRoleListComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          DropdownModule,
          AutoCompleteModule,
          FormsModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: OAuthService,
            RoleService,
          },
          {
            provide: SitesService,
            useValue: siteServiceStub,
          },
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          {
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          {
            provide: RoleService,
            useValue: roleServiceStub,
          },
          TransferState,
        ],
      }).compileComponents();
      siteService = TestBed.inject(SitesService);
      hospiceService = TestBed.inject(HospiceService);
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      roleService = TestBed.inject(RoleService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(UserRoleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `searchSites` of SiteService on searchResource when resourceType is Site', () => {
    component.userRole = {
      resourceType: 'Site',
      role,
    };
    component.searchResource({query: 'query'});
    expect(siteService.searchSites).toHaveBeenCalled();
    expect(component.resourceList).toEqual([
      {
        id: site.id,
        name: site.name,
      },
    ]);
  });

  it('should call `searchHospices` of HospiceService on searchResource when resourceType is Hospice', () => {
    component.userRole = {
      resourceType: 'Hospice',
      role,
    };
    component.searchResource({query: 'query'});
    expect(hospiceService.searchHospices).toHaveBeenCalled();
    expect(component.resourceList).toEqual([
      {
        id: hospice.id,
        name: hospice.name,
      },
    ]);
  });

  it('should call `getHospiceLocations` of HospiceLocationService on searchResource when resourceType is HospiceLocation', () => {
    component.userRole = {
      resourceType: 'HospiceLocation',
      role,
    };
    component.searchResource({query: 'query'});
    expect(hospiceLocationService.getHospiceLocations).toHaveBeenCalled();
    expect(component.resourceList).toEqual([
      {
        id: hospiceLocation.id,
        name: hospiceLocation.name,
      },
    ]);
  });

  it('should call `getAllRoles` of RoleService on getAllRoles', () => {
    component.getAllRoles();
    expect(roleService.getAllRoles).toHaveBeenCalled();
    expect(component.backUpRoles).toEqual([new Role()]);
  });

  const role = {
    id: 5,
    isStatic: true,
    level: 4,
    name: 'Inventory RVP',
    permissions: ['Hospice:Read'],
    roleType: 'Internal',
  };

  const site = {
    address: {},
    capacity: 0,
    currentDriverId: 0,
    currentDriverName: null,
    cvn: '',
    id: 6,
    isActive: false,
    isDisable: false,
    length: 0,
    licensePlate: '',
    locationType: 'Site',
    name: 'Harlingen, TX',
    netSuiteLocationId: 36,
    parentNetSuiteLocationId: 8,
    siteCode: 2430,
    siteId: null,
    siteName: null,
    sitePhoneNumber: [],
    vehicles: null,
    vin: '',
  };

  const siteResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [site],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const hospice = {
    id: 1,
    name: 'Apple Hospices',
    hospiceLocations: [
      {
        id: 1,
        name: 'San DieSan Diego Centre',
        hospiceId: 1,
        siteId: 0,
      },
    ],
  };

  const hospiceResponse: PaginationResponse = {
    records: [hospice],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

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
});

export class Permission {
  isAdmin = true;
  canCreate = true;
  canRead = true;
  canUpdate = true;
  canDelete = true;
  permissionIdnumber = 1;
  name: 'permission';
}

export class Role {
  id = 1;
  permissions = [new Permission()];
  name = 'role';
  level = 1;
}
