import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {ReactiveFormsModule} from '@angular/forms';
import {HospiceLocationService, HospiceService, PatientService} from 'src/app/services';
import {PatientsFiltersComponent} from './patients-filters.component';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('PatientsFiltersComponent', () => {
  let component: PatientsFiltersComponent;
  let fixture: ComponentFixture<PatientsFiltersComponent>;
  let hospiceService: any;
  let hospiceLocationService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', [
        'getAllhospices',
        'searchHospices',
      ]);
      hospiceServiceStub.getAllhospices.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceResponse)
      );
      hospiceServiceStub.searchHospices.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceResponse)
      );

      const hospiceLocationServiceStub = jasmine.createSpyObj('HospiceLocationService', [
        'getHospiceLocations',
      ]);
      hospiceLocationServiceStub.getHospiceLocations.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceLocationResponse)
      );

      TestBed.configureTestingModule({
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          {
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          OAuthService,
        ],
        declarations: [PatientsFiltersComponent],
      }).compileComponents();
      hospiceLocationService = TestBed.inject(HospiceLocationService);
      hospiceService = TestBed.inject(HospiceService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientsFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'buildLocationFilter').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllhospices` of HospiceService on getHospices', () => {
    component.getHospices();
    expect(hospiceService.getAllhospices).toHaveBeenCalled();
  });

  it('should call `searchHospices` of HospiceService on searchField when query is not empty', () => {
    component.filterConfigs = filterConfigs;
    component.searchField({
      query: 'test',
      label: 'Hospice',
    });
    expect(hospiceService.searchHospices).toHaveBeenCalled();
  });

  it('should call `getHospiceLocations` of HospiceLocationService on getHospiceLocations', () => {
    component.buildfilterConf();
    component.getHospiceLocations(1);
    expect(hospiceLocationService.getHospiceLocations).toHaveBeenCalled();
    expect(component.buildLocationFilter).toHaveBeenCalled();
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

  const hospiceResponse: PaginationResponse = {
    records: [hospice],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const hospiceLocationResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 1,
    records: [hospiceLocation],
    totalPageCount: 1,
    totalRecordCount: 1,
  };

  const filterConfigs = [
    {
      field: 'hospiceId',
      label: 'Hospice',
      operator: '==',
      value: [],
      type: 1,
    },
  ];
});
