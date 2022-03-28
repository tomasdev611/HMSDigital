import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RouterTestingModule} from '@angular/router/testing';
import {DispatchService} from 'src/app/services/dispatch.service';
import {
  OrderHeadersService,
  PatientService,
  ToastService,
  VehicleService,
  DriverService,
} from 'src/app/services';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

import {DispatchFulfillOrderComponent} from './dispatch-fulfill-order.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MessageService} from 'primeng/api';
import {TransferState} from '@angular/platform-browser';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject, of} from 'rxjs';

describe('DispatchFulfillOrderComponent', () => {
  let component: DispatchFulfillOrderComponent;
  let fixture: ComponentFixture<DispatchFulfillOrderComponent>;
  let orderHeaderService: any;
  let patientService: any;
  let dispatchService: any;
  let toastService: any;
  let vehicleService: any;
  let driverService: any;

  beforeEach(
    waitForAsync(() => {
      const orderHeaderServiceStub = jasmine.createSpyObj('OrderHeadersService', [
        'getOrderFulfillment',
        'getOrderHeaderById',
      ]);
      orderHeaderServiceStub.getOrderFulfillment.and.returnValue(of(orderFulfillmentResponse));
      orderHeaderServiceStub.getOrderHeaderById.and.returnValue(of(orderRecord));

      const patientServiceStub = jasmine.createSpyObj('PatientService', ['getPatients']);
      patientServiceStub.getPatients.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );

      const dispatchServiceStub = jasmine.createSpyObj('DispatchService', ['fulfillOrder']);
      dispatchServiceStub.fulfillOrder.and.returnValue(new BehaviorSubject(null));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError']);
      toastServiceStub.showError.and.callThrough();

      const vehicleServiceStub = jasmine.createSpyObj('VehicleService', ['searchVehicles']);
      vehicleServiceStub.searchVehicles.and.returnValue(
        new BehaviorSubject<PaginationResponse>(vehiclesResponse)
      );

      const driverServiceStub = jasmine.createSpyObj('DriverService', ['searchDrivers']);
      driverServiceStub.searchDrivers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(driverResponse)
      );

      TestBed.configureTestingModule({
        declarations: [DispatchFulfillOrderComponent],
        imports: [
          ReactiveFormsModule,
          FormsModule,
          RouterTestingModule.withRoutes([{path: 'dispatch', redirectTo: ''}]),
          HttpClientTestingModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: OrderHeadersService,
            useValue: orderHeaderServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          {
            provide: DispatchService,
            useValue: dispatchServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          {
            provide: VehicleService,
            useValue: vehicleServiceStub,
          },
          {
            provide: DriverService,
            useValue: driverServiceStub,
          },
          MessageService,
          TransferState,
          OAuthService,
        ],
      }).compileComponents();
      orderHeaderService = TestBed.inject(OrderHeadersService);
      patientService = TestBed.inject(PatientService);
      dispatchService = TestBed.inject(DispatchService);
      toastService = TestBed.inject(ToastService);
      vehicleService = TestBed.inject(VehicleService);
      driverService = TestBed.inject(DriverService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchFulfillOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'formatOrders').and.callThrough();
    spyOn(component, 'ordersFullfilmentFormat').and.callThrough();
    spyOn(component, 'setOrderForm').and.callThrough();
    spyOn(component, 'calculateProcessingTime').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call some functions on getOrder', () => {
    component.orderId = orderId;
    component.getOrder();
    expect(orderHeaderService.getOrderHeaderById).toHaveBeenCalled();
    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.order).toEqual(orderRecord);
    expect(component.calculateProcessingTime).toHaveBeenCalled();
    expect(component.formatOrders).toHaveBeenCalled();
    expect(component.ordersFullfilmentFormat).toHaveBeenCalled();
    expect(component.setOrderForm).toHaveBeenCalled();
  });

  it('orderForm form invalid when empty', () => {
    component.orderId = orderId;
    component.setOrderForm();
    expect(component.orderForm.valid).toBeFalsy();
  });

  it('orderForm form valid when all required fileds are set', () => {
    component.orderId = orderId;
    component.setOrderForm();
    const date = new Date();
    component.orderForm.patchValue({
      orderId: 679,
      fulfillmentStartDateTime: date,
      fulfillmentEndDateTime: date,
      driverId: 1,
      vehicleId: 455,
    });
    expect(component.orderForm.valid).toBeTruthy();
  });

  it('should be failed to call `dispatchService` when startDate is after than endDate on onSubmitOrder', () => {
    const startDate = new Date();
    const endDate = new Date(startDate.getDate() - 1);
    const orderForm = {
      orderId: 679,
      fulfillmentStartDateTime: startDate,
      fulfillmentEndDateTime: endDate,
      driverId: 1,
      vehicleId: 455,
      fulfillmentItems: [
        {
          assetTagNumber: '13079124',
          count: 1,
          fulfillmentType: 'delivery',
          itemId: null,
          lotNumber: '',
          orderLineItemId: 6100,
          serialNumber: 'BUB0116110763',
        },
        {
          assetTagNumber: '13006635',
          count: 1,
          fulfillmentType: 'delivery',
          itemId: null,
          lotNumber: '',
          orderLineItemId: 6100,
          serialNumber: 'BUB0115070058',
        },
      ],
    };
    component.onSubmitOrder(orderForm);
    expect(toastService.showError).toHaveBeenCalled();
  });

  it('should call `dispatchService` on onSubmitOrder', () => {
    const endDate = new Date();
    const startDate = new Date(endDate.getDate() - 1);
    const orderForm = {
      orderId: 679,
      fulfillmentStartDateTime: startDate,
      fulfillmentEndDateTime: endDate,
      driverId: 1,
      vehicleId: 455,
      fulfillmentItems: [
        {
          assetTagNumber: '13079124',
          count: 1,
          fulfillmentType: 'delivery',
          itemId: null,
          lotNumber: '',
          orderLineItemId: 6100,
          serialNumber: 'BUB0116110763',
        },
        {
          assetTagNumber: '13006635',
          count: 1,
          fulfillmentType: 'delivery',
          itemId: null,
          lotNumber: '',
          orderLineItemId: 6100,
          serialNumber: 'BUB0115070058',
        },
      ],
    };
    component.onSubmitOrder(orderForm);
    expect(dispatchService.fulfillOrder).toHaveBeenCalled();
  });

  it('should call `searchVehicles` of VehicleService on searchVehicles', () => {
    component.searchVehicles({query: 'query'});
    expect(vehicleService.searchVehicles).toHaveBeenCalled();
    expect(component.vehicles).toEqual(vehiclesResponse.records);
  });

  it('should call `searchDrivers` of DriverService on searchDrivers', () => {
    component.searchDrivers({query: 'query'});
    expect(driverService.searchDrivers).toHaveBeenCalled();
    expect(component.drivers).toEqual(driverResponse.records);
  });

  const orderId = 679;

  const orderFulfillmentRecord = {
    assetTag: null,
    deliveredStatus: 'Requested',
    fulfillmentEndAtLatitude: 0,
    fulfillmentEndAtLongitude: 0,
    fulfillmentEndDateTime: '2020-12-15T10:02:15.893Z',
    fulfillmentStartAtLatitude: 0,
    fulfillmentStartAtLongitude: 0,
    fulfillmentStartDateTime: '2020-12-15T10:02:15.893Z',
    fulfilledByDriverId: 5,
    fulfilledByDriverName: 'Jose Olvera',
    fulfilledBySiteId: 0,
    fulfilledByVehicleCvn: '217168',
    fulfilledByVehicleId: 455,
    id: 602,
    isFulfilmentConfirmed: false,
    itemName: 'PAP Mask - Small',
    lotNumber: null,
    netSuiteCustomerId: 2097,
    netSuiteDispatchRecordId: 0,
    netSuiteItemId: 24,
    netSuiteOrderId: 438227,
    orderHeaderId: 616,
    orderLineItemId: 5942,
    orderType: 'pickup',
    patientUuid: '16142463-865c-4db3-86d6-08809ece186a',
    quantity: 1,
    serialNumber: null,
  };

  const orderRecord = {
    confirmationNumber: '',
    deliveryAddress: {},
    requestedStartDateTime: '2020-12-22T09:39:07Z',
    requestedEndDateTime: '2020-12-22T11:39:07Z',
    fulfillmentEndDateTime: '0001-01-01T00:00:00Z',
    fulfillmentStartDateTime: '0001-01-01T00:00:00Z',
    dispatchStatus: 'Planned',
    dispatchStatusId: 10,
    ext_shouldShowApproveBtn: false,
    hospice: {},
    hospiceId: 37,
    hospiceLocationId: 62,
    id: 644,
    netSuiteOrderId: 441682,
    nurse: {},
    orderDateTime: '2020-12-22T09:39:00Z',
    orderLineItems: [],
    orderNotes: [],
    orderNumber: '2222102576',
    orderRecipientUserId: 198,
    orderStatus: 'Pending Approval',
    orderType: 'Delivery',
    orderTypeId: 10,
    originatorUserId: 23,
    patient: {
      createdDateTime: '2020-09-26T11:21:48.8202506Z',
      dateOfBirth: '2020-01-09T00:00:00Z',
      diagnosis: null,
      firstName: 'Todd',
      hms2Id: null,
      hospiceId: 37,
      hospiceLocationId: 62,
      id: 1,
    },
    patientNotes: [],
    patientUuid: '95bbf7c7-4303-435d-9671-853efbd08c3d',
    pickupAddress: {},
    site: {},
    shippingAddressId: 0,
    siteId: 8,
    statOrder: false,
    statusId: 19,
  };

  const patientRecord = {
    createdDateTime: '2020-09-26T11:21:48.8202506Z',
    dateOfBirth: '2020-01-09T00:00:00Z',
    diagnosis: null,
    firstName: 'Todd',
    hms2Id: null,
    hospiceId: 37,
    hospiceLocationId: 62,
    id: 1,
    isInfectious: false,
    lastName: 'Loomis',
    lastOrderDateTime: '2020-12-30T11:13:32.947127Z',
    lastOrderNumber: '2222102602',
    patientAddress: [],
    patientHeight: 177.8,
    patientNotes: [],
    patientWeight: 170,
    phoneNumbers: [],
    uniqueId: '5eb30f32-2abf-4aba-a02f-1b117339f623',
  };

  const orderFulfillmentResponse = {
    records: [orderFulfillmentRecord],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const patientResponse = {
    records: [patientRecord],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
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
});
