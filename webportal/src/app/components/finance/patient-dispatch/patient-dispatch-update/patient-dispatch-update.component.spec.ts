import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng-lts/api';

import {PatientDispatchUpdateComponent} from './patient-dispatch-update.component';

describe('PatientDispatchUpdateComponent', () => {
  let component: PatientDispatchUpdateComponent;
  let fixture: ComponentFixture<PatientDispatchUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ReactiveFormsModule, OAuthModule.forRoot()],
      providers: [MessageService, OAuthService],
      declarations: [PatientDispatchUpdateComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientDispatchUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
