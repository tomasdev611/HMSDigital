import {Component, OnInit, EventEmitter, Output} from '@angular/core';
import {FilterTypes, SieveOperators} from 'src/app/enums';
import {SitesService} from 'src/app/services';

@Component({
  selector: 'app-inventory-filters',
  templateUrl: './inventory-filters.component.html',
  styleUrls: ['./inventory-filters.component.scss'],
})
export class InventoryFiltersComponent implements OnInit {
  @Output() filter = new EventEmitter<any>();
  filterConfigs: any = [];
  filterDefaultId: any;

  constructor(private siteService: SitesService) {}

  ngOnInit(): void {}

  filterInventory(event) {
    if (!event) {
      this.filter.emit('');
      return;
    }
    this.filter.emit(event.filterString);
  }

  buildfilterConf() {
    this.siteService.searchSites({searchQuery: ''}).subscribe((response: any) => {
      const value = response.records.map((r: any) => ({
        name: r.name,
        currentLocationId: r.id,
      }));
      this.buildFilterConfigs('Sites/Vehicles', value);
    });
  }

  removeFilterConf() {
    this.filterConfigs = [];
  }

  buildFilterConfigs(label: string, value: any) {
    this.filterConfigs.splice(1, 2, {
      field: 'currentLocationId',
      label,
      operator: SieveOperators.Equals,
      type: FilterTypes.Autocomplete,
      value,
    });
  }

  searchField(event) {
    const query = (typeof event.query === 'object' ? event.query.name : event.query) || '';
    this.siteService.searchSites({searchQuery: query}).subscribe((searchResponse: any) => {
      const filter = this.filterConfigs.find(f => f.label === event.label);
      filter.value = searchResponse.records.map((r: any) => ({
        name: r.name,
        currentLocationId: r.id,
      }));
    });
  }
}
