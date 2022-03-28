import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {SitesComponent} from './sites.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {SitesService} from 'src/app/services';
import {MessageService, ConfirmationService} from 'primeng/api';
import {TransferState} from '@angular/platform-browser';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('SitesComponent', () => {
  let component: SitesComponent;
  let fixture: ComponentFixture<SitesComponent>;
  let siteSerivce: any;

  beforeEach(
    waitForAsync(() => {
      const siteSerivceStub = jasmine.createSpyObj('SitesService', ['searchSites']);
      siteSerivceStub.searchSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(siteResponse)
      );
      TestBed.configureTestingModule({
        declarations: [SitesComponent],
        imports: [
          RouterTestingModule.withRoutes([{path: 'sites/info/:siteId', redirectTo: ''}]),
          HttpClientTestingModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: SitesService,
            useValue: siteSerivceStub,
          },
          MessageService,
          ConfirmationService,
          OAuthService,
          TransferState,
        ],
      }).compileComponents();
      siteSerivce = TestBed.inject(SitesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SitesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `searchSites` of SiteService on getSites and match the result', () => {
    component.sitesRequest = new SieveRequest();
    component.getSites();
    expect(siteSerivce.searchSites).toHaveBeenCalled();
    expect(component.sitesResponse).toEqual(siteResponse);
  });

  const site = {
    address: {},
    capacity: 0,
    currentDriverId: 0,
    currentDriverName: null,
    cvn: '',
    id: 1,
    isActive: false,
    isDisable: false,
    length: 0,
    licensePlate: '',
    locationType: 'Site',
    name: 'Austin (South), TX',
    netSuiteLocationId: 15,
    parentNetSuiteLocationId: 8,
    siteCode: 2240,
    siteId: null,
    siteName: null,
    sitePhoneNumber: [],
    vehicles: null,
    vin: '',
  };

  const siteResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [site],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
