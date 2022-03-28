import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';

import {TableComponent} from './table.component';
import {RouterTestingModule} from '@angular/router/testing';
import {DatePipe} from '@angular/common';
import {PhonePipe} from 'src/app/pipes';
import {TableModule} from 'primeng/table';

describe('TableComponent', () => {
  let component: TableComponent;
  let fixture: ComponentFixture<TableComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [TableComponent],
        imports: [RouterTestingModule, TableModule],
        providers: [DatePipe, PhonePipe],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(TableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
