import {Component, OnInit} from '@angular/core';
import {FormControl, Validators, FormGroup, FormBuilder} from '@angular/forms';
import {finalize} from 'rxjs/operators';
import {HospiceLocationService, ToastService, HospiceFacilityService} from 'src/app/services';
import {ActivatedRoute, Router} from '@angular/router';
import {Facility} from 'src/app/models/model.facility';
import {createPatch, deepCloneObject} from 'src/app/utils';
import {Location} from '@angular/common';

@Component({
  selector: 'app-add-edit-facility',
  templateUrl: './add-edit-facility.component.html',
  styleUrls: ['./add-edit-facility.component.scss'],
})
export class AddEditFacilityComponent implements OnInit {
  hospiceId: number;
  editmode = false;
  facilityId: number;
  facilityForm: FormGroup;
  facility: any;
  formSubmit = false;
  zipCodes = [];
  hospices = [];
  hospiceLocations = [];
  sites = [];

  constructor(
    private fb: FormBuilder,
    private facilityService: HospiceFacilityService,
    private toastService: ToastService,
    private route: ActivatedRoute,
    private hospiceLocationService: HospiceLocationService,
    private location: Location
  ) {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
      this.facilityId = params.facilityId;
    });
    const {url} = this.route.snapshot;

    const urlLength = url && url.length;
    this.editmode = url[urlLength - 2]?.path === 'edit';

    this.setFacilityForm();

    if (this.editmode) {
      this.getFacilityDetail();
    } else {
      this.getHospiceLocations();
    }
  }

  ngOnInit(): void {}

  setFacilityForm() {
    this.facilityForm = this.fb.group({
      id: new FormControl(null),
      name: new FormControl(null, Validators.required),
      hospiceId: new FormControl(this.hospiceId),
      phoneNumber: new FormControl(null, Validators.required),
      hospiceLocation: new FormControl(null, Validators.required),
      hospiceLocationId: new FormControl(null),
      isDisable: new FormControl(null),
      isActive: new FormControl(null),
      address: this.fb.group({
        addressLine1: new FormControl(null, Validators.required),
        addressLine2: new FormControl(null),
        country: new FormControl('United States of America', Validators.required),
        state: new FormControl(null, Validators.required),
        city: new FormControl(null, Validators.required),
        zipCode: new FormControl(null, Validators.required),
        county: new FormControl(null),
        id: new FormControl(null),
        plus4Code: new FormControl(0, Validators.compose([Validators.maxLength(4)])),
        latitude: new FormControl(0, Validators.compose([Validators.min(-90), Validators.max(90)])),
        longitude: new FormControl(
          0,
          Validators.compose([Validators.min(-180), Validators.max(180)])
        ),
      }),
    });
  }

  getFacilityDetail() {
    this.facilityService
      .getHospiceFacilityById(this.hospiceId, this.facilityId)
      .subscribe((response: Facility) => {
        this.facility = response;
        this.facilityForm.patchValue(response);
        this.facilityForm.controls.address.patchValue(response.address);
        this.facilityForm.controls.phoneNumber.patchValue(
          response.facilityPhoneNumber[0]?.phoneNumber?.number.toString()
        );
        this.facilityForm.controls.isActive.patchValue(!response.isDisable);
        this.getHospiceLocations();
        delete this.facility.hospiceLocation;
        delete this.facility.address.isValid;
        delete this.facility.address.results;
      });
  }

  getHospiceLocations() {
    this.hospiceLocationService
      .getHospiceLocations(this.hospiceId)
      .pipe(finalize(() => {}))
      .subscribe((response: any) => {
        this.hospiceLocations = response.records.map(x => {
          if (this.facility && this.facility.hospiceLocationId === x.id) {
            this.facilityForm.controls.hospiceLocationId.setValue(x.id);
            this.facilityForm.controls.hospiceLocation.setValue(x);
          }
          return x;
        });
      });
  }

  onSubmitFacility(value: any) {
    const body = this.formatValues(value);
    if (body) {
      this.formSubmit = true;
      if (this.editmode) {
        this.updateFacility(body);
      } else {
        this.saveFacility(body);
      }
    }
  }

  saveFacility(value) {
    this.facilityService
      .createHospiceFacility(this.hospiceId, value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe(() => {
        this.toastService.showSuccess(`Facility Created successfully`);
        this.location.back();
      });
  }

  updateFacility(updatedFacility: Facility) {
    this.facility.site = updatedFacility.site;
    const patch = createPatch(this.facility, updatedFacility);
    this.facilityService
      .updateHospiceFacility(this.hospiceId, this.facilityId, patch)
      .pipe(finalize(() => (this.formSubmit = false)))
      .subscribe(() => {
        this.toastService.showSuccess('Facility Updated successfully');
      });
  }

  formatValues(value) {
    value = deepCloneObject(value);
    value.facilityPhoneNumber = parseInt(value.phoneNumber, 10)
      ? [
          {
            phoneNumber: {
              number: parseInt(value.phoneNumber, 10),
              countryCode: 1,
              isPrimary: true,
              isVerified: true,
              numberType: null,
              numberTypeId: null,
              isSelfPhone: this.facility
                ? this.facility.facilityPhoneNumber[0].phoneNumber.isSelfPhone
                : false,
              contactName: this.facility
                ? this.facility.facilityPhoneNumber[0].phoneNumber.contactName
                : null,
              receiveSurveyTextMessage: this.facility
                ? this.facility.facilityPhoneNumber[0].phoneNumber.receiveSurveyTextMessage
                : false,
              receiveEtaTextmessage: this.facility
                ? this.facility.facilityPhoneNumber[0].phoneNumber.receiveEtaTextmessage
                : false,
            },
          },
        ]
      : [];

    value.hospiceLocationId =
      value.hospiceLocation && value.hospiceLocation.id ? value.hospiceLocation.id : 0;
    if (this.editmode) {
      value.isDisable = !value.isActive;
      if (this.facility.address && this.facility.address.addressUuid) {
        value.address = {
          ...value.address,
          isVerified: this.facility.address.isVerified,
          addressUuid: this.facility.address.addressUuid,
          verifiedBy: this.facility.address.verifiedBy,
        };
      }
      value.site = null;
    }
    value.siteId = null;
    value.address.addressLine3 = '';

    delete value.phoneNumber;
    delete value.hospiceLocation;
    delete value.isActive;

    return value;
  }
}
