import {ComponentFixture, TestBed} from '@angular/core/testing';

import {OperationsComponent} from './operations.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {ReportService} from '../../../services/report.service';

describe('OperationsComponent', () => {
  let component: OperationsComponent;
  let fixture: ComponentFixture<OperationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [OperationsComponent],
      imports: [
        RouterTestingModule.withRoutes([{path: 'dashboard', redirectTo: ''}]),
        HttpClientTestingModule,
        OAuthModule.forRoot(),
      ],
      providers: [ReportService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OperationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
