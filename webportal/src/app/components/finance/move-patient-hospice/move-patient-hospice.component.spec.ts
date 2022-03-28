import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng-lts/api';

import {MovePatientHospiceComponent} from './move-patient-hospice.component';

describe('MovePatientHospiceComponent', () => {
  let component: MovePatientHospiceComponent;
  let fixture: ComponentFixture<MovePatientHospiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [OAuthService, MessageService],
      declarations: [MovePatientHospiceComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MovePatientHospiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
