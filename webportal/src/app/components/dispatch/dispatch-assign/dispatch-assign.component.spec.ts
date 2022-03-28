import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DispatchAssignComponent} from './dispatch-assign.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {
  RouteOptimizationService,
  DispatchService,
  OrderHeadersService,
  SitesService,
  ToastService,
  PatientService,
} from 'src/app/services';
import {BehaviorSubject, of} from 'rxjs';
import {TransferState} from '@angular/platform-browser';
import {PaginationResponse} from 'src/app/models';
import {DispatchSchedulerComponent} from './dispatch-scheduler/dispatch-scheduler.component';

describe('DispatchAssignComponent', () => {
  let component: DispatchAssignComponent;
  let fixture: ComponentFixture<DispatchAssignComponent>;
  let routeOptimizationService: any;
  let dispatchService: any;
  let orderHeaderService: any;
  let sitesService: any;
  let patientService: any;
  let toastService: any;

  beforeEach(
    waitForAsync(() => {
      const dispatchServiceStub = jasmine.createSpyObj('DispatchService', [
        'getAllDispatchInstructions',
        'dispatchAssign',
      ]);
      dispatchServiceStub.getAllDispatchInstructions.and.returnValue(
        of(dispatchInstructionsResponse)
      );
      dispatchServiceStub.dispatchAssign.and.returnValue(of(null));

      const orderHeaderServiceStub = jasmine.createSpyObj('OrderHeadersService', [
        'getAllOrderHeaders',
        'getOrderHeaderById',
      ]);
      orderHeaderServiceStub.getAllOrderHeaders.and.returnValue(of(orderHeaderResponse));
      orderHeaderServiceStub.getOrderHeaderById.and.returnValue(of(orderHeader));

      const sitesServiceStub = jasmine.createSpyObj('SitesService', [
        'searchSites',
        'getVehiclesByLocationId',
      ]);
      sitesServiceStub.searchSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(sitesResponse)
      );
      sitesServiceStub.getVehiclesByLocationId.and.returnValue(new BehaviorSubject<any>([vehicle]));

      const patientServiceStub = jasmine.createSpyObj('PatientService', [
        'getPatientNotes',
        'getPatients',
      ]);
      patientServiceStub.getPatientNotes.and.returnValue(of([]));
      patientServiceStub.getPatients.and.returnValue(of(patientResponse));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      TestBed.configureTestingModule({
        declarations: [DispatchAssignComponent, DispatchSchedulerComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: DispatchService,
            useValue: dispatchServiceStub,
          },
          {
            provide: OrderHeadersService,
            useValue: orderHeaderServiceStub,
          },
          {
            provide: SitesService,
            useValue: sitesServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          TransferState,
          OAuthService,
          MessageService,
        ],
      }).compileComponents();
      routeOptimizationService = TestBed.inject(RouteOptimizationService);
      dispatchService = TestBed.inject(DispatchService);
      orderHeaderService = TestBed.inject(OrderHeadersService);
      sitesService = TestBed.inject(SitesService);
      patientService = TestBed.inject(PatientService);
      toastService = TestBed.inject(ToastService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchAssignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'formatOrders').and.callThrough();
    spyOn(component, 'getOnlyVehiclesFromLocation').and.callThrough();
    spyOn(component, 'getDispatchInstructions').and.callThrough();
    spyOn(component, 'closeAssignOrderPreview').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call services on getDispatchInstructions and match the result', () => {
    component.getDispatchInstructions(vehicleId);
    expect(dispatchService.getAllDispatchInstructions).toHaveBeenCalled();
    expect(orderHeaderService.getAllOrderHeaders).toHaveBeenCalled();
    expect(component.dispatchInsRes).toEqual(dispatchInstructionsResponse.records);
  });

  it('should call `getVehiclesByLocationId` of SitesService on getVehicles and call getOnlyVehiclesFromLocation to map result', () => {
    component.getVehicles(siteid);
    expect(sitesService.getVehiclesByLocationId).toHaveBeenCalled();
    expect(component.getOnlyVehiclesFromLocation).toHaveBeenCalled();
    expect(component.getDispatchInstructions).toHaveBeenCalled();
  });

  it('should call `dispatchAssign` of DispatchService on assignOrderToTruck match the result', () => {
    component.assignToTruckId = 103;
    component.assignOrderToTruck();
    expect(dispatchService.dispatchAssign).toHaveBeenCalled();
    expect(component.closeAssignOrderPreview).toHaveBeenCalled();
  });

  it('should call service on getOrderDetailInformation match the result', () => {
    component.currentOrder = orderHeader;
    component.getOrderDetailInformation();
    expect(orderHeaderService.getOrderHeaderById).toHaveBeenCalled();
    expect(patientService.getPatientNotes).toHaveBeenCalled();
    expect(component.currentOrder.patientNotes).toEqual([]);
  });

  const truckId = 457;
  const vehicleId = 110;
  const siteid = 47;

  const location = {
    latitude: 26.204241,
    longitude: -98.214813,
  };

  const optimizedRoutes = {
    driverItineraries: [
      {
        driver: {
          name: 'Jorge Cardenas',
        },
        instructions: [],
        route: {
          endLocation: location,
          endTime: '2020-12-15T12:41:22Z',
          startLocation: location,
          startTime: '2020-12-14T16:00:00Z',
          totalTravelDistance: 11849,
          totalTravelTime: '00:24:11',
          wayPoints: [location],
        },
      },
    ],
    unAssignedItems: [],
  };

  const orders = {
    drivers: [
      {
        name: 'Driver Two McAllen',
        shifts: [
          {
            startTime: '2020-12-14T16:00:00.216Z',
            startLocation: location,
            endTime: '2020-12-15T16:00:00.216Z',
            endLocation: location,
          },
        ],
      },
    ],
    orders: [
      {
        openingTime: '2020-12-15T12:00:00.000Z',
        expectedDeliveryDateTime: '2020-12-15T14:00:00.000Z',
        processingTime: '00:30:00',
        priority: 1,
        name: 605,
        location,
      },
    ],
  };

  const orderLineItem = {
    action: 'Delivery',
    actionId: 10,
    assetTagNumber: null,
    deliveryAddress: null,
    requestedStartDateTime: '2021-01-12T13:48:56Z',
    requestedEndDateTime: '2021-01-12T15:48:56Z',
    dispatchStatus: 'Completed',
    dispatchStatusId: 15,
    equipmentSettings: [],
    id: 6224,
    item: {},
    itemCount: 1,
    itemId: 4061,
    lotNumber: null,
    orderHeaderId: 720,
    pickupAddress: null,
    serialNumber: null,
    shippingAddressId: 0,
    site: null,
    siteId: 10,
    status: 'Completed',
    statusId: 15,
  };

  const orderHeader = {
    confirmationNumber: '259-2222102644',
    deliveryAddress: null,
    requestedStartDateTime: '2021-01-12T13:48:56Z',
    requestedEndDateTime: '2021-01-12T15:48:56Z',
    fulfillmentEndDateTime: '0001-01-01T00:00:00Z',
    fulfillmentStartDateTime: '0001-01-01T00:00:00Z',
    dispatchStatus: 'Completed',
    dispatchStatusId: 15,
    externalOrderNumber: null,
    hospice: {},
    hospiceId: 37,
    hospiceLocationId: 62,
    id: 720,
    netSuiteOrderId: 446975,
    orderDateTime: '2021-01-12T13:38:00Z',
    orderFulfillmentLineItems: [],
    orderLineItems: [orderLineItem],
    orderNotes: [],
    orderNumber: '2222102644',
    orderRecipientUserId: 192,
    orderStatus: 'Completed',
    orderType: 'Delivery',
    orderTypeId: 10,
    originatorUserId: 144,
    patientUuid: '5eb30f32-2abf-4aba-a02f-1b117339f623',
    pickupAddress: null,
    pickupReason: null,
    shippingAddressId: 0,
    siteId: 10,
    statOrder: false,
    statusId: 15,
  };

  const dispatchInstruction = {
    dispatchEndDateTime: '2021-01-12T09:45:00Z',
    dispatchStartDateTime: '2021-01-12T08:45:00Z',
    id: 501,
    orderHeader,
    orderHeaderId: 720,
    sequenceNumber: 1,
    transferRequest: null,
    transferRequestId: null,
    vehicle: {},
    vehicleId: 110,
  };

  const site = {
    address: {},
    capacity: 0,
    currentDriverId: 0,
    currentDriverName: null,
    cvn: '',
    id: 1,
    isActive: false,
    isDisable: false,
    length: 0,
    licensePlate: '',
    locationType: 'Region',
    name: '20 - Central Region',
    netSuiteLocationId: 1,
    parentNetSuiteLocationId: 47,
    siteCode: 0,
    siteId: null,
    siteName: null,
    sitePhoneNumber: [],
    vin: '',
  };

  const dispatchInstructionsResponse = {
    records: [dispatchInstruction],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const orderHeaderResponse = {
    records: [orderHeader],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const sitesResponse = {
    records: [site],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
  const vehicle = {
    locationType: 'Vehicle',
    vehicles: [],
  };

  const patient = {
    createdByUserId: 0,
    createdDateTime: '2020-12-02T18:42:29.1952648Z',
    dateOfBirth: '2021-04-17T00:00:00Z',
    diagnosis: '',
    displayStatus: 'I',
    firstName: 'Carl',
    hms2Id: 744401,
    hospice: 'Allstate - Edinburg, TX',
    hospiceId: 21,
    hospiceLocationId: 29,
    id: 21,
    isInfectious: false,
    lastName: 'Manning',
    lastOrderDateTime: '2021-03-10T18:34:47.4616726Z',
    lastOrderNumber: '1056',
    name: 'Carl Manning',
    patientAddress: [],
    patientHeight: 304.8,
    patientNotes: [],
    patientWeight: 150,
    phoneNumbers: [],
    status: 'Inactive',
    statusChangedDate: '2021-04-21T14:48:28.287Z',
    statusReason: null,
    uniqueId: '6a15f760-2b41-422b-98a6-2d98482f0ca3',
  };

  const patientResponse = {
    records: [patient],
    pageSize: 20,
    pageNumber: 1,
    totalRecordCount: 1,
    totalPageCount: 1,
  };
});
