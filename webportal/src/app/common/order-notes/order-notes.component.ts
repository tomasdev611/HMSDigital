import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from '@angular/forms';

@Component({
  selector: 'app-order-notes',
  templateUrl: './order-notes.component.html',
  styleUrls: ['./order-notes.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: OrderNotesComponent,
    },
  ],
})
export class OrderNotesComponent implements OnInit, ControlValueAccessor {
  touched = false;
  disabled = false;
  orderNotes: any = [];

  @Input() orderHeaderNotes;
  constructor() {}

  ngOnInit(): void {}

  onChange = (notes: any[]) => {};

  onTouched = () => {};

  onNotesBlurred(event) {
    if (!event.srcElement.value) {
      return;
    }
    this.orderNotes.push(event.srcElement.value);
    this.onNotesUpdated(this.orderNotes);
    event.srcElement.value = '';
  }

  onNotesUpdated(notes) {
    this.markAsTouched();
    this.orderNotes = notes;
    this.onChange(this.orderNotes);
  }

  writeValue(notes: any[]) {
    this.orderNotes = notes;
  }

  registerOnChange(onChange: any) {
    this.onChange = onChange;
  }

  registerOnTouched(onTouched: any) {
    this.onTouched = onTouched;
  }

  markAsTouched() {
    if (!this.touched) {
      this.onTouched();
      this.touched = true;
    }
  }

  getDateFromString(dateString: string) {
    return new Date(dateString);
  }
}
