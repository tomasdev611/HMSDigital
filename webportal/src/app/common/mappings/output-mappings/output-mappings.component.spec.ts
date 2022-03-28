import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {OutputMappingsComponent} from './output-mappings.component';

describe('OutputMappingsComponent', () => {
  let component: OutputMappingsComponent;
  let fixture: ComponentFixture<OutputMappingsComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [OutputMappingsComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(OutputMappingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
