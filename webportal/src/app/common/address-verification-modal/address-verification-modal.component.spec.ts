import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {AddressVerificationModalComponent} from './address-verification-modal.component';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('AddressVerificationModalComponent', () => {
  let component: AddressVerificationModalComponent;
  let fixture: ComponentFixture<AddressVerificationModalComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [AddressVerificationModalComponent],
        imports: [HttpClientTestingModule, OAuthModule.forRoot(), ReactiveFormsModule, FormsModule],
        providers: [
          {
            provide: OAuthService,
          },
        ],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressVerificationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
