import {TestBed} from '@angular/core/testing';

import {FeedbackService} from './feedback.service';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('FeedbackService', () => {
  let service: FeedbackService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(FeedbackService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
