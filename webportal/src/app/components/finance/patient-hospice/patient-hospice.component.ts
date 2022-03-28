import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {finalize} from 'rxjs/operators';
import {SieveOperators} from 'src/app/enums';
import {SieveRequest} from 'src/app/models';
import {HospiceLocationService, HospiceService, ToastService} from 'src/app/services';
import {FinanceService} from 'src/app/services/finance.service';
import {buildFilterString} from 'src/app/utils';

@Component({
  selector: 'app-patient-hospice',
  templateUrl: './patient-hospice.component.html',
  styleUrls: ['./patient-hospice.component.scss'],
})
export class PatientHospiceComponent implements OnInit {
  patient: any;
  patientHospiceForm: FormGroup;
  loading = false;
  formSubmit = false;
  hospices = [];
  hospiceLocations = [];

  @Input() set selectedPatient(patient: any) {
    if (patient?.uniqueId) {
      this.patientSelected(patient);
    }
  }
  @Output() refreshPatient = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    private hospiceService: HospiceService,
    private hospiceLocationService: HospiceLocationService,
    private financeService: FinanceService,
    private toaster: ToastService
  ) {}

  ngOnInit(): void {
    this.fetchHospices();
  }

  patientSelected(patient) {
    this.patient = patient;
    this.setPatientHospiceForm();
  }

  setPatientHospiceForm() {
    this.patientHospiceForm = this.fb.group({
      hospiceId: new FormControl(this.patient?.hospiceId ?? null, Validators.required),
      hospiceLocationId: new FormControl(
        this.patient?.hospiceLocationId ?? null,
        Validators.required
      ),
    });
    this.getHospiceLocations(this.patientHospiceForm.controls.hospiceId.value);
  }

  onSubmit() {
    this.formSubmit = true;
    this.financeService
      .fixHospice(this.patient.uniqueId, this.patientHospiceForm.value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((res: any) => {
        this.toaster.showSuccess(`Patient's Hospice fixed successfully`);
        const update = {
          ...this.patientHospiceForm.value,
          hospice: this.hospices.find(x => x.value === this.patientHospiceForm.value.hospiceId)
            ?.data,
          hospiceLocation: this.hospiceLocations.find(
            x => x.value === this.patientHospiceForm.value.hospiceLocationId
          )?.data,
        };
        this.refreshPatient.emit({update});
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
          !this.patientHospiceForm.controls.hospiceLocationId.value
        ) {
          this.patientHospiceForm.patchValue({
            hospiceLocationId: this.hospiceLocations[0].value,
          });
        }
      });
  }

  onHospiceChange(event) {
    if (event.value) {
      this.getHospiceLocations(event.value);
    }
  }
}
