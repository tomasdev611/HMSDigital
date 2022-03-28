import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {hms} from 'src/app/constants';
import {OrderLineItemStatus} from 'src/app/enum-constants';
import {EnumNames} from 'src/app/enums';
import {OrderHeadersService, StorageService, ToastService} from 'src/app/services';
import {
  checkInternalUser,
  deepCloneObject,
  encode,
  getEnum,
  showRequiredFields,
} from 'src/app/utils';

@Component({
  selector: 'app-add-edit-pickup',
  templateUrl: './add-edit-pickup.component.html',
  styleUrls: ['./add-edit-pickup.component.scss'],
})
export class AddEditPickupComponent implements OnInit {
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
  @Input() set patientAddresses(addresses) {
    this.pickupAddresses = addresses;
    if (this.editmode) {
      this.patchPatientAddress();
    }
    this.patchAddressFromState();
    this.patchSelectedInventoryFromState();
  }

  backupFormState = null;
  currentUrl = '';
  selectedPatientInventory = [];
  pickupAddresses = [];
  pickupTime: any;
  formSubmit = false;
  today: Date = new Date();
  pickupReasons = [
    {value: 'Not Needed', label: 'Not Needed'},
    {value: 'Respite', label: 'Respite'},
    {value: 'Discharged', label: 'Discharged'},
    {value: 'Deceased', label: 'Deceased'},
  ];
  selectedValue: any;
  inventoryLoading = false;
  pickupForm: FormGroup;
  orderTypeId = getEnum(EnumNames.OrderTypes).find((l: any) => l.name === 'Pickup')?.id;
  orderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find((l: any) => l.name === 'Planned')
    ?.id;
  orderId: number;
  orderHeader: any;

  @Input() patientInventories;

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
    this.setPickupForm();
  }

  ngOnInit(): void {
    if (this.editmode && this.orderId) {
      this.getOrderDetails();
      if (this.pickupForm && this.editmode) {
        this.pickupForm.get('newOrderNotes').setValidators(Validators.required);
      }
    }
    this.patchOldFormState();
  }

  patchOldFormState() {
    const data = this.getFormState();
    if (data) {
      this.pickupTime = data.pickupTime;
      data.pickupForm.requestedDate = data.pickupForm.requestedDate
        ? new Date(data.pickupForm.requestedDate)
        : null;
      this.pickupForm.patchValue(data.pickupForm);
    }
  }

  patchSelectedInventoryFromState() {
    const data = this.getFormState();
    if (data) {
      this.selectedPatientInventory = data.selectedInventory
        .map(si => {
          // find selected Inventory item in patient Inventory list
          const inventoryItem = this.patientInventories.find(pi => pi.id === si.id);
          if (inventoryItem) {
            // check if the cached return quantity is less that available quantity
            inventoryItem.itemCount =
              inventoryItem.quantity >= si.itemCount ? si.itemCount : inventoryItem.itemCount;
          }
          return inventoryItem;
        })
        .filter(si => si);
    }
  }

  patchAddressFromState() {
    const data = this.getFormState();
    if (data) {
      const pickupAddr = this.pickupAddresses.find(pa =>
        this.matchAddressFields(pa.address, data.pickupForm.pickupAddress)
      );
      this.pickupForm.get('pickupAddress')?.patchValue(pickupAddr?.address);
    }
  }

  patchPatientDependentValues() {
    const patchingObj = {
      hospiceId: this.patientInfo?.hospiceId ?? null,
      hospiceLocationId: this.patientInfo?.hospiceLocationId ?? null,
      patientUuid: this.patientInfo?.uniqueId ?? null,
    };
    this.pickupForm.patchValue(patchingObj);
  }

  patchPatientAddress() {
    if (!this.orderHeader) {
      return;
    }
    const orderPickupAddress = this.pickupAddresses.find(
      pa =>
        pa.address.addressUuid === this.orderHeader.pickupAddress.addressUuid ||
        this.matchAddressFields(pa.address, this.orderHeader.pickupAddress)
    );
    this.pickupForm.patchValue({
      pickupAddress: orderPickupAddress?.address,
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

  getOrderDetails() {
    this.orderHeaderService
      .getOrderHeaderById(this.orderId, true)
      .subscribe((orderHeaderData: any) => {
        this.orderHeader = orderHeaderData;
        this.patchOrderForm();
      });
  }

  patchOrderForm() {
    const patchValues = {
      ...this.orderHeader,
      requestedDate: new Date(this.orderHeader.requestedStartDateTime),
    };
    this.pickupForm.patchValue(patchValues);
    this.patchPickupTime();
  }

  patchPickupTime() {
    const orderStartTime = this.getDateHours(this.orderHeader.requestedStartDateTime);
    this.pickupTime = this.pickupTimeOptions.find(
      pt => pt.value.start === orderStartTime || pt.value.end === orderStartTime
    )?.value;
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

  setPickupForm() {
    this.pickupForm = this.fb.group({
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
      pickupReason: new FormControl(null, Validators.required),
      deliveryAddress: new FormControl(null),
      pickupAddress: new FormControl(null, Validators.required),
      orderLineItems: new FormControl(null),
    });
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
    this.pickupForm.controls.deliveryAddress.setValue(address);
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

  getOrderLineItemArray() {
    // create OrderLineItem object from PatientInventory object; for payload (create or update OrderHeader)
    const newLineItems = this.selectedPatientInventory.map(si => this.getOrderLineItem(si));

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
        newLineItems.push(oli);
      }
    });

    return newLineItems;
  }

  getOrderLineItem(selectedPatientInv) {
    const existingOrderLineItem = this.findLineItemForInv(selectedPatientInv);
    if (existingOrderLineItem) {
      existingOrderLineItem.itemCount = selectedPatientInv.itemCount;
      return existingOrderLineItem;
    }

    return {
      itemId: selectedPatientInv.itemId,
      itemCount: selectedPatientInv.itemCount,
      action: 'pickup',
      equipmentSettings: [],
      serialNumber: selectedPatientInv.serialNumber,
      lotNumber: selectedPatientInv.lotNumber,
      assetTagNumber: selectedPatientInv.assetTagNumber,
    };
  }

  createOrder(body) {
    return this.orderHeaderService.createOrderHeader(body);
  }

  updateOrder(body) {
    return this.orderHeaderService.updateOrderHeader(body);
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
      pickupForm: deepCloneObject(this.pickupForm.value),
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

  checkFormValidity(form) {
    const formConstant = this.editmode ? 'editPickupForm' : 'createPickupForm';
    const extraFields = this.getExtraRequiredFields();
    return showRequiredFields(form, formConstant, extraFields);
  }

  getExtraRequiredFields() {
    const fields = [];
    if (!this.pickupTime) {
      fields.push('Pickup Time');
    }
    if (!(this.selectedPatientInventory && this.selectedPatientInventory.length)) {
      fields.push('Pickup Items');
    }
    return fields;
  }

  inventorySelectionUpdated(selectedInventory) {
    this.selectedPatientInventory = selectedInventory.map(inv => {
      const item = {...inv};
      item.itemCount = inv.orderQuantity;
      return item;
    });
  }
}
