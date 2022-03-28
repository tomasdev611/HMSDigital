import {Location} from '@angular/common';
import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {HospiceFacilityService, ToastService} from 'src/app/services';
import {exportFile} from 'src/app/utils';

@Component({
  selector: 'app-import-hospice-facility',
  templateUrl: './import-hospice-facility.component.html',
  styleUrls: ['./import-hospice-facility.component.scss'],
})
export class ImportHospiceFacilityComponent implements OnInit {
  errors;
  tableHeaders = [];
  validFacilities;
  invalidFacilities;
  loading = false;
  outroMessage: string;
  importSuccess = false;
  hospiceId: number;

  constructor(
    private route: ActivatedRoute,
    private hospiceFacilityService: HospiceFacilityService,
    private toaster: ToastService,
    private location: Location
  ) {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
    });
  }

  ngOnInit(): void {
    this.importSuccess = false;
    this.outroMessage = '';
    this.getHospiceFacilityInputMappings();
  }

  getHospiceFacilityInputMappings() {
    this.loading = true;
    this.hospiceFacilityService
      .getFacilityInputMappings(this.hospiceId)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(({column_name_mapping}: any) => {
        const headers = Object.keys(column_name_mapping).map((key: any) => {
          return {label: column_name_mapping[key].name, field: key};
        });
        this.tableHeaders = headers;
      });
  }

  validateImportFile(file) {
    this.loading = true;
    this.validFacilities = [];
    this.invalidFacilities = [];
    this.errors = [];
    this.hospiceFacilityService
      .createHospiceFacilitesFromCsv(this.hospiceId, file, false, true)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res: any) => {
        this.validFacilities = res.validItems.map(hf => hf.value);
        this.invalidFacilities = res.invalidItems.map(hf => hf.value);
        this.errors = res.invalidItems.map(hf => {
          return hf.errors.reduce((errString, err, index) => {
            return errString + `${index + 1}. ${err}\n`;
          }, '');
        });
      });
  }

  createFacilites(file) {
    this.loading = true;
    this.hospiceFacilityService
      .createHospiceFacilitesFromCsv(this.hospiceId, file)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        (res: any) => {
          if (res && res.length) {
            this.outroMessage = `${res.length} records successfully imported`;
            this.importSuccess = true;
          }
        },
        (err: any) => {
          this.outroMessage = `There were errors while importing facilities`;
          throw err;
        }
      );
  }

  closeWizard() {
    this.location.back();
  }

  downloadCsvSampleFile() {
    this.hospiceFacilityService.getFacilityInputMappings(this.hospiceId).subscribe((res: any) => {
      if (!res.column_name_mapping) {
        this.toaster.showInfo(
          'It looks like upload mappings are not configured, please contact Support'
        );
        return;
      }
      const fileData = Object.values(res.column_name_mapping).map((row: any) => {
        return row.name;
      });
      exportFile(fileData.join(','), 'facilityMappings', 'csv');
    });
  }
}
