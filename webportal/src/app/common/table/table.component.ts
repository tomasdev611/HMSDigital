import {DatePipe} from '@angular/common';
import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {Router} from '@angular/router';
import {getObjectValueByKey} from 'src/app/utils';
import {PhonePipe} from 'src/app/pipes';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent implements OnInit {
  @Input() list = [];
  @Input() rows: number;
  @Input() loading: string;
  @Input() pageNumber: number;
  @Input() totalPage: number;
  @Input() filterFields: [];
  @Input() headers: [];
  @Input() show = true;
  @Input() submit;
  @Input() showPaginator = true;
  @Input() paginate = true;
  @Input() multiSort = false;
  @Input() multiSelection = false;
  @Input() selection = false;
  @Input() checkboxSelection = false;
  @Input() tableScrollHeight = null;
  @Input() getRowClassType = null;
  @Output() toggle = new EventEmitter<any>();
  @Output() fetchPrev = new EventEmitter<any>();
  @Output() fetchNext = new EventEmitter<any>();
  @Output() deleteHandler = new EventEmitter<any>();
  @Output() actionBtnHandler = new EventEmitter<any>();
  @Output() conditionalBtnHandler = new EventEmitter<any>();
  @Output() sort = new EventEmitter<any>();
  @Output() rowSelected = new EventEmitter<any>();

  sortedFields = {};
  selectedRows = [];
  tooltipZIndex = 99999;

  constructor(private router: Router, private datePipe: DatePipe, private phonePipe: PhonePipe) {}

  ngOnInit(): void {}

  toggleAction(event, object) {
    this.toggle.emit({event, object});
  }

  getPrev() {
    this.fetchPrev.emit();
  }

  getNext() {
    this.fetchNext.emit();
  }

  delete(object, index) {
    this.deleteHandler.emit({object, index});
  }

  getValueByField(obj, fieldName, fieldType?) {
    const fieldValue = getObjectValueByKey(obj, fieldName) || '';
    switch (fieldType) {
      case 'Date':
        return this.datePipe.transform(fieldValue, 'LLL dd, yyyy, h:mm a');
      case 'Phone':
        return this.phonePipe.transform(fieldValue);
      default:
        return fieldValue;
    }
  }

  navigateOnClick(header, obj) {
    const navigationLink =
      header.bodyRoute && header.bodyRouteParams
        ? [header.bodyRoute, this.getValueByField(obj, header.bodyRouteParams)]
        : header.bodyRoute
        ? [header.bodyRoute]
        : null;
    if (navigationLink) {
      this.router.navigate(navigationLink);
    }
  }

  action(object, index) {
    this.actionBtnHandler.emit({object, index});
  }

  conditionalBtnClicked(object, index) {
    this.conditionalBtnHandler.emit({object, index});
  }

  sortField({field, order, orderIndex}, sortField) {
    if (!this.multiSort) {
      this.sortedFields = {};
    }
    this.sortedFields[sortField || field] = orderIndex;
    this.sort.emit({
      field: this.multiSort ? Object.keys(this.sortedFields) : sortField || field,
      order: this.multiSort ? Object.values(this.sortedFields) : order,
    });
  }

  getOrderIndex(header) {
    const index = header.sortField || header.field;
    return this.sortedFields[index] || 0;
  }

  onRowSelection(event, checked) {
    this.rowSelected.emit({
      selectedRows: this.selectedRows,
      currentRow: event.data,
      checked,
    });
  }

  getRouterLink(object, currentHeaders) {
    const header = currentHeaders.find(item => item?.editBtn);
    return [header?.editBtnLink, object[header?.linkParams]];
  }

  getQueryParams(object, currentHeaders) {
    const header = currentHeaders.find(item => item?.editBtn);
    return object[header?.queryParams];
  }

  getBodyClassByFieldValue(obj, fieldName, bodyClassType) {
    const fieldValue = getObjectValueByKey(obj, fieldName) || '';
    switch (bodyClassType) {
      case 'PatientStatus':
        return fieldValue ? (fieldValue === 'A' ? 'success' : 'info') : '';
      default:
        return '';
    }
  }

  showTooltip(obj, fieldName) {
    const fieldValue = getObjectValueByKey(obj, fieldName);
    if (fieldName === 'assignedCount') {
      return parseInt(fieldValue, 10);
    } else {
      return fieldValue;
    }
  }
  showTooltipContent(obj, fieldName) {
    const fieldValue = getObjectValueByKey(obj, fieldName);
    return fieldValue;
  }

  getRowClass(obj, type, index) {
    if (type === 'inventory') {
      if (+obj.quantityAvailable < 0) {
        const addonClass = index % 2 === 0 ? '' : ' light';
        return `tr-red${addonClass}`;
      }
    }
  }
}
