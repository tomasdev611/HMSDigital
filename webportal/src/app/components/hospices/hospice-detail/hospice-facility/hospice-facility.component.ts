import {Component, OnInit} from '@angular/core';
import {HospiceFacilityService, PatientService} from 'src/app/services';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {SieveOperators} from 'src/app/enums';
import {buildFilterString, IsPermissionAssigned} from 'src/app/utils';
import {PaginationResponse} from 'src/app/models';

@Component({
  selector: 'app-hospice-facility',
  templateUrl: './hospice-facility.component.html',
  styleUrls: ['./hospice-facility.component.scss'],
})
export class HospiceFacilityComponent implements OnInit {
  hospiceFacilityRequest: any;
  patientsLoading = false;
  patients = [];
  showPatients = false;
  facilitiesHeaders = [
    {label: 'Name', field: 'name', sortable: true},
    {label: 'Location', field: 'hospiceLocation.name', sortable: true},
  ];
  hospiceId: number;
  facilitiesLoading = false;
  facilityResponse: PaginationResponse;

  constructor(
    private hospiceFacilityService: HospiceFacilityService,
    private route: ActivatedRoute,
    private patientService: PatientService
  ) {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
      this.getHospiceFacilities();
    });
  }

  ngOnInit(): void {
    if (IsPermissionAssigned('Facility', 'Update')) {
      const facilityEditBtn = {
        label: '',
        field: '',
        class: 'sm',
        editBtn: 'Edit Hospice Facility',
        editBtnIcon: 'pi pi-pencil',
        editBtnLink: 'facilities/edit',
        linkParams: 'id',
        actionBtn: 'View Assigned Patients',
        actionBtnIcon: 'pi pi-eye',
        sortable: false,
      };
      this.facilitiesHeaders.push(facilityEditBtn);
    }
  }
  getHospiceFacilities() {
    this.facilitiesLoading = true;
    this.hospiceFacilityService
      .getAllHospiceFacilities(this.hospiceId)
      .pipe(
        finalize(() => {
          this.facilitiesLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.facilityResponse = response;
      });
  }
  fetchPatients(facility) {
    this.showPatients = true;
    this.patientsLoading = true;
    this.patients = [];
    this.hospiceFacilityService.getPatientsByFacilityId(facility.hospiceId, facility.id).subscribe(
      (patientHistoryResponse: any) => {
        if (patientHistoryResponse.length > 0) {
          const distinctPatients = [
            ...new Map(patientHistoryResponse.map(item => [item.patientUuid, item])).values(),
          ];
          const patientUuids = distinctPatients.map((r: any) => r.patientUuid);
          const filters = [
            {field: 'UniqueId', operator: SieveOperators.Equals, value: patientUuids},
          ];
          const patientRequest = {
            filters: buildFilterString(filters),
          };
          this.patientService.getPatients(patientRequest).subscribe(
            (patientInfoResponse: PaginationResponse) => {
              this.patients = patientInfoResponse.records.map(p => {
                const patient: any = distinctPatients.find((fp: any) => fp.patientId === p.id);
                if (patient) {
                  p.active = patient.active;
                }
                return p;
              });
              this.patientsLoading = false;
            },
            error => {
              this.patientsLoading = false;
              throw error;
            }
          );
        } else {
          this.patientsLoading = false;
        }
      },
      error => {
        this.patientsLoading = false;
        throw error;
      }
    );
  }
  hasPermission(entity, permission) {
    return IsPermissionAssigned(entity, permission);
  }
}
