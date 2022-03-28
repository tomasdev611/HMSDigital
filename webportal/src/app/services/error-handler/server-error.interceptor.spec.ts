import {TestBed} from '@angular/core/testing';

import {ServerErrorInterceptor} from './server-error.interceptor';
import {RouterTestingModule} from '@angular/router/testing';

describe('ServerErrorInterceptor', () => {
  let service: ServerErrorInterceptor;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
    });
    service = TestBed.inject(ServerErrorInterceptor);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
