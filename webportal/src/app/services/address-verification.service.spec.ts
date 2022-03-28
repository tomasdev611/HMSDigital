import {TestBed} from '@angular/core/testing';

import {AddressVerificationService} from './address-verification.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('AddressVerificationService', () => {
  let service: AddressVerificationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(AddressVerificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
