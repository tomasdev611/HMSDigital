import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {TransferState} from '@angular/platform-browser';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {InputNumberModule} from 'primeng/inputnumber';
import {InputSwitchModule} from 'primeng/inputswitch';
import {MessageService} from 'primeng/api';
import {SystemService, ToastService} from 'src/app/services';
import {FeatureFlagsComponent} from './feature-flags.component';
import {PaginationResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('FeatureFlagsComponent', () => {
  let component: FeatureFlagsComponent;
  let fixture: ComponentFixture<FeatureFlagsComponent>;
  let systemService: any;
  let toastService: any;

  beforeEach(
    waitForAsync(() => {
      const systemServiceStub = jasmine.createSpyObj('SystemService', [
        'getAllSystemFeatures',
        'updateSystemFeature',
      ]);

      systemServiceStub.getAllSystemFeatures.and.returnValue(
        new BehaviorSubject<PaginationResponse>(featureFlagsResponse)
      );
      systemServiceStub.updateSystemFeature.and.returnValue(new BehaviorSubject(featureFlag));

      const toastServiceStub = jasmine.createSpyObj('ToastService', ['showSuccess']);

      toastServiceStub.showSuccess.and.callThrough();

      TestBed.configureTestingModule({
        declarations: [FeatureFlagsComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          OAuthModule.forRoot(),
          ReactiveFormsModule,
          FormsModule,
          InputNumberModule,
          InputSwitchModule,
        ],
        providers: [
          TransferState,
          MessageService,
          {
            provide: SystemService,
            useValue: systemServiceStub,
          },
          {
            provide: ToastService,
            useValue: toastServiceStub,
          },
        ],
      }).compileComponents();
      systemService = TestBed.inject(SystemService);
      toastService = TestBed.inject(ToastService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(FeatureFlagsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAllSystemFeatures` on getAllFeatureFlags and match the result', () => {
    component.getAllFeatureFlags();
    expect(systemService.getAllSystemFeatures).toHaveBeenCalled();
    expect(component.featureFlags).toEqual(featureFlagsResponse.records);
  });

  it('should call `updateSystemFeature` on updateFeatureFlag and match the result', () => {
    const body = {
      name: 'featureFlag',
      isDisabled: false,
    };
    component.updateFeatureFlag(body);
    expect(systemService.updateSystemFeature).toHaveBeenCalled();
    expect(component.featureFlags).toEqual(featureFlagsResponse.records);
    expect(toastService.showSuccess).toHaveBeenCalled();
  });

  const featureFlag = {
    id: 1,
    name: 'laura',
    isDisabled: true,
  };

  const featureFlagsResponse = {
    records: [featureFlag],
    totalRecordCount: 1,
    pageNumber: 1,
    pageSize: 1,
    totalPageCount: 1,
  };
});
