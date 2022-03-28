import {TestBed} from '@angular/core/testing';

import {RouteOptimizationService} from './route-optimization.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('RouteOptimizationService', () => {
  let service: RouteOptimizationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(RouteOptimizationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
