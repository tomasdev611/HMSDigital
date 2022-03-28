import {TestBed} from '@angular/core/testing';

import {HospiceLocationService} from './hospice-location.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('HospiceLocationService', () => {
  let service: HospiceLocationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(HospiceLocationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
