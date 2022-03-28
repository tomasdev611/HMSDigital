import {ComponentFixture, TestBed} from '@angular/core/testing';

import {CurrentInventoryOrderComponent} from './current-inventory-order.component';

describe('CurrentInventoryOrderComponent', () => {
  let component: CurrentInventoryOrderComponent;
  let fixture: ComponentFixture<CurrentInventoryOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CurrentInventoryOrderComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentInventoryOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
