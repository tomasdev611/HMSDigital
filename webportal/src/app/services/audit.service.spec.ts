import {TestBed} from '@angular/core/testing';

import {AuditService} from './audit.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('AuditService', () => {
  let service: AuditService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(AuditService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
