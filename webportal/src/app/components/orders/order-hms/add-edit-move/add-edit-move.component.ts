import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {Table, TableHeaderCheckbox} from 'primeng-lts/table';
import {finalize} from 'rxjs/operators';
import {hms} from 'src/app/constants';
import {OrderLineItemStatus} from 'src/app/enum-constants';
import {EnumNames} from 'src/app/enums';
import {OrderHeadersService, StorageService, ToastService} from 'src/app/services';
import {
  checkInternalUser,
  deepCloneObject,
  getEnum,
  showRequiredFields,
  encode,
} from 'src/app/utils';

@Component({
  selector: 'app-add-edit-move',
  templateUrl: './add-edit-move.component.html',
  styleUrls: ['./add-edit-move.component.scss'],
})
export class AddEditMoveComponent implements OnInit {
  patientInfo: any;

  @Input() patientUniqueId = '';
  @Input() editmode = false;
  @Input() hospiceLocations = [];
  @Input() set patient(patient: any) {
    this.patientInfo = patient;
    this.patchPatientDependentValues();
  }
  @Input() nurses = [];
  @Input() pickupTimeOptions = [];
  @Input() set addresses(addresses) {
    this.patientAddresses = addresses;
    if (this.editmode) {
      this.patchPatientAddress();
    }
    this.patchAddressFromState();
  }

  backupFormState = null;
  currentUrl = '';
  selectedPatientInventory = [];
  patientAddresses = [];
  pickupTime: any;
  formSubmit = false;
  today: Date = new Date();
  selectedValue: any;
  patientInventories = [];
  inventoryLoading = false;
  moveForm: FormGroup;
  orderTypeId = getEnum(EnumNames.OrderTypes).find((l: any) => l.name === 'Patient_Move')?.id;
  orderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find((l: any) => l.name === 'Planned')
    ?.id;
  orderId: number;
  orderHeader: any;

  @Input() set patientInventory(inventories: [any]) {
    this.patientInventories = inventories.map(x => {
      return {
        ...x,
        itemCount: x.quantity,
        action: 'pickup',
        fulfilledQuantity: 0,
      };
    });
    this.groupNonSerializedItems();
    this.updateAvailableQuantity();
    this.setSelectedItems();
    this.patchSelectedInventoryFromState();
  }

  private headerCheckBox: TableHeaderCheckbox;
  @ViewChild('headerCheckBox') set setHeaderCheckbox(checkbox: TableHeaderCheckbox) {
    this.headerCheckBox = checkbox;
    if (this.headerCheckBox) {
      this.headerCheckBox.updateCheckedState = this.checkHeadercheckboxState.bind(this);
    }
  }

  @ViewChild('invTable')
  private invTable: Table;

  constructor(
    private fb: FormBuilder,
    private orderHeaderService: OrderHeadersService,
    private router: Router,
    private toastService: ToastService,
    private route: ActivatedRoute,
    private storageService: StorageService
  ) {
    this.currentUrl = this.router.url;
    this.route.params.subscribe((params: any) => {
      this.patientUniqueId = params.patientUniqueId;
      this.orderId = params.orderId;
    });
    this.setMoveForm();
  }

  ngOnInit(): void {
    if (this.editmode && this.orderId) {
      this.getOrderDetails();
      if (this.moveForm && this.editmode) {
        this.moveForm.get('newOrderNotes').setValidators(Validators.required);
      }
    }
    this.patchOldFormState();
  }

  patchOldFormState() {
    const data = this.getFormState();
    if (data) {
      this.pickupTime = data.pickupTime;
      data.moveForm.requestedDate = data.moveForm.requestedDate
        ? new Date(data.moveForm.requestedDate)
        : null;
      this.moveForm.patchValue(data.moveForm);
    }
  }

  patchSelectedInventoryFromState() {
    const data = this.getFormState();
    if (data) {
      this.selectedPatientInventory = data.selectedInventory
        .map(si => {
          // find selected Inventory item in patient Inventory list
          const patientInv = this.patientInventories.find(pi => pi.id === si.id);
          if (patientInv) {
            // check if the cached return quantity is less that available quantity
            patientInv.itemCount =
              patientInv.quantity >= si.itemCount ? si.itemCount : patientInv.itemCount;
          }
          return patientInv;
        })
        .filter(si => si);
    }
  }

  patchAddressFromState() {
    const data = this.getFormState();
    if (data) {
      const pickupAddr = this.patientAddresses.find(pa =>
        this.matchAddressFields(pa.address, data.moveForm.pickupAddress)
      );
      this.moveForm.get('pickupAddress')?.patchValue(pickupAddr?.address);

      const deliveryAddr = this.patientAddresses.find(pa =>
        this.matchAddressFields(pa.address, data.moveForm.deliveryAddress)
      );
      this.moveForm.get('deliveryAddress')?.patchValue(deliveryAddr?.address);
    }
  }

  groupNonSerializedItems() {
    this.patientInventories = this.patientInventories.reduce(this.groupSerializedItemsReducer, []);
  }

  groupSerializedItemsReducer(groupedInvs, inventory) {
    const nonSerializedItem = groupedInvs.find(
      gi => gi.itemId === inventory.itemId && !gi.serialNumber && !gi.assetTagNumber
    );
    if (nonSerializedItem) {
      nonSerializedItem.quantity += inventory.quantity;
      nonSerializedItem.isPartOfExistingPickup =
        inventory.isPartOfExistingPickup || nonSerializedItem.isPartOfExistingPickup;
      nonSerializedItem.existingPickupCount += inventory.existingPickupCount;
      nonSerializedItem.orderNumbers.push(inventory.orderNumber);
    } else {
      inventory.orderNumbers = [inventory.orderNumber];
      groupedInvs.push(inventory);
    }
    return groupedInvs;
  }

  getOrderNumber(inventory) {
    if (inventory?.orderNumbers && Array.isArray(inventory?.orderNumbers)) {
      return inventory.orderNumbers.join(',');
    }
    return inventory.orderNumber;
  }

  getOrderNumberDomList(inventory) {
    const orderNumbersList = this.getOrderNumber(inventory).split(',');
    let list = ``;
    orderNumbersList.forEach(orderNumber => {
      list += `<li>${orderNumber}</li>`;
    });
    return list;
  }

  getOrderDetails() {
    this.orderHeaderService
      .getOrderHeaderById(this.orderId, true)
      .subscribe((orderHeaderData: any) => {
        this.orderHeader = orderHeaderData;
        this.patchOrderForm();
        this.updateAvailableQuantity();
        this.setSelectedItems();
      });
  }

  patchPatientAddress() {
    if (!this.orderHeader) {
      return;
    }
    const orderPickupAddress = this.patientAddresses.find(
      pa =>
        pa.address.addressUuid === this.orderHeader.pickupAddress.addressUuid ||
        this.matchAddressFields(pa.address, this.orderHeader.pickupAddress)
    );
    const orderDeliveryAddress = this.patientAddresses.find(
      pa =>
        pa.address.addressUuid === this.orderHeader.deliveryAddress.addressUuid ||
        this.matchAddressFields(pa.address, this.orderHeader.deliveryAddress)
    );
    this.moveForm.patchValue({
      pickupAddress: orderPickupAddress?.address,
      deliveryAddress: orderDeliveryAddress?.address,
    });
  }

  matchAddressFields(sourceAddress, destAddress) {
    if (!sourceAddress || !destAddress) {
      return false;
    }
    return (
      sourceAddress.addressLine1 === destAddress.addressLine1 &&
      sourceAddress.addressLine2 === destAddress.addressLine2 &&
      sourceAddress.addressLine3 === destAddress.addressLine3 &&
      sourceAddress.city === destAddress.city &&
      sourceAddress.county === destAddress.county &&
      sourceAddress.country === destAddress.country &&
      sourceAddress.latitude === destAddress.latitude &&
      sourceAddress.longitude === destAddress.longitude &&
      sourceAddress.plus4Code === destAddress.plus4Code &&
      sourceAddress.state === destAddress.state &&
      sourceAddress.zipCode === destAddress.zipCode
    );
  }

  setSelectedItems() {
    if (!this.patientInventories || !this.editmode || !this.orderHeader) {
      return;
    }

    const selection = this.patientInventories.filter(inv => {
      const lineItem = this.findLineItemForInv(inv);
      return lineItem;
    });

    this.selectedPatientInventory = [...selection];
  }

  patchOrderForm() {
    const patchValues = {
      ...this.orderHeader,
      requestedDate: new Date(this.orderHeader.requestedStartDateTime),
    };
    this.moveForm.patchValue(patchValues);
    this.patchPickupTime();
  }

  patchPickupTime() {
    const orderStartTime = this.getDateHours(this.orderHeader.requestedStartDateTime);
    this.pickupTime = this.pickupTimeOptions.find(
      pt => pt.value.start === orderStartTime || pt.value.end === orderStartTime
    )?.value;
  }

  patchPatientDependentValues() {
    const patchingObj = {
      hospiceId: this.patientInfo?.hospiceId ?? null,
      hospiceLocationId: this.patientInfo?.hospiceLocationId ?? null,
      patientUuid: this.patientInfo?.uniqueId ?? null,
    };
    this.moveForm.patchValue(patchingObj);
  }

  setMoveForm() {
    this.moveForm = this.fb.group({
      id: new FormControl(null),
      hospiceId: new FormControl(this.patientInfo?.hospiceId ?? null),
      hospiceLocationId: new FormControl(this.patientInfo?.hospiceLocationId ?? null),
      hospiceMemberId: new FormControl(null, Validators.required),
      patientUuid: new FormControl(this.patientInfo?.uniqueId ?? null),
      orderTypeId: new FormControl(this.orderTypeId),
      orderStatusId: new FormControl(this.orderStatusId),
      orderNotes: new FormControl([]),
      newOrderNotes: new FormControl([]),
      statOrder: new FormControl(false),
      requestedDate: new FormControl(null, Validators.required),
      requestedStartDateTime: new FormControl(null),
      requestedEndDateTime: new FormControl(null),
      confirmationNumber: new FormControl(null),
      externalOrderNumber: new FormControl(null),
      pickupReason: new FormControl(null),
      deliveryAddress: new FormControl(null, Validators.required),
      pickupAddress: new FormControl(null, Validators.required),
      orderLineItems: new FormControl(null),
    });
  }

  checkHeadercheckboxState(): boolean {
    if (!this.invTable) {
      return false;
    }
    const records: any[] = this.invTable.filteredValue || this.invTable.value;
    const selection: any[] = this.invTable.selection?.filter(a => !this.isRowDisabled(a));
    records.forEach(r => {
      if (!selection.find(s => s.id === r.id) && r.fulfilledQuantity) {
        selection.push(r);
      }
    });
    if (!selection) {
      this.updateHeaderCheckBoxState(null);
      return false;
    }
    this.invTable.selection = [...selection];
    this.invTable.updateSelectionKeys();

    if (records.length === selection.length) {
      this.updateHeaderCheckBoxState('all');
      return true;
    } else if (selection.length > 0) {
      this.updateHeaderCheckBoxState('partial');
      return true;
    } else {
      this.updateHeaderCheckBoxState(null);
      return false;
    }
  }

  updateHeaderCheckBoxState(checkedState) {
    if (this.headerCheckBox?.boxViewChild?.nativeElement?.className) {
      let className = 'p-checkbox-box';
      if (checkedState === 'partial' || checkedState === 'all') {
        className += ' p-highlight';
      }
      this.headerCheckBox.boxViewChild.nativeElement.className = className;
    }
    if (this.headerCheckBox?.boxViewChild?.nativeElement?.children[0]?.className) {
      let className = 'p-checkbox-icon';
      if (checkedState === 'all') {
        className += ' pi pi-check';
      } else if (checkedState === 'partial') {
        className += ' pi pi-minus';
      }
      this.headerCheckBox.boxViewChild.nativeElement.children[0].className = className;
    }
  }

  updateAvailableQuantity() {
    if (this.editmode && !this.orderHeader) {
      return;
    }
    this.patientInventories.forEach(inv => {
      // remove quantity from all existing pickup orders (including current)
      inv.quantity = inv.isPartOfExistingPickup
        ? inv.quantity - inv.existingPickupCount
        : inv.quantity;

      // exclude count of items in current order
      const lineItem = this.findLineItemForInv(inv);
      if (lineItem) {
        inv.quantity += lineItem.itemCount;
        inv.itemCount = lineItem.itemCount;
        inv.fulfilledQuantity = this.orderHeader?.orderFulfillmentLineItems
          .filter(fli => fli.orderLineItemId === lineItem.id)
          .reduce((count, fli) => count + fli.quantity, 0);
      }

      // while determining item count above, we havent considered if they are present in other pickups
      // they might exceed available quantity, below takes care of that
      if (inv.itemCount && inv.itemCount > inv.quantity) {
        inv.itemCount = inv.quantity;
      }
    });
  }

  findLineItemForInv(patientInv, action = 'pickup') {
    return this.orderHeader?.orderLineItems?.find(
      (oli: any) =>
        oli.item.id === patientInv.itemId &&
        action.toLowerCase() === oli.action.toLowerCase() &&
        this.matchSerializedItem(oli, patientInv)
    );
  }

  addressChanged(address) {
    this.moveForm.controls.deliveryAddress.setValue(address);
  }

  getAddress(address) {
    return `${address.addressLine1 ?? ''}${address.addressLine2 ? ' ' + address.addressLine2 : ''}${
      address.addressLine3 ? ' ' + address.addressLine3 : ''
    },
    ${address.city || ''}, ${address.state || ''} ${address.zipCode || 0} - ${
      address.plus4Code || 0
    }`;
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

  getOrderNotesArray(existingOrderNotes, newOrderNotes, hospiceMemberId) {
    const notes = newOrderNotes.map(n => {
      return {
        note: n,
        hospiceMemberId,
      };
    });
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

  onSubmit(formValues) {
    const body = deepCloneObject(formValues);
    body.orderNotes = this.getOrderNotesArray(
      body.orderNotes,
      body.newOrderNotes,
      body.hospiceMemberId
    );
    body.orderLineItems = this.getOrderLineItemArray();
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
      .subscribe((response: any) => {
        this.router.navigate([hms.defaultRoute]);
        this.toastService.showSuccess(
          `Your order has been successfully ${this.editmode ? 'updated' : 'created'}`
        );
      });
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

  getOrderLineItemArray() {
    // lineitemIds is used to maintain the ids which are already pushed to the line items list
    const lineItemIds = [];
    const newLineItems = this.selectedPatientInventory.reduce((lineItemsList: any[], si: any) => {
      const [pickupItem, deliveryItem] = this.getOrderLineItems(si, lineItemIds);
      if (pickupItem.id) {
        lineItemIds.push(pickupItem.id);
      }
      if (deliveryItem.id) {
        lineItemIds.push(deliveryItem.id);
      }
      lineItemsList.push(pickupItem);
      lineItemsList.push(deliveryItem);
      return lineItemsList;
    }, []);

    // select line items which are partially fulfilled or completed and not in the
    // patient inventory
    this.orderHeader?.orderLineItems.forEach(oli => {
      if (
        !(
          oli.statusId === OrderLineItemStatus.Completed ||
          oli.statusId === OrderLineItemStatus.PartialFulfillment
        )
      ) {
        return;
      }
      const lineItem = newLineItems.find(nli => nli.id === oli.id);
      if (!lineItem) {
        lineItemIds.push(oli.id);
        newLineItems.push(oli);
        const deliveryLineItem = this.orderHeader?.orderLineItems?.find(li => {
          return (
            !lineItemIds.includes(li.id) &&
            li.item.id === oli.item.id &&
            li.action.toLowerCase() === 'delivery'
          );
        });
        if (deliveryLineItem) {
          newLineItems.push(deliveryLineItem);
          lineItemIds.push(deliveryLineItem.id);
        }
      }
    });
    return newLineItems;
  }

  getOrderLineItems(selectedPatientInv, lineItemIds = []) {
    // find the pickup line item by matching asset tag and serial number if item is serialized
    const pickupLineItem = this.getOrderLineItem(selectedPatientInv, 'pickup');
    // getDeliveryLineItem is used because it will ignore the serial/asset tag number while finding the
    // order line item and also check if the line item is already puhsed to the line items list using linteItemsIds list
    const deliveryLineItem = this.getDeliveryLineItem(selectedPatientInv, lineItemIds);
    return [pickupLineItem, deliveryLineItem];
  }

  getDeliveryLineItem(patientInv, lineItemIds) {
    const deliveryItem = this.orderHeader?.orderLineItems?.find(
      oli =>
        oli.item.id === patientInv.itemId &&
        oli.action.toLowerCase() === 'delivery' &&
        !lineItemIds.includes(oli.id)
    );
    if (!deliveryItem) {
      return this.getNewOrderLineItemPayload(patientInv, 'delivery');
    }
    return {...deliveryItem, itemCount: patientInv.itemCount};
  }

  getNewOrderLineItemPayload(patientInv, action) {
    return {
      itemId: patientInv.itemId,
      itemCount: patientInv.itemCount,
      action,
      equipmentSettings: [],
      serialNumber: action.toLowerCase() === 'pickup' ? patientInv.serialNumber : null,
      lotNumber: action.toLowerCase() === 'pickup' ? patientInv.lotNumber : null,
      assetTagNumber: action.toLowerCase() === 'pickup' ? patientInv.assetTagNumber : null,
    };
  }

  getOrderLineItem(selectedPatientInv, action) {
    const existingLineItem = this.findLineItemForInv(selectedPatientInv, action);
    if (existingLineItem) {
      return {...existingLineItem, itemCount: selectedPatientInv.itemCount};
    }
    return this.getNewOrderLineItemPayload(selectedPatientInv, action);
  }

  createOrder(body) {
    return this.orderHeaderService.createOrderHeader(body);
  }

  updateOrder(body) {
    return this.orderHeaderService.updateOrderHeader(body);
  }

  isRowDisabled(data: any): boolean {
    return data.quantity === 0;
  }

  setNewDateWithHours(date: any, hours: number) {
    const newDate = new Date(date);
    return new Date(newDate.setHours(hours, 0, 0, 0));
  }

  getDateHours(date: any) {
    return new Date(date).getHours();
  }

  shouldShowTpaOrderField() {
    return checkInternalUser();
  }

  navigateToAddNurse() {
    this.storeFormState();
    this.router.navigate(
      [
        `${hms.orderRoutes.addNurse.pre}/${this.patientInfo.hospiceId}/${hms.orderRoutes.addNurse.post}`,
      ],
      {
        queryParams: {
          returnUrl: this.currentUrl,
        },
      }
    );
  }

  checkFormValidity(form) {
    const formConstant = this.editmode ? 'editMoveForm' : 'createMoveForm';
    const extraFields = this.getExtraRequiredFields();
    return showRequiredFields(form, formConstant, extraFields);
  }

  getExtraRequiredFields() {
    const fields = [];
    if (!this.pickupTime) {
      fields.push('Pickup Time');
    }
    if (!(this.selectedPatientInventory && this.selectedPatientInventory.length)) {
      fields.push('Move Items');
    }
    return fields;
  }

  navigateToAddAddress() {
    if (this.patientInfo) {
      this.storeFormState();
      this.router.navigate([`patients/edit/${this.patientInfo.id}`], {
        queryParams: {source: this.currentUrl},
      });
    }
  }

  storeFormState() {
    if (this.editmode) {
      return;
    }
    const formState = {
      moveForm: deepCloneObject(this.moveForm.value),
      pickupTime: this.pickupTime,
      selectedInventory: this.selectedPatientInventory,
    };

    const dataKey = encode(this.currentUrl);
    this.storageService.set(dataKey, JSON.stringify(formState), 'session');
  }

  getFormState() {
    if (this.editmode) {
      return null;
    }
    if (!this.backupFormState) {
      const datakey = encode(this.currentUrl);
      const dataString = this.storageService.get(datakey, 'session');
      this.storageService.remove(datakey, 'session');
      if (dataString) {
        this.backupFormState = JSON.parse(dataString);
      }
    }
    return this.backupFormState;
  }
}
