import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {ImportWizardValidatorComponent} from './import-wizard-validator.component';

describe('ImportWizardValidatorComponent', () => {
  let component: ImportWizardValidatorComponent;
  let fixture: ComponentFixture<ImportWizardValidatorComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ImportWizardValidatorComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportWizardValidatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
