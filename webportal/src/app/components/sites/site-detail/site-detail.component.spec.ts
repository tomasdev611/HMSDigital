import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {SiteDetailComponent} from './site-detail.component';
import {MessageService} from 'primeng/api';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {SitesService} from 'src/app/services';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {InputNumberModule} from 'primeng/inputnumber';
import {InputMaskModule} from 'primeng/inputmask';
import {InputSwitchModule} from 'primeng/inputswitch';
import {TransferState} from '@angular/platform-browser';
import {BehaviorSubject, of} from 'rxjs';

describe('SiteDetailComponent', () => {
  let component: SiteDetailComponent;
  let fixture: ComponentFixture<SiteDetailComponent>;
  let siteService: any;

  beforeEach(
    waitForAsync(() => {
      const siteServiceStub = jasmine.createSpyObj('SitesService', ['getSiteById', 'searchSites']);
      siteServiceStub.getSiteById.and.returnValue(new BehaviorSubject(siteResponse));
      siteServiceStub.searchSites.and.returnValue(new BehaviorSubject(searchSitesResponse));
      TestBed.configureTestingModule({
        declarations: [SiteDetailComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          InputNumberModule,
          InputMaskModule,
          ReactiveFormsModule,
          FormsModule,
          InputSwitchModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: SitesService,
            useValue: siteServiceStub,
          },
          MessageService,
          OAuthService,
          TransferState,
        ],
      }).compileComponents();
      siteService = TestBed.inject(SitesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SiteDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'mapParentSite').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getSiteById` of SiteService on getSiteDetail', () => {
    component.siteId = 3;
    component.getSiteDetail();
    expect(siteService.getSiteById).toHaveBeenCalled();
    expect(component.mapParentSite).toHaveBeenCalled();
  });

  const phoneNumber = {
    countryCode: 1,
    isPrimary: true,
    number: 6232856614,
    numberType: 'Work',
    numberTypeId: 1,
  };

  const address = {
    addressLine1: '3440 Sojourn Dr Ste 150',
    addressLine2: '',
    addressLine3: '""',
    city: 'Carrollton',
    country: 'United States of America',
    county: 'Dallas',
    plus4Code: 2394,
    state: 'TX',
    zipCode: 75006,
  };

  const siteResponse = {
    address,
    id: 3,
    isActive: true,
    isDisable: false,
    locationType: 'Site',
    name: 'Austin (South), TX',
    netSuiteLocationId: 15,
    parentNetSuiteLocationId: 8,
    siteCode: 2240,
    sitePhoneNumber: [
      {
        phoneNumber,
      },
    ],
    capacity: 0,
    currentDriverId: 0,
    currentDriverName: null,
    cvn: '',
    length: 0,
    licensePlate: '',
    siteId: null,
    siteName: null,
    vin: '',
  };

  const searchSitesResponse = {
    pageNumber: 1,
    pageSize: 100,
    records: [],
    totalPageCount: 1,
    totalRecordCount: 100,
  };
});
