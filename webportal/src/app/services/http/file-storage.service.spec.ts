import {TestBed} from '@angular/core/testing';

import {FileStorageService} from './file-storage.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('FileStorageService', () => {
  let service: FileStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(FileStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
