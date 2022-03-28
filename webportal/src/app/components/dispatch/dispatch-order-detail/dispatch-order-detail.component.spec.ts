import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DispatchOrderDetailComponent} from './dispatch-order-detail.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {DispatchService} from 'src/app/services';
import {of} from 'rxjs';

describe('DispatchOrderDetailComponent', () => {
  let component: DispatchOrderDetailComponent;
  let fixture: ComponentFixture<DispatchOrderDetailComponent>;
  let dispatchService: any;

  beforeEach(
    waitForAsync(() => {
      const dispatchServiceStub = jasmine.createSpyObj('DispatchService', [
        'getAllDispatchInstructions',
      ]);
      dispatchServiceStub.getAllDispatchInstructions.and.returnValue(
        of(dispatchInstructionsResponse)
      );

      TestBed.configureTestingModule({
        declarations: [DispatchOrderDetailComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: DispatchService,
            useValue: dispatchServiceStub,
          },
        ],
      }).compileComponents();
      dispatchService = TestBed.inject(DispatchService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchOrderDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllDispatchInstructions` of DispatchService on getDispatchInstruction', () => {
    component.orderId = orderHeader.id;
    component.getDispatchInstruction();
    expect(dispatchService.getAllDispatchInstructions).toHaveBeenCalled();
    expect(component.dispatchInstructions).toEqual(dispatchInstructionsResponse.records);
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

  const dispatchInstructionsResponse = {
    records: [dispatchInstruction],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
