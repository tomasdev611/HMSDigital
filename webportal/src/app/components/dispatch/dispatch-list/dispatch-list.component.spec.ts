import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {OAuthModule} from 'angular-oauth2-oidc';

import {DispatchListComponent} from './dispatch-list.component';

describe('DispatchListComponent', () => {
  let component: DispatchListComponent;
  let fixture: ComponentFixture<DispatchListComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [DispatchListComponent],
        imports: [HttpClientTestingModule, OAuthModule.forRoot()],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
