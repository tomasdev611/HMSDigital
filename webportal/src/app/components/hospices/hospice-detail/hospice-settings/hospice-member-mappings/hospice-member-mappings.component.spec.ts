import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {HospiceMemberMappingsComponent} from './hospice-member-mappings.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthModule} from 'angular-oauth2-oidc';
import {MessageService} from 'primeng/api';
import {HospiceService} from 'src/app/services';
import {BehaviorSubject} from 'rxjs';

describe('HospiceMemberMappingsComponent', () => {
  let component: HospiceMemberMappingsComponent;
  let fixture: ComponentFixture<HospiceMemberMappingsComponent>;
  let hospiceService: any;

  beforeEach(
    waitForAsync(() => {
      const hospiceServiceStub = jasmine.createSpyObj('HospiceService', [
        'updateHospiceMemberInputMapping',
        'updateHospiceMemberOutputMapping',
        'getHospiceMemberInputMapping',
        'getHospiceMemberOutputMapping',
      ]);
      hospiceServiceStub.updateHospiceMemberInputMapping.and.returnValue(
        new BehaviorSubject(hospiceMemberInputMappingResponse)
      );
      hospiceServiceStub.updateHospiceMemberOutputMapping.and.returnValue(
        new BehaviorSubject(hospiceMemberOutputMappingResponse)
      );
      hospiceServiceStub.getHospiceMemberInputMapping.and.returnValue(
        new BehaviorSubject(hospiceMemberInputMappingResponse)
      );
      hospiceServiceStub.getHospiceMemberOutputMapping.and.returnValue(
        new BehaviorSubject(hospiceMemberOutputMappingResponse)
      );
      TestBed.configureTestingModule({
        providers: [
          {
            provide: HospiceService,
            useValue: hospiceServiceStub,
          },
          MessageService,
        ],
        declarations: [HospiceMemberMappingsComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospiceMemberMappingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `updateHospiceMemberInputMapping` of HospiceService on saveMappings', () => {
    component.hospiceId = 1;
    component.saveMappings({
      mappings: columnNameMapping,
      type: 'input',
    });
    expect(hospiceService.updateHospiceMemberInputMapping).toHaveBeenCalled();
  });

  it('should call `updateHospiceMemberOutputMapping` of HospiceService on saveMappings', () => {
    component.hospiceId = 1;
    component.saveMappings({
      mappings: columnNameMapping,
      type: 'output',
    });
    expect(hospiceService.updateHospiceMemberOutputMapping).toHaveBeenCalled();
  });

  const columnNameMapping = {
    canAccessWebStore: {},
    countryCode: {},
    designation: {},
    email: {},
    firstName: {},
    lastName: {},
    phoneNumber: {},
    role: {},
  };

  const hospiceMemberInputMappingResponse = {
    column_name_mapping: columnNameMapping,
  };

  const hospiceMemberOutputMappingResponse = {
    column_name_mapping: columnNameMapping,
  };
});
