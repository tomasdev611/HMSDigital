import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng-lts/api';

import {EquipmentSettingsConfigComponent} from './equipment-settings-config.component';

describe('EquipmentSettingsConfigComponent', () => {
  let component: EquipmentSettingsConfigComponent;
  let fixture: ComponentFixture<EquipmentSettingsConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EquipmentSettingsConfigComponent],
      imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
      providers: [MessageService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EquipmentSettingsConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
