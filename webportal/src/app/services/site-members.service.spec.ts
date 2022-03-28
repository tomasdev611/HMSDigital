import {TestBed} from '@angular/core/testing';

import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {SiteMembersService} from './site-members.service';

describe('SiteMembersService', () => {
  let service: SiteMembersService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(SiteMembersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
