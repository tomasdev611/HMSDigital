import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {HospiceDetailComponent} from './hospice-detail.component';
import {RouterTestingModule} from '@angular/router/testing';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import {HospiceService} from 'src/app/services';
import {MessageService, ConfirmationService} from 'primeng/api';
import {ReactiveFormsModule} from '@angular/forms';
import {BehaviorSubject} from 'rxjs';
import {Hospice} from 'src/app/models/model.hospice';
import {Router} from '@angular/router';
import {Location} from '@angular/common';

describe('HospiceDetailComponent', () => {
  let component: HospiceDetailComponent;
  let fixture: ComponentFixture<HospiceDetailComponent>;
  let hospiceService: any;
  let router: Router;
  let location: Location;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [HospiceDetailComponent],
        imports: [
          RouterTestingModule,
          HttpClientTestingModule,
          ReactiveFormsModule,
          OAuthModule.forRoot(),
        ],
        providers: [
          {
            provide: HospiceService,
            OAuthService,
          },
          MessageService,
          ConfirmationService,
        ],
      }).compileComponents();
      hospiceService = TestBed.inject(HospiceService);
      router = TestBed.inject(Router);
      location = TestBed.inject(Location);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(HospiceDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getHospice').and.callThrough();
    spyOn(component, 'onTabChange').and.callThrough();
    spyOn(hospiceService, 'getHospiceById').and.returnValue(new BehaviorSubject<Hospice>(hospice));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getHospiceById` method of HospiceService on getHospice and match the result', () => {
    component.getHospice();
    expect(hospiceService.getHospiceById).toHaveBeenCalled();
    expect(component.hospice).toEqual(hospice);
  });

  it('should call `onTabChange` method on onTabChange ', () => {
    component.onTabChange({index: 0});
    expect(component.onTabChange).toHaveBeenCalledWith({index: 0});
  });

  it('should change `hospiceView to locations` method on onTabChange with index 0', () => {
    component.onTabChange({index: 0});
    expect(component.hospiceView).toBe('locations');
  });

  it('should change `hospiceView to facilities` method on onTabChange with index 1', () => {
    component.onTabChange({index: 1});
    expect(component.hospiceView).toBe('facilities');
  });

  it('should change `hospiceView to members` method on onTabChange with index2', () => {
    component.onTabChange({index: 2});
    expect(component.hospiceView).toBe('members');
  });

  it('should change `route with locations as view in queryparams` method on onTabChange with index 0', () => {
    const navigateSpy = spyOn(location, 'replaceState');
    component.onTabChange({index: 0});
    expect(component.hospiceView).toBe('locations');
    expect(navigateSpy).toHaveBeenCalledTimes(1);
    expect(navigateSpy).toHaveBeenCalledWith(
      window.location.pathname,
      new URLSearchParams({view: 'locations'}).toString()
    );
  });

  it('should change `route with facilities as view in queryparams` method on onTabChange with index 1', () => {
    const navigateSpy = spyOn(location, 'replaceState');
    component.onTabChange({index: 1});
    expect(component.hospiceView).toBe('facilities');
    expect(navigateSpy).toHaveBeenCalledTimes(1);
    expect(navigateSpy).toHaveBeenCalledWith(
      window.location.pathname,
      new URLSearchParams({view: 'facilities'}).toString()
    );
  });

  it('should change `route with members as view in queryparams` method on onTabChange with index 2', () => {
    const navigateSpy = spyOn(location, 'replaceState');
    component.onTabChange({index: 2});
    expect(component.hospiceView).toBe('members');
    expect(navigateSpy).toHaveBeenCalledTimes(1);
    expect(navigateSpy).toHaveBeenCalledWith(
      window.location.pathname,
      new URLSearchParams({view: 'members'}).toString()
    );
  });

  const hospice = {
    id: 1,
    name: 'Apple Hospices',
    hospiceLocations: [],
  };

  const errorResponse = {
    error: {message: 'error response'},
  };
});
