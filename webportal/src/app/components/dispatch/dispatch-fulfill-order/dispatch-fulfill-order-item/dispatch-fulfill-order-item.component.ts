import {Component, Input, OnInit} from '@angular/core';
import {getFormattedPhoneNumber} from 'src/app/utils';

@Component({
  selector: 'app-dispatch-fulfill-order-item',
  templateUrl: './dispatch-fulfill-order-item.component.html',
  styleUrls: ['./dispatch-fulfill-order-item.component.scss'],
})
export class DispatchFulfillOrderItemComponent implements OnInit {
  @Input() order;

  formatPhoneNumber = getFormattedPhoneNumber;
  constructor() {}

  ngOnInit(): void {}

  getFormattedAddress(addressType = 'delivery') {
    let address;
    switch (addressType) {
      case 'delivery':
        address = this.order?.deliveryAddress;
        break;
      case 'pickup':
        address = this.order?.pickupAddress;
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

  shouldShowAddr(type = 'delivery') {
    if (this.order) {
      switch (type) {
        case 'delivery':
          return ['Delivery', 'Respite', 'Exchange', 'Move'].includes(this.order?.orderType);
          break;
        case 'pickup':
          return ['Pickup', 'Move'].includes(this.order?.orderType);
          break;
      }
    }
    return false;
  }
}
