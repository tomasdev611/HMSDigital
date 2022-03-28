import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {LoginComponent} from './login.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {of} from 'rxjs';
import {TransferState} from '@angular/platform-browser';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  beforeEach(
    waitForAsync(() => {
      const oAuthService = jasmine.createSpyObj('OAuthService', [
        'getIdentityClaims',
        'loadDiscoveryDocumentAndLogin',
        'events',
      ]);
      oAuthService.events = of('subscription value');
      TestBed.configureTestingModule({
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        declarations: [LoginComponent],
        providers: [{provide: OAuthService, useValue: oAuthService}, TransferState],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
