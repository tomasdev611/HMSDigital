import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';

import {ImportHospiceFacilityComponent} from './import-hospice-facility.component';

describe('ImportHospiceFacilityComponent', () => {
  let component: ImportHospiceFacilityComponent;
  let fixture: ComponentFixture<ImportHospiceFacilityComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ImportHospiceFacilityComponent],
        providers: [MessageService],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportHospiceFacilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
