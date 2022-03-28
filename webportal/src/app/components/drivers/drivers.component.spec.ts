import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DriversComponent} from './drivers.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {DropdownModule} from 'primeng/dropdown';
import {InputMaskModule} from 'primeng/inputmask';
import {TransferState} from '@angular/platform-browser';
import {RouterTestingModule} from '@angular/router/testing';
import {DriverService, VehicleService, SitesService} from 'src/app/services';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {TableVirtualScrollComponent} from 'src/app/common';
import {BehaviorSubject} from 'rxjs';
import {DatePipe} from '@angular/common';
import {PhonePipe} from 'src/app/pipes';
import {deepCloneObject} from 'src/app/utils';

describe('DriversComponent', () => {
  let component: DriversComponent;
  let fixture: ComponentFixture<DriversComponent>;
  let driverService: any;
  let vehicleService: any;
  let siteService: any;

  beforeEach(
    waitForAsync(() => {
      const driverServiceStub = jasmine.createSpyObj('DriverService', [
        'getAllDrivers',
        'searchDrivers',
        'updateDriver',
        'deleteDriver',
        'getDriverById',
      ]);
      driverServiceStub.getAllDrivers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(driverResponse)
      );
      driverServiceStub.searchDrivers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(driverResponse)
      );
      driverServiceStub.updateDriver.and.returnValue(new BehaviorSubject(driver));
      driverServiceStub.deleteDriver.and.returnValue(new BehaviorSubject(driver));
      driverServiceStub.getDriverById.and.returnValue(new BehaviorSubject(driver));

      const vehicleServiceStub = jasmine.createSpyObj('VehicleService', [
        'getAllVehicles',
        'searchVehicles',
      ]);
      vehicleServiceStub.getAllVehicles.and.returnValue(
        new BehaviorSubject<PaginationResponse>(vehiclesResponse)
      );
      vehicleServiceStub.searchVehicles.and.returnValue(
        new BehaviorSubject<PaginationResponse>(vehiclesResponse)
      );

      const siteServiceStub = jasmine.createSpyObj('SitesService', ['searchSites']);
      siteServiceStub.searchSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(siteResponse)
      );

      TestBed.configureTestingModule({
        declarations: [DriversComponent, TableVirtualScrollComponent],
        imports: [
          RouterTestingModule.withRoutes([{path: 'drivers/edit/:driverId', redirectTo: ''}]),
          HttpClientTestingModule,
          OAuthModule.forRoot(),
          ReactiveFormsModule,
          FormsModule,
          AutoCompleteModule,
          DropdownModule,
          InputMaskModule,
        ],
        providers: [
          {
            provide: OAuthService,
          },
          {
            provide: DriverService,
            useValue: driverServiceStub,
          },
          {
            provide: VehicleService,
            useValue: vehicleServiceStub,
          },
          {
            provide: SitesService,
            useValue: siteServiceStub,
          },
          MessageService,
          TransferState,
          DatePipe,
          PhonePipe,
        ],
      }).compileComponents();
      driverService = TestBed.inject(DriverService);
      vehicleService = TestBed.inject(VehicleService);
      siteService = TestBed.inject(SitesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DriversComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getDrivers').and.callThrough();
    spyOn(component.driversTable, 'reset').and.returnValue(null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getDrivers` method on ngOnInit', () => {
    component.ngOnInit();
    expect(component.getDrivers).toHaveBeenCalled();
  });

  it('should call `getAllDrivers` method of DriverService on getDrivers and match the result', () => {
    component.searchQuery = '';
    component.driverRequest = driverRequest;
    component.getDrivers();
    expect(driverService.getAllDrivers).toHaveBeenCalled();
    expect(component.driversResponse.records).toEqual(driverResponse.records);
  });

  it('should call `searchDrivers` method of DriverService on getDrivers and match the result', () => {
    component.searchQuery = 'Jose';
    component.driverRequest = driverRequest;
    component.getDrivers();
    expect(driverService.searchDrivers).toHaveBeenCalled();
    expect(component.driversResponse.records).toEqual(driverResponse.records);
  });

  it('should call `updateDriver` method of DriverService on updateDriver and match the result', () => {
    component.selectedDriver = driver;
    component.updateDriver(driver);
    expect(driverService.updateDriver).toHaveBeenCalled();
    expect(component.driversResponse.records).toEqual(driverResponse.records);
  });

  it('should call `deleteDriver` method of DriverService on deleteConfirmed and match the result', () => {
    component.selectedDriver = driver;
    component.deleteConfirmed();
    expect(driverService.deleteDriver).toHaveBeenCalled();
    expect(component.driversResponse.records).toEqual(driverResponse.records);
  });

  it('driverForm form invalid when empty', () => {
    expect(component.driverForm.valid).toBeFalsy();
  });

  it('driverForm form valid when all required fields are set', () => {
    const formValue = {
      firstName: 'first name',
      lastName: 'last name',
      phoneNumber: '(111) 111-1111',
      email: 'a@a.com',
      user: {
        email: 'jgonzalez@hospicesource.net',
        firstName: 'test',
        lastName: 'test',
        phoneNumber: 5555555555,
      },
    };
    component.driverForm.patchValue(formValue);
    expect(component.driverForm.valid).toBeTruthy();
  });

  it('should call `getDriverById` method of DriverService on patchDriverInfo and match the result', () => {
    component.selectedDriver = driver;
    component.patchDriverInfo();
    expect(driverService.getDriverById).toHaveBeenCalled();
    expect(component.vehicle).toEqual(deepCloneObject(driver.currentVehicle));
    expect(component.vehicleId).toEqual(deepCloneObject(driver.currentVehicleId));
  });

  it('should call `searchVehicles` method of VehicleService on searchVehicles and match the result', () => {
    component.searchVehicles({query: '11'});
    expect(vehicleService.searchVehicles).toHaveBeenCalled();
    expect(component.vehicles).toEqual(vehiclesResponse.records);
  });

  it('should call `searchSites` method of SitesService on searchSites and match the result', () => {
    component.searchSites({query: 'query'});
    expect(siteService.searchSites).toHaveBeenCalled();
    expect(component.sites).toEqual(siteResponse.records);
  });

  it('should call `getAllVehicles` method of VehicleService on getCurrentSiteVehicles and match the result', () => {
    component.vehiclesRequest = new SieveRequest();
    const siteVehicles = [
      {
        ...vehicle,
        label: vehicle.name,
        value: vehicle.id,
      },
    ];
    component.getCurrentSiteVehicles();
    expect(vehicleService.getAllVehicles).toHaveBeenCalled();
    expect(component.currentSiteVehicles).toEqual(siteVehicles);
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

  const driverResponse = {
    records: [driver],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const driverRequest = new SieveRequest();

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
});
