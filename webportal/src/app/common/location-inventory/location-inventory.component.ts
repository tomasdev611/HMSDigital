import {Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {PaginationResponse} from 'src/app/models';
import {EnumNames, SieveOperators} from 'src/app/enums';
import {buildFilterString, getEnum} from 'src/app/utils';
import {finalize} from 'rxjs/operators';
import {DispatchService, InventoryService} from 'src/app/services';

@Component({
  selector: 'app-location-inventory',
  templateUrl: './location-inventory.component.html',
  styleUrls: ['./location-inventory.component.scss'],
})
export class LocationInventoryComponent implements OnInit {
  @Input() locationType: string;
  @Input() locationId: number;
  @Input() selectInventory = false;
  @Input() selectedVehicle: any;
  @Output() inventorySelected = new EventEmitter<any>();

  inventoryResponse: PaginationResponse;
  inventoryRequest = {
    filters: '',
    sorts: '-assetTagNumber,itemName',
  };
  inventoryFilter = [];
  loading = false;
  inventoryHeaders = [
    {
      label: 'Item Name',
      field: 'item.name',
      sortable: true,
      sortField: 'itemName',
      class: 'sm value-d-flex',
    },
    {label: 'Asset Tag', field: 'assetTagNumber', class: 'sm'},
    {label: 'Serial Number', field: 'serialNumber', class: 'md'},
    {
      label: 'Count',
      field: 'count',
      sortable: true,
      sortField: 'count',
      class: 'sm',
    },
    {label: 'Status', field: 'status', class: 'sm'},
  ];
  completedOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Completed'
  )?.id;
  cancelledOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Cancelled'
  )?.id;
  assignedItems = [];
  constructor(
    private inventoryService: InventoryService,
    private dispatchService: DispatchService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.setInventoryFilter();
  }

  setInventoryFilter() {
    this.inventoryFilter = [
      {
        field: 'currentLocationId',
        value: [this.locationId],
        operator: SieveOperators.Equals,
      },
    ];
    if (this.locationType === 'Vehicle') {
      const availableCount = {
        label: 'Available Count',
        field: 'quantityAvailable',
        class: 'sm',
      };
      const assignedCount = {
        label: 'Assigned Count',
        field: 'assignedCount',
        class: 'sm',
        showTooltip: 'assignedCount',
        tooltipIcon: 'pi pi-info-circle',
        showTooltipContent: 'assignedTo',
      };
      this.inventoryHeaders.splice(this.inventoryHeaders.length - 1, 0, availableCount);
      this.inventoryHeaders.splice(this.inventoryHeaders.length - 1, 0, assignedCount);
      const countFilter = {
        field: 'count',
        value: [0],
        operator: SieveOperators.NotEquals,
      };
      this.inventoryFilter = [...this.inventoryFilter, countFilter];
    } else {
      this.inventoryHeaders = this.inventoryHeaders.map(x => {
        if (x.field === 'assetTagNumber' || x.field === 'serialNumber') {
          x.class = '';
        }
        return x;
      });
    }
    this.getInventory();
  }

  getInventory() {
    if (!this.locationId) {
      return;
    }
    this.inventoryRequest.filters = buildFilterString(this.inventoryFilter);
    this.loading = true;
    this.inventoryService
      .getInvetoryList(this.inventoryRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.getDispatchInstructions();
        })
      )
      .subscribe((res: PaginationResponse) => {
        this.inventoryResponse = res;
        this.cd.markForCheck();
      });
  }

  sortInventory(event) {
    switch (event.order) {
      case 0:
        this.inventoryRequest.sorts = '';
        break;
      case 1:
        this.inventoryRequest.sorts = event.field;
        break;
      case -1:
        this.inventoryRequest.sorts = '-' + event.field;
        break;
    }
    this.getInventory();
  }

  getDispatchInstructions() {
    const filters = [
      {
        field: 'vehicleId',
        operator: SieveOperators.Equals,
        value: [this.locationId],
      },
    ];
    const dispatchInsReq = {
      filters: buildFilterString(filters),
    };
    this.dispatchService.getAllDispatchInstructions(dispatchInsReq).subscribe((result: any) => {
      const assignedOrderHeaders = result?.records?.filter(
        x => x?.orderHeader?.orderType === 'Delivery' && x?.orderHeader?.orderStatus !== 'Completed'
      );
      assignedOrderHeaders.map(order => {
        order.orderHeader.orderLineItems = order?.orderHeader?.orderLineItems.map(x => {
          x.orderNumber = order.orderHeader.orderNumber;
          return x;
        });
        this.assignedItems = [...this.assignedItems, ...order?.orderHeader?.orderLineItems];
      });
      this.assignedItems.map(x => {
        const item: any = this.inventoryResponse.records.findIndex(y => y.itemId === x.itemId);
        if (item === -1) {
          const items = this.assignedItems.filter(i => i?.itemId === item.itemId);
          const assignedItemCount = items.reduce((a: any, b: any) => {
            return a + b.itemCount;
          }, 0);
          const itemAbsentInInventory = {
            assetTagNumber: x?.assetTagNumber,
            count: -(items?.length ? assignedItemCount : x?.itemCount),
            currentLocationId: null,
            id: x?.id,
            item: x?.item,
            itemId: x?.itemId,
            lotNumber: x?.lotNumber,
            netSuiteInventoryId: null,
            quantityAvailable: 0,
            serialNumber: x?.serialNumber,
            status: '-',
            statusId: null,
          };
          this.inventoryResponse.records = [
            itemAbsentInInventory,
            ...this.inventoryResponse.records,
          ];
        }
      });

      this.inventoryResponse.records = this.inventoryResponse.records.map(x => {
        const items = this.assignedItems.filter(item => item?.itemId === x.itemId);
        const assignedItemCount = items.reduce((a: any, b: any) => {
          return a + b.itemCount;
        }, 0);
        x.quantityAvailable = (x.quantityAvailable - assignedItemCount).toString();
        x.assignedCount = assignedItemCount || '0';
        x.assignedTo = items.reduce((a: any, b: any) => {
          return a + ' ' + `<span>#${b?.orderNumber} : ${b.itemCount}</span><br>`;
        }, ``);
        return x;
      });
      this.cd.markForCheck();
    });
  }
}
