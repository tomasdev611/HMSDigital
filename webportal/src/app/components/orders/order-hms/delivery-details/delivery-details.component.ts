import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {OrderHeadersService, StorageService} from 'src/app/services';
import {checkInternalUser, getDifference, getEnum, showRequiredFields} from 'src/app/utils';
import {EnumNames} from 'src/app/enums';
import {hms} from 'src/app/constants';

@Component({
  selector: 'app-delivery-details',
  templateUrl: './delivery-details.component.html',
  styleUrls: ['./delivery-details.component.scss'],
})
export class DeliveryDetailsComponent implements OnInit {
  orderId: number;
  patientInfo: any;
  deliveryDetailsForm: FormGroup;
  today: Date = new Date();
  deliveryTimeMode = 'regular';
  formSubmit = false;
  orderTypeId = getEnum(EnumNames.OrderTypes).find((l: any) => l.name === 'Delivery')?.id;
  orderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find((l: any) => l.name === 'Planned')
    ?.id;
  fulfilledItems = [];
  pickupTime: any;
  deliveryDetails: any;

  @Input() editmode;
  @Input() patientUniqueId;
  @Input() hospiceLocations;
  @Input() patientInventories;
  @Input() cartItems;
  @Input() patient: any;
  @Input() nurses;
  @Input() deliveryTimeOptions;
  @Input() deliveryAddresses = [];
  @Input() set storedDeliveryDetails(deliveryDetails: any) {
    this.deliveryDetails = deliveryDetails;
    if (this.deliveryDetails?.hospiceMemberId) {
      this.deliveryDetailsForm.patchValue({
        ...this.deliveryDetails,
        requestedDate: new Date(this.deliveryDetails.requestedDate),
      });
      this.pickupTime = this.deliveryDetails.deliveryTime;
      this.deliveryTimeMode = this.deliveryDetails?.statOrder ? 'high-priority' : 'regular';
      this.deliveryTimingModeChanged(this.deliveryTimeMode);
      this.patchPatientAddress(this.deliveryDetails?.deliveryAddress);
    }
  }
  @Input() orderHeader: any;
  @Input() tmpSessionKey;
  @Output() changeStepHandler = new EventEmitter<any>();
  @Output() updateCartHandler = new EventEmitter<any>();

  constructor(
    private orderHeaderService: OrderHeadersService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private storageService: StorageService
  ) {
    this.route.params.subscribe((params: any) => {
      this.patientUniqueId = params.patientUniqueId;
      this.orderId = params.orderId;
    });
    this.setDeliveryForm();
  }

  ngOnInit(): void {
    this.patchPatientDependentValues();
    this.patchPatientAddress(this.deliveryDetails?.deliveryAddress);
    this.patchOrderHeader();
    if (this.editmode && this.orderId && this.deliveryDetailsForm) {
      this.deliveryDetailsForm.get('orderNote').setValidators(Validators.required);
    }
  }

  patchOrderHeader() {
    if (!this.orderHeader) {
      return;
    }
    this.fulfilledItems = this.orderHeader.orderFulfillmentLineItems;
    this.patchOrderForm();
    this.patchPatientAddress(this.deliveryDetails?.deliveryAddress);
  }

  setDeliveryForm() {
    this.deliveryDetailsForm = this.fb.group({
      id: new FormControl(null),
      hospiceId: new FormControl(this.patient?.hospiceId ?? null),
      hospiceLocationId: new FormControl(this.patient?.hospiceLocationId ?? null),
      deliveryAddress: new FormControl(null, Validators.required),
      deliveryHours: new FormControl(null),
      deliveryTime: new FormControl(null, Validators.required),
      hospiceMemberId: new FormControl(null, Validators.required),
      patientUuid: new FormControl(this.patient?.uniqueId ?? null),
      orderTypeId: new FormControl(this.orderTypeId),
      orderStatusId: new FormControl(this.orderStatusId),
      orderNotes: new FormControl([]),
      orderNote: new FormControl([]),
      statOrder: new FormControl(false),
      requestedDate: new FormControl(new Date(), Validators.required),
      confirmationNumber: new FormControl(null),
      externalOrderNumber: new FormControl(null),
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
    this.deliveryDetailsForm.controls.deliveryAddress.setValue(address);
  }

  onSubmit(formValues) {
    this.formSubmit = true;
  }

  patchPatientAddress(address?: any) {
    if (!address && !this.orderHeader) {
      return;
    }
    address = address || this.orderHeader.deliveryAddress;

    let orderDeliveryAddress = this.deliveryAddresses.find(
      pa =>
        pa.address.addressUuid === address?.addressUuid ||
        this.matchAddressFields(pa.address, address)
    );

    orderDeliveryAddress = orderDeliveryAddress || this.deliveryAddresses[0];

    this.deliveryDetailsForm.patchValue({
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

  shouldShowTpaOrderField() {
    return checkInternalUser();
  }

  getDateFromString(dateString: string) {
    return new Date(dateString);
  }

  deliveryTimingModeChanged(mode) {
    if (mode === 'high-priority') {
      this.deliveryDetailsForm.controls.statOrder.setValue(true);
      this.deliveryDetailsForm.controls.deliveryHours.setValue(
        this.deliveryDetailsForm.value.deliveryHours ?? 2
      );
      this.deliveryDetailsForm.controls.deliveryHours.setValidators([Validators.required]);
      this.deliveryDetailsForm.controls.deliveryTime.setValue(null);
      this.deliveryDetailsForm.controls.deliveryTime.clearValidators();
    } else {
      this.deliveryDetailsForm.controls.statOrder.setValue(false);
      this.deliveryDetailsForm.controls.deliveryTime.setValue(
        this.deliveryDetailsForm.value.deliveryTime
      );
      this.deliveryDetailsForm.controls.deliveryTime.setValidators([Validators.required]);
      this.deliveryDetailsForm.controls.deliveryHours.setValue(null);
      this.deliveryDetailsForm.controls.deliveryHours.clearValidators();
    }
    this.deliveryDetailsForm.controls.deliveryHours.updateValueAndValidity();
    this.deliveryDetailsForm.controls.deliveryTime.updateValueAndValidity();
  }

  navigateToAddNurse() {
    this.storeFormState();
    this.router.navigate([
      `${hms.orderRoutes.addNurse.pre}/${this.patient.hospiceId}/${hms.orderRoutes.addNurse.post}`,
    ]);
  }

  navigateToAddAddress() {
    this.storeFormState();
    this.router.navigate([`patients/edit/${this.patient.id}`]);
  }
  storeFormState() {
    if (this.editmode) {
      return;
    }
    const deliveryDetails = this.getFormCurrentState();
    const formState = {
      deliveryDetails,
    };
    this.storageService.set(this.tmpSessionKey, JSON.stringify(formState), 'session');
  }

  patchPatientDependentValues() {
    if (!this.patient) {
      return;
    }
    const patchingObj = {
      hospiceId: this.patient?.hospiceId ?? null,
      hospiceLocationId: this.patient?.hospiceLocationId ?? null,
      patientUuid: this.patient?.uniqueId ?? null,
    };
    this.deliveryDetailsForm.patchValue(patchingObj);
  }

  cancelPlacingOrder() {
    this.router.navigate(['/']);
  }

  patchOrderForm() {
    const deliveryHours = this.orderHeader?.statOrder
      ? getDifference(
          this.orderHeader.requestedEndDateTime,
          this.orderHeader.requestedStartDateTime,
          'hours'
        )
      : null;
    const patchValues = {
      ...this.orderHeader,
      requestedDate: new Date(this.orderHeader.requestedStartDateTime),
      deliveryHours,
    };
    this.deliveryDetailsForm.patchValue(patchValues);
    this.patchPickupTime();
  }

  patchPickupTime() {
    const orderStartTime = this.getDateHours(this.orderHeader.requestedStartDateTime);

    this.pickupTime = this.deliveryTimeOptions.find(pt => pt.value.start === orderStartTime)?.value;
    this.deliveryDetailsForm.controls.deliveryTime.setValue(this.pickupTime);
    this.deliveryTimeMode = this.orderHeader?.statOrder ? 'high-priority' : 'regular';

    this.deliveryTimingModeChanged(this.deliveryTimeMode);
  }

  getDateHours(date: any) {
    return new Date(date).getHours();
  }

  returnToProducts() {
    this.changeStepHandler.emit({step: 'productCatalog'});
  }

  proceedToReview() {
    const {deliveryTime, deliveryHours} = this.deliveryDetailsForm.value;
    if (!this.deliveryDetailsForm.valid || this.formSubmit || !(deliveryTime || deliveryHours)) {
      return;
    }
    this.storeFormState();
    const deliveryDetails = this.getFormCurrentState();
    this.changeStepHandler.emit({
      step: 'review',
      deliveryDetails,
    });
  }
  checkFormValidity(form) {
    return showRequiredFields(form, 'DeliveryDetailsForm');
  }

  updateCart(event: any) {
    this.updateCartHandler.emit(event);
  }

  getFormCurrentState() {
    const {deliveryTime, deliveryHours} = this.deliveryDetailsForm.value;
    const nurse = this.nurses.find(x => x.value === this.deliveryDetailsForm.value.hospiceMemberId);
    return {
      ...this.deliveryDetailsForm.value,
      deliveryTime,
      deliveryHours,
      nurse,
    };
  }
}
