import {Location} from '@angular/common';
import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {EnumNames} from 'src/app/enums';
import {SieveOperators} from 'src/app/enums/sieve-operators';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {
  DriverService,
  InventoryService,
  OrderHeadersService,
  PatientService,
  SitesService,
  ToastService,
  VehicleService,
} from 'src/app/services';
import {DispatchService} from 'src/app/services/dispatch.service';
import {getEnum, removeDuplicatesInArray, showRequiredFields} from 'src/app/utils';
import {buildFilterString} from 'src/app/utils/filter.utils';

@Component({
  selector: 'app-dispatch-fulfill-order',
  templateUrl: './dispatch-fulfill-order.component.html',
  styleUrls: ['./dispatch-fulfill-order.component.scss'],
})
export class DispatchFulfillOrderComponent implements OnInit {
  orderStatusTypes = getEnum(EnumNames.OrderHeaderStatusTypes);
  completedId = this.orderStatusTypes.find(x => x.name === 'Completed')?.id;
  cancelledId = this.orderStatusTypes.find(x => x.name === 'Cancelled')?.id;
  plannedId = this.orderStatusTypes.find(x => x.name === 'Planned')?.id;
  scheduledId = this.orderStatusTypes.find(x => x.name === 'Scheduled')?.id;
  pendingApprovalId = this.orderStatusTypes.find(x => x.name === 'Pending_Approval')?.id;
  orderTypes = getEnum(EnumNames.OrderTypes);
  pickupId = this.orderTypes.find(x => x.name === 'Pickup')?.id;
  deliveryId = this.orderTypes.find(x => x.name === 'Delivery')?.id;
  exchangeId = this.orderTypes.find(x => x.name === 'Exchange')?.id;
  patientMoveId = this.orderTypes.find(x => x.name === 'Patient_Move')?.id;
  inventoryStatusTypes = getEnum(EnumNames.InventoryStatusTypes);
  availableId = this.inventoryStatusTypes.find(x => x.name === 'Available')?.id;
  inTransitId = this.inventoryStatusTypes.find(x => x.name === 'InTransit')?.id;
  orderLineItemStatus = getEnum(EnumNames.orderLineItemStatusTypes);
  completeOrderlineItemStatusId = this.orderLineItemStatus.find(x => x.name === 'Completed')?.id;
  fulfillOrderExceeded = [];
  orderForm: FormGroup;
  formSubmit = false;
  maxDate = new Date();
  orderId: number;
  dispatchInstructions = [];
  truck;
  orderLineItems = [];
  loading = false;
  order: any;
  orderFulfillmentList: Array<any>;
  vehicles = [];
  vehicle: any;
  drivers = [];
  driver: any;
  serials: any;
  inventoryRequest = new SieveRequest();
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private dispatchService: DispatchService,
    private orderHeaderService: OrderHeadersService,
    private patientService: PatientService,
    private vehicleService: VehicleService,
    private driverService: DriverService,
    private inventoryService: InventoryService,
    private toastService: ToastService,
    private location: Location
  ) {
    const {paramMap} = this.route.snapshot;
    this.orderId = Number(paramMap.get('orderId'));
    if (this.orderId) {
      this.getOrder();
    }
  }

  ngOnInit(): void {}

  onSubmitOrder(form) {
    const startDate = form.fulfillmentStartDateTime;
    const endDate = form.fulfillmentEndDateTime;

    if (startDate >= endDate) {
      this.toastService.showError('StartDate should be less than EndDate');
      return;
    }

    this.formSubmit = true;
    form.fulfillmentItems = form.fulfillmentItems.filter(
      item =>
        item.assetTagNumber !== '' ||
        item.lotNumber !== '' ||
        item.serialNumber !== '' ||
        (item.itemId && item.count !== null)
    );
    if (!form.isExceptionFulfillment && form.fulfillmentItems.length === 0) {
      this.toastService.showError(`Inventory Items should be provided for a dispatch request.`);
      this.formSubmit = false;
      return false;
    }
    if (this.fulfillOrderExceeded.length > 0) {
      this.showFulfillmentExceedWarning();
      this.formSubmit = false;
      return false;
    }
    this.dispatchService
      .fulfillOrder(form)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe(() => {
        this.location.back();
      });
  }

  setOrderForm() {
    const orderLineItemsArray = [];
    const dispatchItemForm = [];

    this.orderLineItems.forEach(item => {
      if (
        item.statusId !== this.completedId &&
        item.statusId !== this.cancelledId &&
        ((item.leftToFulfillCount && item.leftToFulfillCount !== 0) ||
          item.leftToFulfillCount === undefined)
      ) {
        let fulfillmentType = '';
        switch (item.actionId) {
          case this.pickupId:
            fulfillmentType = 'pickup';
            break;
          case this.deliveryId:
            fulfillmentType = 'delivery';
            break;
          default:
            fulfillmentType = 'pickup';
            break;
        }
        if (!item.item.isLotNumbered) {
          const formItemCount =
            item.item.isAssetTagged || item.item.isSerialized
              ? item.leftToFulfillCount
                ? item.leftToFulfillCount
                : item.itemCount
              : 1;
          [...Array(formItemCount)].forEach((_, i) => {
            orderLineItemsArray.push(item);
            dispatchItemForm.push(
              this.getFulfillmentItemForm(
                fulfillmentType,
                item.id,
                item.item.isSerialized ? 1 : null,
                item.item.isSerialized ? null : item.itemId
              )
            );
          });
        } else {
          orderLineItemsArray.push(item);
          dispatchItemForm.push(
            this.getFulfillmentItemForm(fulfillmentType, item.id, item.itemCount)
          );
        }
      }
      this.orderLineItems = orderLineItemsArray;
    });

    this.orderForm = this.fb.group({
      orderId: new FormControl(this.orderId, Validators.required),
      fulfillmentStartDateTime: new FormControl(null, Validators.required),
      fulfillmentEndDateTime: new FormControl(null, Validators.required),
      fulfillmentItems: this.fb.array(dispatchItemForm),
      driverId: new FormControl(null, Validators.required),
      vehicleId: new FormControl(null, Validators.required),
      isWebportalFulfillment: new FormControl(true, Validators.required),
      isExceptionFulfillment: new FormControl(this.order?.isExceptionFulfillment || false),
    });
  }

  getFulfillmentItemForm(fulfillmentType, lineItemId, itemCount, itemId?) {
    return this.fb.group({
      fulfillmentType: new FormControl(fulfillmentType),
      count: new FormControl(itemCount),
      serialNumber: new FormControl(''),
      assetTagNumber: new FormControl(''),
      lotNumber: new FormControl(''),
      itemId: new FormControl(itemId ?? null),
      orderLineItemId: new FormControl(lineItemId),
    });
  }

  getOrder() {
    this.loading = true;
    this.orderHeaderService
      .getOrderHeaderById(this.orderId, true)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((orederHeaderResponse: any) => {
        this.order = orederHeaderResponse;
        this.order.processingTime = this.calculateProcessingTime(
          this.order.orderLineItems,
          this.order.orderType
        );
        const filters = [
          {
            field: 'uniqueId',
            operator: SieveOperators.Equals,
            value: [this.order.patientUuid],
          },
        ];
        const patientRequest = {
          filters: buildFilterString(filters),
        };
        this.patientService
          .getPatients(patientRequest)
          .subscribe((patientRes: PaginationResponse) => {
            this.order.patient = patientRes.records?.[0];
          });
        this.formatOrders();
        this.ordersFullfilmentFormat(orederHeaderResponse.orderFulfillmentLineItems);
        this.setOrderForm();
      });
  }

  ordersFullfilmentFormat(fullfillmentOrders: any[]): void {
    this.orderFulfillmentList = fullfillmentOrders;
    this.orderLineItems = this.orderLineItems.map(o => {
      if (!o.item.isLotNumbered) {
        o.fulfilledCount = this.getFulfilledCount(fullfillmentOrders, o.id);
      }
      return o;
    });
    this.orderLineItems = this.orderLineItems.filter(item => {
      this.orderFulfillmentList.forEach(orderFulfillItem => {
        if (item.id === orderFulfillItem.orderLineItemId) {
          orderFulfillItem.statusId = item.statusId;
          orderFulfillItem.statusName = item.status;
          item.leftToFulfillCount = item.itemCount - item.fulfilledCount;
        }
      });
      return item.leftToFulfillCount ? item.leftToFulfillCount > 0 : item.itemCount > 0;
    });
  }

  getFulfilledCount(list, id) {
    return list
      .filter((of: any) => of.orderLineItemId === id)
      .reduce((a: any, b: any) => {
        return a + b.quantity;
      }, 0);
  }

  formatOrders() {
    this.orderLineItems = this.order.orderLineItems;
  }

  searchVehicles({query}) {
    this.vehicleService
      .searchVehicles({searchQuery: query})
      .subscribe((vehiclesRes: PaginationResponse) => {
        this.vehicles = vehiclesRes.records;
      });
  }

  searchSerialNumber(event, idx) {
    const inventoryFilters = [
      {
        field: 'itemId',
        value: [this.orderLineItems[idx].itemId],
        operator: SieveOperators.Equals,
      },
    ];
    if (this.orderLineItems[idx].actionId === this.pickupId) {
      this.inventoryRequest.filters = buildFilterString(inventoryFilters);
      this.inventoryService
        .getPatientInventoryByUuidSearch(this.order.patientUuid, {
          searchQuery: event.query,
          ...this.inventoryRequest,
        })
        .pipe(finalize(() => (this.loading = false)))
        .subscribe((response: PaginationResponse) => {
          this.serials = response?.records ?? [];
        });
    } else {
      if (this.orderLineItems[idx].actionId === this.deliveryId) {
        inventoryFilters.push({
          field: 'statusId',
          value: [this.availableId, this.inTransitId],
          operator: SieveOperators.Equals,
        });
      }
      this.inventoryRequest.filters = buildFilterString(inventoryFilters);
      this.inventoryService
        .searchInventory({...this.inventoryRequest, searchQuery: event.query})
        .pipe(finalize(() => (this.loading = false)))
        .subscribe((response: PaginationResponse) => {
          this.serials = response?.records ?? [];
        });
    }
  }

  searchDrivers({query}) {
    this.driverService
      .searchDrivers({searchQuery: query})
      .subscribe((driverRes: PaginationResponse) => {
        this.drivers = driverRes?.records?.map(x => {
          x.displayName = x.firstName + ' ' + x.lastName;
          return x;
        });
      });
  }

  serialSelected(event, idx) {
    const {serialNumber, assetTagNumber} = this.orderLineItems[idx];
    const controlSerial = this.orderForm.get([
      'fulfillmentItems',
      idx,
      'serialNumber',
    ]) as FormControl;
    const controlAssetTag = this.orderForm.get([
      'fulfillmentItems',
      idx,
      'assetTagNumber',
    ]) as FormControl;
    controlSerial.patchValue(event.serialNumber);
    controlAssetTag.patchValue(event.assetTagNumber);

    if (
      serialNumber !== null &&
      assetTagNumber !== null &&
      serialNumber !== event.serialNumber &&
      assetTagNumber !== event.assetTagNumber
    ) {
      controlAssetTag.setErrors({'invalid-serial': true});
    } else {
      if (this.hasSerialError(idx)) {
        controlAssetTag.errors['invalid-serial'].setErrors(null);
      }
    }
  }

  serialCleared(orderLineItemIndex) {
    const controlSerial = this.orderForm.get([
      'fulfillmentItems',
      orderLineItemIndex,
      'serialNumber',
    ]) as FormControl;
    const controlAssetTag = this.orderForm.get([
      'fulfillmentItems',
      orderLineItemIndex,
      'assetTagNumber',
    ]) as FormControl;
    controlSerial.patchValue('');
    controlAssetTag.patchValue('');
  }

  vehicleSelected(event) {
    this.orderForm.patchValue({vehicleId: this.vehicle?.id || null});
  }

  driverSelected(event) {
    this.orderForm.patchValue({driverId: this.driver?.id || null});
  }

  checkFormValidity(form) {
    return showRequiredFields(form, 'OrderForm');
  }
  showStandaloneFulfillment(orderLineItem) {
    return orderLineItem.fulfilledCount
      ? orderLineItem.itemCount - orderLineItem.fulfilledCount === 0
        ? false
        : true
      : true;
  }
  hasSerialError(idx) {
    const controlAssetTag = this.orderForm.get([
      'fulfillmentItems',
      idx,
      'assetTagNumber',
    ]) as FormControl;
    return controlAssetTag.hasError('invalid-serial');
  }
  getChipClass(id) {
    switch (id) {
      case this.completedId:
        return 'success';
      case this.cancelledId:
        return 'warn';
      case this.plannedId:
        return 'planned';
      case this.scheduledId:
        return 'planned';
      default:
        return 'planned';
    }
  }

  hasValidLineItems(form) {
    const validItems = form.fulfillmentItems.filter(
      item =>
        item.assetTagNumber !== '' ||
        item.lotNumber !== '' ||
        item.serialNumber !== '' ||
        (item.itemId && item.count !== null)
    );
    return form.isExceptionFulfillment || validItems.length > 0;
  }

  checkFulfilledQuantity(event, idx) {
    if (event.target.value === '0') {
      const count = this.orderForm.get(['fulfillmentItems', idx, 'count']) as FormControl;
      count.patchValue(null);
    }
    const maxCount = this.orderLineItems[idx].fulfilledCount
      ? this.orderLineItems[idx].itemCount - this.orderLineItems[idx].fulfilledCount
      : this.orderLineItems[idx].itemCount;
    if (event.target.value > maxCount) {
      if (!this.fulfillOrderExceeded.includes(idx)) {
        this.fulfillOrderExceeded = [...this.fulfillOrderExceeded, idx];
      }
      this.showFulfillmentExceedWarning();
    } else {
      const index = this.fulfillOrderExceeded.findIndex(x => x === idx);
      if (index !== -1) {
        this.fulfillOrderExceeded.splice(index, 1);
      }
    }
  }
  showFulfillmentExceedWarning() {
    this.toastService.showError(`Quantity to fulfill cannot exceed Ordered Quantity`);
  }

  calculateProcessingTime(orderLineItems, orderType) {
    const processingTime = orderLineItems.reduce((a, b) => {
      b.processingTimeSum =
        b.itemCount * (b.item.avgDeliveryProcessingTime + b.item.avgPickUpProcessingTime);
      return a + b.processingTimeSum;
    }, 0);
    return processingTime;
  }

  goBack() {
    this.location.back();
  }
}
