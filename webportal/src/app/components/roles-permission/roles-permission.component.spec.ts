import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RolesPermissionComponent} from './roles-permission.component';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {RoleService} from 'src/app/services';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {BehaviorSubject} from 'rxjs';

describe('RolesPermissionComponent', () => {
  let component: RolesPermissionComponent;
  let fixture: ComponentFixture<RolesPermissionComponent>;
  let roleService: any;
  beforeEach(
    waitForAsync(() => {
      const roleServiceStub = jasmine.createSpyObj('RoleService', ['getAllRoles']);
      roleServiceStub.getAllRoles.and.returnValue(new BehaviorSubject<Role[]>([new Role()]));
      TestBed.configureTestingModule({
        declarations: [RolesPermissionComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: RoleService,
            useValue: roleServiceStub,
          },
          OAuthService,
          MessageService,
        ],
      }).compileComponents();
      roleService = TestBed.inject(RoleService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(RolesPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getRoles').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getRoles` method ', () => {
    component.getRoles();
    expect(component.getRoles).toHaveBeenCalled();
  });

  it('should call `getAllRoles` method of RoleService on getRoles method and match the result', () => {
    component.getRoles();
    expect(roleService.getAllRoles).toHaveBeenCalled();
    expect(component.roles).toEqual([new Role()]);
  });
});

export class Permission {
  isAdmin = true;
  canCreate = true;
  canRead = true;
  canUpdate = true;
  canDelete = true;
  permissionIdnumber = 1;
  name: 'permission';
}

export class Role {
  id = 1;
  permissions = [new Permission()];
  name = 'role';
  level = 1;
}
