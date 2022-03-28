import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DispatchSchedulerComponent} from './dispatch-scheduler.component';
import {DateAdapter} from 'angular-calendar';

describe('DispatchSchedulerComponent', () => {
  let component: DispatchSchedulerComponent;
  let fixture: ComponentFixture<DispatchSchedulerComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [DispatchSchedulerComponent],
        providers: [DateAdapter],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchSchedulerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
