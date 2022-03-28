import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FilterTypes, SieveOperators} from 'src/app/enums';
import {filterByString} from 'src/app/utils';

@Component({
  selector: 'app-sites-filters',
  templateUrl: './sites-filters.component.html',
  styleUrls: ['./sites-filters.component.scss'],
})
export class SitesFiltersComponent implements OnInit {
  @Output() filter = new EventEmitter<any>();
  filterConfigs: any = [];
  siteTypes = [
    {locationType: 'region', name: 'Region'},
    {locationType: 'area', name: 'Area'},
    {locationType: 'tpp', name: 'TPP'},
    {locationType: 'site', name: 'Site'},
    {locationType: 'vehicle', name: 'Vehicle'},
  ];
  constructor() {}

  ngOnInit(): void {}

  filterSite(event) {
    if (!event) {
      this.filter.emit('');
      return;
    }
    this.filter.emit(event.filterString);
  }

  buildfilterConf() {
    const value = this.siteTypes;
    this.filterConfigs.splice(-1, 0, {
      label: 'Site Type',
      field: 'locationType',
      operator: SieveOperators.Equals,
      type: FilterTypes.Autocomplete,
      dependent: 'Site',
      value,
    });
  }

  removeFilterConf() {
    this.filterConfigs = [];
  }

  searchField(event) {
    const query = (typeof event.query === 'object' ? event.query.name : event.query) || '';
    const filter = this.filterConfigs.find(f => f.label === event.label);
    filter.value = filterByString(this.siteTypes, 'locationType', query);
  }
}
