import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {HospiceLocationComponent} from './hospice-location.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService, ConfirmationService} from 'primeng/api';
import {HospiceLocationService} from 'src/app/services';
import {BehaviorSubject} from 'rxjs';
import {PaginationResponse} from 'src/app/models';

describe('HospiceLocationComponent', () => {
  let component: HospiceLocationComponent;
  let fixture: ComponentFixture<HospiceLocationComponent>;
  let hospiceLocationService: any;
  beforeEach(
    waitForAsync(() => {
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
            provide: HospiceLocationService,
            useValue: hospiceLocationServiceStub,
          },
          OAuthService,
          MessageService,
          ConfirmationService,
        ],
        declarations: [HospiceLocationComponent],
      }).compileComponents();
      hospiceLocationService = TestBed.inject(HospiceLocationService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospiceLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getHospiceLocations').and.callThrough();
    component.allLocations = hospiceLocations;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getHospiceLocations` method of hospiceLocationService on getHospiceLocations and match the result', () => {
    component.getHospiceLocations();
    expect(hospiceLocationService.getHospiceLocations).toHaveBeenCalled();
    expect(component.allLocations).toEqual(hospiceLocations);
  });
});

const hospiceLocations = [
  {
    id: 1,
    name: 'San DieSan Diego Centre',
    hospiceId: 1,
    siteId: 0,
  },
];

const hospiceLocationResponse = {
  records: hospiceLocations,
  pageSize: 1,
  totalRecordCount: 1,
  pageNumber: 1,
  totalPageCount: 1,
};
