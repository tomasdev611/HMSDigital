import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {forkJoin} from 'rxjs';
import {finalize} from 'rxjs/operators';
import {SieveOperators} from 'src/app/enums';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {HospiceLocationService, HospiceService, PatientService} from 'src/app/services';
import {buildFilterString, getUniqArray} from 'src/app/utils';

@Component({
  selector: 'app-finance',
  templateUrl: './finance.component.html',
  styleUrls: ['./finance.component.scss'],
})
export class FinanceComponent implements OnInit {
  patients = [];
  backupPatients = [];
  flyoutOpen = false;
  patient: any;
  selectedPatient: any;
  searchKey = '';
  patientForm: FormGroup;
  showPatients = false;
  patientsLoading = false;
  hospiceLocations = [];

  constructor(
    private patientService: PatientService,
    private hospiceLocationService: HospiceLocationService,
    private hospiceService: HospiceService,
    private fb: FormBuilder
  ) {
    this.setPatientForm();
  }

  ngOnInit(): void {
    this.getHospiceLocations();
  }

  setPatientForm() {
    this.patientForm = this.fb.group({
      firstName: new FormControl(null),
      lastName: new FormControl(null),
      hospiceLocationId: new FormControl(null),
    });
  }

  getHospiceLocations() {
    const filterValues = [
      {
        field: 'name',
        operator: SieveOperators.Contains,
        value: [''],
      },
    ];
    const locationReq = {filters: buildFilterString(filterValues)};
    this.hospiceLocationService
      .getHospiceLocations(null, locationReq)
      .subscribe((res: PaginationResponse) => {
        this.hospiceLocations = res.records.map(r => ({
          value: r.id,
          label: r.name,
        }));
      });
  }

  getPatients() {
    const {firstName, lastName, hospiceLocationId} = this.patientForm.value;
    if (firstName || lastName || hospiceLocationId) {
      this.showPatients = true;
      this.patientsLoading = true;
      const req = new SieveRequest();
      req.pageSize = 100;
      const filterValues = [];
      if (this.patientForm.value.firstName) {
        filterValues.push({
          field: 'firstName',
          operator: SieveOperators.Contains,
          value: [this.patientForm.value.firstName],
        });
      }
      if (this.patientForm.value.lastName) {
        filterValues.push({
          field: 'lastName',
          operator: SieveOperators.Contains,
          value: [this.patientForm.value.lastName],
        });
      }
      if (this.patientForm.value.hospiceLocationId) {
        filterValues.push({
          field: 'hospiceLocationId',
          operator: SieveOperators.Equals,
          value: [this.patientForm.value.hospiceLocationId],
        });
      }
      req.filters = buildFilterString(filterValues);
      this.patientService
        .getPatients(req)
        .pipe(
          finalize(() => {
            this.patientsLoading = false;
          })
        )
        .subscribe((res: any) => {
          this.patients = res.records.map((p: any) => {
            return {
              name: `${p?.firstName} ${p?.lastName}`,
              patientUuid: p.uniqueId,
              ...p,
            };
          });
          if (res.records.length) {
            this.getAdditionals(res, 'patients');
          }
        });
    }
  }

  toggleFlyout(event) {
    this.flyoutOpen = event?.flyoutState || false;
  }

  getAdditionals(res, field) {
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
        this[field] = this[field].map(x => {
          const hospice = hospices?.records.find(h => h.id === x.hospiceId);
          if (hospice) {
            x.hospice = hospice;
          }
          const hospiceLocation = hospiceLocations?.records.find(h => h.id === x.hospiceLocationId);
          if (hospiceLocation) {
            x.hospiceLocation = hospiceLocation;
          }
          let address = x.patientAddress.length ? x.patientAddress[0].address : null;
          address = this.getAddressString(address);
          return {label: x.name, value: {...x, address}};
        });
      });
    }
  }

  getAddressString(address) {
    if (!address || (!address.addressLine1 && !address.state)) {
      return `Address not available`;
    }
    return `${address.addressLine1 ? address.addressLine1 + ',' : ','}
     ${address.addressLine2 ? address.addressLine2 + ',' : ''} ${
      address.city ? address.city + ',' : ''
    }
    ${address.state ? address.state : ''}`;
  }

  clearPatientSuggestions() {
    this.searchKey = '';
    this.patients = [];
  }
  patientSelected(event) {
    this.patient = event;
  }

  refreshPatient(event) {
    if (event?.update) {
      this.patient = {...this.patient, ...event?.update};
    }
  }

  closePatientsModal() {
    this.showPatients = false;
  }

  getSearchDisabled() {
    const {firstName, lastName, hospiceLocationId} = this.patientForm.value;
    return firstName || lastName || hospiceLocationId ? false : true;
  }

  selectPatient() {
    this.patient = this.selectedPatient;
    this.showPatients = false;
  }
}
