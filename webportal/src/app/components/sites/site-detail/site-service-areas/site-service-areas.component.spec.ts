import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {TransferState} from '@angular/platform-browser';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {InputMaskModule} from 'primeng/inputmask';
import {MessageService} from 'primeng/api';
import {SitesService, ToastService} from 'src/app/services';
import {SiteServiceAreasComponent} from './site-service-areas.component';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('SiteServiceAreasComponent', () => {
  let component: SiteServiceAreasComponent;
  let fixture: ComponentFixture<SiteServiceAreasComponent>;
  let siteSerivce: any;
  let toastService: any;

  beforeEach(
    waitForAsync(() => {
      const siteSerivceStub = jasmine.createSpyObj('SitesService', [
        'getSiteServiceAreas',
        'addSiteServiceAreas',
        'deleteSiteServiceAreas',
      ]);
      siteSerivceStub.getSiteServiceAreas.and.returnValue(
        new BehaviorSubject<PaginationResponse>(serviceAreaResponse)
      );
      siteSerivceStub.addSiteServiceAreas.and.returnValue(new BehaviorSubject(null));
      siteSerivceStub.deleteSiteServiceAreas.and.returnValue(new BehaviorSubject(null));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showSuccess', 'showError']);
      toastServiceStub.showSuccess.and.callThrough();
      toastServiceStub.showError.and.callThrough();

      TestBed.configureTestingModule({
        declarations: [SiteServiceAreasComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          OAuthModule.forRoot(),
          ReactiveFormsModule,
          FormsModule,
          InputMaskModule,
        ],
        providers: [
          {
            provide: SitesService,
            useValue: siteSerivceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
          TransferState,
          MessageService,
          OAuthService,
        ],
      }).compileComponents();
      siteSerivce = TestBed.inject(SitesService);
      toastService = TestBed.inject(ToastService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SiteServiceAreasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'setZipcodeForm').and.callThrough();
    spyOn(component, 'getServiceAreas').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('zipCodeForm form invalid when empty', () => {
    component.addZipCodes();
    expect(component.zipCodeForm.valid).toBeFalsy();
  });

  it('zipCodeForm form valid when all required fileds are set', () => {
    component.addZipCodes();
    component.zipCodeForm.controls.zipCodes.setValue([
      {
        zipCode: 12345,
      },
    ]);
    expect(component.zipCodeForm.valid).toBeTruthy();
  });

  it('should call `getSiteServiceAreas` of SiteService on getServiceAreas and match the result', () => {
    component.siteId = 1;
    component.serviceAreasRequest = new SieveRequest();
    component.getServiceAreas();
    expect(siteSerivce.getSiteServiceAreas).toHaveBeenCalled();
    expect(component.serviceAreasResponse).toEqual(serviceAreaResponse);
  });

  it('should call `addSiteServiceAreas` of SiteService on onSubmitZipcode and match the result', () => {
    component.siteId = 1;
    const zipCodes = {
      zipCodes: [54321],
    };
    component.onSubmitZipcode(zipCodes);
    expect(siteSerivce.addSiteServiceAreas).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
    expect(component.setZipcodeForm).toHaveBeenCalled();
    expect(component.getServiceAreas).toHaveBeenCalled();
  });

  it('should call `deleteSiteServiceAreas` of SiteService on deleteZipCode and match the result', () => {
    component.siteId = 1;
    const site = {
      zipCode: 1234,
    };
    component.deleteZipCode(site);
    expect(siteSerivce.deleteSiteServiceAreas).toHaveBeenCalled();
    expect(toastService.showSuccess).toHaveBeenCalled();
    expect(component.getServiceAreas).toHaveBeenCalled();
  });

  const serviceArea = {
    zipCode: 1234,
  };

  const serviceAreaResponse: PaginationResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [serviceArea],
    totalPageCount: 1,
    totalRecordCount: 1,
  };
});
