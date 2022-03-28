import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

import {OrderHmsComponent} from './order-hms.component';

describe('OrderHmsComponent', () => {
  let component: OrderHmsComponent;
  let fixture: ComponentFixture<OrderHmsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, OAuthModule.forRoot()],
      declarations: [OrderHmsComponent],
      providers: [OAuthService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderHmsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
