import {Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-import-wizard-validator',
  templateUrl: './import-wizard-validator.component.html',
  styleUrls: ['./import-wizard-validator.component.scss'],
})
export class ImportWizardValidatorComponent implements OnInit {
  @Input() tableHeaders: [];
  @Input() validRecords: [];
  @Input() inValidRecords: [];
  @Input() isValid;
  @Input() errors: [];

  invalidRecordsReport: string;
  validRecordsReport: string;
  constructor() {}

  ngOnInit(): void {
    const totalRecordsCount = this.inValidRecords?.length + this.validRecords?.length;
    this.invalidRecordsReport = `Invalid Records (${this.inValidRecords?.length || 0})`;
    this.validRecordsReport = `Valid Records (${this.validRecords?.length || 0})`;
  }

  shouldShowValidRecord() {
    if (this.validRecords?.length || this.inValidRecords?.length) {
      return !!this.validRecords?.length;
    }
    return true;
  }

  shouldShowInValidRecord() {
    if (this.validRecords?.length || this.inValidRecords?.length) {
      return !!this.inValidRecords?.length;
    }
    return true;
  }
}
