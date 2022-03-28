import {TestBed} from '@angular/core/testing';

import {HospiceService} from './hospice.service';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('HospiceService', () => {
  let service: HospiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(HospiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
