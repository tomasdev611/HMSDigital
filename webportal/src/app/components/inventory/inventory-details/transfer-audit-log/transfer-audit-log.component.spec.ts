import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {TransferAuditLogComponent} from './transfer-audit-log.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';

describe('TransferAuditLogComponent', () => {
  let component: TransferAuditLogComponent;
  let fixture: ComponentFixture<TransferAuditLogComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [TransferAuditLogComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: OAuthService,
          },
          MessageService,
        ],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferAuditLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
