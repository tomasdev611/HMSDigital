import {HttpClientTestingModule} from '@angular/common/http/testing';
import {ComponentFixture, TestBed} from '@angular/core/testing';
import {ReactiveFormsModule} from '@angular/forms';
import {OAuthModule} from 'angular-oauth2-oidc';

import {AddEditAddonsComponent} from './add-edit-addons.component';

describe('AddEditAddonsComponent', () => {
  let component: AddEditAddonsComponent;
  let fixture: ComponentFixture<AddEditAddonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddEditAddonsComponent],
      imports: [HttpClientTestingModule, OAuthModule.forRoot(), ReactiveFormsModule],
      providers: [],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditAddonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
