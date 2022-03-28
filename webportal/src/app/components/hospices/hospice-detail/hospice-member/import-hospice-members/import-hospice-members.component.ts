import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {FormGroup, FormBuilder, FormControl} from '@angular/forms';
import {HospiceMemberService} from 'src/app/services/hospice-member.service';
import {HospiceService} from 'src/app/services/hospice.service';
import {ToastService} from 'src/app/services';
import {finalize} from 'rxjs/operators';
import {exportFile} from 'src/app/utils';
import {Location} from '@angular/common';

@Component({
  selector: 'app-import-hospice-members',
  templateUrl: './import-hospice-members.component.html',
  styleUrls: ['./import-hospice-members.component.scss'],
})
export class ImportHospiceMembersComponent implements OnInit {
  errors;
  tableHeaders = [];
  validMembers;
  invalidMembers;
  loading = false;
  outroMessage: string;
  importSuccess = false;
  hospiceId: number;

  constructor(
    private fb: FormBuilder,
    private hospiceMemberService: HospiceMemberService,
    private toasterService: ToastService,
    private hospiceService: HospiceService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
    });
    this.importSuccess = false;
    this.outroMessage = '';
    this.getHospiceMemberInputMappings();
  }

  getHospiceMemberInputMappings() {
    this.hospiceService
      .getHospiceMemberInputMapping(this.hospiceId, {mappedItemType: 'hospicemember'})
      .subscribe(({column_name_mapping}: any) => {
        if (!column_name_mapping) {
          this.toasterService.showError('No file mappings found, please contact support');
          return;
        }
        const headers = Object.keys(column_name_mapping).map(key => {
          return {label: column_name_mapping[key].name, field: key};
        });
        this.tableHeaders = headers;
      });
  }

  ValidateImportFile(file) {
    this.loading = true;
    this.validMembers = [];
    this.invalidMembers = [];
    this.errors = [];
    this.hospiceMemberService
      .createHospiceMembersFromCsv(this.hospiceId, file, false, true)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((response: any) => {
        this.validMembers = response.validItems.map(hm => hm.value);
        this.invalidMembers = response.invalidItems.map(hm => hm.value);
        this.errors = response.invalidItems.map(hm => {
          return hm.errors.reduce((errTooltip, err, index) => {
            return errTooltip + `${index + 1}. ${err}\n`;
          }, '');
        });
      });
  }

  createMembers(file) {
    this.loading = true;
    this.hospiceMemberService
      .createHospiceMembersFromCsv(this.hospiceId, file)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        (response: any) => {
          if (response && response.length) {
            this.outroMessage = `${response.length} records sucessfully imported`;
            this.importSuccess = true;
          }
        },
        (err: any) => {
          console.log(err, 'error');
          this.toasterService.showError(err.error);
          this.outroMessage = `There were Error while creating hospice Members`;
          this.importSuccess = false;
        }
      );
  }

  closeWizard() {
    this.location.back();
  }

  downloadCsvSampleFile() {
    this.hospiceService
      .getHospiceMemberInputMapping(this.hospiceId, {mappedItemType: 'hospicemember'})
      .subscribe(
        (response: any) => {
          if (!response.column_name_mapping) {
            this.toasterService.showInfo('It looks like upload mappings are not configured');
          } else {
            const fileData = Object.values(response.column_name_mapping).map((row: any) => {
              return row.name;
            });
            exportFile(fileData.join(','), 'mappings', 'csv');
          }
        },
        (err: any) => {
          this.toasterService.showError('There was error while fetching mappings.');
        }
      );
  }
}
