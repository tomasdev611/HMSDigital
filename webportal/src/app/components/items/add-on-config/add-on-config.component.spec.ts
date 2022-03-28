import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng-lts/api';

import {AddOnConfigComponent} from './add-on-config.component';

describe('AddOnConfigComponent', () => {
  let component: AddOnConfigComponent;
  let fixture: ComponentFixture<AddOnConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddOnConfigComponent],
      imports: [HttpClientTestingModule, RouterTestingModule, OAuthModule.forRoot()],
      providers: [MessageService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddOnConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
