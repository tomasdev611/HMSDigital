import {Component, OnInit} from '@angular/core';
import {PatientStatusTypeConstants} from '../../../enums/patient-status-types';
import {SieveOperators} from '../../../enums';
import {SieveRequest} from '../../../models';
import {finalize} from 'rxjs/operators';
import {ReportService} from '../../../services/report.service';
import {buildFilterString} from '../../../utils';

@Component({
  selector: 'app-client-services',
  templateUrl: './client-services.component.html',
  styleUrls: ['./client-services.component.scss'],
})
export class ClientServicesComponent implements OnInit {
  activePatientStatusId = PatientStatusTypeConstants.Active;
  loadingActivePatients = true;
  activePatientsValue = 0;
  activePatientsFilter = [
    {
      field: 'statusId',
      value: [this.activePatientStatusId],
      operator: SieveOperators.Equals,
    },
    {
      field: 'hospiceId',
      value: [97],
      operator: SieveOperators.Equals,
    },
  ];

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.getActivePatients();
  }

  getActivePatients() {
    this.loadingActivePatients = true;
    const request = new SieveRequest();
    request.page = 0;
    request.pageSize = 0;
    request.filters = buildFilterString(this.activePatientsFilter);
    this.reportService
      .getPatientsMetric({
        ...request,
      })
      .pipe(finalize(() => (this.loadingActivePatients = false)))
      .subscribe((response: any) => {
        this.activePatientsValue = response.value;
      });
  }
}
