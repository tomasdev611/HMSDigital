import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {AddEditFacilityComponent} from './add-edit-facility.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {HospiceFacilityService, HospiceLocationService, ToastService} from 'src/app/services';
import {InputMaskModule} from 'primeng/inputmask';
import {DropdownModule} from 'primeng/dropdown';
import {AutoCompleteModule} from 'primeng/autocomplete';
import {TransferState} from '@angular/platform-browser';
import {PaginationResponse} from 'src/app/models';
import {Facility} from 'src/app/models/model.facility';
import {BehaviorSubject} from 'rxjs';

describe('AddEditFacilityComponent', () => {
  let component: AddEditFacilityComponent;
  let fixture: ComponentFixture<AddEditFacilityComponent>;
  let facilityService: any;
  let hospiceLocationService: any;
  let toastService: any;

  beforeEach(
    waitForAsync(() => {
      const facilityServiceStub = jasmine.createSpyObj('HospiceFacilityService', [
        'getHospiceFacilityById',
        'createHospiceFacility',
        'updateHospiceFacility',
      ]);
      facilityServiceStub.getHospiceFacilityById.and.returnValue(
        new BehaviorSubject(hospiceFacilityResponse)
      );
      facilityServiceStub.createHospiceFacility.and.returnValue(new BehaviorSubject(null));
      facilityServiceStub.updateHospiceFacility.and.returnValue(new BehaviorSubject(null));

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getHospiceLocations',
      ]);
      hospiceLocationServiceStub.getHospiceLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceLocationsResponse)
      );

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showError', 'showSuccess']);
      toastServiceStub.showError.and.callThrough();
      toastServiceStub.showSuccess.and.callThrough();

      TestBed.configureTestingModule({
        declarations: [AddEditFacilityComponent],
        imports: [
          RouterTestingModule.withRoutes([{path: 'hospice/:hospiceId', redirectTo: ''}]),
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
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          MessageService,
          OAuthService,
          TransferState,
        ],
      }).compileComponents();
      facilityService = TestBed.inject(HospiceFacilityService);
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      toastService = TestBed.inject(ToastService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditFacilityComponent);
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
    component.facilityForm.patchValue({
      name: 'test_name',
      phoneNumber: '12345678',
      hospiceLocation,
      address: {
        addressLine1: 'Test Address',
        country: 'United States of America',
        state: 'Texas',
        city: 'McAllen',
        zipCode: '78504',
      },
    });
    expect(component.facilityForm.valid).toBeTruthy();
  });

  it('should call `getHospiceFacilityById` of HospiceFacilityService and match the result', () => {
    component.hospiceId = 1;
    component.facilityId = 42;
    component.getFacilityDetail();
    expect(facilityService.getHospiceFacilityById).toHaveBeenCalled();
    expect(component.facility).toEqual(hospiceFacilityResponse);
  });

  it('should call `getHospiceLocations` of HospiceLocationService and match the result', () => {
    component.hospiceId = 1;
    component.getHospiceLocations();
    expect(hospiceLocationService.getHospiceLocations).toHaveBeenCalled();
    expect(component.hospiceLocations).toEqual(hospiceLocationsResponse.records);
  });

  it('should call `createHospiceFacility` of FacilityService on saveFacility and match the result', () => {
    component.hospiceId = 1;
    const address = {
      addressLine1: 'Addressline1',
      addressLine2: 'Addressline2',
      addressLine3: '',
      city: 'Texarkana',
      country: 'United States of America',
      county: null,
      id: null,
      latitude: 0,
      longitude: 0,
      plus4Code: 0,
      state: 'AK',
      zipCode: 12345,
    };
    const phoneNumber = {
      contactName: null,
      countryCode: 1,
      isPrimary: true,
      isSelfPhone: false,
      isVerified: true,
      number: 1234567,
      numberType: null,
      numberTypeId: null,
      receiveEtaTextmessage: false,
      receiveSurveyTextMessage: false,
    };
    const facilityValue = {
      address,
      facilityPhoneNumber: [
        {
          phoneNumber,
        },
      ],
      hospiceId: '1',
      hospiceLocationId: 1,
      id: null,
      isDisable: null,
      name: 'Facility Name',
      siteId: null,
    };
    component.saveFacility(facilityValue);
    expect(facilityService.createHospiceFacility).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  it('should call `updateHospiceFacility` of FacilityService on updateFacility and match the result', () => {
    component.hospiceId = 1;
    const address = {
      addressLine1: '301 W Expressway 83',
      addressLine2: '',
      addressLine3: '',
      addressUuid: '2b397574-774f-4f1b-90cb-ed44f67fa5cc',
      city: 'McAllen',
      country: 'United States of America',
      county: 'Hidalgo',
      id: 863,
      isVerified: true,
      latitude: 26.186591,
      longitude: -98.225911,
      plus4Code: 3045,
      state: 'TX',
      verifiedBy: 'Melissa',
      zipCode: 78503,
    };
    const phoneNumber = {
      contactName: null,
      countryCode: 1,
      isPrimary: true,
      isSelfPhone: false,
      isVerified: true,
      number: 9566324880,
      numberType: null,
      numberTypeId: null,
      receiveEtaTextmessage: false,
      receiveSurveyTextMessage: false,
    };
    const updatedFacility: Facility = {
      address,
      facilityPhoneNumber: [
        {
          phoneNumber,
        },
      ],
      hospiceId: 21,
      hospiceLocationId: 29,
      id: 1,
      isDisable: false,
      name: 'Solara Hospital of McAllen',
      site: null,
    };
    component.facility = updatedFacility;
    component.facilityId = 1;
    component.updateFacility(updatedFacility);
    expect(facilityService.updateHospiceFacility).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
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

  const hospiceLocationsResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
