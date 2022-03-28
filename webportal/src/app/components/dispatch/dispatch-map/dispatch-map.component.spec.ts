import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {DispatchMapComponent} from './dispatch-map.component';

describe('DispatchMapComponent', () => {
  let component: DispatchMapComponent;
  let fixture: ComponentFixture<DispatchMapComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [DispatchMapComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
