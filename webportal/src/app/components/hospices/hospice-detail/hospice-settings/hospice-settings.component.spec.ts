import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';

import {HospiceSettingsComponent} from './hospice-settings.component';
import {MessageService} from 'primeng/api';
import {ReactiveFormsModule} from '@angular/forms';
import {InputSwitchModule} from 'primeng/inputswitch';

describe('HospiceSettingsComponent', () => {
  let component: HospiceSettingsComponent;
  let fixture: ComponentFixture<HospiceSettingsComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [HospiceSettingsComponent],
        providers: [MessageService],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          OAuthModule.forRoot(),
          ReactiveFormsModule,
          InputSwitchModule,
        ],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospiceSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
