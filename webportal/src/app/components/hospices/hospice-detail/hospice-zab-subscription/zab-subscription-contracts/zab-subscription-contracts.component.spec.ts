import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';

import {ZabSubscriptionContractsComponent} from './zab-subscription-contracts.component';

describe('ZabSubscriptionContractsComponent', () => {
  let component: ZabSubscriptionContractsComponent;
  let fixture: ComponentFixture<ZabSubscriptionContractsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
      declarations: [ZabSubscriptionContractsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ZabSubscriptionContractsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
