import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {PatientService, HospiceService} from 'src/app/services';

import {CreateOrderModalComponent} from './create-order-modal.component';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse, SieveRequest} from 'src/app/models';

describe('CreateOrderModalComponent', () => {
  let component: CreateOrderModalComponent;
  let fixture: ComponentFixture<CreateOrderModalComponent>;
  let hospiceService: any;
  let patientService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', [
        'getHospiceById',
        'getAllhospices',
      ]);
      hospiceServiceStub.getHospiceById.and.returnValue(new BehaviorSubject(hospice));
      hospiceServiceStub.getAllhospices.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceResponse)
      );

      const patientServiceStub = jasmine.createSpyObj('PatientService', [
        'searchPatientsBySearchQuery',
        'getPatients',
      ]);
      patientServiceStub.searchPatientsBySearchQuery.and.returnValue(
        new BehaviorSubject(patientResponse)
      );
      patientServiceStub.getPatients.and.returnValue(new BehaviorSubject(patientResponse));

      TestBed.configureTestingModule({
        declarations: [CreateOrderModalComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          MessageService,
        ],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
      patientService = TestBed.inject(PatientService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateOrderModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'searchPatients').and.callThrough();
    spyOn(component, 'subscribePatients').and.callThrough();
    spyOn(component, 'getPatients').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `searchPatients` on showOrdering when patientContext is null', () => {
    component.patientContext = null;
    component.showOrdering();
    expect(component.searchPatients).toHaveBeenCalled();
  });

  it('should call `getHospiceById` of HospiceService on showOrdering when patientContext is set', () => {
    component.patientContext = patient;
    component.showOrdering();
    expect(hospiceService.getHospiceById).toHaveBeenCalled();
  });

  it('should call `searchPatientsBySearchQuery` of PatientService on searchPatients when searchQuery is set', () => {
    component.patientReq = new SieveRequest();
    component.searchPatients('query');
    expect(patientService.searchPatientsBySearchQuery).toHaveBeenCalled();
    expect(component.subscribePatients).toHaveBeenCalled();
  });

  it('should call `getPatients` on searchPatients when searchQuery is not set', () => {
    component.patientReq = new SieveRequest();
    component.searchPatients();
    expect(component.getPatients).toHaveBeenCalled();
    expect(component.subscribePatients).toHaveBeenCalled();
  });

  it('should call `getAllhospices` of HospiceService on getHospices', () => {
    component.patientResponse = patientResponse;
    component.patientsList = [
      {
        label: 'Todd Loomis',
        value: {
          ...patient,
        },
      },
    ];
    component.getHospices();
    expect(hospiceService.getAllhospices).toHaveBeenCalled();
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

  const patient = {
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

  const patientResponse: PaginationResponse = {
    records: [patient],
    pageSize: 20,
    pageNumber: 1,
    totalRecordCount: 1,
    totalPageCount: 1,
  };

  const hospiceResponse = {
    records: [hospice],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
