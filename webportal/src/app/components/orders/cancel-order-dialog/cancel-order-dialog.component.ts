import {Component, EventEmitter, Input, OnInit, ViewChild, Output} from '@angular/core';
import {EnumNames} from 'src/app/enums';
import {Table, TableHeaderCheckbox} from 'primeng-lts/table';
import {deepCloneObject, getEnum} from 'src/app/utils';
import {OrderLineItemStatus, OrderTypes} from 'src/app/enum-constants';

@Component({
  selector: 'app-cancel-order-dialog',
  templateUrl: './cancel-order-dialog.component.html',
  styleUrls: ['./cancel-order-dialog.component.scss'],
})
export class CancelOrderDialogComponent implements OnInit {
  @Input() isVisible: boolean;
  @Input() loading = false;
  @Input() set order(orderInfo: any) {
    this.orderDetails = orderInfo;
    this.showOrderLineItemType = this.orderDetails?.orderTypeId === OrderTypes.Exchange;
  }
  @Input() fulfilledItems: any[] = [];
  @Output() closeDialog = new EventEmitter<any>();
  @Output() cancelOrder = new EventEmitter<any>();
  private headerCheckBox: TableHeaderCheckbox;
  @ViewChild('headerCheckBox') set setHeaderCheckbox(checkbox) {
    this.headerCheckBox = checkbox;
    if (this.headerCheckBox) {
      this.headerCheckBox.updateCheckedState = this.checkHeadercheckboxState.bind(this);
    }
  }
  @ViewChild('lineItemsTable')
  private lineItemsTable: Table;
  orderDetails: any;
  orderTypes = getEnum(EnumNames.OrderTypes);
  showOrderLineItemType = false;
  newOrderNotes = [];

  constructor() {}

  ngOnInit(): void {}

  CloseCancelOrderDialog() {
    this.closeDialog.emit();
  }

  CancelOrder() {
    const newOrderLineItems = this.getNewOrderLineItems();
    if (this.newOrderNotes) {
      const notes = this.newOrderNotes.map(n => {
        return {note: n, hospiceMemberId: this.orderDetails.hospiceMemberId};
      });
      this.orderDetails.orderNotes = this.orderDetails?.orderNotes?.concat(notes);
    }

    const cancelOrderPayload = {
      id: this.orderDetails.id,
      hospiceId: this.orderDetails.hospiceId,
      hospiceLocationId: this.orderDetails.hospiceLocationId,
      hospiceMemberId: this.orderDetails.hospiceMemberId,
      patientUuid: this.orderDetails.patientUuid,
      orderTypeId: this.orderDetails.orderTypeId,
      orderStatusId: this.orderDetails.statusId,
      orderNotes: this.orderDetails.orderNotes.map(orn => {
        return {...orn};
      }),
      statOrder: this.orderDetails.statOrder,
      requestedStartDateTime: this.orderDetails.requestedStartDateTime,
      requestedEndDateTime: this.orderDetails.requestedEndDateTime,
      confirmationNumber: this.orderDetails.confirmationNumber,
      orderDateTime: this.orderDetails.orderDateTime,
      externalOrderNumber: this.orderDetails.externalOrderNumber,
      pickupReason: this.orderDetails.pickupReason,
      deliveryAddress: {...this.orderDetails.deliveryAddress},
      pickupAddress: {...this.orderDetails.pickupAddress},
      orderLineItems: newOrderLineItems,
    };
    this.cancelOrder.emit({
      updatedOrderHeader: deepCloneObject(cancelOrderPayload),
    });
  }

  getNewOrderLineItems() {
    const cancelledItemIds = this.getCancelledLineItemIds();
    return this.orderDetails.orderLineItems.filter(oli => {
      return !cancelledItemIds.includes(oli.id);
    });
  }

  getCancelledLineItemIds(): any[] {
    return this.lineItemsTable?.selection.reduce((ids: any, cancelledItem: any) => {
      ids.push(cancelledItem.id);
      if (this.orderDetails.orderTypeId === OrderTypes.Move) {
        this.findCorrespondingDeliveryOrder(cancelledItem, ids);
      }
      return ids;
    }, []);
  }

  findCorrespondingDeliveryOrder(pickupLineItem: any, cancelledItemIds: any[]) {
    const statuses = [OrderLineItemStatus.PartialFulfillment, OrderLineItemStatus.Completed];
    const correspondingDeliveryItem = this.orderDetails.orderLineItems.find((oli: any) => {
      return (
        pickupLineItem.itemId === oli.itemId &&
        !statuses.includes(oli.statusId) &&
        oli.actionId === OrderTypes.Delivery &&
        !cancelledItemIds.includes(oli.id)
      );
    });

    if (correspondingDeliveryItem) {
      // updating the cancelledItemIds by reference so, dont' modify this array
      cancelledItemIds.push(correspondingDeliveryItem.id);
    }
  }

  shouldEnableSubmitButton() {
    return (
      this.loading ||
      (this.lineItemsTable?.selection &&
        this.lineItemsTable?.selection.length > 0 &&
        this.newOrderNotes.length > 0)
    );
  }

  shouldShowAddr(addrType) {
    return (
      addrType === this.orderDetails?.orderType ||
      this.orderDetails?.orderTypeId === OrderTypes.Move
    );
  }

  getFormattedAddress(address: any) {
    if (!address || (!address.addressLine1 && !address.state)) {
      return `Address not available`;
    }
    return `${address.addressLine1 ? address.addressLine1 + ',' : ','}
     ${address.addressLine2 ? address.addressLine2 + ',' : ''} ${
      address.city ? address.city + ',' : ''
    }
    ${address.state ? address.state : ''}`;
  }

  isPartiallyFulfilled(lineItemId) {
    if (!this.fulfilledItems || !this.fulfilledItems.length) {
      return false;
    }
    const fulfilledItemIndex = this.fulfilledItems.findIndex(
      fli => fli.orderLineItemId === lineItemId
    );
    return fulfilledItemIndex >= 0;
  }

  checkHeadercheckboxState(): boolean {
    if (!this.lineItemsTable) {
      return false;
    }
    const records: any[] = this.lineItemsTable.filteredValue || this.lineItemsTable.value;
    const selection: any[] = this.lineItemsTable.selection
      ? this.lineItemsTable.selection?.filter(a => !this.isPartiallyFulfilled(a.id))
      : [];
    this.lineItemsTable.selection = [...selection];
    this.lineItemsTable.updateSelectionKeys();

    if (records.length === selection.length) {
      this.updateHeaderCheckBoxState('all');
      return true;
    } else if (selection.length > 0) {
      this.updateHeaderCheckBoxState('partial');
      return true;
    } else {
      this.updateHeaderCheckBoxState(null);
      return false;
    }
  }

  updateHeaderCheckBoxState(checkedState) {
    if (this.headerCheckBox?.boxViewChild?.nativeElement?.className) {
      let className = 'p-checkbox-box';
      if (checkedState === 'partial' || checkedState === 'all') {
        className += ' p-highlight';
      }
      this.headerCheckBox.boxViewChild.nativeElement.className = className;
    }
    if (this.headerCheckBox?.boxViewChild?.nativeElement?.children[0]?.className) {
      let className = 'p-checkbox-icon';
      if (checkedState === 'all') {
        className += ' pi pi-check';
      } else if (checkedState === 'partial') {
        className += ' pi pi-minus';
      }
      this.headerCheckBox.boxViewChild.nativeElement.children[0].className = className;
    }
  }

  resetTableSelection() {
    if (this.lineItemsTable) {
      this.lineItemsTable.selection = [];
      this.lineItemsTable.updateSelectionKeys();
    }
    if (this.headerCheckBox) {
      this.headerCheckBox.checked = false;
    }
    this.updateHeaderCheckBoxState(null);
  }

  getRowTooltip(orderLineItem) {
    const fulfilledItem = this.fulfilledItems.find(i => i.orderLineItemId === orderLineItem.id);

    if (fulfilledItem) {
      if (orderLineItem.itemCount === fulfilledItem.quantity) {
        return 'Item is completely fulfilled';
      } else {
        return 'Item is partially fulfilled';
      }
    }
  }

  getDateFromString(dateString: string) {
    return new Date(dateString);
  }

  getOrderLineItems() {
    if (this.orderDetails?.orderTypeId === OrderTypes.Move) {
      return this.orderDetails?.orderLineItems.filter(oli => {
        return oli.actionId === OrderTypes.Pickup;
      });
    }
    return this.orderDetails?.orderLineItems.map(oli => {
      return oli;
    });
  }
}
