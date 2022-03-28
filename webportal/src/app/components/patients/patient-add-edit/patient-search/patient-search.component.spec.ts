import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {PatientSearchComponent} from './patient-search.component';
import {ReactiveFormsModule} from '@angular/forms';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {PatientService} from 'src/app/services';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {CalendarModule} from 'primeng/calendar';
import {DropdownModule} from 'primeng/dropdown';
import {BehaviorSubject} from 'rxjs';

describe('PatientSearchComponent', () => {
  let component: PatientSearchComponent;
  let fixture: ComponentFixture<PatientSearchComponent>;
  let patientService: any;

  beforeEach(
    waitForAsync(() => {
      const patientServiceStub = jasmine.createSpyObj('PatientService', ['searchPatient']);
      patientServiceStub.searchPatient.and.returnValue(new BehaviorSubject(patientSearchResponse));
      TestBed.configureTestingModule({
        declarations: [PatientSearchComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          CalendarModule,
          DropdownModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          OAuthService,
          MessageService,
        ],
      }).compileComponents();
      patientService = TestBed.inject(PatientService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `searchPatient` of PatientService on patientSearch', () => {
    const patientValue = {
      address: {
        addressLine1: null,
        addressLine2: null,
        city: null,
        state: null,
        zipCode: 0,
      },
      addressType: 'Home',
      dateOfBirth: null,
      firstName: 'Tem',
      hospiceId: null,
      lastName: null,
    };
    component.patientSearch(patientValue);
    expect(patientService.searchPatient).toHaveBeenCalled();
  });

  const patient = {
    createdDateTime: '2021-01-29T15:46:59.6624779Z',
    dateOfBirth: '1944-03-19T00:00:00Z',
    diagnosis: '',
    firstName: 'SHANE',
    hms2Id: 1177683,
    hospiceId: 213,
    hospiceLocationId: 390,
    id: 6134,
    isInfectious: false,
    lastName: 'BECK',
    lastOrderDateTime: null,
    lastOrderNumber: null,
    patientAddress: [],
    patientHeight: 1,
    patientNotes: [],
    patientWeight: 150,
    phoneNumbers: [],
    status: null,
    statusChangedDate: null,
    uniqueId: 'f8bb99d6-0453-4a25-805d-386ab104e81c',
  };

  const patientSearchResponse = [patient];
});
