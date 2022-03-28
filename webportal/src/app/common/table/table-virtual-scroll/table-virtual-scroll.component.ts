import {DatePipe} from '@angular/common';
import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ChangeDetectorRef,
  AfterContentChecked,
  SimpleChanges,
  OnChanges,
  ViewChild,
} from '@angular/core';
import {Router} from '@angular/router';
import {LazyLoadEvent} from 'primeng/api';
import {Table} from 'primeng/table/table';
import {PhonePipe} from 'src/app/pipes';
import {getObjectValueByKey} from 'src/app/utils';
@Component({
  selector: 'app-table-virtual-scroll',
  templateUrl: './table-virtual-scroll.component.html',
  styleUrls: ['./table-virtual-scroll.component.scss'],
})
export class TableVirtualScrollComponent implements OnInit, AfterContentChecked, OnChanges {
  @Input() list = [];
  @Input() pageSize: number;
  @Input() totalPage: number;
  @Input() loading: boolean;
  @Input() filterFields: [];
  @Input() headers: [];
  @Input() pageNumber: number;
  @Input() show = true;
  @Input() submit;
  @Input() paginate = true;
  @Input() multiSort = false;
  @Input() multiSelection = false;
  @Input() selection = false;
  @Input() checkboxSelection = false;
  @Input() pageLoadNextOffset = 0;
  @Input() showHideOption = true;
  @Input() moreCompareField: string;
  @Input() continuationTokenLazyLoading: boolean;

  @Input() totalRecords = 0;

  @Output() toggle = new EventEmitter<any>();
  @Output() fetchNext = new EventEmitter<any>();
  @Output() deleteHandler = new EventEmitter<any>();
  @Output() actionBtnHandler = new EventEmitter<any>();
  @Output() conditionalBtnHandler = new EventEmitter<any>();
  @Output() sort = new EventEmitter<any>();
  @Output() rowSelected = new EventEmitter<any>();

  @ViewChild('dataVirtualTable', {static: false}) dataVirtualTable: Table;
  @Input() scrollHeightCustom: any;
  sortedFields = {};
  selectedRows = [];

  virtualScrollList: Array<any>;
  virtualTotalRecordsAux: number;
  visible = true;
  moreValue = '';
  previousList = null;
  tooltipZIndex = 9999;

  constructor(
    private router: Router,
    private datePipe: DatePipe,
    private changeDetector: ChangeDetectorRef,
    private phonePipe: PhonePipe
  ) {}

  ngOnInit(): void {}

  ngOnChanges(changes: SimpleChanges): void {
    if (this.continuationTokenLazyLoading) {
      if (changes.list) {
        this.virtualScrollList = [...this.list];
      }
    } else {
      if (changes.totalRecords) {
        // ? - assumes data has been fetched and totalRecords went from 0 to some value,
        // ? - now make the whole display array empty
        if (this.list) {
          this.virtualTotalRecordsAux =
            (this.totalPage + 1) * (this.pageSize - this.pageLoadNextOffset);
          this.virtualScrollList = Array.from({
            length: this.virtualTotalRecordsAux,
          });
        }
      }
      if (changes.list) {
        // ? if the list gets updated with server response
        this.setNextData();
      }
    }
  }

  ngAfterContentChecked() {
    this.changeDetector.detectChanges();
    if (this.scrollHeightCustom && this.dataVirtualTable) {
      this.dataVirtualTable.scrollHeight = this.scrollHeightCustom;
    }
  }

  toggleAction(event, object) {
    this.toggle.emit({event, object});
  }

  delete(object, index) {
    this.deleteHandler.emit({object, index});
  }

  getValueByField(obj, fieldName, fieldType?, length = 0) {
    let fieldValue = getObjectValueByKey(obj, fieldName);
    fieldValue = length ? fieldValue.substring(0, length) : fieldValue;

    switch (fieldType) {
      case 'Date':
        return this.datePipe.transform(fieldValue, 'LLL dd, yyyy, h:mm a');
      case 'shortDate':
        return this.datePipe.transform(fieldValue, 'LL/dd/yyyy');
      case 'Phone':
        return this.phonePipe.transform(fieldValue);
      case 'BoolToYesNo':
        const value = obj[fieldName];
        return typeof value === 'boolean' ? (value ? 'Yes' : 'No') : '';
      default:
        return fieldValue;
    }
  }

  getValueCharacterLength(obj, fieldName) {
    const fieldValue = getObjectValueByKey(obj, fieldName) || '';
    return fieldValue.toString().length;
  }

  onShowMoreValue(obj) {
    this.moreValue = obj[this.moreCompareField];
  }

  onHideMoreValue() {
    this.moreValue = '';
  }

  shouldShowMore(obj, field) {
    if (this.getValueCharacterLength(obj, field) > 150) {
      if (this.moreValue !== obj[this.moreCompareField]) {
        return true;
      }
    }
    return false;
  }

  shouldHideMore(obj, field, fieldType) {
    if (this.moreValue === obj[this.moreCompareField] && field === 'renderedMessage') {
      if (this.getValueByField(obj, field, fieldType) !== '') {
        return true;
      }
    }
    return false;
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

  loadDataLazy(event: LazyLoadEvent) {
    // ? decide whether to emit fetchNext or not
    // ? can be resolved based on first and rows field of the LazyLoadEvent
    if (this.continuationTokenLazyLoading) {
      if (event.first + event.rows + this.pageLoadNextOffset > this.list.length) {
        this.fetchNext.emit();
      }
    } else {
      if (this.list && !this.loading) {
        let pageNum = Math.ceil(
          (event.first + event.rows + this.pageLoadNextOffset) / this.pageSize - 1
        );
        if (pageNum > this.totalPage) {
          pageNum = this.totalPage;
        }

        if (pageNum > 0) {
          const data = this.virtualScrollList[(pageNum - 1) * this.pageSize];
          if (data === undefined) {
            const nextPage = this.pageNumber + 1;
            if (pageNum !== nextPage) {
              pageNum = nextPage;
            }
            this.fetchNext.emit({pageNum});
          }
        }
      }
    }
  }

  setNextData() {
    if (!this.list) {
      return;
    }

    const indexFrom = (this.pageNumber - 1) * this.pageSize;
    const loadedData = this.list.slice(0, this.list.length);
    if (this.virtualScrollList) {
      Array.prototype.splice.apply(this.virtualScrollList, [
        ...[indexFrom, loadedData.length],
        ...loadedData,
      ]);
      this.virtualScrollList = [...this.virtualScrollList];

      const diffRecordsLoadedData = this.totalRecords - (indexFrom + loadedData.length);
      if (diffRecordsLoadedData === 0) {
        this.virtualScrollList = this.virtualScrollList.slice(0, this.totalRecords);
      }
      if (this.dataVirtualTable) {
        this.dataVirtualTable.scrollToVirtualIndex(indexFrom);
      }
    }
  }

  reset(): void {
    this.dataVirtualTable.reset();
    this.dataVirtualTable.resetScrollTop();

    // This forces the lazyLoadEvent to restart.
    this.list = null;
    this.visible = false;
    setTimeout(() => {
      this.visible = true;
    }, 1);
  }

  resetSort() {
    this.sortedFields = {};
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
    if (bodyClassType !== 'PatientStatus') {
      return '';
    }

    const status = getObjectValueByKey(obj, 'status') || '';
    let className = '';
    switch (status) {
      case 'Active':
        className = 'active';
        break;
      case 'Blank':
        className = 'blank';
        break;
      case 'Inactive':
        className = 'inactive';
        break;
      case 'Pending':
      case 'PendingActive':
        className = 'pending';
        break;
      case '':
      default:
        break;
    }
    return className;
  }

  showAddOnTdIcon(object) {
    return object.isExceptionFulfillment || false;
  }

  getToolTipText(obj, fieldName, tooltipField) {
    if (fieldName !== 'displayStatus') {
      return '';
    }

    const tooltipText = getObjectValueByKey(obj, tooltipField) || '';
    return tooltipText;
  }
}
