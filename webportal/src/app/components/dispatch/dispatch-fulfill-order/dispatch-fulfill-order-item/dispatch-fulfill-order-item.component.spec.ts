import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DispatchFulfillOrderItemComponent} from './dispatch-fulfill-order-item.component';

describe('DispatchFulfillOrderItemComponent', () => {
  let component: DispatchFulfillOrderItemComponent;
  let fixture: ComponentFixture<DispatchFulfillOrderItemComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [DispatchFulfillOrderItemComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchFulfillOrderItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
