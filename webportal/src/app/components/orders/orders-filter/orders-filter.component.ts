import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
  HostListener,
} from '@angular/core';
import {EnumNames, FilterTypes, SieveOperators} from 'src/app/enums';
import {HospiceLocationService, HospiceService} from 'src/app/services';
import {SieveRequest} from 'src/app/models';
import {
  buildFilterString,
  checkInternalUser,
  displayOrderStatus,
  filterByString,
  getEnum,
} from 'src/app/utils';

@Component({
  selector: 'app-orders-filter',
  templateUrl: './orders-filter.component.html',
  styleUrls: ['./orders-filter.component.scss'],
})
export class OrdersFilterComponent implements OnInit {
  @Output() filter = new EventEmitter<any>();
  activetab = '';
  filterConfigs: any = [];
  scrHeight: any;
  scrWidth: any;

  @ViewChild('filter', {static: false}) appFilter;
  @Input() set orderView(activetab: string) {
    this.activetab = activetab;
    if (this.filterConfigs.length) {
      this.appFilter.clearFilter();
    }
  }
  filterDefaultHospiceId: any;
  orderTypes = getEnum(EnumNames.OrderTypes).map(x => {
    x.orderTypeId = x.id;
    return x;
  });
  orderStatuses = getEnum(EnumNames.OrderHeaderStatusTypes).map(x => {
    x.statusId = x.id;
    return x;
  });

  @HostListener('window:resize', ['$event'])
  getScreenSize(event?) {
    this.scrHeight = window.innerHeight;
    this.scrWidth = window.innerWidth;
  }

  constructor(
    private hospiceService: HospiceService,
    private hospiceLocationService: HospiceLocationService
  ) {
    this.getScreenSize();
  }

  ngOnInit(): void {}

  buildfilterConf() {
    const pickupOrderType = this.orderTypes.find(ot => ot.name.toLowerCase() === 'pickup');
    const exceptionPickupOrderType = this.orderTypes.find(
      ot => ot.name.toLowerCase() === 'pickup (exception)'
    );
    if (pickupOrderType && !exceptionPickupOrderType && checkInternalUser()) {
      this.orderTypes.forEach(ot => (ot.isExceptionFulfillment = false));
      this.orderTypes.push({
        id: pickupOrderType.id,
        name: 'Pickup (Exception)',
        orderTypeId: pickupOrderType.orderTypeId,
        isExceptionFulfillment: true,
      });
    }
    this.filterConfigs.push({
      field: 'orderTypeId',
      label: 'Order Type',
      operator: SieveOperators.Equals,
      type: FilterTypes.Autocomplete,
      value: this.orderTypes,
    });
    this.filterConfigs.push({
      field: 'statusId',
      label: 'Order Status',
      operator: SieveOperators.Equals,
      type: FilterTypes.Autocomplete,
      value: this.getOrderStatuses(),
    });
    this.getHospices();
  }

  getHospices() {
    const req = new SieveRequest();
    this.hospiceService.getAllhospices(req).subscribe((res: any) => {
      if (res.records.length === 1) {
        this.filterDefaultHospiceId = res.records[0].id;
        this.getHospiceLocations();
      } else if (res.records.length === 0) {
        this.getHospiceLocations();
      }
      if (res.records.length > 1) {
        this.filterDefaultHospiceId = null;
        const value = res.records.map((h: any) => {
          return {name: h.name, hospiceId: h.id};
        });
        this.filterConfigs.push({
          field: 'hospiceId',
          label: 'Hospice',
          operator: SieveOperators.Equals,
          type: FilterTypes.Autocomplete,
          dependent: 'Locations',
          value,
          class: 'p-col-3',
        });
      }
    });
  }

  getHospiceLocations(hospiceId?) {
    const hospiceLocationRequest = new SieveRequest();
    if (hospiceId) {
      const filterValues = [
        {
          field: 'hospiceId',
          operator: SieveOperators.Equals,
          value: [hospiceId],
        },
      ];
      hospiceLocationRequest.filters = buildFilterString(filterValues);
    }
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
        label: 'Locations',
        field: 'hospiceLocationId',
        operator: SieveOperators.Equals,
        type: FilterTypes.MultiSelect,
        value: values,
        class: 'p-col-3',
      });
    }
  }

  filterOrder(event) {
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

    if (
      event?.currentFilter?.value?.length > 0 &&
      event.currentFilter.value[0].isExceptionFulfillment
    ) {
      const filterValues = [
        {
          field: 'isExceptionFulfillment',
          operator: SieveOperators.Equals,
          value: [true],
        },
      ];
      event.filterString = `${event.filterString},${buildFilterString(filterValues)}`;
    }
    this.filter.emit(event.filterString);
  }

  searchField(event) {
    if (!!event.query) {
      if (event.label === 'Hospice') {
        this.hospiceService.searchHospices({searchQuery: event.query}).subscribe((res: any) => {
          const filter = this.filterConfigs.find(f => f.label === event.label);
          filter.value = res.records.map((r: any) => ({
            name: r.name,
            hospiceId: r.id,
          }));
        });
      } else if (event.label === 'Order Type') {
        const filter = this.filterConfigs.find(f => f.label === event.label);
        filter.value = filterByString(this.orderTypes, 'name', event.query);
      } else if (event.label === 'Order Status') {
        const filter = this.filterConfigs.find(f => f.label === event.label);
        filter.value = filterByString(this.getOrderStatuses(), 'name', event.query);
      }
    }
  }

  getOrderStatuses() {
    const orderStatuses = this.orderStatuses.filter(x => {
      x.name = displayOrderStatus(x.name);
      if (this.activetab === 'open' && x.name !== 'Completed' && x.name !== 'Cancelled') {
        return x;
      } else if (
        this.activetab === 'completed' &&
        (x.name === 'Completed' || x.name === 'Cancelled')
      ) {
        return x;
      }
    });
    return orderStatuses;
  }

  removeFilterConf() {
    this.filterConfigs = [];
  }
}
