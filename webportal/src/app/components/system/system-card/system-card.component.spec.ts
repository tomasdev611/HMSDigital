import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {SystemCardComponent} from './system-card.component';

describe('SystemCardComponent', () => {
  let component: SystemCardComponent;
  let fixture: ComponentFixture<SystemCardComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [SystemCardComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
