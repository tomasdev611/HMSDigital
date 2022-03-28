import {Component, OnInit} from '@angular/core';
import {
  OrderHeadersService,
  PatientService,
  SitesService,
  ToastService,
  DispatchService,
} from 'src/app/services';
import {SieveRequest, PaginationResponse} from 'src/app/models';
import {debounceTime, finalize} from 'rxjs/operators';
import {IsPermissionAssigned, buildFilterString, getEnum, getUniqArray} from 'src/app/utils';
import {SieveOperators, EnumNames} from 'src/app/enums';
import {Router} from '@angular/router';
import {forkJoin, Subject} from 'rxjs';

// comment to try to fix the deploy process between e2e -> training -> production

@Component({
  selector: 'app-dispatch',
  templateUrl: './dispatch.component.html',
  styleUrls: ['./dispatch.component.scss'],
})
export class DispatchComponent implements OnInit {
  orderStatusTypes = getEnum(EnumNames.OrderHeaderStatusTypes);
  completedId = this.orderStatusTypes.find(x => x.name === 'Completed')?.id;
  selectAllOrder = false;
  activeView = 'orderView';
  loading = false;
  orderRequest = new SieveRequest();
  orderResponse: PaginationResponse;
  locations = [];
  location: any;
  filter = {
    patientUUIDs: [],
    status: [],
    type: '',
    orderRequestDate: null,
    orderDate: null,
  };
  searchKeyword = '';
  dispatchStatuses;
  orderStatusEnums = [];
  orderTypes;
  allPatients = [];
  selectedOrders = [];
  searchKeywordChanged = new Subject<string>();
  markedOrderId;
  orderDate = null;
  orderRequestDate = null;
  maxDate = new Date();

  constructor(
    private orderHeaderService: OrderHeadersService,
    private patientService: PatientService,
    private sitesService: SitesService,
    private toasterService: ToastService,
    private router: Router,
    private dispatchService: DispatchService
  ) {
    this.searchKeywordChanged.pipe(debounceTime(1000)).subscribe(() => {
      this.searchOrderByQuery();
    });
  }

  ngOnInit(): void {
    this.dispatchStatuses = [
      {value: 'Stat', label: 'Stat'},
      {value: 10, label: 'Unassigned'},
      {value: 11, label: 'Assigned'},
    ];
    this.orderTypes = getEnum(EnumNames.OrderTypes).map((l: any) => {
      return {
        label: l.name.includes('_') ? l.name.replace('_', ' ') : l.name,
        value: l.id,
      };
    });
    this.orderTypes.unshift({
      label: 'All',
      value: 0,
    });

    this.orderStatusEnums = getEnum(EnumNames.OrderHeaderStatusTypes);
    const location = sessionStorage.getItem('currentSite');
    this.location = location ? JSON.parse(location) : null;
    this.orderRequest.locationId = this.location ? this.location.id : null;
    this.orderRequest.pageSize = 10;
    this.orderRequest.sorts = '-' + 'orderDateTime';
    if (location) {
      this.updateFilterRequest(true);
    }
  }

  searchOrders(cleanOrderSelected: boolean): void {
    this.loading = true;
    if (cleanOrderSelected) {
      this.selectedOrders = [];
    }
    this.selectAllOrder = false;
    this.orderHeaderService
      .getAllOrderHeaders(this.orderRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: PaginationResponse) => {
        this.orderResponse = response;
        this.orderResponse.records.map(o => {
          o.processingTime = this.calculateProcessingTime(o.orderLineItems, o.orderType);
          o.disabled = o.orderStatus === 'Pending Approval' ? true : false;
          o.orderLineItems = o.orderLineItems.filter(item => {
            return item.statusId !== this.completedId;
          });
          return o;
        });
        const patientUuids = getUniqArray(response.records.map(r => r.patientUuid));
        const orderIds = response.records.flatMap(o => {
          switch (o.dispatchStatus) {
            case 'Scheduled':
            case 'Enroute':
            case 'OutForFulfillment':
            case 'OnSite':
              return [o.id];
            default:
              return [];
          }
        });
        const filters = [
          {
            field: 'uniqueId',
            operator: SieveOperators.Equals,
            value: patientUuids,
          },
        ];
        const patientRequest = {
          filters: buildFilterString(filters),
        };
        const req = [];
        req.push(this.patientService.getPatients(patientRequest));
        if (orderIds.length > 0) {
          const dispatchfilters = [
            {
              field: 'orderHeaderId',
              operator: SieveOperators.Equals,
              value: orderIds,
            },
          ];
          const dispatchInsRequest = {
            filters: buildFilterString(dispatchfilters),
          };
          req.push(this.dispatchService.getAllDispatchInstructions(dispatchInsRequest));
        }
        this.loadAdditionDispatchData(req, cleanOrderSelected);
      });
  }

  checkPageOrdersSelected() {
    let currentPageTotalOrderSelected = 0;
    this.orderResponse.records.map(orderRes => {
      const index = this.selectedOrders.findIndex(x => x.id === orderRes.id);
      if (index > -1) {
        orderRes.selected = true;
        currentPageTotalOrderSelected++;
      }
    });

    this.selectAllOrder = currentPageTotalOrderSelected === this.orderResponse.records.length;
  }

  loadAdditionDispatchData(req, cleanOrderSelected: boolean) {
    forkJoin(req)
      .pipe(
        finalize(() => {
          if (!cleanOrderSelected) {
            this.checkPageOrdersSelected();
          }
        })
      )
      .subscribe((responses: any) => {
        const [patientResponse, dispatchInsResponse]: any = responses;
        this.allPatients = patientResponse.records.map(p => {
          p.name = `${p.firstName} ${p.lastName}`;
          return p;
        });
        this.orderResponse?.records.map(o => {
          o.patient = this.allPatients.find(p => p.uniqueId === o.patientUuid);
          if (
            ['Scheduled', 'Enroute', 'OutForFulfillment', 'OnTruck', 'OnSite'].includes(
              o.dispatchStatus
            )
          ) {
            const orderDispatchIns = dispatchInsResponse?.records.find(
              ins => ins.orderHeaderId === o.id
            );
            o.driver = orderDispatchIns?.vehicle?.currentDriverName ?? '';
          }
          o.selected = false;
          return o;
        });
      });
  }

  getNext() {
    if (this.orderResponse.pageNumber >= this.orderResponse.totalPageCount) {
      return;
    }
    this.orderRequest.page = this.orderRequest.page + 1;
    this.updateFilterRequest(false);
  }

  getPrev() {
    if (this.orderResponse.pageNumber <= 1) {
      return;
    }
    this.orderRequest.page = this.orderRequest.page - 1;
    this.updateFilterRequest(false);
  }

  calculateProcessingTime(orderLineItems, orderType) {
    const processingTime = orderLineItems.reduce((a, b) => {
      b.processingTimeSum =
        b.itemCount * (b.item.avgDeliveryProcessingTime + b.item.avgPickUpProcessingTime);
      return a + b.processingTimeSum;
    }, 0);
    return processingTime;
  }

  changed(event) {
    this.searchKeywordChanged.next();
  }

  searchOrderByQuery() {
    this.orderRequest.page = 1;
    if (this.searchKeyword) {
      const regExp = new RegExp('.*[0-9].*');
      if (regExp.test(this.searchKeyword)) {
        this.filter.patientUUIDs = [];
        this.updateFilterRequest(true);
      } else if (this.searchKeyword.length >= 3) {
        this.patientService
          .searchPatientsBySearchQuery({searchQuery: this.searchKeyword})
          .subscribe((response: PaginationResponse) => {
            const patientUUIDs = response.records.map(r => r.uniqueId);
            this.filter.patientUUIDs = patientUUIDs;
            this.updateFilterRequest(true);
          });
      }
    } else {
      this.filter.patientUUIDs = [];
      this.updateFilterRequest(true);
    }
  }

  toggleAllOrder(event) {
    this.selectAllOrder = event.checked;
    let auxIsOrderSelected = false;
    this.orderResponse.records.map(order => {
      if (!order.disabled) {
        auxIsOrderSelected = order.selected;
        order.selected = this.selectAllOrder;
        if (!auxIsOrderSelected) {
          this.selectedOrders = [...this.selectedOrders, order];
        }

        if (!this.selectAllOrder && auxIsOrderSelected) {
          this.removeOrderFromArray(order.id);
        }
      }
      return order;
    });
  }
  selectOrder(event) {
    const {checkbox, order} = event;
    this.selectAllOrder = false;
    if (checkbox.checked) {
      this.selectedOrders = [...this.selectedOrders, order];
    } else {
      this.removeOrderFromArray(order.id);
    }
  }

  removeOrderFromArray(idOrder: number) {
    const index = this.selectedOrders.findIndex(x => x.id === idOrder);
    if (index > -1) {
      this.selectedOrders.splice(index, 1);
    }
  }

  updateFilterRequest(cleanOrderSelected: boolean) {
    this.orderRequest.locationId = this.location?.id;
    let statOrder = false;
    const filter = JSON.parse(JSON.stringify(this.filter));
    if (filter.status.length > 0) {
      const index = filter.status.findIndex(x => x === 'Stat');
      if (index > -1) {
        statOrder = true;
        filter.status.splice(index, 1);
      }
    }
    const orderStatusFilteredOut = this.orderStatusEnums.flatMap(ot =>
      ot.name === 'Completed' || ot.name === 'Cancelled' ? [ot.id] : []
    );
    let filters = [
      {
        field: 'patientUUID',
        operator: SieveOperators.Equals,
        value: filter.patientUUIDs,
      },
      {
        field: 'dispatchStatusId',
        operator: SieveOperators.Equals,
        value: filter.status,
      },
      {
        field: 'orderTypeId',
        operator: SieveOperators.Equals,
        value: filter.type ? [filter.type] : [],
      },
      {
        field: 'statOrder',
        operator: SieveOperators.Equals,
        value: statOrder ? [statOrder] : [],
      },
    ];
    if (this.searchKeyword && (!filter.patientUUIDs || filter.patientUUIDs.length === 0)) {
      const orderNumberFilter = {
        field: 'OrderNumber',
        value: [this.searchKeyword],
        operator: SieveOperators.CI_Contains,
      };
      filters = [...filters, orderNumberFilter];
    }
    if (this.orderDate) {
      const date = new Date(JSON.parse(JSON.stringify(this.orderDate)));
      const selectedDate = this.getDateWithoutTime(date);
      const selectedEndDate = this.addOneDay(date);
      const orderDateFilter = [
        {
          field: 'OrderDateTime',
          value: [selectedDate],
          operator: SieveOperators.GreaterThanEqualTo,
        },
        {
          field: 'OrderDateTime',
          value: [selectedEndDate],
          operator: SieveOperators.LessThan,
        },
      ];
      filters = [...filters, ...orderDateFilter];
    }
    if (this.orderRequestDate) {
      const date = new Date(JSON.parse(JSON.stringify(this.orderRequestDate)));
      const selectedDate = this.getDateWithoutTime(date);
      const selectedEndDate = this.addOneDay(date);
      const orderRequestedDateFilter = [
        {
          field: 'requestedStartDateTime',
          value: [selectedDate],
          operator: SieveOperators.GreaterThanEqualTo,
        },
        {
          field: 'requestedStartDateTime',
          value: [selectedEndDate],
          operator: SieveOperators.LessThan,
        },
      ];
      filters = [...filters, ...orderRequestedDateFilter];
    }
    orderStatusFilteredOut.map(x => {
      const filterOut = {
        field: 'dispatchStatusId',
        operator: SieveOperators.NotEquals,
        value: [x],
      };
      filters = [...filters, filterOut];
    });
    this.filter = {
      ...this.filter,
      orderDate: this.orderDate,
      orderRequestDate: this.orderRequestDate,
    };
    this.orderRequest.filters = buildFilterString(filters);
    this.searchOrders(cleanOrderSelected);
  }
  searchLocation({query}) {
    this.sitesService.searchSites({searchQuery: query}).subscribe((response: any) => {
      this.locations = response.records.filter(
        x => x.locationType === 'Site' || x.locationType === 'Area' || x.locationType === 'Region'
      );
    });
  }
  clearLocation(event) {
    this.orderRequest.page = 1;
    this.updateFilterRequest(true);
  }
  onLocationSelect(event) {
    sessionStorage.setItem('currentSite', JSON.stringify(this.location));
    this.orderRequest.page = 1;
    this.updateFilterRequest(true);
  }
  statusFilterChange(event) {
    this.orderRequest.page = 1;
    this.updateFilterRequest(true);
  }
  typeFilterChange(event) {
    this.orderRequest.page = 1;
    this.updateFilterRequest(true);
  }
  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }
  assignOrders() {
    this.router.navigate(['/dispatch/assign'], {
      state: {data: {orders: this.selectedOrders}},
    });
  }
  markerSelected(id) {
    this.markedOrderId = id;
  }
  dateSelected() {
    if (
      this.filter.orderDate?.getTime() === this.orderDate?.getTime() &&
      this.filter.orderRequestDate?.getTime() === this.orderRequestDate?.getTime()
    ) {
      return;
    }

    this.orderRequest.page = 1;
    this.updateFilterRequest(true);
  }
  getDateWithoutTime(date) {
    return new Date(date.getTime() - date.getTimezoneOffset() * 60000).toISOString();
  }
  addOneDay(date) {
    return this.getDateWithoutTime(new Date(date.setDate(date.getDate() + 1)));
  }

  shouldShowDispatchList() {
    if (!!this.location) {
      return true;
    }

    this.searchKeyword = '';
    this.orderDate = null;
    this.orderRequestDate = null;
    this.orderResponse = null;
    this.selectedOrders = [];

    return false;
  }
}
