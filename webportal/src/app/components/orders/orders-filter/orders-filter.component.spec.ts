import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {OAuthModule} from 'angular-oauth2-oidc';

import {OrdersFilterComponent} from './orders-filter.component';
import {HospiceService, HospiceLocationService} from 'src/app/services';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('OrdersFilterComponent', () => {
  let component: OrdersFilterComponent;
  let fixture: ComponentFixture<OrdersFilterComponent>;
  let hospiceService: any;
  let hospiceLocationService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', ['getAllhospices']);
      hospiceServiceStub.getAllhospices.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceResponse)
      );

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getLocations',
      ]);
      hospiceLocationServiceStub.getLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hopspiceLocationResponse)
      );

      TestBed.configureTestingModule({
        declarations: [OrdersFilterComponent],
        imports: [HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          {
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
        ],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
      hospiceLocationService = TestBed.inject(HospiceLocationService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllhospices` of HospiceService on getHospices', () => {
    component.getHospices();
    expect(hospiceService.getAllhospices).toHaveBeenCalled();
  });

  it('should call `getLocations` of HospiceLocationService on getHospiceLocations', () => {
    component.getHospiceLocations(1);
    expect(hospiceLocationService.getLocations).toHaveBeenCalled();
  });

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

  const hospiceResponse = {
    records: [hospice],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

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

  const hopspiceLocationResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
