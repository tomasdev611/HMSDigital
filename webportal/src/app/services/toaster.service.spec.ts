import {TestBed} from '@angular/core/testing';

import {ToastService} from './toaster.service';
import {MessageService} from 'primeng/api';

describe('ToasterService', () => {
  let service: ToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: MessageService,
        },
      ],
    });
    service = TestBed.inject(ToastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
