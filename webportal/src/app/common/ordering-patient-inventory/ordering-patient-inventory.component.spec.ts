import {ComponentFixture, TestBed} from '@angular/core/testing';

import {OrderingPatientInventoryComponent} from './ordering-patient-inventory.component';

describe('OrderingPatientInventoryComponent', () => {
  let component: OrderingPatientInventoryComponent;
  let fixture: ComponentFixture<OrderingPatientInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [OrderingPatientInventoryComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderingPatientInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
