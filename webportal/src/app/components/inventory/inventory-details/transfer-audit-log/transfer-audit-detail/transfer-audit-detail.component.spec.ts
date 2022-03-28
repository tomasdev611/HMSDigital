import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {TransferAuditDetailComponent} from './transfer-audit-detail.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {TransferState} from '@angular/platform-browser';

describe('TransferAuditDetailComponent', () => {
  let component: TransferAuditDetailComponent;
  let fixture: ComponentFixture<TransferAuditDetailComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [TransferAuditDetailComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [TransferState],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferAuditDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
