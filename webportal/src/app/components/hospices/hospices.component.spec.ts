import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {HospicesComponent} from './hospices.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {HospiceService} from 'src/app/services';
import {MessageService} from 'primeng/api';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('HospicesComponent', () => {
  let component: HospicesComponent;
  let fixture: ComponentFixture<HospicesComponent>;
  let hospiceService: any;
  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', ['getAllhospices']);
      hospiceServiceStub.getAllhospices.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceResponse)
      );
      TestBed.configureTestingModule({
        declarations: [HospicesComponent],
        imports: [
          RouterTestingModule.withRoutes([{path: 'hospice/:hospiceId', redirectTo: ''}]),
          HttpClientTestingModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          OAuthService,
          MessageService,
        ],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getAllHospices').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllHospices` method on ngOnInit ', () => {
    component.ngOnInit();
    expect(component.getAllHospices).toHaveBeenCalled();
  });

  it('should call `getAllhospices` method of HospiceService on getAllHospices', () => {
    component.getAllHospices();
    expect(hospiceService.getAllhospices).toHaveBeenCalled();
  });

  it('should call `getAllhospices` method of HospiceService on getAllHospices and match the result', () => {
    component.getAllHospices();
    expect(hospiceService.getAllhospices).toHaveBeenCalled();
    expect(component.hospiceResponse.records).toEqual(hospice);
  });

  const hospice = [
    {
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
    },
  ];
  const hospiceResponse = {
    records: hospice,
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
