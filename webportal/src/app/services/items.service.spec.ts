import {TestBed} from '@angular/core/testing';

import {ItemsService} from './items.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('ItemService', () => {
  let service: ItemsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(ItemsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
