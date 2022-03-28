import {TestBed} from '@angular/core/testing';

import {HospiceFacilityService} from './hospice-facility.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('HospiceFacilityService', () => {
  let service: HospiceFacilityService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(HospiceFacilityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
