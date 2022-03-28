import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {VehiclesComponent} from './vehicles.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {VehicleService} from 'src/app/services';
import {PaginationResponse} from 'src/app/models';
import {TableVirtualScrollComponent} from 'src/app/common';
import {BehaviorSubject} from 'rxjs';
import {DatePipe} from '@angular/common';
import {PhonePipe} from 'src/app/pipes';

describe('VehiclesComponent', () => {
  let component: VehiclesComponent;
  let fixture: ComponentFixture<VehiclesComponent>;
  let vehicleService: any;

  beforeEach(
    waitForAsync(() => {
      const vehicleServiceStub = jasmine.createSpyObj('VehicleService', [
        'getAllVehicles',
        'searchVehicles',
      ]);
      vehicleServiceStub.getAllVehicles.and.returnValue(
        new BehaviorSubject<PaginationResponse>(vehicleResponse)
      );
      vehicleServiceStub.searchVehicles.and.returnValue(
        new BehaviorSubject<PaginationResponse>(vehicleResponse)
      );
      TestBed.configureTestingModule({
        declarations: [VehiclesComponent, TableVirtualScrollComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: OAuthService,
          },
          {
            provide: VehicleService,
            useValue: vehicleServiceStub,
          },
          MessageService,
          DatePipe,
          PhonePipe,
        ],
      }).compileComponents();
      vehicleService = TestBed.inject(VehicleService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(VehiclesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getVehicles').and.callThrough();
    spyOn(component.vehiclesTable, 'reset').and.returnValue(null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getVehicles` method on ngOnInit and match the result', () => {
    component.ngOnInit();
    expect(component.getVehicles).toHaveBeenCalled();
    expect(vehicleService.getAllVehicles).toHaveBeenCalled();
    expect(component.vehiclesResponse).toEqual(vehicleResponse);
  });

  it('should call `searchVehicles` method on searchVehicles and match the result', () => {
    // when query is empty
    component.searchVehicles('');
    expect(component.getVehicles).toHaveBeenCalled();

    // when query is not empty
    component.searchVehicles('test');
    expect(component.getVehicles).toHaveBeenCalled();
    expect(vehicleService.getAllVehicles).toHaveBeenCalled();
    expect(component.vehiclesResponse).toEqual(vehicleResponse);
  });

  const vehicle = {
    capacity: 0,
    currentDriverId: 21,
    currentDriverName: 'Damaris Fabela',
    cvn: '217168',
    id: 455,
    isActive: true,
    length: 0,
    licensePlate: 'MZX-2502',
    name: '217168',
    parkedLocationAddress: null,
    siteId: 8,
    siteName: 'Harlingen, TX',
    vin: '3C6TRVDG2HE513168',
  };

  const vehicleResponse = {
    records: [vehicle],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
