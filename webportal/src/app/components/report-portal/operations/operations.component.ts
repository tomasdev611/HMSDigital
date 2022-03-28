import {Component, OnInit} from '@angular/core';
import {buildFilterString, getEnum} from '../../../utils';
import {EnumNames, FilterTypes, SieveOperators} from '../../../enums';
import {FilterConfiguration, SieveRequest} from '../../../models';
import {ReportService} from '../../../services/report.service';
import {finalize} from 'rxjs/operators';

@Component({
  selector: 'app-operations',
  templateUrl: './operations.component.html',
  styleUrls: ['./operations.component.scss'],
})
export class OperationsComponent implements OnInit {
  searchPendingQuery = 'groupedBy=Region';
  searchCompletedQuery = 'groupedBy=SiteName';
  searchPickupQuery = 'groupedBy=SiteName';
  toDate = new Date();
  sinceDate = new Date();

  completedOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Completed'
  )?.id;
  cancelledOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Cancelled'
  )?.id;

  pickupOrderTypeId = getEnum(EnumNames.OrderTypes).find(x => x.name === 'Pickup')?.id;
  filterConfiguration: FilterConfiguration[] = [
    {
      label: 'Period',
      field: 'createdDate',
      operator: SieveOperators.Equals,
      type: FilterTypes.DateRangePicker,
    },
  ];
  pendingOrdersFilter = [
    {
      field: 'statusId',
      value: [this.completedOrderStatusId],
      operator: SieveOperators.NotEquals,
    },
    {
      field: 'statusId',
      value: [this.cancelledOrderStatusId],
      operator: SieveOperators.NotEquals,
    },
  ];
  completedOrdersFilter = [
    {
      field: 'statusId',
      value: [this.completedOrderStatusId],
      operator: SieveOperators.Equals,
    },
  ];

  pickupOrdersFilter: any;
  loadingPendingOrders = true;
  loadingCompletedOrders = true;
  loadingPickupOrders = true;
  ordersChartData: any;

  pickupOrdersChartData: any;
  pickupOrdersTableData: any;
  pickupOrdersTableHeader: any;

  ordersTableData: any;
  pieChartDataOption = {
    legend: {
      display: true,
      position: 'bottom',
      usePointStyle: true,
    },
    responsive: true,
    maintainAspectRatio: true,
  };
  ordersTableHeader: any;
  plugin: any;
  filterString = '';
  ordersCount: 0;
  pickupOrdersCount: 0;
  completedOrders: 0;
  completedOrdersChartData: any;
  completedOrdersTableHeader: any;
  completedOrdersTable: any;
  colorList = [
    '#e6194b',
    '#3cb44b',
    '#ffe119',
    '#4363d8',
    '#f58231',
    '#911eb4',
    '#46f0f0',
    '#f032e6',
    '#bcf60c',
    '#fabebe',
    '#008080',
    '#e6beff',
    '#9a6324',
    '#fffac8',
    '#800000',
    '#aaffc3',
    '#808000',
    '#ffd8b1',
    '#000075',
    '#808080',
    '#ffffff',
    '#000000',
  ];

  groupingOptions = [
    {label: 'By Region', value: 'Region'},
    {label: 'By Area', value: 'Area'},
    {label: 'By Site', value: 'SiteName'},
    {label: 'By Order Type', value: 'OrderType'},
  ];

  pickupGroupingOptions = [
    {label: 'Site', value: 'SiteName'},
    {label: 'Status', value: 'Status'},
    {label: 'Hospice', value: 'HospiceName'},
  ];

  // stacked Data
  stackedChartData: any;
  stackedTableData: any;
  stackedChartDataOption: any;
  stackedTableHeader: any;
  loadingStackedBarData = false;
  showStackedBarGroupingSelection = false;

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.sinceDate.setHours(this.sinceDate.getHours() - 24);
    this.pickupOrdersFilter = this.getPickupOrdersFilter();
    this.getPendingOrdersData();
    this.getCompletedOrders();
    this.getPickupOrders();
    this.getExampleStackedBarData();
  }

  pickupGroupingChanged($event: any) {
    this.searchPickupQuery = 'groupedBy=' + $event;
    this.getPickupOrders();
  }

  pendingGroupingChanged($event: any) {
    this.searchPendingQuery = 'groupedBy=' + $event;
    this.getPendingOrdersData();
  }

  completedGroupingChanged($event: any) {
    this.searchCompletedQuery = 'groupedBy=' + $event;
    this.getCompletedOrders();
  }

  getPendingOrdersData() {
    this.loadingPendingOrders = true;
    const request = new SieveRequest();
    request.page = 0;
    request.pageSize = 0;
    request.filters = this.getFilterString(this.pendingOrdersFilter);
    this.reportService
      .getOrdersMetric({
        ...request,
        searchQuery: this.searchPendingQuery,
      })
      .pipe(finalize(() => (this.loadingPendingOrders = false)))
      .subscribe((response: any) => {
        const labels = response.data.map(report => report.label);
        const values = response.data.map(report => report.value);
        this.ordersCount = response.totalCount;
        this.ordersChartData = {
          labels,
          datasets: [
            {
              backgroundColor: this.colorList,
              data: values,
            },
          ],
        };
        this.ordersTableData = response.data.map((report, index) => {
          return {
            id: index + 1,
            label: report.label,
            value: report.value,
          };
        });
        this.ordersTableHeader = response.tableHeader;
      });
  }

  getCompletedOrders() {
    this.loadingCompletedOrders = true;
    const request = new SieveRequest();
    request.page = 0;
    request.pageSize = 0;
    request.filters = this.getFilterString(this.completedOrdersFilter);
    this.reportService
      .getOrdersMetric({
        ...request,
        searchQuery: this.searchCompletedQuery,
      })
      .pipe(finalize(() => (this.loadingCompletedOrders = false)))
      .subscribe((response: any) => {
        const labels = response.data.map(report => report.label);
        const values = response.data.map(report => report.value);
        this.completedOrders = response.totalCount;
        this.completedOrdersChartData = {
          labels,
          datasets: [
            {
              backgroundColor: this.colorList,
              data: values,
            },
          ],
        };
        this.completedOrdersTable = response.data.map((report, index) => {
          return {
            id: index + 1,
            label: report.label,
            value: report.value,
          };
        });
        this.completedOrdersTableHeader = response.tableHeader;
      });
  }

  getPickupOrders() {
    this.loadingPickupOrders = true;
    const request = new SieveRequest();
    request.page = 0;
    request.pageSize = 0;
    request.filters = this.getFilterString(this.pickupOrdersFilter);
    this.reportService
      .getOrdersMetric({
        ...request,
        searchQuery: this.searchPickupQuery,
      })
      .pipe(finalize(() => (this.loadingPickupOrders = false)))
      .subscribe((response: any) => {
        const labels = response.data.map(report => report.label);
        const values = response.data.map(report => report.value);
        this.pickupOrdersCount = response.totalCount;
        this.pickupOrdersChartData = {
          labels,
          datasets: [
            {
              backgroundColor: this.colorList,
              data: values,
            },
          ],
        };
        this.pickupOrdersTableData = response.data.map((report, index) => {
          return {
            id: index + 1,
            label: report.label,
            value: report.value,
          };
        });
        this.pickupOrdersTableHeader = response.tableHeader;
      });
  }

  filterChanged(event) {
    this.filterString = event.filterString;
    this.getPendingOrdersData();
    this.getCompletedOrders();
  }

  getFilterString(baseFilter: any, filterParam: any = this.filterString) {
    let filter = buildFilterString(baseFilter);
    if (filterParam) {
      filter = filter + ',' + filterParam;
    }
    return filter;
  }

  pickupFilterChanged($event: any) {
    this.sinceDate = $event.sinceDate;
    this.toDate = $event.toDate;
    this.pickupOrdersFilter = this.getPickupOrdersFilter();
    this.getPickupOrders();
  }

  getPickupOrdersFilter() {
    return [
      {
        field: 'OrderTypeId',
        value: [this.pickupOrderTypeId],
        operator: SieveOperators.Equals,
      },
      {
        field: 'createdDate',
        value: [this.toDate.toISOString()],
        operator: SieveOperators.LessThanEqualTo,
      },
      {
        field: 'createdDate',
        value: [this.sinceDate.toISOString()],
        operator: SieveOperators.GreaterThanEqualTo,
      },
    ];
  }

  getExampleStackedBarData() {
    this.stackedChartData = {
      labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
      datasets: [
        {
          type: 'bar',
          label: 'Dataset 1',
          backgroundColor: '#42A5F5',
          data: [50, 25, 12, 48, 90, 76, 42],
        },
        {
          type: 'bar',
          label: 'Dataset 2',
          backgroundColor: '#66BB6A',
          data: [21, 84, 24, 75, 37, 65, 34],
        },
        {
          type: 'bar',
          label: 'Dataset 3',
          backgroundColor: '#FFA726',
          data: [41, 52, 24, 74, 23, 21, 32],
        },
      ],
    };
    this.stackedChartDataOption = {
      tooltips: {
        mode: 'index',
        intersect: false,
      },
      responsive: true,
      scales: {
        xAxes: [
          {
            stacked: true,
          },
        ],
        yAxes: [
          {
            stacked: true,
          },
        ],
      },
    };
    this.stackedTableHeader = [
      {
        label: 'Month',
        field: 'month',
      },
      {
        label: 'Dataset 1',
        field: 'dataset1',
      },
      {
        label: 'Dataset 2',
        field: 'dataset2',
      },
      {
        label: 'Dataset 3',
        field: 'dataset3',
      },
    ];
    this.stackedTableData = [
      {
        month: 'January',
        dataset1: 20,
        dataset2: 20,
        dataset3: 20,
      },
      {
        month: 'February',
        dataset1: 20,
        dataset2: 20,
        dataset3: 20,
      },
    ];
  }
}
