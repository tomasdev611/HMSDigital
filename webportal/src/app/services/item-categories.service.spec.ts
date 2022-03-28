import {TestBed} from '@angular/core/testing';

import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {ItemCategoriesService} from './item-categories.service';

describe('ItemCategoriesService', () => {
  let service: ItemCategoriesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [
        {
          provide: OAuthService,
        },
      ],
    });
    service = TestBed.inject(ItemCategoriesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
