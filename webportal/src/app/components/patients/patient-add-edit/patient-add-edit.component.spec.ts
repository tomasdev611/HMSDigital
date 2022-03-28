import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {PatientAddEditComponent} from './patient-add-edit.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {
  PatientService,
  HospiceService,
  HospiceLocationService,
  HospiceFacilityService,
  AddressVerificationService,
  ToastService,
  OrderHeadersService,
} from 'src/app/services';
import {MessageService} from 'primeng/api';
import {DropdownModule} from 'primeng/dropdown';
import {CalendarModule} from 'primeng/calendar';
import {InputNumberModule} from 'primeng/inputnumber';
import {InputMaskModule} from 'primeng/inputmask';
import {InputSwitchModule} from 'primeng/inputswitch';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {ChipsModule} from 'primeng/chips';
import {RadioButtonModule} from 'primeng/radiobutton';
import {CheckboxModule} from 'primeng/checkbox';
import {TransferState} from '@angular/platform-browser';
import {BehaviorSubject, of} from 'rxjs';
import {PaginationResponse} from 'src/app/models';
import {AddressVerificationModalComponent} from 'src/app/common';

describe('PatientAddEditComponent', () => {
  let component: PatientAddEditComponent;
  let fixture: ComponentFixture<PatientAddEditComponent>;
  let hospiceService: any;
  let hospiceLocationService: any;
  let hospiceFacilityService: any;
  let patientService: any;
  let addressVerificationService: any;
  let toastService: any;
  let orderHeaderService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', ['getAllhospices']);
      hospiceServiceStub.getAllhospices.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospicesResponse)
      );

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getLocations',
      ]);
      hospiceLocationServiceStub.getLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospicesLocationResponse)
      );

      const hospiceFacilityServiceStub = jasmine.createSpyObj('HospiceFacilityService', [
        'getAllHospiceFacilities',
        'getHospicePatientsFacilities',
      ]);
      hospiceFacilityServiceStub.getAllHospiceFacilities.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospicesFacilitiesResponse)
      );
      hospiceFacilityServiceStub.getHospicePatientsFacilities.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospicePatientFacilitiesResponse)
      );

      const patientServiceStub = jasmine.createSpyObj('PatientService', [
        'getPatientById',
        'getPatientNotes',
        'createPatient',
        'updatePatient',
      ]);
      patientServiceStub.getPatientById.and.returnValue(of(patient));
      patientServiceStub.getPatientNotes.and.returnValue(of(patientNotesResponse));
      patientServiceStub.createPatient.and.returnValue(of(patient));
      patientServiceStub.updatePatient.and.returnValue(of(patient));

      const addressVerificationServiceStub = jasmine.createSpyObj('AddressVerificationService', [
        'verifyAddress',
      ]);
      addressVerificationServiceStub.verifyAddress.and.returnValue(of(addressVerificationResponse));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      const orderHeaderServiceStub = jasmine.createSpyObj('OrderHeaderService', [
        'getAllOrderHeaders',
        'getOrderHeaderById',
      ]);
      orderHeaderServiceStub.getAllOrderHeaders.and.returnValue(
        new BehaviorSubject<PaginationResponse>(orderResponse)
      );
      orderHeaderServiceStub.getOrderHeaderById.and.returnValue(new BehaviorSubject(orderRecord));

      TestBed.configureTestingModule({
        declarations: [PatientAddEditComponent, AddressVerificationModalComponent],
        imports: [
          RouterTestingModule.withRoutes([{path: 'patients', redirectTo: ''}]),
          HttpClientTestingModule,
          ReactiveFormsModule,
          FormsModule,
          OAuthModule.forRoot(),
          DropdownModule,
          CalendarModule,
          InputNumberModule,
          InputMaskModule,
          InputSwitchModule,
          AutoCompleteModule,
          RadioButtonModule,
          CheckboxModule,
          ChipsModule,
        ],
        providers: [
          {
            provide: PatientService,
            useValue: patientServiceStub,
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
            provide: HospiceFacilityService,
            useValue: hospiceFacilityServiceStub,
          },
          {
            provide: AddressVerificationService,
            useValue: addressVerificationServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          {
            provide: OrderHeadersService,
            useValue: orderHeaderServiceStub,
          },
          MessageService,
          TransferState,
          OAuthService,
        ],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      hospiceFacilityService = TestBed.inject(HospiceFacilityService);
      patientService = TestBed.inject(PatientService);
      addressVerificationService = TestBed.inject(AddressVerificationService);
      toastService = TestBed.inject(ToastService);
      orderHeaderService = TestBed.inject(OrderHeadersService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'updateAddressValidity').and.callThrough();
    spyOn(component, 'refreshPatientInfo').and.callThrough();
    spyOn(component, 'fetchHospices').and.callThrough();
    spyOn(component, 'assignPatientsFacility').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('patientForm invalid when empty', () => {
    component.setPatientForm();
    expect(component.patientForm.valid).toBeFalsy();
  });

  it('patientForm valid when all required fileds are set', () => {
    component.setPatientForm();
    component.addAddressFormInstance();
    const requiredFormValues = {
      firstName: 'first_name',
      lastName: 'lastName',
      hospiceId: patient.hospiceId,
      dateOfBirth: new Date(),
      patientHeightFeet: 1,
      patientHeightInch: 1,
      patientWeight: 1,
      hospiceLocationId: patient.hospiceLocationId,
      patientAddress: [
        {
          address,
          addressTypeId: 1,
        },
      ],
      phoneNumbers: [phoneNumber],
    };
    component.patientForm.patchValue(requiredFormValues);
    expect(component.patientForm.valid).toBeTruthy();
  });

  it('should call `getAllhospices` method of HospiceService on fetchHospices', () => {
    component.fetchHospices();
    expect(hospiceService.getAllhospices).toHaveBeenCalled();
  });

  it('should call `getLocations` method of HospiceLocationService on getHospiceLocations and match the result', () => {
    component.getHospiceLocations();
    const location = {
      hospiceId: hospiceLocation.hospiceId,
      label: hospiceLocation.name,
      value: hospiceLocation.id,
    };
    expect(hospiceLocationService.getLocations).toHaveBeenCalled();
    expect(component.hospiceLocations).toEqual([location]);
  });

  it('should call `getAllHospiceFacilities` method of HospiceFacilityService on getFacilities and match the result', () => {
    const facility = {
      address,
      label: hospiceFacility.name,
      phoneNumber: hospiceFacility.facilityPhoneNumber[0].phoneNumber.number,
      value: hospiceFacility.id,
    };
    component.getFacilities(hospiceFacility.hospiceId);
    expect(hospiceFacilityService.getAllHospiceFacilities).toHaveBeenCalled();
    expect(component.hospiceFacilities).toEqual([facility]);
  });

  it('should call `getHospicePatientsFacilities` method of HospiceFacilityService on getPatientsFacilities and match the result', () => {
    component.facilityLocationId = 2;
    const patientFacilityAddress = {
      address,
      addressTypeId: 2,
      facilityId: 206,
      patientRoomNumber: null,
      phoneNumber: patientFacility.facility.facilityPhoneNumber[0].phoneNumber.number,
    };
    component.patient = patient;
    component.getPatientsFacilities(hospiceFacility.hospiceId);
    expect(hospiceFacilityService.getHospicePatientsFacilities).toHaveBeenCalled();
    expect(component.patientFacilityAddress).toEqual([patientFacilityAddress]);
    expect(component.patientFacilityAddressBackUp).toEqual([patientFacilityAddress]);
    expect(component.updateAddressValidity).toHaveBeenCalled();
  });

  it('should call PatientService on getPatientDetail and match the result', () => {
    component.patientId = patient.id;
    component.getPatientDetail();
    expect(patientService.getPatientById).toHaveBeenCalled();
    expect(patientService.getPatientNotes).toHaveBeenCalled();
    expect(component.patientStatus.status).toEqual(patient.status);
    expect(component.refreshPatientInfo).toHaveBeenCalled();
    expect(component.fetchHospices).toHaveBeenCalled();
  });

  it('should call addressVerificationService on checkAddressValidity and match the result', () => {
    component.setPatientForm();
    component.addAddressFormInstance();
    const requiredFormValues = {
      firstName: 'first_name',
      lastName: 'lastName',
      hospiceId: patient.hospiceId,
      dateOfBirth: new Date(),
      patientHeightFeet: 1,
      patientHeightInch: 1,
      patientWeight: 1,
      hospiceLocationId: patient.hospiceLocationId,
      patientAddress: [
        {
          address,
          addressTypeId: 1,
        },
      ],
      phoneNumbers: [phoneNumber],
    };
    component.patientForm.patchValue(requiredFormValues);
    component.checkAddressValidity();
    expect(addressVerificationService.verifyAddress).toHaveBeenCalled();
  });

  it('should call `createPatient` of PatientService on savePatient and match the result', () => {
    const formValue = {
      dateOfBirth: '04/14/2021',
      diagnosis: '1',
      firstName: 'first',
      hospiceId: 2,
      hospiceLocationId: 2,
      isInfectious: false,
      lastName: 'last',
      patientAddress: [
        {
          address,
          addressTypeId: 1,
        },
      ],
      patientHeight: 1,
      patientNotes: [],
      patientWeight: '1',
      phoneNumbers: [phoneNumber],
    };
    component.savePatient(formValue);
    expect(patientService.createPatient).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call `updatePatient` of PatientService on updatePatient and match the result', () => {
    const patchItem = {
      op: 'replace',
      path: '/patientHeight',
      value: 307.34000000000003,
    };
    component.updatePatient(patient.id, [patchItem]);
    expect(patientService.updatePatient).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
    expect(component.assignPatientsFacility).toHaveBeenCalled();
  });

  it('should call `getAllOrderHeaders` of orderHeaderService on getAllOrderHeaders and match the result', () => {
    component.patient = patient;
    component.getAllOrderHeaders();
    expect(orderHeaderService.getAllOrderHeaders).toHaveBeenCalled();
    expect(component.orderHeadersResponse).toEqual(orderResponse);
  });

  it('should call orderHeaderService and PatientService on getOrderDetailInformation', () => {
    component.currentOrder = orderRecord;
    component.getOrderDetailInformation();
    expect(orderHeaderService.getOrderHeaderById).toHaveBeenCalled();
    expect(patientService.getPatientNotes).toHaveBeenCalled();
  });

  const address = {
    addressLine1: '303 N Mockingbird Ave',
    addressLine2: null,
    addressUuid: 'ce3aea79-cc59-4074-8040-3e6a9a8db405',
    city: 'Mission',
    country: 'United States of America',
    county: 'Hidalgo',
    id: 1068,
    isValid: false,
    isVerified: true,
    latitude: 26.23151,
    longitude: -98.39076,
    plus4Code: 4731,
    state: 'TX',
    zipCode: 78572,
    results: null,
    verifiedBy: 'Melissa',
  };

  const phoneNumber = {
    contactName: null,
    countryCode: 1,
    isPrimary: false,
    isSelfPhone: false,
    number: 5551003975,
    numberTypeId: 2,
    isVerified: true,
    numberType: null,
    receiveEtaTextmessage: false,
    receiveSurveyTextMessage: false,
  };

  const hospiceLocation = {
    address: [],
    hospiceId: 1,
    id: 1,
    name: 'Rejoice - Richardson, TX',
    netSuiteCustomerId: 2355,
    phoneNumber: null,
    site: null,
    siteId: null,
  };

  const hospice = {
    address: null,
    creditHoldByUserId: 215,
    creditHoldByUserName: null,
    creditHoldDateTime: '2021-04-08T11:45:17.8877004Z',
    creditHoldNote: 'test hold 17:30',
    hospiceLocations: [hospiceLocation],
    id: 1,
    isCreditOnHold: true,
    name: 'Rejoice Hospice, Inc',
    netSuiteCustomerId: 1828,
    phoneNumber: null,
  };

  const hospicesResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 335,
    records: [hospice],
    totalPageCount: 1,
    totalRecordCount: 335,
  };

  const hospicesLocationResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const hospiceFacility = {
    address,
    facilityPhoneNumber: [
      {
        phoneNumber,
      },
    ],
    hospiceId: 21,
    hospiceLocation,
    hospiceLocationId: 29,
    id: 1,
    isDisable: false,
    name: 'Solara Hospital of McAllen',
    site: null,
    siteId: null,
  };

  const hospicesFacilitiesResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [hospiceFacility],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const patientFacility = {
    facility: {
      address,
      facilityPhoneNumber: [
        {
          phoneNumber,
        },
      ],
      hospiceId: 219,
      hospiceLocation: null,
      hospiceLocationId: 433,
      id: 206,
      isDisable: false,
      name: 'John Knox Village',
      site: null,
      siteId: null,
    },
    facilityId: 206,
    patientRoomNumber: null,
    patientUuid: 'd7135da3-35ea-4bd1-9f9e-6f04f3f89079',
  };

  const hospicePatientFacilitiesResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [patientFacility],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const patientNote = {
    createdByUserId: 218,
    createdByUserName: 'User Name',
    createdDateTime: '2021-04-29T00:22:07.1516087Z',
    note: 'asdf',
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
    patientAddress: [
      {
        address,
      },
    ],
    patientHeight: 304.8,
    patientNotes: [patientNote],
    patientWeight: 150,
    phoneNumbers: [phoneNumber],
    status: 'Inactive',
    statusChangedDate: '2021-04-21T14:48:28.287Z',
    statusReason: null,
    uniqueId: '6a15f760-2b41-422b-98a6-2d98482f0ca3',
  };

  const patientNotesResponse = [patientNote];

  const addressVerificationResponse = [address];

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
    patient,
    patientNotes: [patientNote],
    patientUuid: '95bbf7c7-4303-435d-9671-853efbd08c3d',
    pickupAddress: {},
    site: {},
    shippingAddressId: 0,
    siteId: 8,
    statOrder: false,
    statusId: 19,
  };

  const orderResponse = {
    pageSize: 1,
    records: [orderRecord],
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
