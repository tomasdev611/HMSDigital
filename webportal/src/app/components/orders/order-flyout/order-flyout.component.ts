import {
  Component,
  OnChanges,
  EventEmitter,
  Input,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import {Router} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {EnumNames} from 'src/app/enums';
import {OrderHeadersService, ToastService} from 'src/app/services';
import {
  getEnum,
  getFormattedPhoneNumber,
  getIsInternalUser,
  IsPermissionAssigned,
} from 'src/app/utils';

@Component({
  selector: 'app-order-flyout',
  templateUrl: './order-flyout.component.html',
  styleUrls: ['./order-flyout.component.scss'],
})
export class OrderFlyoutComponent implements OnInit, OnChanges {
  @Input() currentOrder;
  @Input() fulfilledItems;
  @Input() accessPatientProfile?: boolean;
  @Output() closeFlyout = new EventEmitter<any>();
  @Output() reloadOrderTable = new EventEmitter<any>();
  formatPhoneNumber = getFormattedPhoneNumber;
  completedOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Completed'
  )?.id;
  isInternalUser = getIsInternalUser();
  showEditOrder = false;
  showCancelOrder = false;
  shouldShowCancelOrderDialog = false;
  pickupOrderTypeId = getEnum(EnumNames.OrderTypes).find(x => x.name === 'Pickup')?.id;
  cancelOrderLoading = false;

  constructor(
    private orderHeaderService: OrderHeadersService,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.currentOrder) {
      this.checkEditOrderAvailable();
      this.checkCancelOrderAvailable();
    }
  }

  getFormattedAddress(addressType = 'delivery') {
    let address;
    switch (addressType) {
      case 'delivery':
        address = this.currentOrder?.deliveryAddress;
        break;
      case 'pickup':
        address = this.currentOrder?.pickupAddress;
        break;
    }
    if (address) {
      return `${address.addressLine1 || ''} ${address.addressLine2 || ''} ${
        address.addressLine3 || ''
      },
       ${address.city || ''}, ${address.state || ''} ${address.zipCode || 0} - ${
        address.plus4Code || 0
      }`;
    }
    return '';
  }

  getPrimaryContact() {
    if (this.currentOrder?.patient?.phoneNumbers?.length) {
      const contact = this.currentOrder?.patient?.phoneNumbers?.find(
        p => p.numberType === 'Personal'
      );
      if (contact) {
        return getFormattedPhoneNumber(contact?.number);
      }
    }
    return '';
  }

  shouldShowAddr(type = 'delivery') {
    if (this.currentOrder) {
      switch (type) {
        case 'delivery':
          return ['Delivery', 'Respite', 'Exchange', 'Move'].includes(this.currentOrder?.orderType);
        case 'pickup':
          return ['Pickup', 'Move'].includes(this.currentOrder?.orderType);
      }
    }
    return false;
  }

  getFulfilledItemsForLineItem(orderLineItemId) {
    return this.fulfilledItems?.filter(fi => fi.orderLineItemId === orderLineItemId);
  }

  getFulfilledLineItemsCount(orderLineItemId) {
    return this.getFulfilledItemsForLineItem(orderLineItemId).reduce((a: any, b: any) => {
      return a + b.quantity;
    }, 0);
  }

  closeOrderDetails() {
    this.closeFlyout.emit();
  }

  editOrder() {
    const editOrderUrl = `orders/edit/${this.currentOrder.orderType.toLowerCase()}/${
      this.currentOrder.patientUuid
    }/${this.currentOrder.id}`;
    this.router.navigate([editOrderUrl]);
  }

  cancelOrder() {
    this.shouldShowCancelOrderDialog = true;
  }

  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }

  getEquipmentSettingValue(equipment) {
    const keyNames = Object.keys(equipment);
    return equipment[keyNames[0]];
  }

  getEquipmentSettingName(equipment) {
    const keyNames = Object.keys(equipment);
    return keyNames[0];
  }

  checkEditOrderAvailable() {
    const updatePermission = this.checkPermission('Orders', 'Update');
    if (
      !['Cancelled', 'Completed'].includes(this.currentOrder?.orderStatus) &&
      this.currentOrder?.orderNumber &&
      updatePermission
    ) {
      this.showEditOrder = true;
    } else {
      this.showEditOrder = false;
    }
  }

  checkCancelOrderAvailable() {
    const updatePermission = this.checkPermission('Orders', 'Update');
    if (
      !['Cancelled', 'Completed'].includes(this.currentOrder?.orderStatus) &&
      this.currentOrder?.orderNumber &&
      updatePermission
    ) {
      this.showCancelOrder = true;
    } else {
      this.showCancelOrder = false;
    }
  }

  closeCancelOrderDialog() {
    this.shouldShowCancelOrderDialog = false;
  }

  cancelOrderLineItems({updatedOrderHeader}) {
    this.cancelOrderLoading = true;

    this.shouldShowCancelOrderDialog = false;
    this.orderHeaderService
      .updateOrderHeader(updatedOrderHeader)
      .pipe(
        finalize(() => {
          this.shouldShowCancelOrderDialog = false;
          this.cancelOrderLoading = false;
        })
      )
      .subscribe((res: any) => {
        this.toastService.showSuccess(
          `Order ${this.currentOrder.orderNumber} cancelled successfully`
        );
        this.reloadTables();
      });
  }

  reloadTables() {
    this.closeFlyout.emit();
    this.reloadOrderTable.emit();
  }
}
