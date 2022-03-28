import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DispatchComponent} from './dispatch.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {OrderHeadersService, PatientService} from 'src/app/services';
import {MessageService} from 'primeng/api';
import {TransferState} from '@angular/platform-browser';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('DispatchComponent', () => {
  let component: DispatchComponent;
  let fixture: ComponentFixture<DispatchComponent>;
  let orderHeaderService: any;
  let patientService: any;

  beforeEach(
    waitForAsync(() => {
      const orderHeaderServiceStub = jasmine.createSpyObj('OrderHeadersService', [
        'getAllOrderHeaders',
      ]);
      orderHeaderServiceStub.getAllOrderHeaders.and.returnValue(
        new BehaviorSubject<PaginationResponse>(orderHeaderResponse)
      );

      const patientServiceStub = jasmine.createSpyObj('PatientService', ['getPatients']);
      patientServiceStub.getPatients.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );

      TestBed.configureTestingModule({
        declarations: [DispatchComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: OrderHeadersService,
            useValue: orderHeaderServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          OAuthService,
          MessageService,
          TransferState,
        ],
      }).compileComponents();
      orderHeaderService = TestBed.inject(OrderHeadersService);
      patientService = TestBed.inject(PatientService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'loadAdditionDispatchData').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllOrderHeaders` of orderHeaderService on searchOrders and match the result', () => {
    component.orderRequest = new SieveRequest();
    component.searchOrders(true);
    expect(orderHeaderService.getAllOrderHeaders).toHaveBeenCalled();
    expect(component.orderResponse).toEqual(orderHeaderResponse);
    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.loadAdditionDispatchData).toHaveBeenCalled();
  });

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

  const orderHeaderResponse = {
    records: [orderHeader],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
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
