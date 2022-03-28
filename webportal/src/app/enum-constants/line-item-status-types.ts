import {stringEqualsIgnoreCase, getEnum} from '../utils';
import {EnumNames} from '../enums/enumeration-names';

class OrderLineItemStatusTypes {
  orderLineStatusTypesList = getEnum(EnumNames.orderLineItemStatusTypes);

  private plannedId = null;
  get Planned() {
    if (!this.plannedId) {
      this.plannedId = this.findLineItemStatusId('Planned');
    }
    return this.plannedId;
  }

  private scheduledId = null;
  get Scheduled() {
    if (!this.scheduledId) {
      this.scheduledId = this.findLineItemStatusId('Scheduled');
    }
    return this.scheduledId;
  }

  private stagedId = null;
  get Staged() {
    if (!this.stagedId) {
      this.stagedId = this.findLineItemStatusId('Staged');
    }
    return this.stagedId;
  }

  private onTruckId = null;
  get OnTruck() {
    if (!this.onTruckId) {
      this.onTruckId = this.findLineItemStatusId('OnTruck');
    }
    return this.onTruckId;
  }

  private outForFulfillmentId = null;
  get OutForFulfillment() {
    if (!this.outForFulfillmentId) {
      this.outForFulfillmentId = this.findLineItemStatusId('OutForFulfillment');
    }
    return this.outForFulfillmentId;
  }

  private completedId = null;
  get Completed() {
    if (!this.completedId) {
      this.completedId = this.findLineItemStatusId('Completed');
    }
    return this.completedId;
  }

  private backOrderedId = null;
  get BackOrdered() {
    if (!this.backOrderedId) {
      this.backOrderedId = this.findLineItemStatusId('BackOrdered');
    }
    return this.backOrderedId;
  }

  private cancelledId = null;
  get Cancelled() {
    if (!this.cancelledId) {
      this.cancelledId = this.findLineItemStatusId('Cancelled');
    }
    return this.cancelledId;
  }

  private partialFulfillmentId = null;
  get PartialFulfillment() {
    if (!this.partialFulfillmentId) {
      this.partialFulfillmentId = this.findLineItemStatusId('Partial_Fulfillment');
    }
    return this.partialFulfillmentId;
  }

  private preLoadId = null;
  get PreLoad() {
    if (!this.preLoadId) {
      this.preLoadId = this.findLineItemStatusId('PreLoad');
    }
    return this.preLoadId;
  }

  private loadingTruckId = null;
  get LoadingTruck() {
    if (!this.loadingTruckId) {
      this.loadingTruckId = this.findLineItemStatusId('Loading_Truck');
    }
    return this.loadingTruckId;
  }

  private enrouteId = null;
  get Enroute() {
    if (!this.enrouteId) {
      this.enrouteId = this.findLineItemStatusId('Enroute');
    }
    return this.enrouteId;
  }

  private onSiteId = null;
  get OnSite() {
    if (!this.onSiteId) {
      this.onSiteId = this.findLineItemStatusId('OnSite');
    }
    return this.onSiteId;
  }

  private findLineItemStatusId(orderStatusType: string) {
    return this.orderLineStatusTypesList.find(ost =>
      stringEqualsIgnoreCase(ost.name, orderStatusType)
    )?.id;
  }
}

const OrderLineItemStatus = new OrderLineItemStatusTypes();

export {OrderLineItemStatus};
