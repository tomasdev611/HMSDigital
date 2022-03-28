import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {getEnum, getFormattedPhoneNumber, IsPermissionAssigned} from 'src/app/utils';
import {EnumNames} from 'src/app/enums';
import {displayOrderStatus} from 'src/app/utils';
@Component({
  selector: 'app-dispatch-list-item',
  templateUrl: './dispatch-list-item.component.html',
  styleUrls: ['./dispatch-list-item.component.scss'],
})
export class DispatchListItemComponent implements OnInit {
  @Input() order;
  @Input() activeView;
  @Input() markedOrderId;
  @Output() orderSelected = new EventEmitter<any>();
  orderTypes = getEnum(EnumNames.OrderTypes);
  patientMoveId = null;
  pickupId = null;
  formatPhoneNumber = getFormattedPhoneNumber;
  orderStatusTypes = getEnum(EnumNames.OrderHeaderStatusTypes);
  pendingApprovalId = this.orderStatusTypes.find(x => x.name === 'Pending_Approval')?.id;
  constructor() {
    this.patientMoveId = this.orderTypes.find(x => x.name === 'Patient_Move');
    this.pickupId = this.orderTypes.find(x => x.name === 'Pickup');
  }

  ngOnInit(): void {}

  selectOrder(event, order) {
    this.orderSelected.emit({checkbox: event, order});
  }
  getOrderList(orderList) {
    return `${orderList?.map(x => {
      return ' ' + x?.item?.name;
    })}`;
  }
  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }
  saveFulfillmentFrom(route) {
    sessionStorage.setItem('orderFulfillmentFrom', route);
  }
  getOrderStatus(status) {
    return displayOrderStatus(status);
  }
  getDriverName(order) {
    if (
      ['Scheduled', 'Enroute', 'OutForFulfillment', 'OnTruck', 'OnSite'].includes(
        order.dispatchStatus
      )
    ) {
      return order.driver ? ` - ${order.driver}` : ' - No driver on truck';
    }
    return '';
  }
}
