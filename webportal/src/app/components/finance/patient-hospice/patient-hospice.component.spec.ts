import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng-lts/api';

import {PatientHospiceComponent} from './patient-hospice.component';

describe('PatientHospiceComponent', () => {
  let component: PatientHospiceComponent;
  let fixture: ComponentFixture<PatientHospiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [OAuthService, MessageService],
      declarations: [PatientHospiceComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientHospiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
