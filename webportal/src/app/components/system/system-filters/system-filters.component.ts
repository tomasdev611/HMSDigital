import {Component, OnInit, Output, EventEmitter, Input, ViewChild} from '@angular/core';
import {FilterTypes, SieveOperators} from 'src/app/enums';
import {SieveRequest} from 'src/app/models';
import {HospiceLocationService, PatientService} from 'src/app/services';

@Component({
  selector: 'app-system-filters',
  templateUrl: './system-filters.component.html',
  styleUrls: ['./system-filters.component.scss'],
})
export class SystemFiltersComponent implements OnInit {
  @Output() filter = new EventEmitter<any>();
  @ViewChild('filter', {static: false}) appFilter;
  activetab = '';
  @Input() set activeTabView(activetab: string) {
    this.activetab = activetab;
    if (this.filterConfigs.length) {
      this.appFilter.clearFilter();
    }
  }
  filterConfigs: any = [];
  filterDefaultId: any;
  apiLogTypes = [
    {name: 'Core', apiLogType: 'Core'},
    {name: 'Fulfillment', apiLogType: 'Fulfillment'},
    {name: 'Notification', apiLogType: 'Notification'},
    {name: 'Patient', apiLogType: 'Patient'},
  ];
  constructor(
    private patientService: PatientService,
    private hospiceLocationService: HospiceLocationService
  ) {}

  ngOnInit(): void {}

  filterSystem(event) {
    if (!event) {
      this.filter.emit('');
      return;
    }
    this.filter.emit(event.filterString);
  }

  buildfilterConf() {
    if (this.activetab === 'apiLogs' || this.activetab === 'netsuiteLogs') {
      this.filterConfigs = [
        ...this.filterConfigs,
        {
          label: 'Date Range',
          field: 'dateRange',
          operator: SieveOperators.Equals,
          type: FilterTypes.DateRangePicker,
          datePickerConfig: {
            showTime: true,
            maxDate: new Date(),
          },
        },
      ];
      if (this.activetab === 'apiLogs') {
        this.filterConfigs.push({
          field: 'apiLogType',
          label: 'Type',
          operator: SieveOperators.Equals,
          type: FilterTypes.Autocomplete,
          value: this.apiLogTypes,
        });
      }
    }
    if (this.activetab === 'dispatchOrders') {
      this.filterConfigs = [
        ...this.filterConfigs,
        {
          label: 'Delivery Date(range)',
          field: 'deliveryDate',
          operator: SieveOperators.Equals,
          type: FilterTypes.DateRangePicker,
        },
        {
          label: 'Pickup Date(range)',
          field: 'pickupDate',
          operator: SieveOperators.Equals,
          type: FilterTypes.DateRangePicker,
        },
      ];
      this.getPatients();
      this.getHospiceLocations();
    }
  }

  removeFilterConf() {
    this.filterConfigs = [];
  }

  getPatients() {
    const req = new SieveRequest();
    this.patientService.getPatients(req).subscribe((res: any) => {
      if (res.records.length) {
        const value = res.records.map((p: any) => {
          return {
            name: `${p?.firstName} ${p?.lastName}`,
            patientUuid: p.uniqueId,
          };
        });
        this.filterConfigs.push({
          field: 'patientUuid',
          label: 'Patient',
          operator: SieveOperators.Equals,
          type: FilterTypes.Autocomplete,
          value,
        });
      }
    });
  }

  searchField(event) {
    if (event.label === 'Patient') {
      this.patientService
        .searchPatientsBySearchQuery({searchQuery: event.query || ''})
        .subscribe((res: any) => {
          const filter = this.filterConfigs.find(f => f.label === event.label);
          if (!!filter) {
            filter.value = res.records.map((r: any) => ({
              name: `${r?.firstName} ${r?.lastName}`,
              patientUuid: r.uniqueId,
            }));
          }
        });
    }
  }

  getHospiceLocations() {
    const hospiceLocationRequest = new SieveRequest();
    this.hospiceLocationService.getLocations(hospiceLocationRequest).subscribe((res: any) => {
      const value = res.records.map((r: any) => {
        return {label: r.name, value: r.id};
      });
      if (res.records.length > 1) {
        this.buildLocationFilter(value);
      }
    });
  }

  buildLocationFilter(values) {
    const x = this.filterConfigs.find(f => f.label === 'Locations');
    if (x) {
      x.value = values;
    } else {
      this.filterConfigs.push({
        label: 'Hospice Locations',
        field: 'hospiceLocationId',
        operator: SieveOperators.Equals,
        type: FilterTypes.MultiSelect,
        value: values,
      });
    }
  }
}
