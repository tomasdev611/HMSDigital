import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {LocationInventoryComponent} from './location-inventory.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';

describe('LocationInventoryComponent', () => {
  let component: LocationInventoryComponent;
  let fixture: ComponentFixture<LocationInventoryComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [LocationInventoryComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
