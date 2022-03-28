import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {finalize} from 'rxjs/operators';
import {SieveRequest} from 'src/app/models';
import {HospiceFacilityService, HospiceService, ToastService} from 'src/app/services';
import {exportFile, IsPermissionAssigned} from 'src/app/utils';

@Component({
  selector: 'app-hospice-settings',
  templateUrl: './hospice-settings.component.html',
  styleUrls: ['./hospice-settings.component.scss'],
})
export class HospiceSettingsComponent implements OnInit {
  selectedHospice: any;
  @Input() set hospice(hospice) {
    if (hospice) {
      this.selectedHospice = hospice;
      this.setcreditHoldForm();
    }
  }
  mappings = [];
  hospiceId: number;
  creditHoldForm: FormGroup;
  formSubmit = false;
  creditHoldHistoryLoading = false;
  creditHoldHistoryOpen = false;
  creditHoldHistory = [];

  @Output() updateHospice = new EventEmitter<any>();

  constructor(
    private route: ActivatedRoute,
    private hospiceService: HospiceService,
    private toaster: ToastService,
    private hospiceFacilityService: HospiceFacilityService,
    private fb: FormBuilder
  ) {
    this.route.params.subscribe((params: any) => {
      this.hospiceId = params.hospiceId;
    });
  }

  setcreditHoldForm() {
    this.creditHoldForm = this.fb.group({
      isCreditOnHold: new FormControl(this.selectedHospice?.isCreditOnHold),
      creditHoldNote: new FormControl(null, Validators.required),
    });
    this.creditHoldValidators();
  }

  ngOnInit(): void {
    this.setcreditHoldForm();
  }

  getHospiceMemberInputMappings() {
    this.hospiceService
      .getHospiceMemberInputMapping(this.hospiceId, {
        mappedItemType: 'hospicemember',
      })
      .subscribe(
        (response: any) => {
          if (!response.column_name_mapping) {
            this.toaster.showInfo('It looks like upload mappings are not configured');
          } else {
            const fileData = Object.values(response.column_name_mapping).map((row: any) => {
              return row.name;
            });
            exportFile(fileData.join(','), 'membersBulkUploadFormat', 'csv');
          }
        },
        (err: any) => {
          this.toaster.showError('There was error while fetching mappings.');
        }
      );
  }

  getHospiceFacilityInputMappings() {
    this.hospiceFacilityService
      .getFacilityInputMappings(this.hospiceId)
      .subscribe(({column_name_mapping}: any) => {
        const headers = Object.values(column_name_mapping).map((row: any) => {
          return row.name;
        });
        exportFile(headers.join(','), 'facilityBulkUploadFormat', 'csv');
      });
  }
  onSubmitCreditHold(credithold) {
    this.formSubmit = true;
    this.hospiceService
      .updateCreditHold(this.hospiceId, credithold)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.updateHospice.emit({...this.selectedHospice, ...response});
        this.toaster.showSuccess(`Credit hold submitted successfully`);
      });
  }

  hasPermission(entity, permission = 'Read') {
    return IsPermissionAssigned(entity, permission);
  }
  creditHoldValidators() {
    if (this.creditHoldForm.value.isCreditOnHold) {
      this.creditHoldForm.controls.creditHoldNote.setValidators([Validators.required]);
    } else {
      this.creditHoldForm.controls.creditHoldNote.clearValidators();
    }
    this.creditHoldForm.controls.creditHoldNote.updateValueAndValidity();
  }

  isRequiredFileds(field) {
    if (this.creditHoldForm?.controls[field].validator !== null) {
      return true;
    }
    return false;
  }

  toggleCreditHoldHistory() {
    this.creditHoldHistoryOpen = !this.creditHoldHistoryOpen;
    if (this.creditHoldHistoryOpen) {
      this.getCreditHoldHistory();
    }
  }

  getCreditHoldHistory() {
    this.creditHoldHistoryLoading = true;
    const req = new SieveRequest();
    req.sorts = '-creditHoldDateTime';
    this.hospiceService
      .getCreditHoldHistory(this.hospiceId, req)
      .pipe(
        finalize(() => {
          this.creditHoldHistoryLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.creditHoldHistory = response;
      });
  }
}
