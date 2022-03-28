import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {SitesFiltersComponent} from './sites-filters.component';

describe('SitesFiltersComponent', () => {
  let component: SitesFiltersComponent;
  let fixture: ComponentFixture<SitesFiltersComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [SitesFiltersComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SitesFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
