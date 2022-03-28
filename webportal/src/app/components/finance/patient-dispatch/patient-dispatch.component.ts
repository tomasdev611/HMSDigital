import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Table, TableHeaderCheckbox} from 'primeng-lts/table';
import {finalize} from 'rxjs/operators';
import {NetsuiteDispatchOrderRequest, PaginationResponse, SieveRequest} from 'src/app/models';
import {AuditService, DispatchService, SystemService, ToastService} from 'src/app/services';
import {getDifference, IsPermissionAssigned, setDateFromInlineInput} from 'src/app/utils';
import {PatientDispatchUpdateComponent} from './patient-dispatch-update/patient-dispatch-update.component';

@Component({
  selector: 'app-patient-dispatch',
  templateUrl: './patient-dispatch.component.html',
  styleUrls: ['./patient-dispatch.component.scss'],
})
export class PatientDispatchComponent implements OnInit {
  patients = [];
  patient: any;
  @Input() set selectedPatient(patient: any) {
    if (patient?.uniqueId) {
      this.patientSelected(patient);
    }
  }
  patientDispatchRequest = new NetsuiteDispatchOrderRequest();
  patientDispatchResponse: any;
  dispatchLoading = false;
  selectedItems = [];
  selectedDispatchRecordIds = [];
  activeView = 'records';
  auditLogResponse: PaginationResponse;
  auditRequest: any = this.getDefaultAuditRequest();
  logsLoading = false;
  selectedAuditLog: any;
  auditDetailsViewOpen = false;
  @ViewChild('dispatchTable')
  private dispatchTable: Table;

  @ViewChild('headerCheckBox')
  private headerCheckBox: TableHeaderCheckbox;
  currentDate = new Date();
  auditLogHeaders = [
    {label: 'Action Type', field: 'auditAction', sortable: true},
    {
      label: 'Total Modified Fields',
      field: 'auditData.length',
      sortable: true,
    },
    {
      label: 'Modified by',
      field: 'user.name',
      sortable: true,
    },
    {
      label: 'Modified On',
      field: 'auditDate',
      sortable: true,
      fieldType: 'Date',
    },
  ];

  @Output() toggleFlyout = new EventEmitter<any>();
  @ViewChild('fixmodal') fixModal: PatientDispatchUpdateComponent;
  clonedRecord: {[s: string]: any} = {};
  constructor(
    private systemService: SystemService,
    private auditService: AuditService,
    private dispatchService: DispatchService,
    private toastService: ToastService
  ) {}

  getDefaultAuditRequest() {
    const request = new SieveRequest();
    request.sorts = '-auditDate';
    return request;
  }

  ngOnInit(): void {}

  patientSelected(event) {
    this.activeView = '';
    this.patient = event;
    this.patientDispatchRequest.patientUuid = this.patient.uniqueId;
    setTimeout(() => {
      this.getDispatchOrders();
    });
  }

  getDispatchOrders() {
    this.selectedItems = [];
    this.dispatchLoading = true;
    this.activeView = 'records';
    this.systemService
      .getDispatchOrders(this.patientDispatchRequest)
      .pipe(
        finalize(() => {
          this.dispatchLoading = false;
        })
      )
      .subscribe((response: any) => {
        response.records = response.records.map(x => {
          x.editEnabled = false;
          x.hmsDeliveryDate = x.hmsDeliveryDate ? new Date(x.hmsDeliveryDate) : null;
          x.hmsPickupRequestDate = x.hmsPickupRequestDate ? new Date(x.hmsPickupRequestDate) : null;
          x.pickupDate = x.pickupDate ? new Date(x.pickupDate) : null;
          return x;
        });
        this.patientDispatchResponse = response;
        this.patientDispatchResponse.records = [...this.patientDispatchResponse.records];
        this.checkHeadercheckboxState();
      });
  }

  editDispatch(dispatch, index) {
    this.selectedDispatchRecordIds = [dispatch.nsDispatchId];
    this.fixModal.showFixModal();
  }

  refreshRecords(event) {
    this.getDispatchOrders();
  }

  fixSelectedDates() {
    this.selectedDispatchRecordIds = this.selectedItems.map(x => x.nsDispatchId);
    this.fixModal.showFixModal();
  }

  viewLogs() {
    this.activeView = 'logs';
    this.auditRequest = this.getDefaultAuditRequest();
    this.getAuditLogs();
  }

  viewRecords() {
    this.activeView = 'records';
    this.getDispatchOrders();
  }

  getAuditLogs() {
    this.logsLoading = true;
    this.auditRequest.filters = `patientUuid==${this.patient.uniqueId}`;
    this.auditService
      .getDispatchAuditLogs(this.auditRequest)
      .pipe(
        finalize(() => {
          this.logsLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.auditLogResponse = response;
      });
  }

  fetchNextAuditLogPage({pageNum}) {
    if (!this.auditLogResponse) {
      return;
    }
    this.auditRequest.page = pageNum;
    this.getAuditLogs();
  }

  showAuditLogDetails({currentRow}) {
    this.auditDetailsViewOpen = true;
    this.selectedAuditLog = currentRow;
    this.toggleFlyout.emit({flyoutState: this.auditDetailsViewOpen});
  }

  closeAuditLogDeatils() {
    this.auditDetailsViewOpen = false;
    this.selectedAuditLog = null;
    this.toggleFlyout.emit({flyoutState: this.auditDetailsViewOpen});
  }
  onSelectionChange(selection: any[]) {
    for (let i = selection.length - 1; i >= 0; i--) {
      const data = selection[i];
      if (this.isRowDisabled(data)) {
        selection.splice(i, 1);
      }
    }
    this.selectedItems = selection;
  }

  isRowDisabled(data: any): boolean {
    const monthDifference = getDifference(
      this.currentDate,
      new Date(data.createdDate),
      'months',
      true
    );
    const result = monthDifference > 3;
    return result;
  }

  checkHeadercheckboxState(): void {
    if (this.headerCheckBox) {
      this.headerCheckBox.updateCheckedState = () => {
        const records: any[] = this.dispatchTable.filteredValue || this.dispatchTable.value;
        const selection: any[] = this.dispatchTable.selection;
        for (const record of records) {
          if (!this.isRowDisabled(record)) {
            const selected = selection && selection.indexOf(record) >= 0;
            if (!selected) {
              this.updateHeaderCheckBoxState(false);
              return false;
            }
          }
        }
        this.updateHeaderCheckBoxState(true);
        return true;
      };
    }
  }

  updateHeaderCheckBoxState(checked) {
    if (this.headerCheckBox.boxViewChild?.nativeElement?.className) {
      this.headerCheckBox.boxViewChild.nativeElement.className = checked
        ? 'p-checkbox-box p-highlight'
        : 'p-checkbox-box';
    }
    if (this.headerCheckBox.boxViewChild?.nativeElement?.children[0]?.className) {
      this.headerCheckBox.boxViewChild.nativeElement.children[0].className = checked
        ? 'p-checkbox-icon pi pi-check'
        : 'p-checkbox-icon';
    }
  }
  hasPermission(entity, permission = 'Update') {
    return IsPermissionAssigned(entity, permission);
  }

  onRowEditInit(value) {
    value.editEnabled = true;
    this.clonedRecord[value.nsDispatchId] = {...value};
  }

  onRowEditSave(value: any) {
    value.editEnabled = false;
    const obj = {
      dispatchRecordId: value?.nsDispatchId,
      hmsDeliveryDate: value?.hmsDeliveryDate,
      hmsPickupRequestDate: value?.hmsPickupRequestDate,
      pickupDate: value?.pickupDate,
    };
    this.dispatchService
      .updateDispatch([obj])
      .pipe(
        finalize(() => {
          value.editEnabled = false;
          delete this.clonedRecord[value.nsDispatchId];
        })
      )
      .subscribe((response: any) => {
        this.toastService.showSuccess(`Fulfillment Date fixed successfully`);
      });
  }

  onRowEditCancel(value: any, index: number) {
    this.patientDispatchResponse.records[index] = this.clonedRecord[value.nsDispatchId];
    delete this.clonedRecord[value.nsDispatchId];
    this.patientDispatchResponse.records[index].editEnabled = false;
  }

  updateDate(event, value, field) {
    const dateToAppend = setDateFromInlineInput(event.target.value);
    if (dateToAppend) {
      value[field] = dateToAppend;
    }
  }
}
