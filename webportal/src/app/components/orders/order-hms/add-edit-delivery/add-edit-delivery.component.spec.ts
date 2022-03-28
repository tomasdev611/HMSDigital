import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

import {AddEditDeliveryComponent} from './add-edit-delivery.component';

describe('AddEditDeliveryComponent', () => {
  let component: AddEditDeliveryComponent;
  let fixture: ComponentFixture<AddEditDeliveryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, OAuthModule.forRoot(), RouterTestingModule],
      declarations: [AddEditDeliveryComponent],
      providers: [OAuthService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditDeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
