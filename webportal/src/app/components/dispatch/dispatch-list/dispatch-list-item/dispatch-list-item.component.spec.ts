import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DispatchListItemComponent} from './dispatch-list-item.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {ItemsService} from 'src/app/services';

describe('DispatchListItemComponent', () => {
  let component: DispatchListItemComponent;
  let fixture: ComponentFixture<DispatchListItemComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [DispatchListItemComponent],
        imports: [HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: ItemsService,
            OAuthService,
          },
        ],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
