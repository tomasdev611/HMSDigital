import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {AuditLogsComponent} from './audit-logs.component';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MessageService} from 'primeng/api';
import {AuditService} from 'src/app/services';
import {ContinuationTokenResponse} from 'src/app/models';
import {BehaviorSubject} from 'rxjs';

describe('AuditLogsComponent', () => {
  let component: AuditLogsComponent;
  let fixture: ComponentFixture<AuditLogsComponent>;
  let auditService: any;

  beforeEach(
    waitForAsync(() => {
      const auditServiceStub = jasmine.createSpyObj('AuditService', [
        'getAuditLogs',
        'getAuditLogsAsCsv',
      ]);
      auditServiceStub.getAuditLogs.and.returnValue(
        new BehaviorSubject<ContinuationTokenResponse>(auditResponse)
      );
      auditServiceStub.getAuditLogsAsCsv.and.returnValue(new BehaviorSubject(''));
      TestBed.configureTestingModule({
        imports: [HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: OAuthService,
          },
          {
            provide: AuditService,
            useValue: auditServiceStub,
          },
          MessageService,
        ],
        declarations: [AuditLogsComponent],
      }).compileComponents();
      auditService = TestBed.inject(AuditService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditLogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getAuditLogs').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getAuditLogs` method on ngOnInit and match the result', () => {
    component.ngOnInit();
    expect(component.getAuditLogs).toHaveBeenCalled();
    expect(auditService.getAuditLogs).toHaveBeenCalled();
    expect(component.auditLogResponse.hasOwnProperty('records')).toBeTruthy();
  });

  it('should call `getAuditLogsAsCsv` method on exportUserAuditLogs and match the result', () => {
    component.exportUserAuditLogs();
    expect(auditService.getAuditLogsAsCsv).toHaveBeenCalled();
  });

  const audit = {
    auditAction: 'UpdateRoles',
    auditData: [],
    auditDate: '2020-12-28T08:53:41.903Z',
    auditId: 869,
    clientIPAddress: '89.187.161.220',
    entityId: 40,
    targetUser: [],
    targetUserId: 40,
    user: [],
    userId: 250,
  };

  const auditResponse = {
    records: [audit],
    continuationToken: null,
  };
});
