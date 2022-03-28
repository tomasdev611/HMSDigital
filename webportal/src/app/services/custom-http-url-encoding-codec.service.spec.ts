import {TestBed} from '@angular/core/testing';

import {CustomHttpUrlEncodingCodecService} from './custom-http-url-encoding-codec.service';

describe('CustomHttpUrlEncodingCodecService', () => {
  let service: CustomHttpUrlEncodingCodecService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomHttpUrlEncodingCodecService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
