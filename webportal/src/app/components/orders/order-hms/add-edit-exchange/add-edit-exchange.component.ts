import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {
  buildFilterString,
  getEnum,
  deepCloneObject,
  checkInternalUser,
  showRequiredFields,
  getUniqArray,
} from 'src/app/utils';
import {EnumNames, SieveOperators} from 'src/app/enums';
import {ActivatedRoute, Router} from '@angular/router';
import {OrderHeadersService, ProductCatalogService, ToastService} from 'src/app/services';
import {SieveRequest} from 'src/app/models';
import {hms} from 'src/app/constants';
import {finalize} from 'rxjs/operators';
import {Table, TableHeaderCheckbox} from 'primeng-lts/table';
import {OrderLineItemStatus} from 'src/app/enum-constants';

@Component({
  selector: 'app-add-edit-exchange',
  templateUrl: './add-edit-exchange.component.html',
  styleUrls: ['./add-edit-exchange.component.scss'],
})
export class AddEditExchangeComponent implements OnInit {
  exchangeForm: FormGroup;
  orderMode = 'exchange';
  today = new Date();
  pickupTime: any;
  pickupAddresses = [];
  orderHeader: any;
  orderId: number;
  replacementItems: any[] = [];
  itemsList: any[] = [];
  defaultProductImageUrl = '/assets/images/png/no-image-available.png';
  formSubmit = false;
  selectedPatientInventory = [];
  patientInventory = [];
  patientInfo: any;

  @Input() pickupTimeOptions = [];
  @Input() nurses = [];
  @Input() editmode;
  @Input() set patient(patient: any) {
    this.patientInfo = patient;
    this.patchPatientDependentValues();
    this.getDeliveryLineItemPrices();
  }
  @Input() set patientInventories(inventories: [any]) {
    this.patientInventory = deepCloneObject(inventories);
  }
  @Input() set patientAddresses(addresses) {
    this.pickupAddresses = addresses;
    this.exchangeForm.patchValue({
      pickupAddress: this.pickupAddresses[0]?.address,
      deliveryAddress: this.pickupAddresses[0]?.address,
    });
  }

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private productCatalogService: ProductCatalogService,
    private toastService: ToastService,
    private orderHeaderService: OrderHeadersService,
    private route: ActivatedRoute
  ) {
    this.route.params.subscribe((params: any) => {
      this.orderId = params.orderId;
    });
    this.setExchangeForm();
  }

  ngOnInit(): void {
    this.orderMode = this.editmode ? 'replace' : 'exchange';
    if (this.editmode && this.orderId) {
      this.getOrderDetails();
      if (this.exchangeForm && this.editmode) {
        this.exchangeForm.get('newOrderNotes').setValidators(Validators.required);
      }
    }
  }

  searchItems({query}) {
    const filter = [];
    if (query) {
      filter.push({
        field: 'itemName',
        operator: SieveOperators.CI_Contains,
        value: [query],
      });
    }
    const catalogRequest = new SieveRequest();
    catalogRequest.filters = buildFilterString(filter);
    this.productCatalogService
      .getProducts(this.patientInfo.hospiceId, this.patientInfo.hospiceLocationId, catalogRequest)
      .subscribe((response: any) => {
        this.itemsList = response.records;
      });
  }

  selectReplacementItem(event) {
    const {item, rate} = event;
    let replacementItem = this.replacementItems.find(ri => ri.itemId === item.id);
    if (!replacementItem) {
      replacementItem = {
        item: deepCloneObject(item),
        rate,
        itemId: item.id,
        orderQuantity: 1,
        action: 'delivery',
        equipmentSettings: [],
        serialNumber: null,
        assetTagNumber: null,
        lotNumber: null,
      };
      this.replacementItems.push(replacementItem);
    }
  }

  updateCart(item, count = null) {
    if (!count) {
      const itemIndex = this.replacementItems.indexOf(item);
      if (itemIndex >= 0) {
        this.replacementItems.splice(itemIndex, 1);
      }
      return;
    }
    item.orderQuantity += count;
    if (item.orderQuantity === 0) {
      item.orderQuantity = 1;
    }
  }

  getImageUrl(product) {
    if (!(product.itemImageUrls && product.itemImageUrls.length)) {
      return this.defaultProductImageUrl;
    }
    return this.getEncodedUrl(product.itemImageUrls[0]);
  }

  getEncodedUrl(urlString: string) {
    return encodeURI(urlString);
  }

  onSubmit(formValues) {
    const body = deepCloneObject(formValues);
    body.orderNotes = this.getOrderNotesArray(
      body.orderNotes,
      body.newOrderNotes,
      body.hospiceMemberId
    );
    body.orderLineItems = this.prepareOrderLineItems();
    body.requestedStartDateTime = this.setNewDateWithHours(
      body.requestedDate,
      this.pickupTime.start
    );
    body.requestedEndDateTime = this.setNewDateWithHours(body.requestedDate, this.pickupTime.end);
    body.pickupAddress = this.getAddressPayloadObject({
      ...body.pickupAddress,
    });
    body.deliveryAddress = this.getAddressPayloadObject({
      ...body.deliveryAddress,
    });

    this.deleteUncessaryProps(body);

    this.formSubmit = true;
    const createOrUpdateOrderReq = this.editmode ? this.updateOrder(body) : this.createOrder(body);
    createOrUpdateOrderReq
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe(() => {
        this.router.navigate([hms.defaultRoute]);
        this.toastService.showSuccess(
          `Your order has been successfully ${this.editmode ? 'updated' : 'created'}`
        );
      });
  }

  setExchangeForm() {
    const orderTypeId = getEnum(EnumNames.OrderTypes).find((l: any) => l.name === 'Exchange')?.id;
    const orderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
      (l: any) => l.name === 'Planned'
    )?.id;

    this.exchangeForm = this.fb.group({
      id: new FormControl(null),
      hospiceId: new FormControl(this.patientInfo?.hospiceId ?? null),
      hospiceLocationId: new FormControl(this.patientInfo?.hospiceLocationId ?? null),
      hospiceMemberId: new FormControl(null, Validators.required),
      patientUuid: new FormControl(this.patientInfo?.uniqueId ?? null),
      orderTypeId: new FormControl(orderTypeId),
      orderStatusId: new FormControl(orderStatusId),
      orderNotes: new FormControl([]),
      newOrderNotes: new FormControl([]),
      statOrder: new FormControl(false),
      requestedStartDateTime: new FormControl(null),
      requestedEndDateTime: new FormControl(null),
      confirmationNumber: new FormControl(null),
      externalOrderNumber: new FormControl(null),
      pickupAddress: new FormControl(null, Validators.required),
      deliveryAddress: new FormControl(Validators.required),
      requestedDate: new FormControl(null, Validators.required),
    });
  }

  navigateToAddAddress() {
    this.router.navigate([`/patients/edit/${this.patientInfo.id}`]);
  }

  getAddress(address) {
    return `${address.addressLine1 ?? ''}${address.addressLine2 ? ' ' + address.addressLine2 : ''}${
      address.addressLine3 ? ' ' + address.addressLine3 : ''
    },
    ${address.city || ''}, ${address.state || ''} ${address.zipCode || 0} - ${
      address.plus4Code || 0
    }`;
  }

  addressChanged(address) {
    this.exchangeForm.controls.pickupAddress.setValue(address);
    this.exchangeForm.controls.deliveryAddress.setValue(address);
  }

  navigateToAddNurse() {
    this.router.navigate([
      `${hms.orderRoutes.addNurse.pre}/${this.patientInfo.hospiceId}/${hms.orderRoutes.addNurse.post}`,
    ]);
  }

  shouldShowTpaOrderField() {
    return checkInternalUser();
  }

  checkFormValidity(form) {
    const formConstant = this.editmode ? 'editPickupForm' : 'createExchangeForm';
    const extraFields = this.getExtraRequiredFields();
    return showRequiredFields(form, formConstant, extraFields);
  }

  getExtraRequiredFields() {
    const fields = [];
    if (!this.pickupTime) {
      fields.push('Pickup Time');
    }
    if (!(this.selectedPatientInventory && this.selectedPatientInventory.length)) {
      fields.push('Exchange Items');
    }
    if (this.orderMode === 'replace' && !(this.replacementItems && this.replacementItems.length)) {
      fields.push('Replace Items');
    }
    return fields;
  }

  getOrderNotesArray(existingOrderNotes, newOrderNotes, hospiceMemberId) {
    const notes = newOrderNotes
      .map(n => {
        return {
          note: n,
          hospiceMemberId,
        };
      })
      .filter(n => n);
    return existingOrderNotes && existingOrderNotes.length
      ? [...existingOrderNotes, ...notes]
      : [...notes];
  }

  deleteUncessaryProps(body) {
    delete body.newOrderNotes;
    delete body.requestedDate;
    if (!this.editmode) {
      delete body.id;
    }
  }

  setNewDateWithHours(date: any, hours: number) {
    const newDate = new Date(date);
    return new Date(newDate.setHours(hours, 0, 0, 0));
  }

  getAddressPayloadObject(address: any) {
    return {
      addressUuid: address.addressUuid,
      latitude: address.latitude,
      longitude: address.longitude,
      addressLine1: address.addressLine1,
      addressLine2: address.addressLine2,
      addressLine3: address.addressLine3,
      state: address.state,
      city: address.city,
      county: address.county,
      zipCode: address.zipCode,
      plus4Code: address.plus4Code,
    };
  }

  createOrder(body) {
    return this.orderHeaderService.createOrderHeader(body);
  }

  updateOrder(body) {
    return this.orderHeaderService.updateOrderHeader(body);
  }

  prepareOrderLineItems() {
    const pickupItems = this.selectedPatientInventory.map(inv => {
      const lineItem = this.findLineItemForInv(inv, 'pickup');
      if (lineItem) {
        lineItem.itemCount = inv.orderQuantity;
      }
      return lineItem ?? this.getNewOrderLineItemPayload(inv, 'pickup');
    });

    let deliveryItems = [];

    switch (this.orderMode) {
      case 'exchange':
        deliveryItems = this.selectedPatientInventory.map(inv => {
          const lineItem = this.getDeliveryLineItem(inv);
          return lineItem ?? this.getNewOrderLineItemPayload(inv, 'delivery');
        });
        break;
      case 'replace':
        deliveryItems = this.replacementItems.map(ri => {
          const lineItem = this.getDeliveryLineItem(ri);
          return lineItem ?? this.getNewReplacementItemPayload(ri);
        });
        break;
      default:
        break;
    }

    let orderLineItems = pickupItems.concat(deliveryItems);
    orderLineItems.forEach(oli => delete oli.orderQuantity);

    const fulfilledItems = this.orderHeader?.orderLineItems?.filter(fOli => {
      const isFulfilled =
        fOli.statusId === OrderLineItemStatus.PartialFulfillment ||
        fOli.statusId === OrderLineItemStatus.Completed;

      const isAlreadyPresent = orderLineItems.find(oli => oli.id === fOli.id);

      return isFulfilled && !isAlreadyPresent;
    });

    if (fulfilledItems) {
      orderLineItems = orderLineItems.concat(fulfilledItems);
    }

    return orderLineItems;
  }

  getNewOrderLineItemPayload(patientInv, action) {
    return {
      itemId: patientInv.itemId,
      itemCount: patientInv.orderQuantity,
      action,
      equipmentSettings: [],
      serialNumber: action.toLowerCase() === 'pickup' ? patientInv.serialNumber : null,
      lotNumber: action.toLowerCase() === 'pickup' ? patientInv.lotNumber : null,
      assetTagNumber: action.toLowerCase() === 'pickup' ? patientInv.assetTagNumber : null,
    };
  }

  getNewReplacementItemPayload(ri) {
    return {
      id: ri?.id,
      itemId: ri?.itemId,
      itemCount: ri?.orderQuantity,
      action: ri?.action,
      equipmentSettings: ri?.equipmentSettings,
      serialNumber: ri?.serialNumber,
      lotNumber: ri?.lotNumber,
      assetTagNumber: ri?.assetTagNumber,
    };
  }

  patchPatientDependentValues() {
    const patchingObj = {
      hospiceId: this.patientInfo?.hospiceId ?? null,
      hospiceLocationId: this.patientInfo?.hospiceLocationId ?? null,
      patientUuid: this.patientInfo?.uniqueId ?? null,
    };
    this.exchangeForm.patchValue(patchingObj);
  }

  getOrderDetails() {
    this.orderHeaderService
      .getOrderHeaderById(this.orderId, true)
      .subscribe((orderHeaderData: any) => {
        this.orderHeader = orderHeaderData;

        this.patchExchangeForm();
        this.setReplacementItems();
        this.getDeliveryLineItemPrices();
      });
  }

  patchExchangeForm() {
    const patchValues = {
      ...this.orderHeader,
      requestedDate: new Date(this.orderHeader.requestedStartDateTime),
    };

    this.exchangeForm.patchValue(patchValues);
    this.patchPickupTime();
  }

  patchPickupTime() {
    const orderStartTime = this.getDateHours(this.orderHeader.requestedStartDateTime);
    this.pickupTime = this.pickupTimeOptions.find(
      pt => pt.value.start === orderStartTime || pt.value.end === orderStartTime
    )?.value;
  }

  getDateHours(date: any) {
    return new Date(date).getHours();
  }

  findLineItemForInv(patientInv, action) {
    return this.orderHeader?.orderLineItems?.find(
      (oli: any) =>
        oli.item.id === patientInv.itemId &&
        this.matchSerializedItem(oli, patientInv) &&
        action.toUpperCase() === oli.action.toUpperCase()
    );
  }

  setReplacementItems() {
    this.replacementItems = this.orderHeader?.orderLineItems?.filter(
      li => li.action.toLowerCase() === 'delivery'
    );
    this.replacementItems.forEach(item => {
      item.orderQuantity = item.itemCount;
      item.fulfilledQuantity = this.orderHeader?.orderFulfillmentLineItems
        .filter(fli => fli.orderLineItemId === item.id)
        .reduce((count, fli) => count + fli.quantity, 0);
    });
  }

  getDeliveryLineItem(inv) {
    const deliveryInv = {...inv};
    deliveryInv.serialNumber = null;
    deliveryInv.lotNumber = null;
    deliveryInv.assetTagNumber = null;

    const lineItem = this.findLineItemForInv(deliveryInv, 'delivery');
    if (lineItem) {
      lineItem.itemCount = inv.orderQuantity;
    }

    return lineItem;
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

  isRowDisabled(inv, lineItemType) {
    const lineItem = this.findLineItemForInv(inv, lineItemType);
    let isFulfilled = false;

    if (lineItem) {
      isFulfilled =
        lineItem.statusId === OrderLineItemStatus.PartialFulfillment ||
        lineItem.statusId === OrderLineItemStatus.Completed;
    }

    return inv.availableQuantity === 0;
  }

  getRowTooltip(inv, action) {
    const orderLineItem = this.findLineItemForInv(inv, action);

    if (orderLineItem) {
      const fulfilledItem = this.orderHeader?.orderFulfillmentLineItems.find(
        i => i.orderLineItemId === orderLineItem.id
      );

      if (fulfilledItem) {
        if (orderLineItem.itemCount === fulfilledItem.quantity) {
          return 'Item is completely fulfilled';
        } else {
          return 'Item is partially fulfilled';
        }
      }
    }
  }

  getDeliveryLineItemPrices() {
    if (!this.patientInfo || !this.orderHeader) {
      return;
    }

    const itemIds = this.replacementItems.map(item => item.item.id);
    const filters = [
      {
        field: 'itemId',
        operator: SieveOperators.Equals,
        value: getUniqArray(itemIds),
      },
    ];

    const catalogRequest = new SieveRequest();
    catalogRequest.filters = buildFilterString(filters);
    catalogRequest.pageSize = itemIds.length;

    this.productCatalogService
      .getProducts(this.patientInfo.hospiceId, this.patientInfo.hospiceLocationId, catalogRequest)
      .subscribe((response: any) => {
        response?.records?.forEach(record => {
          const replacementItemIndex = this.replacementItems.findIndex(
            ri => ri.itemId === record.item.id
          );
          this.replacementItems[replacementItemIndex].rate = record.rate;
        });
      });
  }

  inventorySelectionUpdated(selectedItems) {
    this.selectedPatientInventory = deepCloneObject(selectedItems);
  }
}
