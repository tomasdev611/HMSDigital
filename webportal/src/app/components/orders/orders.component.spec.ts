import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {OrdersComponent} from './orders.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {RouterTestingModule} from '@angular/router/testing';
import {
  OrderHeadersService,
  HospiceMemberService,
  PatientService,
  HospiceService,
  DispatchService,
  HospiceLocationService,
  ToastService,
  FeedbackService,
} from '../../services';
import {TransferState} from '@angular/platform-browser';
import {ConfirmDialogComponent} from 'src/app/common/confirm-dialog/confirm-dialog.component';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject, of} from 'rxjs';
import {ReactiveFormsModule} from '@angular/forms';
import {MessageService, ConfirmationService} from 'primeng/api';
import {RadioButtonModule} from 'primeng/radiobutton';
import {DropdownModule} from 'primeng/dropdown';
import {DatePipe} from '@angular/common';

describe('OrdersComponent', () => {
  let component: OrdersComponent;
  let fixture: ComponentFixture<OrdersComponent>;
  let orderHeaderService: any;
  let hospiceMemberService: any;
  let patientService: any;
  let dispatchService: any;
  let hospiceLocationService: any;
  let toastService: any;
  let feedbackService: any;

  beforeEach(
    waitForAsync(() => {
      const orderHeaderServiceStub = jasmine.createSpyObj('OrderHeadersService', [
        'getAllOrderHeaders',
        'getOrderFulfillment',
        'getOrderHeaderById',
      ]);
      orderHeaderServiceStub.getAllOrderHeaders.and.returnValue(of(orderResponse));
      orderHeaderServiceStub.getOrderFulfillment.and.returnValue(
        new BehaviorSubject<PaginationResponse>(orderFulfillmentResponse)
      );
      orderHeaderServiceStub.getOrderHeaderById.and.returnValue(new BehaviorSubject(orderRecord));
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', ['getAllhospices']);
      hospiceServiceStub.getAllhospices.and.returnValue(of(hospiceResponse));
      const hospiceMemberServiceStub = jasmine.createSpyObj('HospiceMemberService', [
        'getApproverList',
        'getAllHospiceMembers',
      ]);
      hospiceMemberServiceStub.getApproverList.and.returnValue(of(approverListResponse));
      hospiceMemberServiceStub.getAllHospiceMembers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceMemberServiceResponse)
      );

      const patientServiceStub = jasmine.createSpyObj('PatientService', [
        'getPatients',
        'getPatientNotes',
        'updateStatus',
      ]);
      patientServiceStub.getPatients.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );
      patientServiceStub.getPatientNotes.and.returnValue(new BehaviorSubject(patientNotes));
      patientServiceStub.updateStatus.and.returnValue(new BehaviorSubject(null));

      const dispatchServiceStub = jasmine.createSpyObj('DispatchService', [
        'getDispatchOrderLocations',
      ]);
      dispatchServiceStub.getDispatchOrderLocations.and.returnValue(new BehaviorSubject([]));

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getLocations',
      ]);
      hospiceLocationServiceStub.getLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceLocationServiceResponse)
      );

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      const feedbackServiceStub = jasmine.createSpyObj('FeedbackService', ['submitFeedback']);
      feedbackServiceStub.submitFeedback.and.returnValue(new BehaviorSubject(null));

      TestBed.configureTestingModule({
        declarations: [OrdersComponent, ConfirmDialogComponent],
        imports: [
          HttpClientTestingModule,
          ReactiveFormsModule,
          RouterTestingModule,
          HttpClientTestingModule,
          OAuthModule.forRoot(),
          RadioButtonModule,
          DropdownModule,
        ],
        providers: [
          {
            provide: OAuthService,
          },
          {
            provide: OrderHeadersService,
            useValue: orderHeaderServiceStub,
          },
          {
            provide: HospiceMemberService,
            useValue: hospiceMemberServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          {
            provide: DispatchService,
            useValue: dispatchServiceStub,
          },
          {
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          {
            provide: FeedbackService,
            useValue: feedbackServiceStub,
          },
          TransferState,
          MessageService,
          ConfirmationService,
          DatePipe,
        ],
      }).compileComponents();
      orderHeaderService = TestBed.inject(OrderHeadersService);
      hospiceMemberService = TestBed.inject(HospiceMemberService);
      patientService = TestBed.inject(PatientService);
      dispatchService = TestBed.inject(DispatchService);
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      toastService = TestBed.inject(ToastService);
      feedbackService = TestBed.inject(FeedbackService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('feedbackForm invalid when empty', () => {
    component.setFeedbackForm();
    expect(component.feedbackForm.valid).toBeFalsy();
  });

  it('feedbackForm valid when all required fileds are set', () => {
    component.setFeedbackForm();
    const requiredFormValues = {
      email: 'a@a.com',
      name: 'name',
      subject: 'feedback',
      comments: 'this is comment',
      type: 'Concern',
      hospiceLocation,
    };
    component.feedbackForm.patchValue(requiredFormValues);
    expect(component.feedbackForm.valid).toBeTruthy();
  });

  it('should call `getAllOrderHeaders` on ngOnInit and match the result', () => {
    component.ngOnInit();
    expect(orderHeaderService.getAllOrderHeaders).toHaveBeenCalled();
    expect(hospiceMemberService.getApproverList).toHaveBeenCalled();
    expect(hospiceMemberService.getApproverList).toHaveBeenCalled();
    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.allPatients).toEqual(patientResponse.records);
  });

  it('should call `getDispatchOrderLocations` of dispatchService on getOrdersCurrentLocation', () => {
    component.getOrdersCurrentLocation([1, 2]);
    expect(dispatchService.getDispatchOrderLocations).toHaveBeenCalled();
  });

  it('should call `getLocations` of hospiceLocationService on loadHospiceLocations and match the result', () => {
    const hospiceLocationIds = [201];
    const hospiceLocations = [
      {
        label: hospiceLocation.name,
        value: hospiceLocation.name,
      },
    ];
    component.loadHospiceLocations(hospiceLocationIds);
    expect(hospiceLocationService.getLocations).toHaveBeenCalled();
    expect(component.hospiceLocations).toEqual(hospiceLocations);
  });

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

  const orderResponse = {
    records: [orderRecord],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
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

  const patientResponse = {
    records: [patientRecord],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const approverListResponse = {
    hospiceIds: [],
    hospiceLocationIds: [],
  };

  const orderSelectedEvent = {
    checked: true,
    currentRow: orderRecord,
    selectedRows: orderRecord,
  };

  const hopsiceMember = {
    userId: 192,
  };

  const hospiceMemberServiceResponse = {
    records: [hopsiceMember],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
  const hospiceResponse = {
    records: [],
    pageSize: 0,
    totalRecordCount: 0,
    pageNumber: 0,
    totalPageCount: 0,
  };

  const orderFulfillment = {
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

  const orderFulfillmentResponse = {
    records: [orderFulfillment],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const patientNotes = [
    {
      createdByUserId: 2,
      createdByUserName: 'Todd Loomis',
      createdDateTime: '2020-09-28T19:13:01.7701378Z',
      note: 'Testing',
    },
  ];

  const hospiceLocation = {
    address: [],
    hospiceId: 25,
    id: 38,
    name: 'Amara - Edinburg, TX',
    netSuiteCustomerId: 2065,
    phoneNumber: null,
    site: null,
    siteId: null,
  };

  const hospiceLocationServiceResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
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

  const confirmData = {
    acceptLabel: 'Yes',
    data: {
      patient,
      orderType: 'pickup',
    },
    header: 'Activate Patient',
    icon: 'pi pi-info-circle',
    message: 'Do you want to activate the patient: <strong>Carl Manning</strong>?',
    rejectLabel: 'No',
  };
});
