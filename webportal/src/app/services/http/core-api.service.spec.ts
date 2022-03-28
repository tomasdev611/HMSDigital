import {TestBed} from '@angular/core/testing';

import {CoreApiService} from './core-api.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {HttpTestingController} from '@angular/common/http/testing';
import {environment} from 'src/environments/environment';

describe('CoreApiService', () => {
  let service: CoreApiService;
  let httpMock: HttpTestingController;
  const API_BASE_URL = environment.apiServerURL + '/api/';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
        CoreApiService,
      ],
    });
    service = TestBed.inject(CoreApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve result to the API via GET', () => {
    const dummyResult: Result[] = [
      {
        id: 1,
        data: 'test data 1',
      },
      {
        id: 2,
        data: 'test data 2',
      },
    ];
    service.get('').subscribe((result: any) => {
      expect(result.length).toBe(2);
      expect(result).toEqual(dummyResult);
    });
    const request = httpMock.expectOne(`${API_BASE_URL}`);
    expect(request.request.method).toBe('GET');
    request.flush(dummyResult);
  });

  it('should retrieve result to the API via POST', () => {
    const dummyResult: Result = {
      id: 1,
      data: 'test data 1',
    };
    service.post('', {data: 'test data 1'}).subscribe((result: any) => {
      expect(result).toEqual(dummyResult);
    });
    const request = httpMock.expectOne(`${API_BASE_URL}`);
    expect(request.request.method).toBe('POST');
    request.flush(dummyResult);
  });

  it('should retrieve result to the API via PUT', () => {
    const dummyResult: Result = {
      id: 1,
      data: 'test data 1',
    };
    service.put('', dummyResult).subscribe((result: any) => {
      expect(result).toEqual(dummyResult);
    });
    const request = httpMock.expectOne(`${API_BASE_URL}`);
    expect(request.request.method).toBe('PUT');
    request.flush(dummyResult);
  });

  it('should retrieve success to the API via DELETE', () => {
    const dummyResult = {};
    service.delete('').subscribe((result: any) => {
      expect(result).toEqual(dummyResult);
    });
    const request = httpMock.expectOne(`${API_BASE_URL}`);
    expect(request.request.method).toBe('DELETE');
    request.flush({});
  });
});

export interface Result {
  id: number;
  data: string;
}
