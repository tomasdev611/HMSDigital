import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {AddUserComponent} from './add-user.component';

import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {UserService} from 'src/app/services';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {MessageService} from 'primeng/api';
import {InputMaskModule} from 'primeng/inputmask';
import {DropdownModule} from 'primeng/dropdown';
import {MultiSelectModule} from 'primeng/multiselect';
import {RadioButtonModule} from 'primeng/radiobutton';

describe('AddUserComponent', () => {
  let component: AddUserComponent;
  let fixture: ComponentFixture<AddUserComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [AddUserComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          FormsModule,
          InputMaskModule,
          DropdownModule,
          RadioButtonModule,
          MultiSelectModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: UserService,
            OAuthService,
          },
          MessageService,
        ],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('userForm invalid when empty', () => {
    expect(component.userForm.valid).toBeFalsy();
  });

  it('userForm valid when all required fields are set', () => {
    component.userForm.controls.firstName.setValue('test_first_name');
    component.userForm.controls.lastName.setValue('test_last_name');
    component.userForm.controls.email.setValue('test@test.com');
    expect(component.userForm.valid).toBeTruthy();
  });
});
