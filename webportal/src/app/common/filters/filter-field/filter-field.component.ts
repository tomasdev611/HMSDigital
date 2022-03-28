import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  ViewChild,
  ElementRef,
} from '@angular/core';
import {FilterTypes, SieveOperators} from 'src/app/enums';
import {map, debounceTime, distinctUntilChanged} from 'rxjs/operators';
import {fromEvent} from 'rxjs';

@Component({
  selector: 'app-filter-field',
  templateUrl: './filter-field.component.html',
  styleUrls: ['./filter-field.component.scss'],
})
export class FilterFieldComponent implements OnInit, OnChanges {
  @Input() filterType: FilterTypes;
  @Input() filterValues: any;
  @Input() operator: SieveOperators;
  @Input() field: string;
  @Input() fields: string[];
  @Input() label: string;
  @Input() datePickerConfig: any;
  @Input() selectedValue: any;
  @Output() filterChanged = new EventEmitter<any>();
  @Output() searchField = new EventEmitter<any>();
  filterTypesList = FilterTypes;
  values: any;
  inputFilter: ElementRef;

  @ViewChild('inputFilter') set content(content: ElementRef) {
    if (content) {
      this.inputFilter = content;
      fromEvent(this.inputFilter.nativeElement, 'keyup')
        .pipe(
          map((query: any) => query),
          debounceTime(1000),
          distinctUntilChanged()
        )
        .subscribe(() => {
          this.textFilterValueChanged();
        });
    }
  }

  constructor() {}

  ngOnChanges(changes: any) {
    if (changes.selectedValue) {
      if (this.filterType === this.filterTypesList.DatePicker) {
        this.values = this.selectedValue?.value ? new Date(this.selectedValue?.value) : null;
      } else if (this.filterType === this.filterTypesList.DateRangePicker) {
        if (this.selectedValue?.value) {
          const dates = this.selectedValue?.value.map(x => new Date(x.value));
          this.values = dates;
        } else {
          this.values = null;
        }
      } else if (this.filterType === this.filterTypesList.Autocomplete) {
        this.values = this.selectedValue?.value ? this.selectedValue?.value[0] : null;
      } else if (this.filterType === this.filterTypesList.TextInput) {
        this.values = this.selectedValue?.value?.join(' ') || '';
      } else if (this.filterType === this.filterTypesList.Dropdown) {
        if (this.selectedValue?.length > 0) {
          this.values = this.selectedValue[0];
        }
      } else {
        this.values = this.selectedValue?.value || [];
      }
    }
  }

  ngOnInit(): void {
    this.selectDefaultLabel();
  }

  selectDefaultLabel() {
    if (this.filterType === this.filterTypesList.MultiSelect) {
      if (this.filterValues.length === 1) {
        this.values = [this.filterValues[0].value];
      }
    }
  }

  shouldShow(type) {
    return type === this.filterType;
  }

  autocompleteSelected(event) {
    this.filterChanged.emit({
      field: this.field,
      value: [event],
      label: this.label,
      operator: this.operator,
    });
  }

  dropDownSelected(event) {
    this.filterChanged.emit({
      field: this.field,
      value: [event.value],
      label: this.label,
      operator: this.operator,
    });
  }

  search(event) {
    if (event.query) {
      this.searchField.emit({
        query: !!this.values ? this.values : event.query,
        label: this.label,
      });
      return;
    }
    this.filterValues = [...this.filterValues];
  }

  dateSelected() {
    if (!this.values) {
      this.filterChanged.emit({
        field: this.field,
        value: null,
        label: this.label,
        operator: this.operator,
      });
      return;
    }
    const date = this.datePickerConfig?.noDateAlteration
      ? new Date(this.values.getTime()).toLocaleDateString().split('T')[0]
      : new Date(this.values).toLocaleDateString().split('T')[0];
    this.filterChanged.emit({
      field: this.field,
      value: [date],
      label: this.label,
      operator: this.operator,
    });
  }

  dateRangeSelected(event, closed = false) {
    if (!this.values) {
      this.filterChanged.emit({
        field: this.field,
        value: null,
        label: this.label,
        operator: this.operator,
      });
      return;
    } else if (this.values[0] && this.values[1] && !closed) {
      this.filterChanged.emit({
        field: this.field,
        value: [
          {
            field: this.field,
            value: [this.getFormatedDateString(this.values[0])],
            label: this.label,
            operator: SieveOperators.GreaterThanEqualTo,
          },
          {
            field: this.field,
            value: [this.getFormatedDateString(this.values[1])],
            label: this.label,
            operator: SieveOperators.LessThanEqualTo,
          },
        ],
        label: this.label,
        operator: this.operator,
      });
    }
  }

  getFormatedDateString(value) {
    const offset = new Date().getTimezoneOffset() * 60000;
    const formatedDate = new Date(value.getTime() - offset).toISOString();
    return formatedDate;
  }

  removeFilter() {
    this.values = null;
    this.filterValueChanged();
  }

  filterValueChanged() {
    let value = this.values;
    if (value && !Array.isArray(this.values)) {
      value = [this.values];
    }
    this.filterChanged.emit({
      field: this.field,
      value,
      label: this.label,
      operator: this.operator,
    });
  }

  textFilterValueChanged() {
    if (this.values) {
      const value = this.values.split(' ');
      this.filterChanged.emit({
        fields: this.fields,
        field: this.field,
        value,
        label: this.label,
        operator: this.operator,
      });
    } else {
      this.filterChanged.emit({
        field: this.field,
        value: null,
        label: this.label,
        operator: this.operator,
      });
    }
  }
}
