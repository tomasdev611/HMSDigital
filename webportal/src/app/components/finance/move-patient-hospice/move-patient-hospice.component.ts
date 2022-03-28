import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {finalize} from 'rxjs/operators';
import {EnumNames, SieveOperators} from 'src/app/enums';
import {SieveRequest} from 'src/app/models';
import {
  HospiceFacilityService,
  HospiceLocationService,
  HospiceService,
  OrderHeadersService,
  ToastService,
} from 'src/app/services';
import {FinanceService} from 'src/app/services/finance.service';
import {buildFilterString, getEnum, setDateFromInlineInput} from 'src/app/utils';

@Component({
  selector: 'app-move-patient-hospice',
  templateUrl: './move-patient-hospice.component.html',
  styleUrls: ['./move-patient-hospice.component.scss'],
})
export class MovePatientHospiceComponent implements OnInit {
  patient: any;
  loading = false;
  formSubmit = false;
  hospices = [];
  hospiceLocations = [];
  hospiceFacilities = [];
  movePatientForm: FormGroup;
  minMovementDate: Date;
  openOrders = [];
  @Input() set selectedPatient(patient: any) {
    if (patient?.uniqueId) {
      this.patientSelected(patient);
    }
  }
  completedOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Completed'
  )?.id;
  cancelledOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Cancelled'
  )?.id;
  facilityAddress = '';
  @Output() refreshPatient = new EventEmitter<any>();
  constructor(
    private fb: FormBuilder,
    private hospiceLocationService: HospiceLocationService,
    private financeService: FinanceService,
    private toaster: ToastService,
    private hospiceService: HospiceService,
    private hospiceFacilityService: HospiceFacilityService,
    private orderHeaderService: OrderHeadersService
  ) {}

  ngOnInit(): void {
    this.fetchHospices();
  }

  getOpenOrders() {
    this.loading = true;
    const orderRequest = new SieveRequest();
    const orderFilters = [
      {
        field: 'statusId',
        value: [this.completedOrderStatusId],
        operator: SieveOperators.NotEquals,
      },
      {
        field: 'statusId',
        value: [this.cancelledOrderStatusId],
        operator: SieveOperators.NotEquals,
      },
      {
        field: 'PatientUUID',
        value: [this.patient?.uniqueId],
        operator: SieveOperators.Equals,
      },
    ];
    orderRequest.filters = buildFilterString(orderFilters);
    this.orderHeaderService
      .getAllOrderHeaders(orderRequest)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((response: any) => {
        this.openOrders = response?.records ?? [];
      });
  }

  patientSelected(patient) {
    this.patient = patient;
    this.minMovementDate = new Date(this.patient.createdDateTime);
    this.setMovePatientForm();
    this.getOpenOrders();
  }

  setMovePatientForm() {
    this.movePatientForm = this.fb.group({
      hospiceId: new FormControl(this.patient?.hospiceId ?? null, Validators.required),
      hospiceLocationId: new FormControl(
        this.patient?.hospiceLocationId ?? null,
        Validators.required
      ),
      facilityId: new FormControl(null),
      movementDate: new FormControl(null, Validators.required),
      patientRoomNumber: new FormControl(null),
    });
    this.getHospiceLocations(this.patient?.hospiceId);
    this.getHospiceFacilities(this.patient?.hospiceId);
  }

  getHospiceLocations(hospiceId) {
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
            data: x,
          })),
        ];
        if (
          this.hospiceLocations.length === 1 ||
          !this.movePatientForm.controls.hospiceLocationId.value
        ) {
          this.movePatientForm.patchValue({
            hospiceLocationId: this.hospiceLocations[0].value,
          });
        }
      });
  }

  getHospiceFacilities(hospiceId) {
    const hospiceFacilitiyRequest = new SieveRequest();
    if (hospiceId) {
      const filterValues = [
        {
          field: 'hospiceId',
          operator: SieveOperators.Equals,
          value: [hospiceId],
        },
      ];
      hospiceFacilitiyRequest.filters = buildFilterString(filterValues);
    }
    this.hospiceFacilityService
      .getFacilities(hospiceFacilitiyRequest)
      .pipe(finalize(() => {}))
      .subscribe((response: any) => {
        this.hospiceFacilities = [
          ...response.records.map(x => ({
            label: x.name,
            value: x.id,
            hospiceId: x.hospiceId,
            data: x,
          })),
        ];
      });
  }

  onSubmit() {
    this.formSubmit = true;
    const {facilityId, patientRoomNumber} = this.movePatientForm.value;
    const body = {
      ...this.movePatientForm.value,
      facilities: facilityId
        ? [
            {
              facilityId,
              patientRoomNumber: patientRoomNumber ?? 0,
            },
          ]
        : [],
    };
    delete body.facilityId;
    delete body.patientRoomNumber;
    this.financeService
      .movePatientHospice(this.patient.uniqueId, body)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((res: any) => {
        const update = {
          ...this.patient,
        };
        this.facilityAddress = '';
        this.refreshPatient.emit({update});
        this.toaster.showSuccess(`Patient's Hospice moved successfully`);
      });
  }

  fetchHospices() {
    this.hospiceService.getAllhospices().subscribe((response: any) => {
      this.hospices = [
        ...response.records.map(h => ({
          label: h.name,
          value: h.id,
          data: h,
        })),
      ];
    });
  }
  onHospiceChange(event) {
    if (event.value) {
      this.getHospiceLocations(event.value);
      this.getHospiceFacilities(event.value);
      this.movePatientForm.patchValue({
        hospiceLocationId: null,
        facilityId: null,
      });
      this.facilityAddress = '';
    }
  }

  updateDate(event) {
    const dateToAppend = setDateFromInlineInput(event.target.value);
    if (dateToAppend) {
      this.movePatientForm.patchValue({
        movementDate: dateToAppend,
      });
    }
  }
  onFacilitiesChange(event) {
    const address = this.hospiceFacilities.find(x => x.value === event.value).data?.address;
    this.facilityAddress = `${address.addressLine1 ?? ''}${
      address.addressLine2 ? ' ' + address.addressLine2 : ''
    }${address.addressLine3 ? ' ' + address.addressLine3 : ''}, ${address.city || ''}, ${
      address.state || ''
    } ${address.zipCode || 0} - ${address.plus4Code || 0}`;
  }
}
