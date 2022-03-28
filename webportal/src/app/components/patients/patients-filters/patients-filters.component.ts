import {Component, EventEmitter, OnInit, Output, HostListener} from '@angular/core';
import {FilterTypes, SieveOperators} from 'src/app/enums';
import {SieveRequest} from 'src/app/models';
import {HospiceLocationService, HospiceService} from 'src/app/services';

@Component({
  selector: 'app-patients-filters',
  templateUrl: './patients-filters.component.html',
  styleUrls: ['./patients-filters.component.scss'],
})
export class PatientsFiltersComponent implements OnInit {
  @Output() filter = new EventEmitter<any>();
  filterConfigs: any = [];
  filterDefaultHospiceId: any;
  scrHeight: any;
  scrWidth: any;

  @HostListener('window:resize', ['$event'])
  getScreenSize(event?) {
    this.scrHeight = window.innerHeight;
    this.scrWidth = window.innerWidth;
  }

  constructor(
    private hospiceService: HospiceService,
    private locationService: HospiceLocationService
  ) {
    this.getScreenSize();
  }

  ngOnInit(): void {}

  filterPatient(event) {
    if (!event) {
      this.filter.emit('');
      return;
    }
    const hospiceId = event?.filters?.Hospice?.value?.length
      ? event?.filters?.Hospice?.value[0]
      : this.filterDefaultHospiceId;
    if (event.currentFilter.label === 'Hospice') {
      this.getHospiceLocations(hospiceId);
    }
    this.filter.emit(event.filterString);
  }

  buildfilterConf() {
    this.filterConfigs.splice(-1, 0, {
      label: 'Status',
      field: 'status',
      operator: SieveOperators.Equals,
      type: FilterTypes.Dropdown,
      value: [
        {
          status: 'active',
          name: 'A',
          className: 'status active',
          selectedClassName: 'selected-status active',
        },
        {
          status: 'pending',
          name: 'P',
          className: 'status pending',
          selectedClassName: 'selected-status active',
        },
        {
          status: 'Pending Active',
          name: 'A',
          className: 'status pending',
          selectedClassName: 'selected-status active',
        },
        {
          status: 'inactive | blank',
          name: ['I', 'I'],
          className: ['status inactive', 'status blank'],
          doubleValue: true,
        },
      ],
    });
    this.filterConfigs.splice(-1, 0, {
      label: 'DOB',
      field: 'dateOfBirth',
      operator: SieveOperators.Equals,
      type: FilterTypes.DatePicker,
    });
    this.getHospices();
  }

  removeFilterConf() {
    this.filterConfigs = [];
  }

  getHospices() {
    const req = new SieveRequest();
    this.hospiceService.getAllhospices(req).subscribe((res: any) => {
      if (res.records.length === 1) {
        this.filterDefaultHospiceId = res.records[0].id;
        this.getHospiceLocations(res.records[0].id);
        return;
      }
      this.filterDefaultHospiceId = null;
      const value = res.records.map((h: any) => {
        return {name: h.name, hospiceId: h.id};
      });
      this.filterConfigs.splice(-1, 0, {
        field: 'hospiceId',
        label: 'Hospice',
        operator: SieveOperators.Equals,
        type: FilterTypes.Autocomplete,
        dependent: 'Locations',
        value,
        class: 'p-col-3',
      });
    });
  }

  searchField(event) {
    if (!!event.query) {
      this.hospiceService.searchHospices({searchQuery: event.query}).subscribe((res: any) => {
        const filter = this.filterConfigs.find(f => f.label === event.label);
        filter.value = res.records.map((r: any) => ({
          name: r.name,
          hospiceId: r.id,
        }));
      });
    } else {
      const req = new SieveRequest();
      this.hospiceService.getAllhospices(req).subscribe((res: any) => {
        const filter = this.filterConfigs.find(f => f.label === event.label);
        filter.value = res.records.map((r: any) => ({
          name: r.name,
          hospiceId: r.id,
        }));
      });
    }
  }

  getHospiceLocations(hospiceId) {
    this.locationService.getHospiceLocations(hospiceId).subscribe((res: any) => {
      const value = res.records.map((r: any) => {
        return {label: r.name, value: r.id};
      });
      this.buildLocationFilter(value);
    });
  }

  buildLocationFilter(values) {
    const x = this.filterConfigs.find(f => f.label === 'Locations');
    if (x) {
      x.value = values;
    } else {
      this.filterConfigs.splice(-1, 0, {
        label: 'Locations',
        field: 'hospiceLocationId',
        operator: SieveOperators.Equals,
        type: FilterTypes.MultiSelect,
        value: values,
        class: 'p-col-3',
      });
    }
  }
}
