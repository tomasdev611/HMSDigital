import {TestBed} from '@angular/core/testing';

import {RoleService} from './role.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';

describe('RoleService', () => {
  let service: RoleService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(RoleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
