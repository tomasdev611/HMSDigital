import {Component, Input, OnInit} from '@angular/core';
import {finalize} from 'rxjs/operators';
import {SieveOperators} from 'src/app/enums';
import {PaginationResponse, SieveRequest} from 'src/app/models';
import {InventoryService} from 'src/app/services';
import {buildFilterString, getIsInternalUser} from 'src/app/utils';

@Component({
  selector: 'app-patient-inventory',
  templateUrl: './patient-inventory.component.html',
  styleUrls: ['./patient-inventory.component.scss'],
})
export class PatientInventoryComponent implements OnInit {
  @Input() patientUuid: string;

  patientsLoading = false;
  patientInventoryRequest = new SieveRequest();
  patientInventoryResponse: PaginationResponse;
  isInternalUser = getIsInternalUser();

  inventoryHeaders = [
    {label: 'Item Name', field: 'itemName'},
    {label: 'Asset Tag', field: 'assetTagNumber'},
    {label: 'Serial Number', field: 'serialNumber'},
    {label: 'Count', field: 'quantity'},
  ];

  constructor(private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.getPatientInventory();
    if (this.isInternalUser) {
      const exceptionFulfillment = {
        label: 'Exception Fulfillment',
        field: 'isExceptionFulfillment',
        class: 'sm',
        fieldType: 'BoolToYesNo',
      };
      this.inventoryHeaders = [...this.inventoryHeaders, exceptionFulfillment];
    }
  }

  getPatientInventory() {
    this.patientsLoading = true;
    let filter = [
      {
        field: 'isConsumable',
        operator: SieveOperators.Equals,
        value: [false],
      },
    ];
    if (!this.isInternalUser) {
      const excludeExceptionFulfilled = {
        field: 'isExceptionFulfillment',
        operator: SieveOperators.NotEquals,
        value: [true],
      };
      filter = [...filter, excludeExceptionFulfilled];
    }
    this.patientInventoryRequest.filters = buildFilterString(filter);
    this.inventoryService
      .getPatientInventoryByUuid(this.patientUuid, this.patientInventoryRequest)
      .pipe(finalize(() => (this.patientsLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.patientInventoryResponse = res;
      });
  }

  getNextInventoryPage({pageNum}) {
    const {totalPageCount} = this.patientInventoryResponse;
    if (pageNum > totalPageCount) {
      return;
    }
    this.patientInventoryRequest.page = pageNum;
    this.getPatientInventory();
  }
}
