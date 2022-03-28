import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {getFormattedPhoneNumber} from 'src/app/utils';

@Component({
  selector: 'app-current-inventory-order',
  templateUrl: './current-inventory-order.component.html',
  styleUrls: ['./current-inventory-order.component.scss'],
})
export class CurrentInventoryOrderComponent implements OnInit {
  formatPhoneNumber = getFormattedPhoneNumber;

  @Input() patientInfo: any;
  @Input() editmode = false;
  @Input() patientInventories: any;
  @Input() cartItems: any;
  @Input() showCurrentOrder = true;
  @Input() fulfilledItems: any;
  @Input() showUpdatePatientLink = true;

  @Output() updateCartHandler = new EventEmitter<any>();
  activeState = {
    currentOrder: true,
    currentInventory: false,
  };
  constructor() {}

  ngOnInit(): void {}

  updateCart(action: string, product: any) {
    this.updateCartHandler.emit({action, product});
  }

  getEquipmentSettingValue(equipment) {
    const keyNames = Object.keys(equipment);
    return equipment[keyNames[0]];
  }

  getEquipmentSettingName(equipment) {
    const keyNames = Object.keys(equipment);
    return keyNames[0];
  }

  getFulfilledItemsForLineItem(orderLineItemId) {
    return this.fulfilledItems?.filter(fi => fi.orderLineItemId === orderLineItemId);
  }

  getFulfilledLineItemsCount(orderLineItemId) {
    return this.getFulfilledItemsForLineItem(orderLineItemId).reduce((a: any, b: any) => {
      return a + b.quantity;
    }, 0);
  }

  checkDisabled(item: any, type: string) {
    const fulfilledItems = this.getFulfilledLineItemsCount(item.id) || 0;
    switch (type) {
      case 'decrement':
        return item.count > fulfilledItems ? false : true;
      case 'delete':
        return fulfilledItems > 0 ? true : false;
      default:
        return;
    }
  }
}
