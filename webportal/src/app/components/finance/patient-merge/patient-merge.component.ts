import {DatePipe} from '@angular/common';
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {forkJoin} from 'rxjs';
import {finalize} from 'rxjs/operators';
import {SieveOperators} from 'src/app/enums';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {PhonePipe} from 'src/app/pipes';
import {
  HospiceLocationService,
  HospiceService,
  PatientService,
  ToastService,
} from 'src/app/services';
import {FinanceService} from 'src/app/services/finance.service';
import {
  buildFilterString,
  convertToFtInch,
  getFormattedPhoneNumber,
  getUniqArray,
} from 'src/app/utils';

@Component({
  selector: 'app-patient-merge',
  templateUrl: './patient-merge.component.html',
  styleUrls: ['./patient-merge.component.scss'],
})
export class PatientMergeComponent implements OnInit {
  patient: any;
  searchKey = '';
  tobeMergedPatient: any;
  patients = [];
  backupPatients = [];
  formSubmit = false;
  mergeHeader = [
    {
      label: 'First Name',
      value: 'firstName',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
    },
    {
      label: 'Last Name',
      value: 'lastName',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
    },
    {
      label: 'D.O.B',
      value: 'dateOfBirth',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
      fieldType: 'Date',
    },
    {
      label: 'Diagnosis',
      value: 'diagnosis',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
    },
    {
      label: 'Height',
      value: 'patientHeight',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
      fieldType: 'feetInches',
    },
    {
      label: 'Weight',
      value: 'patientWeight',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
      fieldType: 'weight',
    },
    {
      label: 'Infectious Patient',
      value: 'isInfectious',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
      fieldType: 'BoolToYesNo',
    },
    {
      label: 'Primary Phone',
      value: 'primaryPhone',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
      fieldType: 'Phone',
    },
    {
      label: 'Secondary Phone',
      value: 'secondaryPhone',
      firstValue: '',
      secondValue: '',
      selected: 'firstValue',
      fieldType: 'Phone',
    },
  ];
  patientMergeHistory: PaginationResponse;
  shouldShowPatientMergeHistory = false;

  @Input() set selectedPatient(patient: any) {
    if (patient?.uniqueId) {
      this.patientSelected(patient);
    }
  }
  @Output() refreshPatient = new EventEmitter<any>();

  constructor(
    private patientService: PatientService,
    private hospiceService: HospiceService,
    private hospiceLocationService: HospiceLocationService,
    private datePipe: DatePipe,
    private phonePipe: PhonePipe,
    private financeService: FinanceService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {}

  patientSelected(patient) {
    this.patient = patient;
    this.getPatients();
  }

  patientSelectedToMerge(patient) {
    this.tobeMergedPatient = patient;
    this.patient.selected = 1;
    this.tobeMergedPatient.selected = 0;
    this.mergeHeader = this.mergeHeader.map((x, i) => {
      x.firstValue = this.patient[x.value];
      x.secondValue = this.tobeMergedPatient[x.value];
      x.selected = 'firstValue';
      if (x.fieldType === 'Phone') {
        if (x.value === 'primaryPhone') {
          x.firstValue = this.patient.phoneNumbers.find(p => p.isPrimary)?.number ?? null;
          x.secondValue =
            this.tobeMergedPatient.phoneNumbers.find(p => p.isPrimary)?.number ?? null;
        } else if (x.value === 'secondaryPhone') {
          x.firstValue = this.patient.phoneNumbers.find(p => !p.isPrimary)?.number ?? null;
          x.secondValue =
            this.tobeMergedPatient.phoneNumbers.find(p => !p.isPrimary)?.number ?? null;
        }
      }

      return x;
    });
  }

  getPatients() {
    const req = new SieveRequest();
    const filterValues = [
      {
        field: 'hospiceLocationId',
        operator: SieveOperators.Equals,
        value: [this.patient.hospiceLocationId],
      },
      {
        field: 'uniqueId',
        operator: SieveOperators.NotEquals,
        value: [this.patient.uniqueId],
      },
    ];
    req.filters = buildFilterString(filterValues);
    this.patientService.getPatients(req).subscribe((res: any) => {
      if (res.records.length) {
        this.backupPatients = res.records.map((p: any) => {
          return {
            name: `${p?.firstName} ${p?.lastName}`,
            patientUuid: p.uniqueId,
            ...p,
          };
        });
        this.getAdditionals(res, this.backupPatients);
      }
    });
  }

  getAdditionals(res, patients) {
    const requests = [];
    if (res.records.length) {
      const hospiceIds = getUniqArray(
        res.records.map(({hospiceId}: any) => {
          return hospiceId;
        })
      );
      const filter = [
        {
          field: 'id',
          operator: SieveOperators.Equals,
          value: [...hospiceIds],
        },
      ];
      const hospiceReq = {filters: buildFilterString(filter)};
      requests.push(this.hospiceService.getAllhospices(hospiceReq));
      const hospiceLocationIds = getUniqArray(
        res.records.map(({hospiceLocationId}: any) => {
          return hospiceLocationId;
        })
      );
      const filterValues = [
        {
          field: 'id',
          operator: SieveOperators.Equals,
          value: [...hospiceLocationIds],
        },
      ];
      const hospiceLocationReq = {filters: buildFilterString(filterValues)};
      requests.push(this.hospiceLocationService.getLocations(hospiceLocationReq));
      forkJoin(requests).subscribe(response => {
        const [hospices, hospiceLocations]: any = response;
        patients = patients.map(x => {
          const hospice = hospices?.records.find(h => h.id === x.hospiceId);
          if (hospice) {
            x.hospice = hospice;
          }
          const hospiceLocation = hospiceLocations?.records.find(h => h.id === x.hospiceLocationId);
          if (hospiceLocation) {
            x.hospiceLocation = hospiceLocation;
          }
          return x;
        });
      });
    }
  }

  clearPatientSuggestions() {
    this.searchKey = '';
    this.patients = [];
  }

  handleDropdown(event) {
    if (event.query) {
      if (this.searchKey === event.query) {
        this.patients = [...this.patients];
      } else {
        this.search(event);
      }
    } else {
      this.patients = [...this.backupPatients];
    }
  }

  search(event) {
    if (event.query) {
      this.searchKey = event.query;
      const filters = buildFilterString([
        {
          field: 'uniqueId',
          operator: SieveOperators.NotEquals,
          value: [this.patient.uniqueId],
        },
      ]);
      this.patientService
        .searchPatientsBySearchQuery({searchQuery: event.query, filters})
        .subscribe((res: any) => {
          if (res) {
            this.patients = res?.records.map((r: any) => ({
              name: `${r?.firstName} ${r?.lastName}`,
              patientUuid: r.uniqueId,
              ...r,
            }));
            this.getAdditionals(res, this.patients);
          }
        });
    }
  }

  selectAll(selected, value) {
    value.selected = 0;
    this.mergeHeader = this.mergeHeader.map(x => {
      x.selected = selected;
      return x;
    });
  }

  selectValue(value) {
    value.selected = 0;
  }

  getValue(value, type?) {
    switch (type) {
      case 'Date':
        return this.datePipe.transform(value, 'LLL dd, yyyy');
      case 'BoolToYesNo':
        return typeof value === 'boolean' ? (value ? 'Yes' : 'No') : '';
      case 'Phone':
        return this.phonePipe.transform(value);
      case 'weight':
        return (value += ' lbs');
      case 'feetInches':
        return this.getHeightInFeetInches(value);
      default:
        return value;
    }
  }

  submitMerge() {
    const obj: any = {};
    obj.phoneNumbers = [];
    obj.duplicatePatientUUID = this.tobeMergedPatient.uniqueId;
    this.mergeHeader.map(x => {
      obj[x.value] = x[x.selected];
      if (x.fieldType === 'Phone') {
        const phonelist =
          x.selected === 'firstValue'
            ? this.patient.phoneNumbers
            : this.tobeMergedPatient.phoneNumbers;
        const phone = phonelist.find(p => p.number === x[x.selected]);
        if (phone) {
          obj.phoneNumbers.push(phone);
        }
        delete obj[x.value];
      }
    });
    this.formSubmit = true;
    this.financeService
      .mergePatients(this.patient.uniqueId, obj)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((res: any) => {
        this.toastService.showSuccess(`Patients merged successfully`);
        const update = {...obj};
        this.tobeMergedPatient = null;
        this.refreshPatient.emit({update});
      });
  }

  showMergeHistory() {
    const filters = [
      {
        fields: ['patientUuid', 'duplicatePatientUuid'],
        value: [this.patient.uniqueId],
        operator: SieveOperators.Equals,
      },
    ];
    const sieveRequest = new SieveRequest();
    sieveRequest.filters = buildFilterString(filters);
    sieveRequest.sorts = '-createdDateTime';
    this.patientService
      .getPatientMergeHistory(sieveRequest)
      .subscribe((res: PaginationResponse) => {
        this.patientMergeHistory = res;
        this.shouldShowPatientMergeHistory = true;
      });
  }

  closeFlyout() {
    this.patientMergeHistory = null;
    this.shouldShowPatientMergeHistory = false;
  }

  getPhoneNumber(phoneNumbers: any[], primary = false) {
    const phoneNumber = phoneNumbers.find(pn => pn.isPrimary === primary);
    if (phoneNumber) {
      return getFormattedPhoneNumber(phoneNumber.number);
    }
    return '';
  }

  getHeightInFeetInches(heightInCm) {
    const convertedValue = convertToFtInch(heightInCm);
    return `${convertedValue?.feet} ft ${convertedValue.inch} inches`;
  }
}
