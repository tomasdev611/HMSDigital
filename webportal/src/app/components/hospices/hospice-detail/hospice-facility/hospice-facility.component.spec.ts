import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {HospiceFacilityComponent} from './hospice-facility.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {HospiceFacilityService, PatientService} from 'src/app/services';
import {OAuthModule} from 'angular-oauth2-oidc';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';
import {CommonModule} from '@angular/common';

describe('HospiceFacilityComponent', () => {
  let component: HospiceFacilityComponent;
  let fixture: ComponentFixture<HospiceFacilityComponent>;
  let facilityService: any;
  let patientService: any;

  beforeEach(
    waitForAsync(() => {
      const facilityServiceStub = jasmine.createSpyObj('HospiceFacilityService', [
        'getAllHospiceFacilities',
        'getPatientsByFacilityId',
      ]);
      facilityServiceStub.getAllHospiceFacilities.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceFacilitiesResponse)
      );
      facilityServiceStub.getPatientsByFacilityId.and.returnValue(
        new BehaviorSubject(patientsByFacilityIdResponse)
      );
      const patientServiceStub = jasmine.createSpyObj('PatientService', ['getPatients']);
      patientServiceStub.getPatients.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );
      TestBed.configureTestingModule({
        declarations: [HospiceFacilityComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: HospiceFacilityService,
            useValue: facilityServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
        ],
      }).compileComponents();
      facilityService = TestBed.inject(HospiceFacilityService);
      patientService = TestBed.inject(PatientService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospiceFacilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllHospiceFacilities` of HospiceFacilityService and match the result', () => {
    component.hospiceId = 1;
    component.getHospiceFacilities();
    expect(facilityService.getAllHospiceFacilities).toHaveBeenCalled();
    expect(component.facilityResponse.records).toEqual(hospiceFacilitiesResponse.records);
    expect(component.facilityResponse.totalRecordCount).toEqual(
      hospiceFacilitiesResponse.totalRecordCount
    );
    expect(component.facilityResponse.pageNumber).toEqual(hospiceFacilitiesResponse.pageNumber);
    expect(component.facilityResponse.pageSize).toEqual(hospiceFacilitiesResponse.pageSize);
    expect(component.facilityResponse.totalPageCount).toEqual(
      hospiceFacilitiesResponse.totalPageCount
    );
  });

  it('should call `getPatientsByFacilityId` of HospiceFacilityService and match the result', () => {
    component.fetchPatients(hospiceFacility);
    expect(facilityService.getPatientsByFacilityId).toHaveBeenCalled();
    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.patients).toEqual(patientResponse.records);
  });
  const hospiceFacility = {
    address: {},
    facilityPhoneNumber: [],
    hospiceId: 1,
    hospiceLocation: {},
    hospiceLocationId: 2,
    id: 42,
    isDisable: false,
    name: 'stifen scott',
    site: null,
    siteId: null,
  };

  const hospiceFacilitiesResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceFacility],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const patientByFacility = {
    active: false,
    assignedDateTime: '2020-10-21T07:40:47.2970851Z',
    facility: {},
    facilityId: 42,
    patientRoomNumber: null,
    patientUuid: 'f969cb0b-aa3b-4992-a7a1-3d6d590c68bd',
  };

  const patientsByFacilityIdResponse = [patientByFacility];

  const patient = {
    createdDateTime: '2020-10-19T12:17:40.3137408Z',
    dateOfBirth: '2020-08-29T00:00:00Z',
    diagnosis: null,
    firstName: 'Tesla',
    hms2Id: null,
    hospiceId: 1,
    hospiceLocationId: 2,
    id: 23,
    isInfectious: false,
    lastName: 'X',
    lastOrderDateTime: null,
    lastOrderNumber: null,
    patientAddress: [],
    patientHeight: 180.34,
    patientNotes: [],
    patientWeight: 180,
    phoneNumbers: [],
    uniqueId: 'f969cb0b-aa3b-4992-a7a1-3d6d590c68bd',
  };

  const patientResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [patient],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
