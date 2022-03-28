import {Component, Input, Output, OnInit, EventEmitter} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators, FormArray} from '@angular/forms';
import {forkJoin} from 'rxjs';
import {AddressVerificationService} from 'src/app/services';

@Component({
  selector: 'app-address-verification-modal',
  templateUrl: './address-verification-modal.component.html',
  styleUrls: ['./address-verification-modal.component.scss'],
})
export class AddressVerificationModalComponent implements OnInit {
  @Input() addresses = [];
  @Output() update = new EventEmitter<any>();

  showVerification = false;
  addressForm: FormGroup;
  addressSuggestions = [];

  get addressArray(): FormArray {
    return this.addressForm.get('addressArray') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private addressVerificationService: AddressVerificationService
  ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.addressForm = this.fb.group({
      addressArray: this.fb.array([]),
    });

    const suggestionRequests = [];

    this.addresses.forEach((address: any) => {
      const request = {
        addressLine1: address.addressLine1,
        addressLine2: address.addressLine2,
        addressLine3: address.addressLine3,
        city: address.city,
        country: address.country,
        plus4Code: address.plus4Code,
        state: address.state,
        zipCode: address.zipCode,
      };
      suggestionRequests.push(this.addressVerificationService.verifiedAddressSuggestions(request));
    });
    forkJoin(suggestionRequests).subscribe((resArray: any) => {
      resArray.forEach((res, index) => {
        res = res.map(item => {
          item.selected = false;
          return item;
        });
        this.addressArray.push(
          this.fb.group({
            invalidAddress: [this.addresses[index]],
            addressIndex: [this.addresses[index].addressIndex],
            addressSuggestions: [res],
            selectedAddress: [''],
            previousSelectedAddress: [''],
          })
        );
      });
    });
  }

  showInvalidAddress(address) {
    const {addressLine1, addressLine2, city, state, zipCode} = address;
    return `${addressLine1}, ${addressLine2 ? addressLine2 : ''}, ${city}, ${state}, ${
      zipCode ? zipCode : ''
    }`;
  }

  showModal() {
    this.showVerification = true;
    this.buildForm();
  }

  closeModal() {
    this.showVerification = false;
  }

  onSubmitAddress(value: any) {
    const requests = [];
    const requestedIndexes = [];
    const selectedAddresses = [];
    this.addresses.forEach((address: any, index) => {
      if (value.addressArray[index].selectedAddress) {
        requests.push(
          this.addressVerificationService.verifyAddress(value.addressArray[index].selectedAddress)
        );
        requestedIndexes.push(index);
        address.addressLine1 = value.addressArray[index].selectedAddress.addressLine1;
        address.addressLine2 = value.addressArray[index].selectedAddress.addressLine2;
        address.plus4Code = value.addressArray[index].selectedAddress.plus4Code;
        address.city = value.addressArray[index].selectedAddress.city;
        address.state = value.addressArray[index].selectedAddress.state;
        address.zipCode = value.addressArray[index].selectedAddress.zipCode;
        address.addressKey = value.addressArray[index].selectedAddress.addressKey;
        address.addressUuid = value.addressArray[index].selectedAddress.addressUuid;
        address.addressIndex = value.addressArray[index].addressIndex;

        selectedAddresses.push(address);
      }
    });
    if (requests.length !== 0) {
      forkJoin(requests).subscribe((resArray: any) => {
        resArray.forEach((res, index) => {
          selectedAddresses[requestedIndexes[index]].latitude = res.latitude;
          selectedAddresses[requestedIndexes[index]].longitude = res.longitude;
        });
        this.update.emit(selectedAddresses);
      });
    } else {
      this.update.emit(selectedAddresses);
    }
  }

  formatAddressForVerification(address) {
    return {
      country: address.country,
      addressLine1: address.addressLine1,
      addressLine2: address.addressLine2,
      state: address.state,
      city: address.city,
      county: address.county,
      zipCode: address.zipCode,
      plus4Code: address.plus4Code,
    };
  }
}
