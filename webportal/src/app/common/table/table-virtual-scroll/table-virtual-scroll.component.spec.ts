import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {PhonePipe} from 'src/app/pipes';
import {DatePipe} from '@angular/common';

import {TableVirtualScrollComponent} from './table-virtual-scroll.component';

describe('TableVirtualScrollComponent', () => {
  let component: TableVirtualScrollComponent;
  let fixture: ComponentFixture<TableVirtualScrollComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        imports: [RouterTestingModule],
        declarations: [TableVirtualScrollComponent],
        providers: [DatePipe, PhonePipe],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(TableVirtualScrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
