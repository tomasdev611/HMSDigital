import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {PatientsComponent} from './patients.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {InventoryService, PatientService, ToastService, UserService} from 'src/app/services';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService, ConfirmationService} from 'primeng/api';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse, Hospice, SieveRequest} from 'src/app/models';
import {TableVirtualScrollComponent, ConfirmDialogComponent} from 'src/app/common';
import {DatePipe} from '@angular/common';
import {PhonePipe} from 'src/app/pipes';
import {CreateOrderModalComponent} from 'src/app/common/create-order-modal/create-order-modal.component';

describe('PatientsComponent', () => {
  let component: PatientsComponent;
  let fixture: ComponentFixture<PatientsComponent>;
  let patientService: any;
  let toastService: any;
  let inventoryService: any;
  let userService: any;

  beforeEach(
    waitForAsync(() => {
      const patientServiceStub = jasmine.createSpyObj('PatientService', [
        'getPatients',
        'searchPatientsBySearchQuery',
        'getPatientNotes',
      ]);
      patientServiceStub.getPatients.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );
      patientServiceStub.searchPatientsBySearchQuery.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );
      patientServiceStub.getPatientNotes.and.returnValue(new BehaviorSubject([patientNote]));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      const inventoryServiceStub = jasmine.createSpyObj('InventoryService', [
        'getPatientInventoryByUuid',
      ]);
      inventoryServiceStub.getPatientInventoryByUuid.and.returnValue(
        new BehaviorSubject<PaginationResponse>(inventoryResponse)
      );

      const userServiceStub = jasmine.createSpyObj('UserService', ['getUserById']);
      userServiceStub.getUserById.and.returnValue(new BehaviorSubject(user));

      TestBed.configureTestingModule({
        declarations: [
          PatientsComponent,
          TableVirtualScrollComponent,
          ConfirmDialogComponent,
          CreateOrderModalComponent,
        ],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          {
            provide: InventoryService,
            useValue: inventoryServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          {
            provide: UserService,
            useValue: userServiceStub,
          },
          OAuthService,
          MessageService,
          ConfirmationService,
          DatePipe,
          PhonePipe,
        ],
      }).compileComponents();
      patientService = TestBed.inject(PatientService);
      toastService = TestBed.inject(ToastService);
      inventoryService = TestBed.inject(InventoryService);
      userService = TestBed.inject(UserService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getPatients').and.callThrough();
    spyOn(component.patientsTable, 'reset').and.returnValue(null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getPatients` method on ngOnInit ', () => {
    component.ngOnInit();
    expect(component.getPatients).toHaveBeenCalled();
  });

  it('should call `getPatients and match result` method of patientService on getPatients ', () => {
    component.getPatients();
    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.patientResponse).toEqual(patientResponse);
  });

  it('should call `getPatients` method on nextPatients ', () => {
    component.patientResponse = {
      pageNumber: 1,
      totalPageCount: 3,
      totalRecordCount: 1,
      records: [],
      pageSize: 20,
    };
    component.nextPatients({pageNum: 1});
    expect(component.getPatients).toHaveBeenCalled();
  });

  it('should format patients with hospice and patient name on method on formatPatientResponse', () => {
    component.formatPatientResponse(hospiceResponse);
    expect(component.patientResponse.records[0].name).toEqual(
      `${patient.firstName} ${patient.lastName || ''}`
    );
  });

  it('should call `searchPatientsBySearchQuery` method on searchPatient ', () => {
    component.patientRequest = new SieveRequest();
    component.searchPatient('query');
    expect(patientService.searchPatientsBySearchQuery).toHaveBeenCalled();
    expect(component.patientResponse).toEqual(patientResponse);
  });

  it('should call `getPatients` method on filterPatients ', () => {
    component.filterPatients();
    expect(component.getPatients).toHaveBeenCalled();
  });

  it('should call `getUserById` of UserService on getMyCreatorName', () => {
    component.currentPatientDetails = patient;
    component.getMyCreatorName();
    expect(userService.getUserById).toHaveBeenCalled();
    expect(component.currentPatientDetails.creatorName).toEqual(
      `${user.firstName} ${user.lastName}`
    );
  });

  it('should call `getPatientNotes` of patientService on getNoteCreaterName', () => {
    component.currentPatientDetails = patient;
    component.getNoteCreaterName();
    expect(patientService.getPatientNotes).toHaveBeenCalled();
    expect(component.currentPatientDetails.patientNotes).toEqual([patientNote]);
  });

  it('should call `getPatientInventoryByUuid` method on getPatientInventory ', () => {
    component.currentPatientDetails = patient;
    component.getPatientInventory();
    expect(inventoryService.getPatientInventoryByUuid).toHaveBeenCalled();
    expect(component.patientInventoryResponse).toEqual(inventoryResponse);
  });

  const patientNote = {
    createdByUserId: 218,
    createdByUserName: 'User Name',
    createdDateTime: '2021-04-28T19:35:08.4486953Z',
    note: 'Patient Note',
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

  const hospices: Hospice[] = [
    {
      id: 1,
      name: 'XYZ Hospice',
      hospiceLocations: [],
    },
  ];

  const patientResponse: PaginationResponse = {
    records: [patient],
    pageSize: 20,
    pageNumber: 1,
    totalRecordCount: 1,
    totalPageCount: 1,
  };

  const inventory = {
    assetTagNumber: '1',
    cogsAccountName: '700T COS - Supplies',
    inventoryId: 1,
    itemDescription: 'CC1003-Wheelchair - Reclining - 18 inch',
    itemId: 1,
    itemName: 'WHEELCHAIR FULLY RECLINING 18',
    itemNumber: '1',
    lotNumber: null,
    netSuiteInventoryId: 1,
    netSuiteItemId: 1,
    netSuiteOrderId: 1,
    netSuiteOrderLineItemId: 1,
    orderHeaderId: 1,
    patientUuid: '1',
    quantity: 1,
    serialNumber: '1',
  };

  const hospiceResponse: PaginationResponse = {
    records: hospices,
    pageSize: 20,
    pageNumber: 1,
    totalRecordCount: 1,
    totalPageCount: 1,
  };

  const inventoryResponse: PaginationResponse = {
    records: [inventory],
    pageSize: 20,
    pageNumber: 1,
    totalRecordCount: 1,
    totalPageCount: 1,
  };

  const confirmData = {
    acceptLabel: 'Yes',
    data: patient,
    header: 'Activate Patient',
    icon: 'pi pi-info-circle',
    message: 'Do you want to activate the patient: <strong>Carl Manning</strong>?',
    rejectLabel: 'No',
  };

  const user = {
    countryCode: 1,
    email: 'dday@hospicesource.net',
    enabled: true,
    firstName: 'DeAna',
    id: 68,
    isEmailVerified: true,
    isPhoneNumberVerified: false,
    lastName: 'Day',
    name: 'DeAna Day',
    phoneNumber: 5553654599,
    profilePictureUrl: null,
    userId: 'cfa0fe4b-3bb2-4daf-aca6-54aee010bbeb',
    userRoles: [],
    userStatus: null,
  };
});
