import {TestBed} from '@angular/core/testing';

import {EnumService} from './enum.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('EnumService', () => {
  let service: EnumService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(EnumService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
