import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {AddFacilityComponent} from './add-facility.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {InputMaskModule} from 'primeng/inputmask';
import {DropdownModule} from 'primeng/dropdown';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {HospiceFacilityService, HospiceLocationService} from 'src/app/services';
import {MessageService} from 'primeng/api';
import {TransferState} from '@angular/platform-browser';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse} from 'src/app/models';

describe('AddFacilityComponent', () => {
  let component: AddFacilityComponent;
  let fixture: ComponentFixture<AddFacilityComponent>;
  let facilityService: HospiceFacilityService;
  let hospiceLocationService: any;

  beforeEach(
    waitForAsync(() => {
      const facilityServiceStub = jasmine.createSpyObj('HospiceFacilityService', [
        'getHospiceFacilityById',
        'createHospiceFacility',
      ]);
      facilityServiceStub.getHospiceFacilityById.and.returnValue(
        new BehaviorSubject(hospiceFacilityResponse)
      );
      facilityServiceStub.createHospiceFacility.and.returnValue(
        new BehaviorSubject(createHospiceFacilityResponse)
      );

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getHospiceLocations',
      ]);
      hospiceLocationServiceStub.getHospiceLocations.and.returnValue(
        new BehaviorSubject(hospiceLocationsResponse)
      );

      TestBed.configureTestingModule({
        declarations: [AddFacilityComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          InputMaskModule,
          DropdownModule,
          OAuthModule.forRoot(),
          AutoCompleteModule,
        ],
        providers: [
          {
            provide: HospiceFacilityService,
            useValue: facilityServiceStub,
          },
          {
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          MessageService,
          OAuthService,
          TransferState,
        ],
      }).compileComponents();
      facilityService = TestBed.inject(HospiceFacilityService);
      hospiceLocationService = TestBed.inject(HospiceLocationService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AddFacilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('facilityForm form invalid when empty', () => {
    expect(component.facilityForm.valid).toBeFalsy();
  });

  it('facilityForm form valid when all required fileds are set', () => {
    component.facilityForm.controls.name.setValue('Hao Ying');
    component.facilityForm.controls.phoneNumber.setValue(5555555555);
    component.facilityForm.controls.hospiceLocationId.setValue(hospiceLocation.id);
    component.facilityForm.controls.address.patchValue({
      addressLine1: 'Address 1',
      country: 'United States of America',
      state: 'Texas',
      city: 'Houston',
      zipCode: 12345,
    });
    expect(component.facilityForm.valid).toBeTruthy();
  });

  it('should call `getHospiceFacilityById` of FacilityService on getFacilityDetail and match the result', () => {
    component.hospiceId = 1;
    component.facilityId = 42;
    component.getFacilityDetail();
    expect(facilityService.getHospiceFacilityById).toHaveBeenCalled();
    expect(component.facility).toEqual(hospiceFacilityResponse);
  });

  it('should call `createHospiceFacility` of FacilityService on saveFacility', () => {
    component.hospiceId = 1;
    const formValue = {
      address: {},
      facilityPhoneNumber: [],
      hospiceId: 2,
      hospiceLocationId: 3,
      id: null,
      isDisable: null,
      name: 'ASDF',
      siteId: null,
    };
    component.saveFacility(formValue);
    expect(facilityService.createHospiceFacility).toHaveBeenCalled();
  });

  it('should call `getHospiceLocations` of HospiceLocationService on getHospiceLocations and match the result', () => {
    component.hospiceId = 1;
    component.getHospiceLocations();
    expect(hospiceLocationService.getHospiceLocations).toHaveBeenCalled();
    expect(component.hospiceLocation).toEqual(hospiceLocation);
  });

  const hospiceLocation = {
    address: null,
    hospiceId: 1,
    id: 2,
    name: '1st Quality Hospice, LLC : 1st Quality - Woodville, TX',
    netSuiteCustomerId: 2027,
    phoneNumber: null,
    site: null,
    siteId: null,
  };

  const hospiceFacilityResponse = {
    address: {},
    facilityPhoneNumber: [],
    hospiceId: 1,
    hospiceLocationId: 2,
    id: 42,
    isDisable: false,
    name: 'stifen scott',
    site: null,
    siteId: null,
  };

  const createHospiceFacilityResponse = {
    address: {},
    facilityPhoneNumber: [],
    hospiceId: 2,
    hospiceLocation: {},
    hospiceLocationId: 3,
    id: 66,
    isDisable: false,
    name: 'asdf',
    site: null,
    siteId: null,
  };

  const hospiceLocationsResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
