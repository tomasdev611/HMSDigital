import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {ImportHospiceMembersComponent} from './import-hospice-members.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ReactiveFormsModule} from '@angular/forms';
import {MessageService} from 'primeng/api';

describe('ImportHospiceMembersComponent', () => {
  let component: ImportHospiceMembersComponent;
  let fixture: ComponentFixture<ImportHospiceMembersComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        providers: [MessageService],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          OAuthModule.forRoot(),
        ],
        declarations: [ImportHospiceMembersComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportHospiceMembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
