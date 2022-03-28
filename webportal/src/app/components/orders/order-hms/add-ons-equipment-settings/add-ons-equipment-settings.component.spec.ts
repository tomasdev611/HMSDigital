import {ComponentFixture, TestBed} from '@angular/core/testing';

import {AddOnsEquipmentSettingsComponent} from './add-ons-equipment-settings.component';

describe('AddOnsEquipmentSettingsComponent', () => {
  let component: AddOnsEquipmentSettingsComponent;
  let fixture: ComponentFixture<AddOnsEquipmentSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddOnsEquipmentSettingsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddOnsEquipmentSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
