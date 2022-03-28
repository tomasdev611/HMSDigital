import {stringEqualsIgnoreCase, getEnum} from '../utils';
import {EnumNames} from '../enums/enumeration-names';

class OrderTypeConstants {
  orderTypesList = getEnum(EnumNames.OrderTypes);

  private deliveryId = null;
  get Delivery() {
    if (!this.deliveryId) {
      this.deliveryId = this.findOrderTypeId('Delivery');
    }
    return this.deliveryId;
  }

  private exchangeId = null;
  get Exchange() {
    if (!this.exchangeId) {
      this.exchangeId = this.findOrderTypeId('Exchange');
    }
    return this.exchangeId;
  }

  private moveId = null;
  get Move() {
    if (!this.moveId) {
      this.moveId = this.findOrderTypeId('Patient_Move');
    }
    return this.moveId;
  }

  private pickupId = null;
  get Pickup() {
    if (!this.pickupId) {
      this.pickupId = this.findOrderTypeId('Pickup');
    }
    return this.pickupId;
  }

  private findOrderTypeId(orderType: string) {
    return this.orderTypesList.find(ot => stringEqualsIgnoreCase(ot.name, orderType))?.id;
  }

  findOrderTypeLabel(orderTypeId: number) {
    return this.orderTypesList.find(ot => ot.id === orderTypeId)?.name;
  }
}

const OrderTypes = new OrderTypeConstants();
export {OrderTypes};
