import {TestBed} from '@angular/core/testing';

import {RouteOptimizationApiService} from './route-optimization-api.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {HttpTestingController} from '@angular/common/http/testing';
import {environment} from 'src/environments/environment';

describe('RouteOptimizationApiService', () => {
  let service: RouteOptimizationApiService;
  let httpMock: HttpTestingController;
  const API_BASE_URL = environment.routeOptimizerURL + '/api/';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
        RouteOptimizationApiService,
      ],
    });
    service = TestBed.inject(RouteOptimizationApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
