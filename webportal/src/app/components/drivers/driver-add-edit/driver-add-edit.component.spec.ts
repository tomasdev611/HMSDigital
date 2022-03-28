import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DriverAddEditComponent} from './driver-add-edit.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {
  VehicleService,
  DriverService,
  SitesService,
  UserService,
  ToastService,
} from 'src/app/services';
import {MessageService} from 'primeng/api';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {DropdownModule} from 'primeng/dropdown';
import {InputMaskModule} from 'primeng/inputmask';
import {TransferState} from '@angular/platform-browser';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('DriverAddEditComponent', () => {
  let component: DriverAddEditComponent;
  let fixture: ComponentFixture<DriverAddEditComponent>;
  let driverService: any;
  let vehicleService: any;
  let siteService: any;
  let userService: any;
  let toastService: any;

  beforeEach(
    waitForAsync(() => {
      const driverServiceStub = jasmine.createSpyObj('DriverService', [
        'saveDriver',
        'updateDriver',
        'deleteDriver',
        'getDriverById',
      ]);
      driverServiceStub.saveDriver.and.returnValue(new BehaviorSubject(driver));
      driverServiceStub.updateDriver.and.returnValue(new BehaviorSubject(driver));
      driverServiceStub.getDriverById.and.returnValue(new BehaviorSubject(driver));
      driverServiceStub.deleteDriver.and.returnValue(new BehaviorSubject(driver));

      const siteServiceStub = jasmine.createSpyObj('SitesService', ['searchSites']);
      siteServiceStub.searchSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(siteResponse)
      );

      const vehicleServiceStub = jasmine.createSpyObj('VehicleService', ['searchVehicles']);
      vehicleServiceStub.searchVehicles.and.returnValue(
        new BehaviorSubject<PaginationResponse>(vehiclesResponse)
      );

      const userServiceStub = jasmine.createSpyObj('UserService', ['searchUser']);
      userServiceStub.searchUser.and.returnValue(
        new BehaviorSubject<PaginationResponse>(userResponse)
      );

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      TestBed.configureTestingModule({
        declarations: [DriverAddEditComponent],
        imports: [
          RouterTestingModule.withRoutes([
            {path: 'drivers', redirectTo: ''},
            {path: 'drivers/edit/:driverId', redirectTo: ''},
          ]),
          HttpClientTestingModule,
          ReactiveFormsModule,
          FormsModule,
          OAuthModule.forRoot(),
          AutoCompleteModule,
          DropdownModule,
          InputMaskModule,
        ],
        providers: [
          {
            provide: DriverService,
            useValue: driverServiceStub,
          },
          {
            provide: SitesService,
            useValue: siteServiceStub,
          },
          {
            provide: VehicleService,
            useValue: vehicleServiceStub,
          },
          {
            provide: UserService,
            useValue: userServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          MessageService,
          OAuthService,
          TransferState,
        ],
      }).compileComponents();
      driverService = TestBed.inject(DriverService);
      siteService = TestBed.inject(SitesService);
      vehicleService = TestBed.inject(VehicleService);
      userService = TestBed.inject(UserService);
      toastService = TestBed.inject(ToastService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'refreshDriverInfo').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `saveDriver` method of DriverService on saveDriver and match the result', () => {
    component.saveDriver(driver);
    expect(driverService.saveDriver).toHaveBeenCalled();
  });

  it('driverForm form invalid when empty', () => {
    expect(component.driverForm.valid).toBeFalsy();
  });

  it('driverForm form valid when all required fields are set', () => {
    const requiredValue = {
      user: 'test@test.com',
      firstName: 'first_name',
      lastName: 'last_name',
    };
    component.driverForm.patchValue(requiredValue);
    expect(component.driverForm.valid).toBeTruthy();
  });

  it('should call `getDriverById` method of DriverService on getDriverDetail and match the result', () => {
    component.driverId = driver.id;
    component.getDriverDetail();
    expect(driverService.getDriverById).toHaveBeenCalled();
    expect(component.refreshDriverInfo).toHaveBeenCalled();
  });

  it('should call `searchSites` method of SitesService on searchSites and match the result', () => {
    component.searchSites({query: 'query'});
    expect(siteService.searchSites).toHaveBeenCalled();
    expect(component.sites).toEqual(siteResponse.records);
  });

  it('should call `searchVehicles` method of VehicleService on searchVehicles and match the result', () => {
    component.searchVehicles({query: '11'});
    expect(vehicleService.searchVehicles).toHaveBeenCalled();
    expect(component.vehicles).toEqual(vehiclesResponse.records);
  });

  it('should call `updateDriver` method of DriverService on updateDriver and match the result', () => {
    component.driverId = driver.id;
    component.updateDriver(driver);
    expect(driverService.updateDriver).toHaveBeenCalled();
    expect(component.driver).toEqual(driver);
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call `searchUser` method of UserService on searchUsers when add a driver and match the result', () => {
    component.editmode = false;
    const users = [
      {
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        phoneNumber: user.phoneNumber,
      },
    ];
    component.searchUsers({query: 'test'});
    expect(userService.searchUser).toHaveBeenCalled();
    expect(component.searchedUsers).toEqual(users);
  });

  it('should return empty on searchUsers when edit the driver', () => {
    component.editmode = true;
    component.searchUsers({query: 'test'});
    expect(component.searchedUsers).toEqual([]);
  });

  it('should call `deleteDriver` method of DriverService on deleteConfirmed and match the result', () => {
    component.driverId = driver.id;
    component.deleteConfirmed();
    expect(driverService.deleteDriver).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  const driver = {
    countryCode: 1,
    currentSiteId: 5,
    currentSiteName: 'Corpus Christi, TX',
    currentVehicle: null,
    currentVehicleId: null,
    division: null,
    email: 'jgonzalez@hospicesource.net',
    firstName: 'Josue',
    id: 1,
    lastName: 'Gonzalez',
    network: null,
    phoneNumber: 5555555555,
    userId: 40,
    vehicleId: 0,
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

  const vehicle = {
    capacity: 0,
    currentDriverId: 5,
    currentDriverName: 'Alvaro Valle',
    cvn: '217403',
    id: 101,
    isActive: false,
    length: 0,
    licensePlate: 'JXD0862',
    name: '217403',
    netSuiteLocationId: 462,
    parentNetSuiteLocationId: 36,
    siteId: 6,
    siteName: 'Harlingen, TX',
    vin: '3C6TRVDG6HE539403',
  };

  const vehiclesResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [vehicle],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const user = {
    countryCode: 1,
    email: 'test@test.com',
    enabled: true,
    firstName: 'first_name',
    id: 23,
    isEmailVerified: true,
    isPhoneNumberVerified: false,
    lastName: 'last_name',
    name: 'first_name last_name',
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
