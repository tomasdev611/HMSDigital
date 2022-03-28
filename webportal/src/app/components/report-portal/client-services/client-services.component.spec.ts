import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ClientServicesComponent} from './client-services.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {ReportService} from '../../../services/report.service';

describe('ClientServicesComponent', () => {
  let component: ClientServicesComponent;
  let fixture: ComponentFixture<ClientServicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClientServicesComponent],
      imports: [
        RouterTestingModule.withRoutes([{path: 'dashboard', redirectTo: ''}]),
        HttpClientTestingModule,
        OAuthModule.forRoot(),
      ],
      providers: [ReportService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
