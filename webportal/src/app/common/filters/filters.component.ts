import {Component, OnInit, Input, EventEmitter, Output, OnChanges} from '@angular/core';
import {FilterConfiguration} from 'src/app/models';
import {buildFilterString} from 'src/app/utils';
import {createPatch} from 'src/app/utils/common.utils';
import {FilterService} from 'src/app/services';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.scss'],
})
export class FiltersComponent implements OnInit, OnChanges {
  @Output() filter = new EventEmitter<any>();
  @Input() filterConfiguration: FilterConfiguration[];
  @Output() autocompleteSearch = new EventEmitter<any>();
  @Output() filterBarVisible = new EventEmitter<any>();
  @Output() filterBarHidden = new EventEmitter<any>();
  @Input() disabled: boolean;
  visible: boolean;
  filters: any = {};
  selectedValues: any = {};
  previousFilter: any = null;

  constructor(private filterService: FilterService) {}

  ngOnChanges(changes) {
    if (!changes) {
      return;
    }
    if (!changes.filterConfiguration.firstChange) {
      this.removeUnusedValues();
    }
  }

  ngOnInit(): void {
    this.filterService.filterValue.subscribe((filterObject: any) => {
      if (filterObject) {
        this.selectedValues[filterObject.label] = {
          value: filterObject.value,
        };
      } else {
        this.selectedValues = {};
      }
    });
  }

  filterChanged(filter) {
    const {label, field, value, operator, fields} = filter;

    if (this.previousFilter === null && value === null) {
      return;
    }
    if (this.previousFilter && createPatch(this.previousFilter, filter).length === 0) {
      return;
    }

    this.previousFilter = filter;
    this.filters[label] = {field, value, operator, fields};
    this.filters[label].value = this.filters[label]?.value?.map(v => {
      return v && v[field] ? v[field] : v;
    });
    this.handleDependentFields(label);
    this.selectedValues[label] = {value};
    if (value && value.length === 0) {
      this.removeUnusedFilters(label);
    }
    this.emitFilterString(filter);
  }

  handleDependentFields(label) {
    const currentFilter = this.filterConfiguration.find(fc => fc.label === label);
    if (currentFilter?.dependent) {
      if (this.selectedValues[currentFilter?.dependent]) {
        delete this.selectedValues[currentFilter?.dependent];
      }
      if (this.filters[currentFilter?.dependent]) {
        delete this.filters[currentFilter?.dependent];
      }
    }
  }

  removeUnusedValues() {
    const obj = {};
    this.filterConfiguration.forEach(fc => {
      obj[fc.label] = this.filters[fc.label];
    });

    this.filters = obj;
  }

  removeUnusedFilters(label) {
    delete this.selectedValues[label];
    delete this.filters[label];
  }

  emitFilterString(filter) {
    Object.keys(this.filters).forEach(
      key => this.filters[key] === undefined && delete this.filters[key]
    );
    const filterString = buildFilterString(Object.values(this.filters));
    this.filter.emit({
      filterString,
      filters: this.filters,
      currentFilter: filter,
    });
  }

  clearFilter() {
    this.visible = false;
    if (Object.keys(this.filters).length !== 0 || Object.keys(this.selectedValues).length !== 0) {
      this.filters = {};
      this.selectedValues = {};
      this.previousFilter = null;
      this.filter.emit('');
      this.filterBarHidden.emit();
      this.filterService.forceFilterValueUpdate(null);
    }
  }

  showFilterBar() {
    this.visible = true;
    if (this.filterConfiguration.length === 0) {
      this.filterBarVisible.emit();
    }
  }

  searchField(event) {
    this.autocompleteSearch.emit({...event, filters: this.filters});
  }
}
