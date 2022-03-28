import {TestBed} from '@angular/core/testing';

import {HospiceMemberService} from './hospice-member.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('HospiceMemberService', () => {
  let service: HospiceMemberService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(HospiceMemberService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
