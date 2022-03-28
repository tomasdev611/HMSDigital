import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ReportPortalComponent} from './report-portal.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';

describe('ReportPortalComponent', () => {
  let component: ReportPortalComponent;
  let fixture: ComponentFixture<ReportPortalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReportPortalComponent],
      imports: [
        RouterTestingModule.withRoutes([{path: 'dashboard', redirectTo: ''}]),
        HttpClientTestingModule,
        OAuthModule.forRoot(),
      ],
      providers: [OAuthService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportPortalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
