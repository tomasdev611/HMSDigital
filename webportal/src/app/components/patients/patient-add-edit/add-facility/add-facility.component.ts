import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {FormControl, Validators, FormGroup, FormBuilder} from '@angular/forms';
import {finalize} from 'rxjs/operators';
import {HospiceLocationService, ToastService, HospiceFacilityService} from 'src/app/services';
import {Facility} from 'src/app/models/model.facility';
import {deepCloneObject} from 'src/app/utils';

@Component({
  selector: 'app-add-facility',
  templateUrl: './add-facility.component.html',
  styleUrls: ['./add-facility.component.scss'],
})
export class AddFacilityComponent implements OnInit {
  @Input() hospiceId: number;
  @Output() facilityAdded = new EventEmitter<any>();

  hospiceLocation: any = [];
  facilityId: number;
  backUrl: string;
  facilityForm: FormGroup;
  facility: any;
  formSubmit = false;
  zipCodes = [];
  hospices = [];
  sites = [];

  constructor(
    private fb: FormBuilder,
    private facilityService: HospiceFacilityService,
    private toastService: ToastService,
    private hospiceLocationService: HospiceLocationService
  ) {}

  ngOnInit(): void {
    this.setFacilityForm();
    this.getHospiceLocations();
  }

  setFacilityForm() {
    this.facilityForm = this.fb.group({
      id: new FormControl(null),
      name: new FormControl(null, Validators.required),
      hospiceId: new FormControl(this.hospiceId),
      phoneNumber: new FormControl(null, Validators.required),
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
        this.facilityForm.patchValue({
          ...response,
          ...response.address,
          phoneNumber: response.facilityPhoneNumber[0]?.phoneNumber?.number.toString(),
          isActive: !response.isDisable,
        });
      });
  }

  getHospiceLocations() {
    this.hospiceLocationService
      .getHospiceLocations(this.hospiceId)
      .pipe(finalize(() => {}))
      .subscribe((response: any) => {
        this.hospiceLocation = response.records.map(x => {
          if (this.facility && this.facility.hospiceLocationId === x.id) {
            this.facilityForm.controls.hospiceLocationId.setValue(x);
          }
          return x;
        })[0];
      });
  }

  onSubmitFacility(value: any) {
    const body = this.formatValues(value);
    if (body) {
      this.saveFacility(body);
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
      .subscribe((response: Facility) => {
        this.toastService.showSuccess(`Facility Created successfully`);
        this.facilityAdded.emit({facility: response});
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
            },
          },
        ]
      : [];

    value.hospiceLocationId =
      this.hospiceLocation && this.hospiceLocation.id ? this.hospiceLocation.id : 0;
    value.siteId = null;
    value.address.addressLine3 = '';
    delete value.phoneNumber;
    delete value.isActive;

    return value;
  }
}
