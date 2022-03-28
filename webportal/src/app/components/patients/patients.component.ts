import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {
  HospiceFacilityService,
  HospiceLocationService,
  PatientService,
  ToastService,
  UserService,
  FilterService,
} from 'src/app/services';
import {concatMap, finalize} from 'rxjs/operators';
import {SieveRequest, PaginationResponse, ConfirmDialog} from 'src/app/models';
import {
  buildFilterString,
  getFormattedPhoneNumber,
  getUniqArray,
  IsPermissionAssigned,
  groupByField,
  getObjectValueByKey,
  getObjectKeys,
  getIsInternalUser,
  formatDateToString,
  getUTCDate,
} from 'src/app/utils';
import {SieveOperators} from 'src/app/enums';
import {CreateOrderModalComponent, TableVirtualScrollComponent} from 'src/app/common';
import {InventoryService} from 'src/app/services';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {of, Subscription} from 'rxjs';

@Component({
  selector: 'app-patients',
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.scss'],
})
export class PatientsComponent implements OnInit, OnDestroy {
  @ViewChild('createordermodal') orderingModal: CreateOrderModalComponent;
  formatPhoneNumber = getFormattedPhoneNumber;
  loading = false;
  patientRequest = new SieveRequest();
  patientResponse: PaginationResponse;
  groupedInventories = [];
  headers = [
    {
      label: 'Status',
      field: 'displayStatus',
      bodyClassType: 'PatientStatus',
      class: 'md',
      sortable: true,
      sortField: 'statusRelevance',
      tooltipTextField: 'statusInfo',
    },
    {
      label: 'Name',
      field: 'name',
      sortable: true,
      sortField: 'FirstName',
    },
    {
      label: 'DOB',
      field: 'dateOfBirth',
      fieldType: 'shortDate',
    },
    {
      label: 'Created on',
      field: 'createdDateTime',
      sortable: true,
      sortField: 'createdDateTime',
      fieldType: 'Date',
    },
    {
      label: 'Last Order',
      field: 'lastOrderDateTime',
      sortable: true,
      sortField: 'lastOrderDateTime',
      fieldType: 'Date',
    },
    {
      label: 'Hospice Location',
      field: 'hospice',
    },
  ];
  detailsViewOpen = false;
  currentPatientDetails;
  inventoryLoading = true;
  patientInventoryRequest = new SieveRequest();
  patientInventoryResponse: PaginationResponse;
  subscriptions: Subscription[] = [];
  isInternalUser = getIsInternalUser();
  activeFilterString = '';
  searchFilterString = '';
  searchQuery = '';

  orderingPatient: any;
  filterPatientStatus = false;

  @ViewChild('patientsTable ', {static: false})
  patientsTable: TableVirtualScrollComponent;
  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  confirmData = new ConfirmDialog();

  constructor(
    private patientService: PatientService,
    private inventoryService: InventoryService,
    private navbarSearchService: NavbarSearchService,
    private hospiceFacilityService: HospiceFacilityService,
    private hospiceLocationService: HospiceLocationService,
    private toastService: ToastService,
    private userService: UserService,
    private filterService: FilterService
  ) {}

  ngOnInit(): void {
    this.addEditPatientButton();
    const navSearch = sessionStorage.getItem('navSearch');
    this.patientRequest.sorts = 'statusRelevance,firstName';
    if (navSearch) {
      this.searchPatient(navSearch);
    } else {
      this.getPatients();
    }
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(text => this.searchPatient(text))
    );
    this.filterService.filterValue.subscribe((filterObject: any) => {
      if (!filterObject) {
        this.filterPatientStatus = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  addEditPatientButton() {
    if (IsPermissionAssigned('Orders', 'Create')) {
      const orderBtnHeader: any = {
        label: '',
        field: '',
        class: 'md',
        bodyClassType: '',
        actionBtn: 'Create Order',
        actionBtnLabel: 'Create Order',
        actionBtnIcon: '',
      };
      this.headers.push(orderBtnHeader);
    }

    if (IsPermissionAssigned('Patient', 'Update')) {
      const editBtnHeader: any = {
        label: '',
        field: '',
        class: 'xs',
        bodyClassType: '',
        editBtn: 'Edit Patient',
        editBtnIcon: 'pi pi-pencil',
        editBtnLink: '/patients/edit',
        linkParams: 'id',
      };
      this.headers.push(editBtnHeader);
    }
  }

  getPatients() {
    this.loading = true;
    this.patientService
      .getPatients(this.patientRequest)
      .pipe(
        concatMap((patientRes: any) => {
          patientRes.records = patientRes.records.map((response: any) => {
            response.dateOfBirth = getUTCDate(response.dateOfBirth, 'MM/DD/YYYY');
            return response;
          });
          this.patientResponse = patientRes;

          return this.getHospiceRequest();
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((hospices: PaginationResponse) => {
        this.formatPatientResponse(hospices);
      });
  }

  getHospiceRequest() {
    const hospiceIds = getUniqArray(
      this.patientResponse.records.map(({hospiceId}: any) => {
        return hospiceId;
      })
    );
    const hospiceLocationIds = getUniqArray(
      this.patientResponse.records.map(({hospiceLocationId}: any) => {
        return hospiceLocationId;
      })
    );
    if (hospiceIds.length > 0) {
      const filterValues = [
        {
          field: 'id',
          operator: SieveOperators.Equals,
          value: [...hospiceLocationIds],
        },
      ];
      const request = {filters: buildFilterString(filterValues)};
      return this.hospiceLocationService.getHospiceLocations(request);
    } else {
      this.headers = this.headers.filter(x => x.label !== 'Hospice Location');
      return of([]);
    }
  }

  formatPatientResponse(hospices) {
    this.patientResponse.records = this.patientResponse.records.map(patient => {
      const hospice = hospices?.records?.find((h: any) => h.id === patient.hospiceLocationId);
      if (hospice) {
        patient.hospice = hospice.name;
      }
      patient.name = `${patient.firstName} ${patient.lastName || ''}`;

      switch (patient.status) {
        case 'Active':
          patient.displayStatus = 'A';
          break;
        case 'Inactive':
        case 'Blank':
          patient.displayStatus = 'I';
          break;
        case 'Pending':
          patient.displayStatus = 'P';
          break;
        case 'PendingActive':
          patient.displayStatus = 'A';
          break;
        default:
          patient.displayStatus = '';
          break;
      }
      return patient;
    });
  }

  nextPatients({pageNum}) {
    if (!this.patientResponse || pageNum > this.patientResponse?.totalPageCount) {
      return;
    }
    this.patientRequest.page = pageNum;
    if (!!this.searchQuery) {
      this.searchPatient(this.searchQuery);
    } else {
      this.getPatients();
    }
  }

  searchPatient(searchQuery) {
    if (!searchQuery) {
      this.dataTablesReset();
      this.getPatients();
      return;
    }
    if (!this.searchQuery || this.searchQuery !== searchQuery) {
      this.dataTablesReset();
    }
    this.searchQuery = searchQuery;
    this.loading = true;
    this.patientService
      .searchPatientsBySearchQuery({...this.patientRequest, searchQuery})
      .pipe(
        concatMap((patientRes: any) => {
          this.patientResponse = patientRes;
          return this.getHospiceRequest();
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((hospices: PaginationResponse) => {
        this.formatPatientResponse(hospices);
      });
  }

  filterPatients() {
    this.patientRequest.filters = `${this.searchFilterString},${this.activeFilterString}`;
    this.patientRequest.page = 1;
    this.searchPatient(this.searchQuery);
  }

  filterPatientsBySearch(filterString) {
    this.activeFilterString = '';
    this.filterPatientStatus = false;
    this.searchFilterString = filterString;
    this.filterPatients();
  }

  showOrdering(patient) {
    this.orderingPatient = patient;
    if (patient?.status === 'Inactive') {
      this.confirmData.header = 'Inactive Patient';
      const patientReason = patient.statusReason
        ? `as patient was identified as ${patient.statusReason} on
        ${formatDateToString(patient.statusChangedDate, 'MM/DD/YYYY')}`
        : '';
      this.confirmData.message = `Please confirm that you want to create a new order for <strong>${
        patient.firstName || ''
      }
        ${patient.lastName || ''}</strong> ${patientReason}. <div>Do you want to continue?</div>`;
      this.confirmData.data = {
        patient,
      };
      this.confirmDialog.showDialog(this.confirmData);
    } else {
      setTimeout(() => {
        this.orderingModal.showOrdering();
      });
    }
  }

  confirmAccepted() {
    this.confirmDialog.close();
    setTimeout(() => {
      this.orderingModal.showOrdering();
    });
  }

  confirmRejected() {
    this.confirmDialog.close();
  }

  sortPatients(event) {
    switch (event.order) {
      case 0:
        this.patientRequest.sorts = '';
        break;
      case 1:
        this.patientRequest.sorts = event.field;
        break;
      case -1:
        this.patientRequest.sorts = '-' + event.field;
        break;
    }
    this.dataTablesReset();
    this.searchPatient(this.searchQuery);
  }

  patientSelected(event) {
    this.currentPatientDetails = event.currentRow;
    this.patientInventoryRequest.page = 1;
    this.detailsViewOpen = true;
    this.getPatientInventory();
    if (this.currentPatientDetails.patientAddress.length === 0) {
      this.getPatientFacilities();
    }
    this.getNoteCreaterName();
    if (
      this.isInternalUser &&
      this.currentPatientDetails.createdByUserId &&
      IsPermissionAssigned('User', 'Read')
    ) {
      this.getMyCreatorName();
    }
  }

  getMyCreatorName() {
    this.userService
      .getUserById(this.currentPatientDetails.createdByUserId)
      .subscribe((user: any) => {
        this.currentPatientDetails.creatorName = `${user.firstName} ${user.lastName}`;
      });
  }

  getNoteCreaterName() {
    this.patientService
      .getPatientNotes(this.currentPatientDetails.id)
      .subscribe((response: any) => {
        this.currentPatientDetails.patientNotes = response;
      });
  }

  closeOrderDetails() {
    this.detailsViewOpen = false;
  }

  getPatientInventory() {
    this.inventoryLoading = true;
    this.groupedInventories = [];
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
      .getPatientInventoryByUuid(this.currentPatientDetails.uniqueId, this.patientInventoryRequest)
      .pipe(finalize(() => (this.inventoryLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.patientInventoryResponse = res;
        const groupedRecords = groupByField(this.patientInventoryResponse.records, 'itemNumber');
        const keys = getObjectKeys(groupedRecords);
        keys.forEach(key => {
          const item = getObjectValueByKey(groupedRecords, key);
          item.itemName = getObjectValueByKey(item[0], 'itemName');
          item.totalQuantity = item.reduce((a, b) => {
            return a + b.quantity;
          }, 0);
          this.groupedInventories.push(item);
        });
      });
  }

  inventoryPagePrevious() {
    if (this.patientInventoryResponse.pageNumber > 1) {
      this.patientInventoryRequest.page -= 1;
      this.getPatientInventory();
    }
  }

  inventoryPageNext() {
    if (this.patientInventoryResponse.pageNumber < this.patientInventoryResponse.totalPageCount) {
      this.patientInventoryRequest.page += 1;
      this.getPatientInventory();
    }
  }

  getPatientFacilities() {
    const filterValues = [
      {
        field: 'patientUuid',
        operator: SieveOperators.Equals,
        value: [this.currentPatientDetails.uniqueId],
      },
    ];
    const facilityReq = {filters: buildFilterString(filterValues)};
    this.hospiceFacilityService
      .getHospicePatientsFacilities(this.currentPatientDetails.hospiceId, facilityReq)
      .pipe(finalize(() => {}))
      .subscribe((response: any) => {
        this.currentPatientDetails.patientFacilityAddress = [];
        response.records.map(hf => {
          this.currentPatientDetails.patientFacilityAddress = [
            ...this.currentPatientDetails.patientFacilityAddress,
            hf.facility.address,
          ];
        });
      });
  }

  filterPatientByStatus(event?) {
    this.dataTablesReset(false);
    if (this.filterPatientStatus) {
      const patientFilters = [
        {
          field: 'IsPending',
          operator: SieveOperators.Equals,
          value: [this.filterPatientStatus],
        },
      ];
      const statusFilter = buildFilterString(patientFilters);
      this.activeFilterString = statusFilter;
      this.filterPatients();
      const filterObject = {
        field: 'status',
        label: 'Status',
        operator: '==',
        value: [
          {
            status: 'active',
            name: 'Active',
          },
        ],
      };
      this.filterService.forceFilterValueUpdate(filterObject);
    } else {
      this.searchFilterString = '';
      this.activeFilterString = '';
      this.filterPatients();
      this.filterService.forceFilterValueUpdate(null);
    }
  }

  dataTablesReset(clearSearchQuery = true) {
    if (this.patientsTable) {
      this.patientsTable.reset();
    }
    this.patientRequest.page = 1;
    if (clearSearchQuery) {
      this.searchQuery = '';
    }
  }

  copyToClipBoard(value) {
    navigator.clipboard
      .writeText(value)
      .then(() => {
        this.toastService.showSuccess(`Patient ID copied to the clipboard`, 1000);
      })
      .catch(e => console.error(e));
  }
}
