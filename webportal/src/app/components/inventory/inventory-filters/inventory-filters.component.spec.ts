import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {TransferState} from '@angular/platform-browser';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {SitesService} from 'src/app/services';

import {InventoryFiltersComponent} from './inventory-filters.component';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('InventoryFiltersComponent', () => {
  let component: InventoryFiltersComponent;
  let fixture: ComponentFixture<InventoryFiltersComponent>;
  let sitesService: any;

  beforeEach(
    waitForAsync(() => {
      const sitesServiceStub = jasmine.createSpyObj('SitesService', ['searchSites']);
      sitesServiceStub.searchSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(sitesResponse)
      );

      TestBed.configureTestingModule({
        declarations: [InventoryFiltersComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: OAuthService,
          },
          {
            provide: SitesService,
            useValue: sitesServiceStub,
          },
          TransferState,
        ],
      }).compileComponents();
      sitesService = TestBed.inject(SitesService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'buildFilterConfigs').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `searchSites` of SiteService on buildfilterConf', () => {
    component.buildfilterConf();
    expect(sitesService.searchSites).toHaveBeenCalled();
    expect(component.buildFilterConfigs).toHaveBeenCalled();
  });

  const siteRecord = {
    address: {},
    id: 1,
    isDisable: false,
    locationType: null,
    name: '20 - Central Region',
    netSuiteLocationId: 1,
    parentNetSuiteLocationId: 0,
    siteCode: 1111,
    sitePhoneNumber: [],
  };

  const sitesResponse = {
    records: [siteRecord],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };
});
