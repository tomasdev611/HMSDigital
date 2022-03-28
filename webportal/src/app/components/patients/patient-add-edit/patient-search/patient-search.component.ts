import {Component, OnInit, Input, Output, EventEmitter, OnChanges} from '@angular/core';
import {FormGroup, FormBuilder, FormControl} from '@angular/forms';
import {PatientService, ToastService} from 'src/app/services';
import {finalize} from 'rxjs/operators';

@Component({
  selector: 'app-patient-search',
  templateUrl: './patient-search.component.html',
  styleUrls: ['./patient-search.component.scss'],
})
export class PatientSearchComponent implements OnInit, OnChanges {
  @Input() searchCriteria: any;
  @Input() searchEnabled = false;
  @Input() hospices: any[];
  @Input() findDuplicates = false;
  @Output() showSearchBox = new EventEmitter<any>();
  @Output() closeSearchBox = new EventEmitter<any>();
  @Output() ignoreDuplicates = new EventEmitter<any>();

  searchLoading = false;
  maxDate = new Date();
  patientSearchForm: FormGroup;
  searchedPatients = [];
  headers = [
    {
      label: 'Name',
      field: 'name',
      sortable: true,
      bodyRoute: '/users/edit',
      bodyRouteParams: 'patientId',
    },
    {label: 'Phone', field: 'phoneNumber', fieldType: 'Phone'},
    {
      label: 'Emergency Contact Name',
      field: 'emergencyContactName',
      sortable: true,
    },
    {
      label: 'Emergency Contact Phone',
      field: 'emergencyContactNumber',
      sortable: true,
      fieldType: 'Phone',
    },
    {
      label: 'Date Of Birth',
      field: 'dateOfBirth',
      sortable: true,
      fieldType: 'Date',
    },
    {
      label: '',
      field: '',
      class: 'xs',
      editBtn: 'Edit Patient',
      editBtnIcon: 'pi pi-user-edit',
      editBtnLink: '/patients/edit',
      linkParams: 'id',
    },
  ];
  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private toastService: ToastService
  ) {}

  ngOnChanges(changes) {
    if (changes?.searchCriteria?.currentValue && this.findDuplicates) {
      this.patientSearchForm.patchValue(changes.searchCriteria.currentValue);
      this.patientSearch(this.patientSearchForm.value);
    }
  }

  ngOnInit(): void {
    this.setPatientSearchForm();
  }

  setPatientSearchForm() {
    this.patientSearchForm = this.fb.group({
      firstName: new FormControl(null),
      lastName: new FormControl(null),
      hospiceId: new FormControl(null),
      dateOfBirth: new FormControl(null),
      addressType: new FormControl('Home'),
      address: this.fb.group({
        addressLine1: new FormControl(null),
        addressLine2: new FormControl(null),
        state: new FormControl(null),
        city: new FormControl(null),
        zipCode: new FormControl(0),
      }),
    });
  }

  continueWithDuplicate() {
    this.searchedPatients = [];
    this.searchEnabled = false;
    this.ignoreDuplicates.emit();
  }

  patientSearch(patient) {
    patient = {...patient};
    patient.dateOfBirth = patient.dateOfBirth || null;
    patient.hospiceId =
      patient.hospiceId && patient.hospiceId.id ? patient.hospiceId.id : patient.hospiceId;
    patient.address.zipCode = patient.address.zipCode || 0;

    this.searchLoading = true;
    this.patientService
      .searchPatient(patient)
      .pipe(
        finalize(() => {
          this.searchLoading = false;
        })
      )
      .subscribe(
        (response: any) => {
          if (response.length === 0) {
            if (this.findDuplicates) {
              this.ignoreDuplicates.emit();
            } else {
              this.toastService.showInfo('No Records found for your search');
            }
          } else if (this.findDuplicates) {
            this.showSearchBox.emit();
          }
          this.searchedPatients = response.map(x => {
            x.name = `${x.firstName}${x.lastName ? ' ' + x.lastName : ''}`;
            x.phoneNumber = x.phoneNumbers.find(pn => pn.isPrimary)?.number;
            x.emergencyContactNumber = x.phoneNumbers.find(
              pn => pn.numberType === 'Emergency'
            )?.number;
            return x;
          });
        },
        error => {
          console.log('error', error);
          throw error;
        }
      );
  }

  closeSearch(event) {
    this.patientSearchForm.reset();
    this.searchedPatients = [];
    this.closeSearchBox.emit();
  }
}
