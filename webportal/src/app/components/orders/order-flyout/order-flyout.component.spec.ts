import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng-lts/api';

import {OrderFlyoutComponent} from './order-flyout.component';

describe('OrderFlyoutComponent', () => {
  let component: OrderFlyoutComponent;
  let fixture: ComponentFixture<OrderFlyoutComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        imports: [HttpClientTestingModule, OAuthModule.forRoot(), RouterTestingModule],
        declarations: [OrderFlyoutComponent],
        providers: [OAuthService, MessageService],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderFlyoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
