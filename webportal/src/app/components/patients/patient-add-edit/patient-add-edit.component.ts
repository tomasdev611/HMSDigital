import {Component, OnInit, ViewChild} from '@angular/core';
import {OAuthService} from 'angular-oauth2-oidc';
import {FormGroup, FormBuilder, FormControl, Validators, FormArray} from '@angular/forms';
import {
  PatientService,
  ToastService,
  HospiceService,
  HospiceLocationService,
  HospiceFacilityService,
  OrderHeadersService,
  HospiceMemberService,
  SitesService,
  InventoryService,
  AddressVerificationService,
  UserService,
} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {Router, ActivatedRoute} from '@angular/router';
import {
  createPatch,
  convertToFtInch,
  convertToCm,
  getEnum,
  IsPermissionAssigned,
  deepCloneObject,
  showRequiredFields,
  buildFilterString,
  getDifferenceArray,
  getUTCDateAsLocalDate,
  getFormattedPhoneNumber,
  deepMerge,
  sortBy,
  redirectToSCA,
} from 'src/app/utils';
import {PaginationResponse, SieveRequest, VerifiedAddress, ConfirmDialog} from 'src/app/models';
import {EnumNames, SieveOperators} from 'src/app/enums';
import {forkJoin} from 'rxjs';
import {patientStatuses} from 'src/app/constants';
import {AddressVerificationModalComponent} from 'src/app/common';
import {Location} from '@angular/common';

@Component({
  selector: 'app-patient-add-edit',
  templateUrl: './patient-add-edit.component.html',
  styleUrls: ['./patient-add-edit.component.scss'],
})
export class PatientAddEditComponent implements OnInit {
  @ViewChild('addressverificationmodal')
  addressVerificationModal: AddressVerificationModalComponent;
  user: any = this.oAuthService.getIdentityClaims();
  searchCriteria: any;
  findDuplicates = false;
  maxDate = new Date();
  minDate = new Date(1901, 1, 1);
  patientForm: FormGroup;
  patient: any;
  editmode =
    this.route.snapshot && this.route.snapshot.url[1] && this.route.snapshot.url[1].path === 'edit'
      ? true
      : false;
  patientId =
    this.route.snapshot && this.route.snapshot.paramMap.get('patientId')
      ? parseInt(this.route.snapshot.paramMap.get('patientId'), 10)
      : null;
  patientSearchEnabled = false;
  formSubmit = false;
  patientToAdd: any;
  hospices = [];
  hospiceLocations = [];
  hospiceFacilities = [];
  notes = [];
  newPatientNote = '';
  patientFacilityAddress = [];
  patientFacilityAddressBackUp = [];
  addressTypes = getEnum(EnumNames.AddressTypes).map(x => {
    return {label: x.name, value: x.id};
  });
  phoneTypes = getEnum(EnumNames.PhoneNumberTypes);
  addressTypeHome = getEnum(EnumNames.AddressTypes).find((addr: any) => addr.name === 'Home');
  homeLocationId = null;
  showFacilitySelect = false;
  facilityLocationId = null;
  personalPhoneId = null;
  emergencyPhoneId = null;
  personalContact = getEnum(EnumNames.PhoneNumberTypes).find((ph: any) => ph.name === 'Personal');
  locationType = null;
  height = {
    feet: null,
    inch: null,
  };
  hospiceFacility = null;
  disablePatientUpdate = true;
  statusChangeEnabled = false;
  orderingEnabled = false;
  patientView = 'patient';
  orderType: any;
  showAddFacilityFlyer = false;
  userResponse: any;
  scaPath: any;
  scaHash: any;
  netSuiteOrderId: any;
  orderAction: any;
  orderHeadersResponse: PaginationResponse;
  loading = false;
  orderFilters = [];
  completedOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Completed'
  )?.id;
  orderHeadersFilter = new SieveRequest();
  orderHeaders = [
    {
      label: 'Order #',
      field: 'orderNumber',
      sortable: true,
      sortField: 'orderNumber',
    },
    {
      label: 'Order Date',
      field: 'orderDateTime',
      sortable: true,
      sortField: 'orderDateTime',
      fieldType: 'Date',
    },
    {
      label: 'Order Type',
      field: 'orderType',
      sortable: true,
      sortField: 'orderTypeId',
    },
    {
      label: 'Status',
      field: 'orderStatus',
      sortable: true,
      sortField: 'statusId',
    },
    {
      label: 'Completed Date',
      field: 'fulfillmentEndDateTime',
      sortable: true,
      sortField: 'fulfillmentEndDateTime',
      fieldType: 'Date',
    },
  ];
  orderDetailViewOpen = false;
  currentOrder: any;
  fulfilledItems = null;

  patientNotesHeader = [
    {
      label: 'Note',
      field: 'note',
    },
    {
      label: 'Created By',
      field: 'createdByUserName',
      class: 'lg',
    },
    {
      label: 'Created On',
      field: 'createdDateTime',
      fieldType: 'Date',
      class: 'lg',
    },
    {
      label: '',
      field: '',
      class: 'xs',
      deleteBtn: 'Delete',
      deleteBtnIcon: 'pi pi-trash',
    },
  ];
  statuses = patientStatuses;
  patientStatus = {
    status: null,
  };
  addressesToVerify = [];
  source: any;
  birthYearRange: string;
  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  confirmData = new ConfirmDialog();
  statusInvisible = true;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private toastService: ToastService,
    private route: ActivatedRoute,
    private hospiceService: HospiceService,
    private hospiceLocationService: HospiceLocationService,
    private hospiceFacilityService: HospiceFacilityService,
    private orderHeaderService: OrderHeadersService,
    private sitesService: SitesService,
    private oAuthService: OAuthService,
    private addressVerificationService: AddressVerificationService,
    private location: Location
  ) {
    this.route.queryParams.subscribe(this.handleQueryParams.bind(this));
    this.addressTypes.forEach(x => {
      if (x.label === 'Home') {
        this.homeLocationId = x.value;
      } else if (x.label === 'Facility') {
        this.facilityLocationId = x.value;
      }
    });
    this.phoneTypes.forEach(x => {
      if (x.name === 'Personal') {
        this.personalPhoneId = x.id;
      } else if (x.name === 'Emergency') {
        this.emergencyPhoneId = x.id;
      }
    });
  }

  ngOnInit(): void {
    this.setPatientForm();
    if (this.editmode && this.patientId) {
      this.getPatientDetail();
    } else {
      this.fetchHospices();
    }
    this.birthYearRange = '1901:' + new Date().getFullYear();
  }

  handleQueryParams(params) {
    this.patientView = params.view || this.patientView || 'patient';
    const queryParams = {view: this.patientView};
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams(queryParams).toString()
    );
  }

  tabChanged(event) {
    if (event.index === 0) {
      this.patientView = 'patient';
    } else if (event.index === 1) {
      this.patientView = 'inventory';
    } else if (event.index === 2) {
      this.patientView = 'order';
      if (!this.orderHeadersResponse) {
        this.getAllOrderHeaders();
      }
    }
    this.showAddFacilityFlyer = false;
    this.orderDetailViewOpen = false;
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.patientView}).toString()
    );
  }

  createAddressInstance() {
    return this.fb.group({
      address: this.fb.group({
        addressLine1: new FormControl(null, Validators.required),
        addressLine2: new FormControl(null),
        addressUuid: new FormControl(null),
        country: new FormControl('United States of America', Validators.required),
        state: new FormControl(null, Validators.required),
        city: new FormControl(null, Validators.required),
        zipCode: new FormControl(null, Validators.required),
        plus4Code: new FormControl(0, Validators.compose([Validators.maxLength(4)])),
        latitude: new FormControl(0, Validators.compose([Validators.min(-90), Validators.max(90)])),
        longitude: new FormControl(
          0,
          Validators.compose([Validators.min(-180), Validators.max(180)])
        ),
      }),
      addressTypeId: new FormControl(this.addressTypeHome?.id),
    });
  }

  setPatientForm() {
    this.patientForm = this.fb.group({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      hospiceId: new FormControl(null, Validators.required),
      dateOfBirth: new FormControl(null, Validators.required),
      addressType: new FormControl(this.addressTypeHome?.id),
      patientHeight: new FormControl(null),
      patientHeightFeet: new FormControl(null, Validators.required),
      patientHeightInch: new FormControl(null),
      patientWeight: new FormControl(null, Validators.required),
      isInfectious: new FormControl(false),
      hospiceLocationId: new FormControl(null, Validators.required),
      patientNotes: new FormControl([]),
      diagnosis: new FormControl(null),
      patientAddress: this.fb.array([], Validators.required),
      phoneNumbers: this.fb.array([
        this.fb.group({
          number: new FormControl(null, Validators.required),
          countryCode: new FormControl(1),
          numberTypeId: new FormControl(this.personalPhoneId),
          isSelfPhone: new FormControl(true),
          isPrimary: new FormControl(true),
          contactName: new FormControl(''),
        }),
        this.fb.group({
          number: new FormControl(null),
          countryCode: new FormControl(1),
          numberTypeId: new FormControl(this.emergencyPhoneId),
          isSelfPhone: new FormControl(true),
          isPrimary: new FormControl(false),
          contactName: new FormControl(''),
        }),
      ]),
    });
  }

  addAddressFormInstance() {
    const patientAddress = this.patientForm.controls.patientAddress as FormArray;
    const arraylen = patientAddress.length;
    const newPatientAddress: FormGroup = this.createAddressInstance();
    patientAddress.insert(arraylen, newPatientAddress);
  }

  fetchHospices() {
    this.hospiceService.getAllhospices().subscribe((response: any) => {
      // map the response to dropdown model {label: XYZ, value: 12}
      this.hospices = [...response.records.map(h => ({label: h.name, value: h.id}))];
      let patientHospice = null;
      if (this.patient) {
        patientHospice = this.hospices.find(h => h.value === this.patient.hospiceId);
      }
      // If any hospice is assigned to patient then select it otherwise,
      // check if the only one hospice is accessible for the user then select that
      patientHospice = patientHospice || (this.hospices.length === 1 ? this.hospices[0] : null);
      if (patientHospice) {
        this.getHospiceLocations(patientHospice.value);
        if (this.patient) {
          this.getPatientsFacilities(patientHospice.value);
        }
        this.getFacilities(patientHospice.value);
        this.patientForm.patchValue({hospiceId: patientHospice.value});
        return;
      }
      if (!this.hospices.length) {
        this.patientForm.controls.hospiceId.clearValidators();
        this.patientForm.controls.hospiceId.updateValueAndValidity();
        this.getHospiceLocations();
        this.getFacilities();
      }
    });
  }

  getHospiceLocations(hospiceId?) {
    const hospiceLocationRequest = new SieveRequest();
    if (hospiceId) {
      const filterValues = [
        {
          field: 'hospiceId',
          operator: SieveOperators.Equals,
          value: [hospiceId],
        },
      ];
      hospiceLocationRequest.filters = buildFilterString(filterValues);
    }
    this.hospiceLocationService
      .getLocations(hospiceLocationRequest)
      .pipe(finalize(() => {}))
      .subscribe((response: any) => {
        this.hospiceLocations = [
          ...response.records.map(x => ({
            label: x.name,
            value: x.id,
            hospiceId: x.hospiceId,
          })),
        ];
        if (
          this.hospiceLocations.length === 1 ||
          !this.patientForm.controls.hospiceLocationId.value
        ) {
          this.patientForm.patchValue({
            hospiceLocationId: this.hospiceLocations[0].value,
          });
        }
      });
  }

  getFacilities(hospiceId?) {
    const facilitiesRequest = new SieveRequest();
    if (hospiceId) {
      this.hospiceFacilityService
        .getAllHospiceFacilities(hospiceId)
        .pipe(finalize(() => {}))
        .subscribe((response: any) => {
          this.hospiceFacilities = [
            ...response.records.map(hf => {
              const phoneNumber = hf.facilityPhoneNumber?.[0]?.phoneNumber?.number;
              return {
                label: hf.name,
                value: hf.id,
                address: hf.address,
                phoneNumber,
              };
            }),
          ];
        });
    } else {
      this.hospiceFacilityService
        .getFacilities(facilitiesRequest)
        .pipe(finalize(() => {}))
        .subscribe((response: any) => {
          this.hospiceFacilities = [
            ...response.records.map(hf => {
              const phoneNumber = hf.facilityPhoneNumber?.[0]?.phoneNumber?.number;
              return {
                label: hf.name,
                value: hf.id,
                address: hf.address,
                phoneNumber,
              };
            }),
          ];
        });
    }
  }

  getPatientsFacilities(hospiceId) {
    const filterValues = [
      {
        field: 'patientUuid',
        operator: SieveOperators.Equals,
        value: [this.patient.uniqueId],
      },
    ];
    const facilityReq = {filters: buildFilterString(filterValues)};
    this.hospiceFacilityService
      .getHospicePatientsFacilities(hospiceId, facilityReq)
      .pipe(finalize(() => {}))
      .subscribe((response: any) => {
        response.records.map(hf => {
          const phoneNumber = hf.facility?.facilityPhoneNumber?.[0]?.phoneNumber?.number;
          this.patientFacilityAddress = [
            ...this.patientFacilityAddress,
            {
              addressTypeId: this.facilityLocationId,
              address: hf.facility.address,
              facilityId: hf.facilityId,
              phoneNumber,
              patientRoomNumber: hf.patientRoomNumber,
            },
          ];
          this.patientFacilityAddressBackUp = [
            ...this.patientFacilityAddressBackUp,
            {
              addressTypeId: this.facilityLocationId,
              address: hf.facility.address,
              facilityId: hf.facilityId,
              phoneNumber,
              patientRoomNumber: hf.patientRoomNumber,
            },
          ];
        });
        this.updateAddressValidity();
      });
  }

  getPatientDetail() {
    forkJoin([
      this.patientService.getPatientById(this.patientId),
      this.patientService.getPatientNotes(this.patientId),
    ]).subscribe((patientResponses: any) => {
      const [patientProfile, patientNotes] = patientResponses;

      patientProfile.patientAddress.forEach(patientAddress => {
        if (!patientAddress.address.country) {
          patientAddress.address.country = 'United States of America';
        }
      });
      patientProfile.patientNotes = patientNotes;
      this.patientStatus.status = patientProfile.status;
      this.refreshPatientInfo(patientProfile, true);
      if (this.patient?.status !== 'Blank') {
        this.statusInvisible = false;
      }
      this.fetchHospices();
      if (this.patientView === 'order') {
        this.getAllOrderHeaders();
      }
    });
  }

  refreshPatientInfo(response, updateForm = false) {
    this.patient = deepCloneObject(response);

    const patientAddress = this.patientForm.controls.patientAddress as FormArray;
    let arraylen = patientAddress.length;
    const formDataModel = this.mapResponseToFormModel(response);
    this.patientForm.patchValue(formDataModel);
    formDataModel.patientAddress.forEach(x => {
      const newPatientAddress: FormGroup = this.createAddressInstance();
      newPatientAddress.patchValue(x);
      patientAddress.insert(arraylen, newPatientAddress);
      arraylen++;
    });
  }

  mapResponseToFormModel(responseObj) {
    const patientAddress = responseObj.patientAddress.length
      ? responseObj.patientAddress[0]
      : {address: deepCloneObject(this.patientForm.get('address')?.value)};

    const patientHeightFtInch = convertToFtInch(responseObj.patientHeight);
    const formDataModel = {
      ...responseObj,
      dateOfBirth: getUTCDateAsLocalDate(responseObj.dateOfBirth),
      addressType: patientAddress?.addressTypeId,
      patientHeightFeet: patientHeightFtInch.feet,
      patientHeightInch: patientHeightFtInch.inch,
      patientWeight: responseObj.patientWeight,
    };
    return formDataModel;
  }

  mapFormDataToRequest(formData) {
    let requestData = {};
    if (this.editmode) {
      requestData = {
        ...this.patient,
      };
    }
    const phoneNumbers = formData.phoneNumbers.filter(x => {
      if (x.number) {
        x.contactName = x.contactName?.trim();
        x.number = parseInt(x.number, 10) ?? 0;
        return x;
      }
    });
    requestData = {
      ...requestData,
      ...formData,
      firstName: formData.firstName.trim(),
      lastName: formData.lastName.trim(),
      diagnosis: formData.diagnosis?.trim() ?? null,
      dateOfBirth: formData.dateOfBirth ?? null,
      patientHeight: convertToCm(formData?.patientHeightFeet, formData?.patientHeightInch),
      phoneNumbers,
    };
    return requestData;
  }

  onSubmitPatient() {
    this.checkAddressValidity();
  }

  handleSubmitPatient() {
    if (this.patientForm.value.patientHeightInch === null) {
      this.patientForm.patchValue({patientHeightInch: 0});
    }
    if (this.patientForm.valid) {
      this.onSubmitPatient();
    }
  }

  checkAddressValidity() {
    this.addressesToVerify = [];
    const verifyRequests = [];
    const addressUniqueIds = [];

    this.formSubmit = true;

    const patientAddress = this.patientForm.value.patientAddress;

    if (patientAddress.length) {
      patientAddress.forEach((address, i) => {
        addressUniqueIds.push(address.address.addressUuid);
        const params = this.formatAddressForVerification(address.address);
        verifyRequests.push(this.addressVerificationService.verifyAddress(params));
      });

      forkJoin(verifyRequests)
        .pipe(
          finalize(() => {
            this.formSubmit = false;
          })
        )
        .subscribe((resArray: VerifiedAddress[]) => {
          resArray.forEach((res, index) => {
            res.addressUuid = addressUniqueIds[index];
            const isZipValid =
              patientAddress[index].address.zipCode === res.zipCode &&
              patientAddress[index].address.plus4Code === res.plus4Code;
            if (!res.isValid || !isZipValid) {
              this.addressesToVerify.push({
                ...this.patientForm.value.patientAddress[index].address,
                addressIndex: index,
              });
            }
          });
          if (this.addressesToVerify.length) {
            this.addressVerificationModal.showModal();
          } else {
            this.verificationSucces();
          }
        });
    } else {
      this.verificationSucces();
    }
  }

  verificationSucces(doNotVerifyAddress = false) {
    const patientFormValue = this.patientForm.getRawValue();
    if (!patientFormValue.hospiceId) {
      const location = this.hospiceLocations.find(
        x => x.value === patientFormValue.hospiceLocationId
      );
      if (location) {
        patientFormValue.hospiceId = location.hospiceId;
      }
    }
    const value = deepCloneObject(patientFormValue);
    const body = deepMerge(this.patient, this.formatValues(value));
    if (this.patientId) {
      const patch = createPatch(this.patient, body);
      if (patch.length > 0) {
        this.formSubmit = true;
        this.updatePatient(this.patientId, patch, {doNotVerifyAddress});
      } else {
        this.toastService.showInfo(`No value to update`);
      }
    } else {
      this.patientToAdd = body;
      this.findDuplicates = true;
      this.searchCriteria = {...this.patientForm.value};
    }
  }

  updateAddressWithSuggestions(suggestedAddresses) {
    const patientAddresses = this.patientForm.controls.patientAddress.value;
    patientAddresses.forEach((patientAddress, index) => {
      const address = suggestedAddresses.find(
        (suggestedaddress: any) => suggestedaddress.addressIndex === index
      );
      if (address) {
        patientAddress.address.plus4Code = address.plus4Code;
        patientAddress.address.zipCode = address.zipCode;
        patientAddress.address.addressLine1 = address.addressLine1;
        patientAddress.address.addressLine2 = address.addressLine2;

        patientAddress.address.city = address.city;
        patientAddress.address.state = address.state;
        patientAddress.address.latitude = address.latitude;
        patientAddress.address.longitude = address.longitude;

        patientAddress.address.isVerified = true;
      }
    });
    const doNotVerifyAddress = !patientAddresses.every(addr => addr.address.isVerified);
    patientAddresses.forEach(addr => delete addr.address.isVerified);
    this.patientForm.controls.patientAddress.setValue(patientAddresses);

    this.verificationSucces(doNotVerifyAddress);
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

  savePatient(value) {
    this.formSubmit = true;
    this.patientService
      .createPatient(value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.toastService.showSuccess(`Patient Created successfully`);
        if (this.patientFacilityAddress.length > 0) {
          const body = {
            facilityIds: this.patientFacilityAddress,
            patientUuid: response.uniqueId,
          };
          this.assignPatientsFacility(response.hospiceId, body);
        } else {
          this.location.back();
        }
      });
  }

  updatePatient(patientId, patch, queryParams?) {
    this.patientService
      .updatePatient(patientId, patch, queryParams)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe(
        (response: any) => {
          this.toastService.showSuccess(`Patient updated successfully`);
          const body = {
            facilityIds: this.patientFacilityAddress,
            patientUuid: response.uniqueId,
          };
          this.assignPatientsFacility(response.hospiceId, body);
        },
        error => {
          console.log('error', error);
          throw error;
        }
      );
  }

  assignPatientsFacility(hospiceId, body) {
    const differenceArray = getDifferenceArray(
      body.facilityIds.map(x => x.facilityId),
      this.patientFacilityAddressBackUp.map(x => x.facilityId)
    );
    if (differenceArray.length > 0) {
      body.facilityPatientRooms = body.facilityIds.map(facility => {
        return {
          facilityId: facility.facilityId,
          patientRoomNumber: facility.patientRoomNumber,
        };
      });
      this.hospiceFacilityService
        .assignPatientToFacility(hospiceId, body)
        .pipe(finalize(() => {}))
        .subscribe(
          (response: any) => {
            this.toastService.showSuccess(`Patient's facility association updated successfully`);
            if (this.orderType) {
              this.proceedToCreateOrder(body.patientUuid);
            } else {
              this.location.back();
            }
          },
          error => {
            console.log('error', error);
            this.toastService.showError(`Failed to assign patient to facility`);
          }
        );
    } else {
      if (this.orderType) {
        this.proceedToCreateOrder(body.patientUuid);
      } else {
        this.location.back();
      }
    }
  }

  formatValues(value) {
    value = deepCloneObject(value);
    value = this.mapFormDataToRequest(value);
    delete value.addressType;
    delete value.patientHeightFeet;
    delete value.patientHeightInch;
    delete value.phoneNumber;
    return value;
  }

  onHospiceChange(event) {
    if (event.value) {
      this.getHospiceLocations(event.value);
      this.getFacilities(event.value);
    }
  }

  closeOrderEnabled(event?) {
    this.orderingEnabled = false;
  }

  canView(tabName) {
    return IsPermissionAssigned(tabName, 'Read');
  }

  showSearch() {
    this.patientSearchEnabled = true;
  }

  continueWithDuplicate() {
    this.closeSearch();
    this.addressVerificationModal.closeModal();
    this.savePatient(this.patientToAdd);
  }

  closeSearch() {
    this.patientSearchEnabled = false;
    this.findDuplicates = false;
  }

  addNote() {
    const existingNotes = this.patientForm.get('patientNotes').value;
    if (this.newPatientNote) {
      existingNotes.push({
        note: this.newPatientNote,
        createdByUserName: this.user.name,
        createdDateTime: new Date(),
      });
      this.patientForm.get('patientNotes').setValue([...existingNotes]);
    }
    this.newPatientNote = '';
  }

  changeNote(event, index) {
    if (!event.target.value) {
      return;
    }
    const notesToChange = this.patientForm.get('patientNotes').value;
    notesToChange.splice(index, 1, {
      ...notesToChange[index],
      note: event.target.value,
    });
    this.patientForm.get('patientNotes').setValue([...notesToChange]);
  }

  isCurrentView(view) {
    return this.patientView === view;
  }

  checkFormValidity(form) {
    return showRequiredFields(form, 'PatientForm');
  }

  facilitySelected(event) {
    const facility = this.hospiceFacilities.find(x => x.value === event.value);
    const index = this.patientFacilityAddress.findIndex(x => x.facilityId === event.value);
    if (index === -1) {
      this.patientFacilityAddress = [
        ...this.patientFacilityAddress,
        {
          addressTypeId: this.facilityLocationId,
          address: facility.address,
          facilityId: event.value,
          phoneNumber: facility.phoneNumber,
        },
      ];
    }
    this.showFacilitySelect = false;
    this.hospiceFacility = null;
    this.updateAddressValidity();
  }
  locationTypeSelected(event) {
    if (this.addressTypeHome.id === event.value) {
      this.addAddressFormInstance();
    } else {
      this.showFacilitySelect = true;
    }
  }

  removeHomeAddress(index) {
    const patientAddress = this.patientForm.controls.patientAddress as FormArray;
    patientAddress.removeAt(index);
    this.updateAddressValidity();
  }

  deleteFacilityAddress(event) {
    this.patientFacilityAddress.splice(event, 1);
    this.updateAddressValidity();
  }

  updateAddressValidity() {
    if (this.patientFacilityAddress.length === 0) {
      this.patientForm.controls.patientAddress.setValidators([Validators.required]);
    } else {
      this.patientForm.controls.patientAddress.clearValidators();
    }
    this.patientForm.controls.patientAddress.updateValueAndValidity();
  }

  addFacility() {
    this.showAddFacilityFlyer = true;
  }

  closeFacilityAdd() {
    this.showAddFacilityFlyer = false;
  }

  recieveAddedFacility(event) {
    const phoneNumber = event.facility?.facilityPhoneNumber?.[0]?.phoneNumber?.number;
    this.patientFacilityAddress = [
      ...this.patientFacilityAddress,
      {
        addressTypeId: this.facilityLocationId,
        address: event.facility.address,
        facilityId: event.facility.id,
        phoneNumber,
      },
    ];
    this.getHospiceLocations(this.patientForm.get('hospiceId').value);
    this.getFacilities(this.patientForm.get('hospiceId').value);
    this.showFacilitySelect = false;
    this.hospiceFacility = null;
    this.showAddFacilityFlyer = false;
    this.updateAddressValidity();
  }

  proceedToCreateOrder(uniqueId) {
    const reidrectUri = `/orders/sca/${this.orderType}/${uniqueId}?scaPath=${
      this.scaPath || ''
    }&scaHash=${this.scaHash || ''}&netSuiteOrderId=${this.netSuiteOrderId || ''}&action=${
      this.orderAction || ''
    }`;
    redirectToSCA(reidrectUri);
  }

  getAllOrderHeaders() {
    this.loading = true;
    this.orderFilters = [
      {
        field: 'PatientUUID',
        value: [this.patient?.uniqueId],
        operator: SieveOperators.Equals,
      },
    ];
    this.orderHeadersFilter.filters = buildFilterString(this.orderFilters);

    this.orderHeaderService
      .getAllOrderHeaders(this.orderHeadersFilter)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((response: any) => {
        this.orderHeadersResponse = response;
        this.orderHeadersResponse.records.map(record => {
          if (record.statusId !== this.completedOrderStatusId) {
            record.fulfillmentEndDateTime = null;
          }
        });
      });
  }

  fetchNextOrders({pageNum}) {
    if (!this.orderHeadersResponse || pageNum > this.orderHeadersResponse.totalPageCount) {
      return;
    }
    this.orderHeadersFilter.page = pageNum;
    this.getAllOrderHeaders();
  }

  fetchPrevOrders() {
    if (this.orderHeadersResponse.pageNumber <= 1) {
      return;
    }
    this.orderHeadersFilter.page = this.orderHeadersFilter.page - 1;
    this.getAllOrderHeaders();
  }

  sortOrders(event) {
    switch (event.order) {
      case 0:
        this.orderHeadersFilter.sorts = '';
        break;
      case 1:
        this.orderHeadersFilter.sorts = event.field;
        break;
      case -1:
        this.orderHeadersFilter.sorts = '-' + event.field;
        break;
    }
    this.getAllOrderHeaders();
  }

  orderSelected(event) {
    this.orderDetailViewOpen = true;
    this.currentOrder = event.currentRow;
    this.currentOrder.patient = this.patient;
    this.currentOrder.patient.name = `${this.currentOrder.patient.firstName} ${this.currentOrder.patient.lastName}`;
    this.getSiteInfo();
    this.getOrderDetailInformation();
  }

  getOrderDetailInformation() {
    this.fulfilledItems = [];
    this.orderHeaderService
      .getOrderHeaderById(this.currentOrder.id, true)
      .subscribe((response: any) => {
        this.currentOrder.orderNotes = response?.orderNotes ?? [];
        this.currentOrder.nurse = response?.orderingNurse ?? '';
        this.fulfilledItems = response?.orderFulfillmentLineItems ?? [];
        this.currentOrder.createdByUser = response?.createdByUser ?? '';
        this.currentOrder.modifiedByUser = response?.modifiedByUser ?? '';
        this.currentOrder.assignedDriver = response?.assignedDriver ?? '';
        this.currentOrder.orderNotes = sortBy(
          this.currentOrder.orderNotes,
          'createdDateTime',
          'date'
        );
      });
    this.patientService
      .getPatientNotes(this.currentOrder.patient?.id)
      .subscribe((response: any) => {
        response = sortBy(response, 'createdDateTime', 'date');
        this.currentOrder.patientNotes = response;
      });
  }

  closeOrderDetails(event) {
    this.orderDetailViewOpen = false;
  }

  getPrimaryContact() {
    if (this.patient?.phoneNumbers?.length) {
      const contact = this.patient?.phoneNumbers?.find(p => p.numberType === 'Personal');
      if (contact) {
        return getFormattedPhoneNumber(contact?.number);
      }
    }
    return '';
  }

  shouldShowAddr(type = 'delivery') {
    if (this.currentOrder) {
      switch (type) {
        case 'delivery':
          return ['Delivery', 'Respite', 'Exchange', 'Move'].includes(this.currentOrder?.orderType);
        case 'pickup':
          return ['Pickup', 'Move'].includes(this.currentOrder?.orderType);
      }
    }
    return false;
  }

  getFormattedAddress(addressType = 'delivery') {
    let address;
    switch (addressType) {
      case 'delivery':
        address = this.currentOrder?.deliveryAddress;
        break;
      case 'pickup':
        address = this.currentOrder?.pickupAddress;
        break;
    }
    if (address) {
      return `${address.addressLine1 || ''} ${address.addressLine2 || ''} ${
        address.addressLine3 || ''
      },
       ${address.city || ''}, ${address.state || ''} ${address.zipCode || 0} - ${
        address.plus4Code || 0
      }`;
    }
    return '';
  }

  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }

  getFulfilledItemsForLineItem(orderLineItemId) {
    return this.fulfilledItems.filter(fi => fi.orderLineItemId === orderLineItemId);
  }

  getFulfilledLineItemsCount(orderLineItemId) {
    return this.getFulfilledItemsForLineItem(orderLineItemId).reduce((a: any, b: any) => {
      return a + b.quantity;
    }, 0);
  }

  getSiteInfo() {
    if (this.checkPermission('Site', 'Read')) {
      this.sitesService.searchSites({searchQuery: ''}).subscribe(res => {
        const site = res.records.find(x => x.id === this.currentOrder.siteId);
        this.currentOrder.site = site;
      });
    }
  }

  deletePatientNote(index) {
    const notesToChange = this.patientForm.get('patientNotes').value;
    notesToChange.splice(index, 1);
    this.patientForm.get('patientNotes').setValue([...notesToChange]);
  }

  confirmAccepted() {
    this.orderType = 'Pickup';
    this.proceedToCreateOrder(this.patient.uniqueId);
  }

  confirmRejected() {
    this.patientStatus = {
      ...this.patientStatus,
      ...{
        status: this.patient.status,
      },
    };
    this.checkAddressValidity();
  }

  getHospiceNameById(hospiceId: number) {
    return this.hospices.find(hospice => hospice.value === hospiceId).label;
  }

  goBack() {
    this.location.back();
  }
}
