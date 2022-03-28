import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {SystemService} from 'src/app/services/system.service';
import {HealthCheckTabComponent} from './health-check-tab.component';
import {BehaviorSubject} from 'rxjs';

describe('HealthCheckTabComponent', () => {
  let component: HealthCheckTabComponent;
  let fixture: ComponentFixture<HealthCheckTabComponent>;
  let systemService: any;

  beforeEach(
    waitForAsync(() => {
      const systemServiceStub = jasmine.createSpyObj('SystemService', ['getHealthCheck']);

      systemServiceStub.getHealthCheck.and.returnValue(new BehaviorSubject(healthCheckResponse));

      TestBed.configureTestingModule({
        declarations: [HealthCheckTabComponent],
        imports: [HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: SystemService,
            useValue: systemServiceStub,
          },
          OAuthService,
          MessageService,
        ],
      }).compileComponents();
      systemService = TestBed.inject(SystemService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HealthCheckTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'formatAllTime').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getHealthCheck` on ngOnInit and match the result', () => {
    component.ngOnInit();
    expect(systemService.getHealthCheck).toHaveBeenCalled();
    expect(component.statusResponse).toEqual(healthCheckResponse);
    expect(component.formatAllTime).toHaveBeenCalled();
  });

  const healthCheckResponse = {
    entries: {
      'HMSDigital Db': {
        data: {},
        duration: 0.001,
        status: 'Healthy',
        tags: ['db', 'sql', 'SqlServer', 'HMSDigitalDb'],
      },
      NetSuite: {
        data: {},
        description: 'NetSuite is Up',
        duration: 0.888,
        status: 'Healthy',
        tags: ['netsuite'],
      },
    },
    status: 'Healthy',
    totalDuration: 0.888,
  };
});
