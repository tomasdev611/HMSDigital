import {DatePipe} from '@angular/common';
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Router} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {hms} from 'src/app/constants';
import {OrderHeadersService, StorageService, ToastService} from 'src/app/services';
import {checkInternalUser, deepCloneObject} from 'src/app/utils';

@Component({
  selector: 'app-order-review',
  templateUrl: './order-review.component.html',
  styleUrls: ['./order-review.component.scss'],
})
export class OrderReviewComponent implements OnInit {
  @Input() patientInfo;
  @Input() nurses;
  @Input() editmode;
  @Input() orderHeader;
  @Input() cartItems;
  @Input() patientInventories;
  @Input() deliveryDetails;
  @Input() storeKey;
  @Input() pickupTimeOptions = [];
  @Input() tmpSessionKey;
  currentDate = new Date();
  user: any;

  formSubmit = false;
  orderNotes = [];
  @Output() changeStepHandler = new EventEmitter<any>();

  products = [];
  constructor(
    private router: Router,
    private orderHeaderService: OrderHeadersService,
    private toastService: ToastService,
    private storageService: StorageService,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    const myInfo: any = localStorage.getItem('me');
    if (myInfo) {
      this.user = JSON.parse(myInfo);
    }
    this.orderNotes = this.getOrderNotesArray(
      this.deliveryDetails?.orderNotes ?? [],
      this.deliveryDetails?.orderNote,
      this.deliveryDetails?.hospiceMemberId
    );
  }

  getDateFromString(dateString: string) {
    return new Date(dateString);
  }

  navigateToAddNurse() {
    this.router.navigate([
      `${hms.orderRoutes.addNurse.pre}/${this.patientInfo.hospiceId}/${hms.orderRoutes.addNurse.post}`,
    ]);
  }

  navigateToAddAddress() {
    this.router.navigate([`/patients/edit/${this.patientInfo.id}`]);
  }

  shouldShowTpaOrderField() {
    return checkInternalUser() && this.deliveryDetails?.externalOrderNumber;
  }

  returnToAddress() {
    this.changeStepHandler.emit({
      step: 'deliveryDetails',
      deliveryDetails: this.deliveryDetails,
    });
  }

  getOrderNotesArray(existingOrderNotes, orderNote, hospiceMemberId) {
    let notes = orderNote ?? [];
    notes = notes.map(n => {
      return {
        note: n,
        createdByUserName: n?.createdByUserName ?? this.user?.name,
        createdDateTime: n?.createdDateTime ?? this.currentDate,
        hospiceMemberId,
      };
    });
    return existingOrderNotes && existingOrderNotes?.length
      ? [...existingOrderNotes, ...notes]
      : [...notes];
  }

  getOrderLineItemArray() {
    // create OrderLineItem object from PatientInventory object; for payload (create or update OrderHeader)
    return this.cartItems.map(si => this.getOrderLineItem(si));
  }
  findLineItemForInv(patientInv) {
    return this.orderHeader?.orderLineItems?.find(
      (oli: any) => oli.item.id === patientInv.itemId && this.matchSerializedItem(oli, patientInv)
    );
  }

  matchSerializedItem(orderLineItem, invItem) {
    if (!(orderLineItem.item.isAssetTagged && orderLineItem.item.isSerialized)) {
      return true;
    }
    return (
      orderLineItem.assetTagNumber === invItem.assetTagNumber ||
      orderLineItem.serialNumber === invItem.serialNumber
    );
  }

  getEquipmentSettings(settings) {
    settings = settings.map((setting: any) => {
      return {
        [setting.name]: setting.value,
      };
    });
    return settings;
  }

  getOrderLineItem(selectedPatientInv) {
    const existingOrderLineItem = this.findLineItemForInv(selectedPatientInv);
    if (existingOrderLineItem) {
      existingOrderLineItem.itemCount = selectedPatientInv.count;
      return existingOrderLineItem;
    }
    const lineItem = {
      id: selectedPatientInv.id ?? null,
      itemId: selectedPatientInv.item?.id,
      itemCount: selectedPatientInv.count,
      action: 'delivery',
      equipmentSettings: this.getEquipmentSettings(selectedPatientInv.equipmentSettings),
    };
    if (!selectedPatientInv.id) {
      delete lineItem.id;
    }
    return lineItem;
  }

  deleteUncessaryProps(body) {
    delete body.orderNote;
    delete body.requestedDate;
    delete body.deliveryTime;
    delete body.deliveryHours;
    delete body.nurse;
    if (!this.editmode) {
      delete body.id;
    }
  }
  setNewDateWithHours(date: any, hours: number, minutes = 0) {
    const newDate = new Date(date);
    return new Date(newDate.setHours(hours, minutes, 0, 0));
  }

  placeOrder() {
    const body = deepCloneObject(this.deliveryDetails);
    body.orderNotes = this.getOrderNotesArray(
      body.orderNotes,
      body.orderNote,
      body.hospiceMemberId
    );
    const currentHour = this.currentDate.getHours();
    const currentMinute = this.currentDate.getMinutes();
    body.orderLineItems = this.getOrderLineItemArray();
    body.requestedStartDateTime = this.setNewDateWithHours(
      body.statOrder ? this.currentDate : body.requestedDate,
      body.statOrder ? currentHour : body.deliveryTime.start,
      body.statOrder ? currentMinute : 0
    );
    body.requestedEndDateTime = this.setNewDateWithHours(
      body.statOrder ? this.currentDate : body.requestedDate,
      body.statOrder ? currentHour + body.deliveryHours : body.deliveryTime.end,
      body.statOrder ? currentMinute : 0
    );
    this.deleteUncessaryProps(body);
    this.formSubmit = true;
    const createOrUpdateOrderReq = this.editmode ? this.updateOrder(body) : this.createOrder(body);
    createOrUpdateOrderReq
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.storageService.remove(this.storeKey);
        this.storageService.remove(this.tmpSessionKey, 'session');
        this.router.navigate([hms.defaultRoute]);
        this.toastService.showSuccess(
          `Your order has been successfully ${this.editmode ? 'updated' : 'created'}`
        );
      });
  }

  createOrder(body) {
    return this.orderHeaderService.createOrderHeader(body);
  }

  updateOrder(body) {
    return this.orderHeaderService.updateOrderHeader(body);
  }

  getTime(type) {
    let hours = '';
    let date = '';

    if (this.pickupTimeOptions && this.deliveryDetails?.deliveryTime) {
      const time = this.pickupTimeOptions.find(t => {
        return (
          t.value.start === this.deliveryDetails?.deliveryTime.start &&
          t.value.end === this.deliveryDetails?.deliveryTime.end
        );
      });

      hours = time ? time.label : '';

      date = this.datePipe.transform(this.deliveryDetails.requestedDate, 'fullDate');
    }
    return type === 'hours' ? hours : date;
  }

  getAddress(address) {
    return `${address.addressLine1 ?? ''}${address.addressLine2 ? ' ' + address.addressLine2 : ''}${
      address.addressLine3 ? ' ' + address.addressLine3 : ''
    },
  ${address.city || ''}, ${address.state || ''} ${address.zipCode || 0} - ${
      address.plus4Code || 0
    }`;
  }
}
