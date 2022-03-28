import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {InputMappingsComponent} from './input-mappings.component';

describe('InputMappingsComponent', () => {
  let component: InputMappingsComponent;
  let fixture: ComponentFixture<InputMappingsComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [InputMappingsComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(InputMappingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
