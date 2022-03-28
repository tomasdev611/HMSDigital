import {Component, EventEmitter, OnInit, Output, Input, OnChanges, ViewChild} from '@angular/core';
import {ItemsService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {finalize} from 'rxjs/operators';
import {ItemImageBaseResponse} from '../../models/model.product-image';
import {TableVirtualScrollComponent} from 'src/app/common';
import {forkJoin} from 'rxjs';
import {SieveOperators} from 'src/app/enums';
import {buildFilterString} from 'src/app/utils';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.scss'],
})
export class ItemComponent implements OnInit, OnChanges {
  loading = false;
  itemsResponse: PaginationResponse;
  itemsRequest = new SieveRequest();
  itemHeaders = [
    {label: 'Item Name', field: 'name', sortable: true},
    {label: 'Item Number', field: 'itemNumber', sortable: true},
    {label: 'Description', field: 'description'},
    {label: 'Category', field: 'category'},
  ];
  equipmentSettingsExist = false;
  addOnsExist = false;
  @Input() showToggleFilter = false;

  @ViewChild('inventoryItemTable ', {static: false})
  inventoryItemTable: TableVirtualScrollComponent;

  @Input() searchQuery: string;
  @Output() showDetails = new EventEmitter<any>();
  @Output() setTotalRecordCount = new EventEmitter<any>();

  constructor(private itemService: ItemsService) {}

  ngOnInit(): void {}

  ngOnChanges(changes: any) {
    this.getItemsList();
  }

  getItemsList() {
    const filter = [];
    if (this.equipmentSettingsExist) {
      filter.push({
        field: 'equipmentSettingsExist',
        value: [true],
        operator: SieveOperators.Equals,
      });
    }
    if (this.addOnsExist) {
      filter.push({
        field: 'AddOnsExist',
        value: [true],
        operator: SieveOperators.Equals,
      });
    }
    this.itemsRequest.filters = buildFilterString(filter);
    this.loading = true;
    (!this.searchQuery
      ? this.itemService.getItemsList(this.itemsRequest)
      : this.itemService.searchItems({
          ...this.itemsRequest,
          searchQuery: this.searchQuery,
        })
    )
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res: any) => {
        this.itemsResponse = res;
        this.itemsResponse.records = this.itemsResponse.records.map((item: any) => {
          const category = item.categories.map(c => c.name);
          item.category = category.join(', ');
          return item;
        });
        this.setTotalRecordCount.emit({
          totalCount: this.itemsResponse.totalRecordCount,
        });
      });
  }

  fetchNext({pageNum}) {
    if (!this.itemsResponse || pageNum > this.itemsResponse.totalPageCount) {
      return;
    }
    this.itemsRequest.page = pageNum;
    this.getItemsList();
  }

  itemSelected({currentRow}) {
    const itemRequests = [];
    itemRequests.push(this.itemService.getItemDetailsById(currentRow.id));
    itemRequests.push(this.itemService.getItemImages(currentRow.id));
    this.showDetails.emit({currentRow: {item: currentRow}});

    forkJoin(itemRequests).subscribe(([itemDetails, imageResponse]) => {
      this.showDetails.emit({
        currentRow: {item: itemDetails, images: imageResponse},
      });
    });
  }

  sortItems(event) {
    switch (event.order) {
      case 0:
        this.itemsRequest.sorts = '';
        break;
      case 1:
        this.itemsRequest.sorts = event.field;
        break;
      case -1:
        this.itemsRequest.sorts = '-' + event.field;
        break;
    }
    this.dataTablesReset();
    this.getItemsList();
  }

  dataTablesReset() {
    this.itemsResponse = null;
    if (this.inventoryItemTable) {
      this.inventoryItemTable.reset();
    }
    this.itemsRequest.page = 1;
  }
  toggle($event, inverseField) {
    this.dataTablesReset();
    this[inverseField] = false;
    this.getItemsList();
  }
}
