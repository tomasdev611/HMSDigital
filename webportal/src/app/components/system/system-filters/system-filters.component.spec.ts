import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {SystemFiltersComponent} from './system-filters.component';
import {HospiceLocationService, PatientService} from 'src/app/services';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('SystemFiltersComponent', () => {
  let component: SystemFiltersComponent;
  let fixture: ComponentFixture<SystemFiltersComponent>;
  let hospiceLocationService: any;
  let patientService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getLocations',
      ]);

      hospiceLocationServiceStub.getLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hopspiceLocationResponse)
      );

      const patientServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'searchPatientsBySearchQuery',
        'getPatients',
      ]);

      patientServiceStub.searchPatientsBySearchQuery.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );
      patientServiceStub.getPatients.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );

      TestBed.configureTestingModule({
        declarations: [SystemFiltersComponent],
        imports: [HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          {
            provide: OAuthService,
          },
        ],
      }).compileComponents();
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      patientService = TestBed.inject(PatientService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getPatients` on getPatients', () => {
    component.getPatients();
    expect(patientService.getPatients).toHaveBeenCalled();
  });

  it('should call `searchPatientsBySearchQuery` on searchField', () => {
    component.searchField({
      label: 'Patient',
      query: 'query',
    });
    expect(patientService.searchPatientsBySearchQuery).toHaveBeenCalled();
  });

  it('should call `getLocations` on getHospiceLocations', () => {
    component.getHospiceLocations();
    expect(hospiceLocationService.getLocations).toHaveBeenCalled();
  });

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
});
