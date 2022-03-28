import {DatePipe} from '@angular/common';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng-lts/api';
import {AutoCompleteModule} from 'primeng-lts/autocomplete';
import {RadioButtonModule} from 'primeng-lts/radiobutton';
import {PhonePipe} from 'src/app/pipes';

import {PatientMergeComponent} from './patient-merge.component';

describe('PatientMergeComponent', () => {
  let component: PatientMergeComponent;
  let fixture: ComponentFixture<PatientMergeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        FormsModule,
        HttpClientTestingModule,
        OAuthModule.forRoot(),
        AutoCompleteModule,
        RadioButtonModule,
      ],
      providers: [OAuthService, MessageService, DatePipe, PhonePipe],
      declarations: [PatientMergeComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientMergeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
