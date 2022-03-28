import {TestBed} from '@angular/core/testing';

import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OrderHeadersService} from './order-headers.service';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('OrderHeadersService', () => {
  let service: OrderHeadersService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(OrderHeadersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
