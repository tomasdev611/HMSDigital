import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss'],
})
export class ChartComponent implements OnInit {
  @Input() chartData: any;
  @Input() tableData: any;
  @Input() options: any;
  @Input() type: any;
  @Input() tableHeader: any;
  @Input() plugins: any;
  @Input() title: string;
  @Input() totalCount = 0;
  @Input() loading: boolean;
  @Input() showGroupingSelection = true;
  @Output() groupingChanged = new EventEmitter<any>();
  @Output() filterChanged = new EventEmitter<any>();

  toggleView = true;
  toggleIconClass = 'pi pi-chart-bar toggle';
  grouping: any = {};
  filter: any = {};

  @Input() groupingOptions = [
    {label: 'By Site', value: 'SiteName'},
    {label: 'By Order Type', value: 'OrderType'},
    {label: 'By Hospice', value: 'HospiceName'},
  ];

  @Input() showFilterOption = false;
  @Input() filterOptions = [
    {label: 'Last 24 hours', value: '24'},
    {label: 'Last 48 hours', value: '48'},
    {label: 'Last week', value: '168'},
  ];

  constructor() {}

  ngOnInit(): void {
    this.grouping = {label: 'Order Type', value: 'OrderType'};
  }

  toggleChart() {
    this.toggleView = !this.toggleView;
    this.toggleIconClass = this.toggleView ? 'pi pi-chart-bar toggle' : 'pi pi-list toggle';
  }

  groupingSelectionChanged() {
    this.groupingChanged.emit(this.grouping.value);
  }

  filterSelectionChanged() {
    const currentDate = new Date();
    const date = new Date();
    date.setHours(date.getHours() - this.filter.value);
    const filter = {
      toDate: currentDate,
      sinceDate: date,
    };

    this.filterChanged.emit(filter);
  }
}
