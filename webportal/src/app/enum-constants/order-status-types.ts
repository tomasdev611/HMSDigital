import {stringEqualsIgnoreCase, getEnum} from '../utils';
import {EnumNames} from '../enums/enumeration-names';

class OrderHeaderStatusTypes {
  orderHeaderStatusTypeList = getEnum(EnumNames.OrderHeaderStatusTypes);

  private plannedId = null;
  get Planned() {
    if (!this.plannedId) {
      this.plannedId = this.findOrderStatusId('Planned');
    }
    return this.plannedId;
  }

  private scheduledId = null;
  get Scheduled() {
    if (!this.scheduledId) {
      this.scheduledId = this.findOrderStatusId('Scheduled');
    }
    return this.scheduledId;
  }

  private stagedId = null;
  get Staged() {
    if (!this.stagedId) {
      this.stagedId = this.findOrderStatusId('Staged');
    }
    return this.stagedId;
  }

  private onTruckId = null;
  get OnTruck() {
    if (!this.onTruckId) {
      this.onTruckId = this.findOrderStatusId('OnTruck');
    }
    return this.onTruckId;
  }

  private outForFulfillmentId = null;
  get OutForFulfillment() {
    if (!this.outForFulfillmentId) {
      this.outForFulfillmentId = this.findOrderStatusId('OutForFulfillment');
    }
    return this.outForFulfillmentId;
  }

  private completedId = null;
  get Completed() {
    if (!this.completedId) {
      this.completedId = this.findOrderStatusId('Completed');
    }
    return this.completedId;
  }

  private backOrderedId = null;
  get BackOrdered() {
    if (!this.backOrderedId) {
      this.backOrderedId = this.findOrderStatusId('BackOrdered');
    }
    return this.backOrderedId;
  }

  private cancelledId = null;
  get Cancelled() {
    if (!this.cancelledId) {
      this.cancelledId = this.findOrderStatusId('Cancelled');
    }
    return this.cancelledId;
  }

  private savedId = null;
  get Saved() {
    if (!this.savedId) {
      this.savedId = this.findOrderStatusId('Saved');
    }
    return this.savedId;
  }

  private pendingApprovalId = null;
  get PendingApproval() {
    if (!this.pendingApprovalId) {
      this.pendingApprovalId = this.findOrderStatusId('Pending_Approval');
    }
    return this.pendingApprovalId;
  }

  private submittedId = null;
  get Submitted() {
    if (!this.submittedId) {
      this.submittedId = this.findOrderStatusId('Submitted');
    }
    return this.submittedId;
  }

  private partialFulfillmentId = null;
  get PartialFulfillment() {
    if (!this.partialFulfillmentId) {
      this.partialFulfillmentId = this.findOrderStatusId('Partial_Fulfillment');
    }
    return this.partialFulfillmentId;
  }

  private preLoadId = null;
  get PreLoad() {
    if (!this.preLoadId) {
      this.preLoadId = this.findOrderStatusId('PreLoad');
    }
    return this.preLoadId;
  }

  private loadingTruckId = null;
  get LoadingTruck() {
    if (!this.loadingTruckId) {
      this.loadingTruckId = this.findOrderStatusId('Loading_Truck');
    }
    return this.loadingTruckId;
  }

  private enrouteId = null;
  get Enroute() {
    if (!this.enrouteId) {
      this.enrouteId = this.findOrderStatusId('Enroute');
    }
    return this.enrouteId;
  }

  private onSiteId = null;
  get OnSite() {
    if (!this.onSiteId) {
      this.onSiteId = this.findOrderStatusId('OnSite');
    }
    return this.onSiteId;
  }

  private findOrderStatusId(orderStatusType: string) {
    return this.orderHeaderStatusTypeList.find(ost =>
      stringEqualsIgnoreCase(ost.name, orderStatusType)
    )?.id;
  }
}

const OrderHeaderStatus = new OrderHeaderStatusTypes();
export {OrderHeaderStatus};
