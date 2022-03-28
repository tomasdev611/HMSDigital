import {Component, OnInit, ViewChild} from '@angular/core';
import {forkJoin, of} from 'rxjs';
import {finalize, mergeMap} from 'rxjs/operators';
import {UserAuditActionTypes} from 'src/app/constants';
import {FilterTypes, SieveOperators} from 'src/app/enums';
import {ActivatedRoute} from '@angular/router';
import {
  AuditFilter,
  ConfirmDialog,
  NetsuiteDispatchOrderRequest,
  PaginationResponse,
  BasePaginationReponse,
  SieveRequest,
  SystemTools,
  FilterConfiguration,
  UnconfirmedInNetsuiteFixConfig,
  BaseContinuationTokenResponse,
} from 'src/app/models';
import {
  SystemService,
  ToastService,
  AuditService,
  PatientService,
  HospiceMemberService,
  SitesService,
  OrderHeadersService,
  UserService,
} from 'src/app/services';
import {
  buildFilterString,
  deepCloneObject,
  getObjectFromFilterString,
  getUniqArray,
  IsPermissionAssigned,
} from 'src/app/utils';
import {TableVirtualScrollComponent, ConfirmDialogComponent} from 'src/app/common';
import {Location} from '@angular/common';

@Component({
  selector: 'app-system',
  templateUrl: './system.component.html',
  styleUrls: ['./system.component.scss'],
})
export class SystemComponent implements OnInit {
  // loading flags
  @ViewChild('filter', {static: false}) appFilter;
  @ViewChild('orderSelected', {static: false}) orderDetailFlyout;
  OrdersCardLoading = false;
  patientInventoryLoading = false;
  actionTypes = deepCloneObject(UserAuditActionTypes).filter((x: any) => {
    x.auditAction = x.name || '*';
    x.name = x.label;
    return x;
  });
  AudiTypeUrls: any = {
    Hospice: '/hospice/',
    User: '/users/edit/',
    Inventory: '/inventory/edit/',
    Site: '/sites/info/',
  };
  userAuditLogsExtraFilterConf: FilterConfiguration[] = [
    {
      label: 'Target User',
      field: 'targetUser',
      fields: ['targetUserFirstName', 'targetUserLastName'],
      type: FilterTypes.Autocomplete,
      value: [],
      operator: SieveOperators.CI_Contains,
    },
  ];
  orderheaderAuditLogsExtraFilterConf: FilterConfiguration[] = [
    {
      label: 'Order #',
      field: 'entityId',
      type: FilterTypes.Autocomplete,
      value: '',
      operator: SieveOperators.Equals,
    },
  ];
  baseAuditLogsFilterConfiguration: FilterConfiguration[] = [
    {
      label: 'Modified On(range)',
      field: 'auditDate',
      operator: SieveOperators.Equals,
      type: FilterTypes.DateRangePicker,
    },
    {
      label: 'Modified By(Name)',
      field: 'username',
      fields: ['userFirstName', 'userLastName'],
      type: FilterTypes.Autocomplete,
      value: [],
      operator: SieveOperators.CI_Contains,
    },
    {
      label: 'Action Type',
      field: 'auditAction',
      operator: SieveOperators.Equals,
      type: FilterTypes.Autocomplete,
      value: this.actionTypes,
    },
  ];

  auditLogsFilterConfiguration: FilterConfiguration[];

  // user
  identityMissingUsersCount: number;

  auditRequest: any = this.getDefaultAuditRequest();
  apiLogFromDate: Date;
  apiLogToDate: Date;
  selectedAuditType: any = 'Core';
  auditTypes = [
    {name: 'Users'},
    {name: 'Hospices'},
    {name: 'HospiceLocations'},
    {name: 'OrderHeaders'},
    {name: 'OrderLineItems'},
    {name: 'Inventory'},
    {name: 'Items'},
    {name: 'Sites'},
  ];
  maxDate: Date = new Date();
  patientInventoryResponse: any;
  selectedPatient: any;
  selectedPatientInventory: any;
  auditLogResponse: any;
  coreApiLogsResponse: any = new BaseContinuationTokenResponse();
  continuationToken = null;
  countLogsInPage = 50;
  defaultTimeDifference = 3;
  logsLoading = false;
  auditDetailsViewOpen = false;
  orderDetailViewOpen = false;
  netsuiteDetailsViewOpen = false;
  selectedAuditLog: any;
  selectedNetsuiteLog: any;
  baseUserHeader = [
    {label: 'Name', field: 'name', sortable: true},
    {label: 'Email Address', field: 'email', sortable: true},
    {label: 'Phone', field: 'phoneNumber', fieldType: 'Phone'},
  ];
  userHeaders = [
    ...this.baseUserHeader,
    {
      label: '',
      field: '',
      class: 'xs',
      editBtn: 'Edit User',
      editBtnIcon: 'pi pi-user-edit',
      editBtnLink: '/users/edit',
      linkParams: 'id',
    },
  ];
  memberHeaders = [
    ...this.baseUserHeader,
    {
      label: '',
      field: '',
      class: 'xs',
      editBtn: 'Edit User',
      editBtnIcon: 'pi pi-user-edit',
      editBtnLink: '/users/edit',
      linkParams: 'userId',
    },
  ];
  deletedPatientInventoryHeaders = [
    {label: 'Patient Name', field: 'patientName', sortable: true},
    {label: 'Patient  UUID', field: 'patientUuid', sortable: true},
    {label: 'Item Name', field: 'itemName', sortable: true},
    {label: 'Asset Tag', field: 'assetTagNumber', sortable: true},
    {label: 'Serial Number', field: 'serialNumber', sortable: true},
    {
      label: 'Deleted Date',
      field: 'deletedDateTime',
      sortable: true,
      fieldType: 'Date',
    },
    {label: 'Deleted By User', field: 'deletedByUserName', sortable: true},
  ];
  addressHeaders = [
    {label: 'Street Address', field: 'addressLine1'},
    {label: 'Apt/Unit/Building', field: 'addressLine2'},
    {label: 'City', field: 'city'},
    {label: 'State', field: 'state', class: 'sm'},
    {label: 'Zip Code', field: 'zipCode', class: 'md'},
    {label: 'Plus4 Code', field: 'plus4Code', class: 'md'},
    {
      label: '',
      field: '',
      class: 'sm',
      actionBtn: 'Fix Address',
      actionBtnLabel: 'Fix',
      actionBtnIcon: '',
    },
  ];
  apiLogHeaders = [
    {label: 'Level', field: 'level', class: 'md'},
    {label: 'Message', field: 'renderedMessage'},
    {label: 'Timestamp', field: 'timestamp', class: 'md', fieldType: 'Date'},
  ];
  auditLogHeaders = [];
  baseAuditLogHeaders = [
    {label: 'Action Type', field: 'auditAction'},
    {
      label: 'Total Modified Fields',
      field: 'auditData.length',
    },
    {
      label: 'Modified by',
      field: 'user.name',
    },
    {
      label: 'Modified On',
      field: 'auditDate',
      fieldType: 'Date',
    },
  ];
  netsuiteLogHeaders = [
    {label: 'API', field: 'api', class: 'md'},
    {label: 'Type', field: 'logType', class: 'sm'},
    {label: 'Request', field: 'request'},
    {label: 'Response', field: 'response'},
    {label: 'Timestamp', field: 'createdOn', class: 'md', fieldType: 'Date'},
  ];
  orderHeaders = [
    {
      label: 'Order #',
      field: 'orderNumber',
      sortable: true,
      sortField: 'orderNumber',
    },
    {label: 'Patient Name', field: 'patient.name'},
    {
      label: 'Order Date',
      field: 'orderDateTime',
      sortable: true,
      sortField: 'orderDateTime',
      fieldType: 'Date',
    },
    {
      label: 'Order Type',
      field: 'orderType',
      sortable: true,
      sortField: 'orderTypeId',
    },
    {
      label: 'Order Status',
      field: 'orderStatus',
    },
    {
      label: 'Dispatch Status',
      field: 'dispatchStatus',
    },
  ];
  currentPatientInventoryHeaders = [
    {
      label: 'Item Name',
      field: 'item.name',
    },
    {label: 'Serial Number', field: 'serialNumber'},
    {
      label: 'Asset Tag',
      field: 'assetTagNumber',
    },
    {
      label: 'Item Number',
      field: 'item.itemNumber',
    },
    {
      label: 'Is Deleted',
      field: 'isDeleted',
    },
  ];
  newPatientInventoryHeaders = [
    {
      label: 'Item Name',
      field: 'item.name',
    },
    {label: 'Serial Number', field: 'serialNumber'},
    {
      label: 'Asset Tag',
      field: 'assetTagNumber',
    },
    {
      label: 'Item Number',
      field: 'item.itemNumber',
    },
    {
      label: '',
      field: '',
      class: 'md',
      actionBtn: 'Fix Issues',
      actionBtnLabel: 'Fix Issues',
      actionBtnIcon: '',
    },
  ];
  currentPatientItemHeaders = [
    {
      label: 'ID',
      field: 'id',
    },
    {
      label: 'Item Name',
      field: 'name',
    },
    {
      label: 'Item Number',
      field: 'itemNumber',
    },
    {
      label: 'Description',
      field: 'description',
    },
    {
      label: 'Avg Delivery Processing Time',
      field: 'avgDeliveryProcessingTime',
    },
  ];
  newPatientItemHeaders = [
    {
      label: 'ID',
      field: 'id',
    },
    {
      label: 'Item Name',
      field: 'name',
    },
    {
      label: 'Item Number',
      field: 'itemNumber',
    },
    {
      label: 'Description',
      field: 'description',
    },
    {
      label: 'Avg Delivery Processing Time',
      field: 'avgDeliveryProcessingTime',
    },
    {
      label: 'Action',
      field: '',
      class: 'md',
      actionBtn: 'Fix Issues',
      actionBtnLabel: 'Fix Issues',
      actionBtnIcon: '',
    },
  ];
  hospiceHeaders = [
    {
      label: 'Hospice Name',
      field: 'name',
    },
  ];
  ActionTypes = deepCloneObject(UserAuditActionTypes);
  auditFilter = new AuditFilter();
  showFilters = false;
  filtersApplied = false;
  currentOrder: any;
  fulfilledItems = null;
  allPatients = [];
  orderFixData = new ConfirmDialog();
  @ViewChild('confirmDialogOrder', {static: false})
  confirmDialogOrder: ConfirmDialogComponent;
  selectedOrder: any;
  apiLogTypes = [
    {label: 'Core', value: 'Core'},
    {label: 'Fulfillment', value: 'Fulfillment'},
    {label: 'Notification', value: 'Notification'},
    {label: 'Patient', value: 'Patient'},
  ];
  apiLogType: string;
  // nesuiteId
  paginationResponse: PaginationResponse;
  sieveRequest = new SieveRequest();

  activeTableView = '';
  activeTabView = 'fix';

  netsuiteRequest: any;
  netsuiteLogResponse: any;
  totalNetsuiteLogs = 0;

  patientInventoryCount: number;
  isAssetTagged = true;

  showModalPatientInventoryConsumable = false;
  patientInventoryData: any;

  @ViewChild('apiLogsTable ', {static: false})
  apiLogsTable: TableVirtualScrollComponent;
  @ViewChild('auditLogsTable ', {static: false})
  auditLogsTable: TableVirtualScrollComponent;
  @ViewChild('netsuiteLogsTable ', {static: false})
  netsuiteLogsTable: TableVirtualScrollComponent;
  @ViewChild('tableList', {static: false})
  tableList: TableVirtualScrollComponent;

  netsuiteLogFromDate: Date;
  netsuiteLogToDate: Date;

  patientBaseHeader = [
    {
      label: '',
      field: 'displayStatus',
      bodyClassType: 'PatientStatus',
      class: 'xs',
    },
    {
      label: 'Name',
      field: 'name',
    },
    {
      label: 'Created on',
      field: 'createdDateTime',
      fieldType: 'Date',
    },
    {
      label: 'Last Order',
      field: 'lastOrderDateTime',
      fieldType: 'Date',
    },
    {
      label: 'Status',
      field: 'status',
    },
  ];
  inactivePatientWithConsumableHeaders = [
    ...this.patientBaseHeader,
    {
      label: '',
      field: '',
      actionBtn: 'Remove Consumable Inventory',
      actionBtnLabel: 'Remove Consumable Inventory',
      actionBtnIcon: '',
    },
  ];
  patientWithInvalidStatusHeaders = [
    ...this.patientBaseHeader,
    {
      label: '',
      field: '',
      class: 'md',
      actionBtn: 'Fix Status',
      actionBtnLabel: 'Fix Status',
      actionBtnIcon: '',
    },
  ];
  @ViewChild('dispatchOrdersTable ', {static: false})
  dispatchOrdersTable: TableVirtualScrollComponent;

  dispatchOrderRequest = new NetsuiteDispatchOrderRequest();
  dispatchOrderResponse: any;
  dispatchOrderHeaders = [
    {label: 'Dispatch Id', field: 'nsDispatchId'},
    {label: 'Item Id', field: 'netsuiteItemId'},
    {label: 'Patient', field: 'patientName'},
    {label: 'Order Status', field: 'hmsPickupOrderStatus'},
    {label: 'Customer Id', field: 'customerId'},
    {label: 'Location Id', field: 'customerLocationId'},
    {
      label: 'Delivery Date',
      field: 'hmsDeliveryDate',
      fieldType: 'Date',
    },
    {
      label: 'Pickup Request Date',
      field: 'hmsPickupRequestDate',
      fieldType: 'Date',
    },
    {
      label: 'Pickup Date',
      field: 'pickupDate',
      fieldType: 'Date',
    },
    {
      label: 'Created Date',
      field: 'createdDate',
      fieldType: 'Date',
    },
  ];

  userTools = new SystemTools().userTools;
  orderTools = new SystemTools().orderTools;
  patientTools = new SystemTools().patientTools;
  hospiceTools = new SystemTools().hospiceTools;
  listTitle = '';
  tableHeaderLabel = '';
  headerBtnAction = '';
  userToolLoading = false;
  orderToolLoading = false;
  patientToolLoading = false;
  hospiceToolLoading = false;
  selectedToolAction = '';
  tableHeaders = [];
  selectedActionBtn = '';
  unconfirmedInNetsuiteFixConfig = new UnconfirmedInNetsuiteFixConfig();
  patientInventoryIssue = '';

  constructor(
    private systemService: SystemService,
    private auditService: AuditService,
    private toastService: ToastService,
    private patientService: PatientService,
    private hospiceMemberService: HospiceMemberService,
    private sitesService: SitesService,
    private orderHeaderService: OrderHeadersService,
    private route: ActivatedRoute,
    private location: Location,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.activeTabView = params.view || 'fix';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.activeTabView}).toString()
      );
    });
    this.auditLogHeaders = [...this.baseAuditLogHeaders];
    const event = {index: 0};
    switch (this.activeTabView) {
      case 'fix':
        event.index = 0;
        break;
      case 'apiLogs':
        event.index = 1;
        break;
      case 'auditLogs':
        event.index = 2;
        break;
      case 'netsuiteLogs':
        event.index = 3;
        break;
      case 'healthCheck':
        event.index = 4;
        break;
      case 'dispatchOrders':
        event.index = 5;
        break;
      case 'featureFlags':
        event.index = 6;
        break;
    }
    this.onTabChange(event);
  }

  getDefaultAuditRequest() {
    const request = new SieveRequest();
    request.sorts = '-auditDate';
    return request;
  }

  canUpdateSystem() {
    return IsPermissionAssigned('System', 'Update');
  }

  getIdentityMissingUsers() {
    this.tableHeaders = [...this.userHeaders];
    this.userToolLoading = true;
    this.systemService
      .getMissingIdentityCount(this.sieveRequest)
      .pipe(finalize(() => (this.userToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  fixIdentityMissingUsers() {
    this.userToolLoading = true;
    this.systemService
      .fixMissingIdentityUsers()
      .pipe(finalize(() => (this.userToolLoading = false)))
      .subscribe((res: any) => {
        this.toastService.showSuccess(
          `Successfully fixed ${this.identityMissingUsersCount} User's identity missing issues`
        );
        this.setInitialRequestResponse();
        this.getIdentityMissingUsers();
      });
  }

  getUsersCount() {
    this.getUsers();
  }

  getUsers() {
    this.tableHeaders = [...this.userHeaders];
    this.userToolLoading = true;
    this.activeTableView = 'users';
    this.systemService
      .getUsersWithoutEmail(this.sieveRequest)
      .pipe(finalize(() => (this.userToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  getNext(event, response, request, action) {
    if (
      this[response]?.pageNumber >= this[response]?.totalPageCount ||
      this[response]?.currentPage >= this[response]?.totalPages
    ) {
      return;
    }
    if (this[request]?.page >= 0) {
      this[request].page = event.pageNum;
    } else if (this[request]?.pageNumber >= 0) {
      this[request].pageNumber = event.pageNum;
    }
    this[action]();
  }

  getNonVerifiedAddresses() {
    this.tableHeaders = [...this.addressHeaders];
    this.patientToolLoading = true;
    this.systemService
      .getNonVerifiedAddress(false, this.sieveRequest)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  getNonVerifiedHomeAddresses() {
    this.tableHeaders = [...this.addressHeaders];
    this.patientToolLoading = true;
    this.systemService
      .getNonVerifiedAddress(true, this.sieveRequest)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  fixNonVerifiedAddress(address) {
    this.patientToolLoading = true;
    this.systemService
      .fixNonVerifiedAddress(address.id, false)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`Address has been verified`);
        this.setInitialRequestResponse();
        this.getNonVerifiedAddresses();
      });
  }

  fixNonVerifiedHomeAddress(address) {
    this.patientToolLoading = true;
    this.systemService
      .fixNonVerifiedAddress(address.id, true)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`Address has been verified`);
        this.setInitialRequestResponse();
        this.getNonVerifiedHomeAddresses();
      });
  }

  fixNonVerifiedAddresses() {
    this.patientToolLoading = true;
    this.systemService
      .fixNonVerifiedAddresses(false)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`${res} Addresses have been verified`);
        this.setInitialRequestResponse();
        this.getNonVerifiedAddresses();
      });
  }

  fixNonVerifiedHomeAddresses() {
    this.patientToolLoading = true;
    this.systemService
      .fixNonVerifiedAddresses(true)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`${res} Addresses have been verified`);
        this.setInitialRequestResponse();
        this.getNonVerifiedHomeAddresses();
      });
  }

  fixWithoutFhirRecord() {
    this.patientToolLoading = true;
    this.systemService
      .fixWithoutFhirRecord()
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`${res} patients have been fixed`);
        this.setInitialRequestResponse();
        this.getPatientsWithoutFhirRecord();
      });
  }

  getOrdersWithoutStatuses() {
    const validateBtn = {
      label: '',
      field: '',
      class: 'md',
      actionBtn: 'Validate Fix',
      actionBtnLabel: 'Validate Fix',
      actionBtnIcon: '',
    };
    this.tableHeaders = [...this.orderHeaders, validateBtn];
    this.orderToolLoading = true;
    this.activeTableView = 'orders';
    this.systemService
      .getOrdersWithoutStatus(this.sieveRequest)
      .pipe(
        mergeMap((res: PaginationResponse) => {
          this.paginationResponse = res;
          const patientUuids = getUniqArray(
            this.paginationResponse.records.map(r => r.patientUuid)
          );
          if (patientUuids.length > 0) {
            const filters = [
              {
                field: 'uniqueId',
                operator: SieveOperators.Equals,
                value: patientUuids,
              },
            ];
            const patientRequest = {
              filters: buildFilterString(filters),
            };
            return this.patientService.getPatients(patientRequest);
          } else {
            return of();
          }
        }),
        finalize(() => (this.orderToolLoading = false))
      )
      .subscribe((patientsResponse: PaginationResponse) => {
        this.allPatients = patientsResponse?.records.map(p => {
          p.name = `${p.firstName} ${p.lastName}`;
          return p;
        });
        this.paginationResponse.records.map(o => {
          o.patient = this.allPatients.find(p => p.uniqueId === o.patientUuid);
          return o;
        });
      });
  }

  previewOrderStatus(order) {
    this.selectedOrder = order;
    this.orderToolLoading = true;
    this.systemService
      .fixOrderWithoutStatus(order.id, true)
      .pipe(finalize(() => (this.orderToolLoading = false)))
      .subscribe((res: number) => {
        this.showOrderStatusFixPreview(order, res);
      });
  }

  showOrderStatusFixPreview(order, preview) {
    this.orderFixData.header = 'Status Fix Preview';
    const lineItems = order.orderLineItems.flatMap(x => {
      const previewLineItem = preview.orderLineItems.find(li => li.id === x.id);
      const obj = {
        name: `<b>${x.item.name}</b><br>`,
        orderStatus:
          x.status !== previewLineItem.status
            ? `Line Item: ${x.status} &#8594; ${previewLineItem.status}<br>`
            : '',
        disPatchStatus:
          x.dispatchStatus !== previewLineItem.dispatchStatus
            ? `Dispatch: ${x.dispatchStatus} &#8594; ${previewLineItem.dispatchStatus}<br>`
            : '',
      };
      return obj.orderStatus || obj.disPatchStatus ? [obj] : [];
    });
    const showOrderPreview =
      order.orderStatus !== preview.orderStatus
        ? `Order Status: ${order.orderStatus} &#8594; ${preview.orderStatus}<br>`
        : '';
    const showdispatchPreview =
      order.dispatchStatus !== preview.dispatchStatus
        ? `Dispatch Status: ${order.dispatchStatus} &#8594; ${preview.dispatchStatus}<br>`
        : '';
    this.orderFixData.message = `ORDER<br>
    ${showOrderPreview}
    ${showdispatchPreview}
    ${lineItems.length > 0 ? 'ORDER LINE ITEMS:<br>' : ''}
    <ul>
      ${lineItems
        .map(x => {
          return x.name + x.orderStatus + x.disPatchStatus;
        })
        .join('')}
    </ul>`;
    this.orderFixData.acceptLabel = 'Fix';
    this.orderFixData.rejectLabel = 'Cancel';
    this.orderFixData.icon = '';
    this.confirmDialogOrder.showDialog(this.orderFixData);
  }

  fixOrderStatusConfirmed() {
    this.orderToolLoading = true;
    this.systemService
      .fixOrderWithoutStatus(this.selectedOrder.id, false)
      .pipe(finalize(() => (this.orderToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`Order status has been fixed`);
        this.setInitialRequestResponse();
        this.getOrdersWithoutStatuses();
      });
  }

  getMembersWithoutNetsuiteContactId() {
    this.tableHeaders = [...this.memberHeaders];
    this.userToolLoading = true;
    this.systemService
      .getCountWithMissingNetSuiteContact(this.sieveRequest)
      .pipe(finalize(() => (this.userToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  fixMembersWithoutNetsuiteId() {
    this.userToolLoading = true;
    this.systemService
      .fixMembersWithMissingNetSuiteContact()
      .pipe(finalize(() => (this.userToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`netsuite id for ${res} Members have been added`);
        this.setInitialRequestResponse();
        this.getMembersWithoutNetsuiteContactId();
      });
  }

  getInternalUsersWithoutNetsuiteContactId() {
    this.tableHeaders = [...this.userHeaders];
    this.userToolLoading = true;
    this.systemService
      .getInternalUsersCountWithMissingNetSuiteContact(this.sieveRequest)
      .pipe(finalize(() => (this.userToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  fixInternalUsersWithoutNetsuiteId() {
    this.userToolLoading = true;
    this.systemService
      .fixInternalUsersWithMissingNetSuiteContact()
      .pipe(finalize(() => (this.userToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`netsuite id for ${res} Internal Users have been added`);
        this.setInitialRequestResponse();
        this.getInternalUsersWithoutNetsuiteContactId();
      });
  }

  getUnConfirmedOrderFulfillments() {
    const fixBtn = {
      label: '',
      field: '',
      class: 'sm',
      actionBtn: 'Fix',
      actionBtnLabel: 'Fix',
      actionBtnIcon: '',
    };
    this.tableHeaders = [...this.orderHeaders, fixBtn];
    this.unconfirmedInNetsuiteFixConfig = new UnconfirmedInNetsuiteFixConfig();
    this.activeTableView = 'unconfirmedInNetsuite';
    this.orderToolLoading = true;
    this.systemService
      .getUnconfirmedOrders()
      .pipe(finalize(() => (this.orderToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  fixUnConfirmedOrderFulfillment(order) {
    this.orderToolLoading = true;
    this.systemService
      .fixUnconfirmedOrder(order.id)
      .pipe(finalize(() => (this.orderToolLoading = false)))
      .subscribe(() => {
        this.toastService.showSuccess(`Unconfirmed order have been confirmed`);
        this.setInitialRequestResponse();
        this.getUnConfirmedOrderFulfillments();
      });
  }

  fixUnConfirmedOrderFulfillments() {
    this.orderToolLoading = true;
    this.systemService
      .fixUnconfirmedOrders(this.unconfirmedInNetsuiteFixConfig)
      .pipe(finalize(() => (this.orderToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`${res} Unconfirmed orders have been confirmed`);
        this.setInitialRequestResponse();
        this.getUnConfirmedOrderFulfillments();
      });
  }

  getOrdersWithoutSites() {
    this.tableHeaders = [...this.orderHeaders];
    this.orderToolLoading = true;
    this.systemService
      .getOrdersWithoutAssignedSites(this.sieveRequest)
      .pipe(finalize(() => (this.orderToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
      });
  }

  fixOrdersWithoutAssignedSites() {
    this.orderToolLoading = true;
    this.systemService
      .fixOrdersWithoutAssignedSites()
      .pipe(finalize(() => (this.orderToolLoading = false)))
      .subscribe((res: number) => {
        this.toastService.showSuccess(`${res} orders without assigned sites have been fixed`);
        this.setInitialRequestResponse();
        this.getOrdersWithoutSites();
      });
  }

  fetchApiLogs() {
    const apiLogRequest = this.prepareCoreApiLogRequest();
    this.logsLoading = true;

    this.systemService.getApiLogs(apiLogRequest).subscribe((response: any) => {
      this.logsLoading = false;

      if (!this.continuationToken && this.coreApiLogsResponse?.records.length > 0) {
        return;
      }
      if (response.apiLogs) {
        this.continuationToken = response.continuationToken;
        this.coreApiLogsResponse.records = this.coreApiLogsResponse.records.concat(
          response.apiLogs
        );
        this.coreApiLogsResponse.continuationToken = response.continuationToken;
      }
    });
  }

  getAuditLogs() {
    if (
      !this.selectedAuditType?.name ||
      this.logsLoading ||
      this.auditLogResponse.continuationToken === null
    ) {
      return;
    }

    this.logsLoading = true;
    const auditLogRequest = this.prepareAuditLogRequest();
    const res = this.auditService
      .getAuditLogs(auditLogRequest)
      .pipe(
        finalize(() => {
          this.logsLoading = false;
        })
      )
      .subscribe((response: any) => {
        if (response.apiLogs) {
          this.auditLogResponse.records = this.auditLogResponse.records.concat(response.apiLogs);
          this.auditLogResponse.continuationToken = response.continuationToken;

          switch (this.selectedAuditType.name) {
            case 'Users':
              const userIds = response.apiLogs.map(l => l.entityId);
              this.fetchTargetUsersForAuditLog(userIds);
              break;
            case 'OrderHeaders':
              const orderIds = getUniqArray(response.apiLogs.map(l => l.entityId));
              this.fetchOrderNumberFromOrderId(orderIds);
              break;
            default:
              break;
          }
        }
      });
  }

  fetchNextAuditLogPage() {
    if (!this.auditLogResponse) {
      return;
    }
    this.getAuditLogs();
  }

  fetchPrevAuditLogPage() {
    this.auditRequest.page--;
    this.getAuditLogs();
  }

  changeAuditType(event) {
    this.orderDetailViewOpen = false;
    let headerToAppend;
    this.auditLogsFilterConfiguration = [...this.baseAuditLogsFilterConfiguration];
    switch (event?.value?.name) {
      case 'Users':
        headerToAppend = {
          label: 'Target User',
          field: 'targetUser.name',
        };
        this.auditLogsFilterConfiguration = [
          ...this.auditLogsFilterConfiguration,
          ...this.userAuditLogsExtraFilterConf,
        ];
        break;
      case 'Hospices':
        headerToAppend = {
          label: 'Hospice Id',
          field: 'entityId',
        };
        break;

      case 'HospiceLocations':
        headerToAppend = {
          label: 'Hospice Location Id',
          field: 'entityId',
        };
        break;
      case 'OrderHeaders':
        headerToAppend = {
          label: 'Order #',
          field: 'orderNumber',
        };
        this.auditLogsFilterConfiguration = [
          ...this.auditLogsFilterConfiguration,
          ...this.orderheaderAuditLogsExtraFilterConf,
        ];
        break;
      case 'OrderLineItems':
        headerToAppend = {
          label: 'Order LineItem Id',
          field: 'entityId',
        };
        break;
      case 'Inventory':
        headerToAppend = {
          label: 'Inventory Id',
          field: 'entityId',
        };
        break;
      case 'Items':
        headerToAppend = {
          label: 'Item Id',
          field: 'entityId',
        };
        break;
      case 'Sites':
        headerToAppend = {
          label: 'Site Id',
          field: 'entityId',
        };
        break;
      default:
        break;
    }
    if (headerToAppend?.label) {
      this.auditLogHeaders = [headerToAppend, ...this.baseAuditLogHeaders];
    } else {
      this.auditLogHeaders = [...this.baseAuditLogHeaders];
    }

    this.auditRequest = this.getDefaultAuditRequest();
    this.auditRequest.auditType = this.selectedAuditType.name;
    this.auditDetailsViewOpen = false;
    this.selectedAuditLog = null;
    if (this.auditLogResponse) {
      this.auditLogResponse = new BaseContinuationTokenResponse();
    }

    this.resetSort();
    this.resetFilters();
    this.getAuditLogs();
  }

  showAuditLogDetails({currentRow}) {
    this.orderDetailViewOpen = false;
    this.auditDetailsViewOpen = true;
    this.selectedAuditLog = currentRow;
  }

  closeAuditLogDetails() {
    this.auditDetailsViewOpen = false;
    this.selectedAuditLog = null;
  }

  closeNetsuiteLogDeatils() {
    this.netsuiteDetailsViewOpen = false;
    this.selectedNetsuiteLog = null;
  }

  resetFilters() {
    this.appFilter?.clearFilter();
  }

  orderSelected(event) {
    this.currentOrder = event.currentRow;
    this.fulfilledItems = [];
    this.getNurse(this.currentOrder);
    this.getOrderFulfillment(this.currentOrder.id);
    this.getSite(this.currentOrder.siteId);
    const patient = this.allPatients.find((p: any) => {
      return p.uniqueId === this.currentOrder.patientUuid;
    });
    this.currentOrder.patientNotes = patient?.patientNotes;
    this.orderDetailViewOpen = true;
  }

  getNurse(order) {
    const hospiceMemberfilters = [
      {
        field: 'userId',
        operator: SieveOperators.Equals,
        value: [order.orderRecipientUserId],
      },
    ];
    const hospiceMemberRequest = {
      filters: buildFilterString(hospiceMemberfilters),
    };
    this.hospiceMemberService
      .getAllHospiceMembers(order.hospiceId, hospiceMemberRequest)
      .subscribe((memberResponse: any) => {
        this.currentOrder.nurse = memberResponse.records.find(
          p => p.userId === this.currentOrder.orderRecipientUserId
        );
      });
  }

  getSite(siteId) {
    this.sitesService.getSiteById(siteId).subscribe((site: any) => {
      this.currentOrder.site = site;
    });
  }
  getOrderFulfillment(id) {
    this.orderHeaderService.getOrderFulfillment(id).subscribe((orderFulfillmentRes: any) => {
      this.fulfilledItems = orderFulfillmentRes.records;
    });
  }

  closeOrderDetails(event) {
    this.orderDetailViewOpen = false;
  }

  onTabChange(event) {
    this.closeAuditLogDetails();
    switch (event.index) {
      case 0: {
        this.activeTabView = 'fix';
        break;
      }
      case 1: {
        this.activeTabView = 'apiLogs';
        this.coreApiLogsResponse = new BaseContinuationTokenResponse();
        this.dataTablesReset();
        this.fetchApiLogs();
        break;
      }
      case 2: {
        this.activeTabView = 'auditLogs';
        this.dataTablesReset();
        this.auditLogResponse = new BaseContinuationTokenResponse();
        this.selectedAuditType = null;
        break;
      }
      case 3: {
        this.activeTabView = 'netsuiteLogs';
        this.dataTablesReset();
        this.fetchNetsuiteLogs();
        break;
      }
      case 4: {
        this.activeTabView = 'healthCheck';
        break;
      }
      case 5: {
        this.activeTabView = 'dispatchOrders';
        this.dataTablesReset();
        this.getDispatchOrders();
        break;
      }
      case 6: {
        this.activeTabView = 'featureFlags';
        break;
      }
    }
    this.location.replaceState(
      window.location.pathname,
      new URLSearchParams({view: this.activeTabView}).toString()
    );
  }

  fetchNetsuiteLogs() {
    this.netsuiteLogResponse = [];
    this.netsuiteRequest = {
      pageNumber: 1,
      pageSize: 25,
      fromDate: this.netsuiteLogFromDate,
      toDate: this.netsuiteLogToDate,
    };
    this.getNetsuiteLogs();
  }

  getNetsuiteLogs() {
    this.logsLoading = true;
    this.systemService
      .getNetsuiteLogs(this.netsuiteRequest)
      .pipe(
        finalize(() => {
          this.logsLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.netsuiteLogResponse = response;
      });
  }

  fetchDispatchOrders() {
    this.netsuiteLogResponse = [];
    this.dispatchOrderRequest = {
      ...this.dispatchOrderRequest,
      ...{pageNumber: 1, pageSize: 25},
    };
    this.getDispatchOrders();
  }

  getDispatchOrders() {
    this.logsLoading = true;
    this.systemService
      .getDispatchOrders(this.dispatchOrderRequest)
      .pipe(
        finalize(() => {
          this.logsLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.dispatchOrderResponse = response;
        const patientUuids = getUniqArray(
          this.dispatchOrderResponse?.records?.map(r => r.patientGuid)
        );
        this.getPatients(patientUuids, true);
      });
  }

  fetchPatientInventoryWithIssues({patientInventory}) {
    this.patientInventoryIssue = patientInventory.issue;
    this.tableHeaderLabel = '';
    this.setInitialRequestResponse();
    this.patientInventoryLoading = true;
    this.activeTableView = 'patientInventory';
    this.systemService
      .getPatientInventoryWithIssues(patientInventory)
      .pipe(finalize(() => (this.patientInventoryLoading = false)))
      .subscribe((response: any) => {
        if (!response) {
          this.patientInventoryCount = 0;
          return;
        }

        if (response.currentInventory) {
          response.currentInventory.isDeleted = response.currentInventoryIsDeleted;
        }

        this.patientInventoryResponse = {
          ...this.patientInventoryResponse,
          patientInventory: response.patientInventory ? [response.patientInventory] : [],
          currentInventory: response.currentInventory ? [response.currentInventory] : [],
          newInventory: response.newInventory ? response.newInventory : [],
          currentItem: response.currentItem ? [response.currentItem] : [],
          newItem: response.newItem ? [response.newItem] : [],
        };
        this.patientInventoryCount = 1;
      });
  }

  fixPatientInventoryIssues(event) {
    let payload: any = {
      patientInventoryId: this.patientInventoryResponse.patientInventory[0].id,
    };
    if (this.patientInventoryIssue === 'invalid-inventory') {
      payload = {
        ...payload,
        newInventoryId: this.patientInventoryResponse.newInventory[0].id,
      };
    } else {
      payload = {
        ...payload,
        newItemId: this.patientInventoryResponse.newItem[0].id,
      };
    }
    this.systemService
      .fixPatientInventoryWithIssues(payload, this.patientInventoryIssue)
      .pipe(finalize(() => (this.patientInventoryLoading = false)))
      .subscribe(() => {
        this.patientInventoryResponse = null;
        this.toastService.showSuccess(`Patient Inventory issue has been fixed`);
      });
  }

  shouldShowFixPatientInventoryModal() {
    return (
      this.activeTableView === 'patientInventory' && this.patientInventoryResponse?.patientInventory
    );
  }

  fetchDeletedPatientInventory(event?) {
    this.tableHeaders = [...this.deletedPatientInventoryHeaders];
    if (event && event.isAssetTagged !== this.isAssetTagged) {
      this.sieveRequest.page = 1;
    }
    this.isAssetTagged = event ? event.isAssetTagged : this.isAssetTagged;
    this.patientToolLoading = true;
    this.activeTableView = 'deletedPatientInventory';
    const filter = [
      {
        field: 'isAssetTagged',
        operator: SieveOperators.Equals,
        value: [this.isAssetTagged],
      },
    ];
    this.sieveRequest.filters = buildFilterString(filter);
    this.systemService
      .getDeletedPatientInventory(this.sieveRequest)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((res: PaginationResponse) => {
        this.paginationResponse = res;
        const patientUuids = getUniqArray(this.paginationResponse.records.map(r => r.patientUuid));
        this.getPatients(patientUuids);
      });
  }

  getPatients(uuids, dispatchOrderResponse = false) {
    const filters = [
      {
        field: 'uniqueId',
        operator: SieveOperators.Equals,
        value: uuids,
      },
    ];
    const patientRequest: any = {
      filters: buildFilterString(filters),
    };
    this.patientService.getPatients(patientRequest).subscribe((res: PaginationResponse) => {
      if (this.activeTableView === 'inactivePatientWithConsumable') {
        this.paginationResponse.records = [...this.paginationResponse.records, ...res.records];
        this.paginationResponse.records = this.paginationResponse.records.map(x => {
          x.name = `${x?.firstName} ${x?.lastName}`;
          x.showRemoveBtn = x.status === 'Active' ? false : true;
          return x;
        });
      }
      if (this.activeTableView === 'deletedPatientInventory') {
        this.paginationResponse.records.map(x => {
          const patient = res.records.find(p => p.uniqueId === x.patientUuid);
          x.patientName = `${patient?.firstName} ${patient?.lastName}`;
        });
      }
      if (this.activeTableView === 'patientWithInvalidStatus') {
        this.paginationResponse.records = res.records;
        this.paginationResponse.records = this.paginationResponse.records.map(x => {
          x.name = `${x?.firstName} ${x?.lastName}`;
          return x;
        });
      }
      if (this.activeTableView === 'patientWithoutFhirRecord') {
        this.paginationResponse.records = [...this.paginationResponse.records, ...res.records];
        this.paginationResponse.records = this.paginationResponse.records.map(x => {
          x.name = `${x?.firstName} ${x?.lastName}`;
          return x;
        });
      }
      if (dispatchOrderResponse) {
        this.dispatchOrderResponse?.records?.map(x => {
          const patient = res.records.find(p => p.uniqueId === x.patientGuid);
          x.patientName = patient ? `${patient?.firstName} ${patient?.lastName}` : '';
          return x;
        });
      }
    });
  }

  dataTablesReset() {
    if (this.activeTabView === 'apiLogs' && this.apiLogsTable) {
      this.apiLogsTable.reset();
    }
    if (this.activeTabView === 'auditLogs' && this.auditLogsTable) {
      this.auditLogsTable.reset();
    }
    if (this.activeTabView === 'netsuiteLogs' && this.netsuiteLogsTable) {
      this.netsuiteLogsTable.reset();
    }
    if (this.activeTabView === 'dispatchOrders' && this.dispatchOrdersTable) {
      this.dispatchOrdersTable.reset();
    }
    this.auditRequest.page = 1;
  }

  resetSort() {
    if (this.activeTabView === 'apiLogs' && this.apiLogsTable) {
      this.apiLogsTable.resetSort();
    }
    if (this.activeTabView === 'auditLogs' && this.auditLogsTable) {
      this.auditLogsTable.resetSort();
    }
    if (this.activeTabView === 'netsuiteLogs' && this.netsuiteLogsTable) {
      this.netsuiteLogsTable.resetSort();
    }
    if (this.activeTabView === 'dispatchOrders' && this.dispatchOrdersTable) {
      this.dispatchOrdersTable.resetSort();
    }
  }

  filterSystem(event) {
    if (event) {
      const filterValue = getObjectFromFilterString(event, 'dateRange');
      if (this.activeTabView === 'apiLogs') {
        this.coreApiLogsResponse = new BaseContinuationTokenResponse();
        this.setAuditFilterValues(filterValue);
        this.fetchApiLogs();
      }
      if (this.activeTabView === 'netsuiteLogs') {
        this.setNetsuiteFilterValues(filterValue);
        if (this.netsuiteLogFromDate && this.netsuiteLogToDate) {
          this.fetchNetsuiteLogs();
        }
      }
      if (this.activeTabView === 'dispatchOrders') {
        this.setDispatchOrderFilterValues(filterValue);
        this.fetchDispatchOrders();
      }
    } else {
      if (this.activeTabView === 'netsuiteLogs') {
        this.setNetsuiteFilterValues(null);
        this.fetchNetsuiteLogs();
      } else if (this.activeTabView === 'netsuiteLogs' || this.activeTabView === 'apiLogs') {
        this.coreApiLogsResponse = new BaseContinuationTokenResponse();
        this.setAuditFilterValues(null);
        this.fetchApiLogs();
      } else if (this.activeTabView === 'dispatchOrders') {
        this.setDispatchOrderFilterValues(null);
        this.fetchDispatchOrders();
      }
    }
  }

  setAuditFilterValues(filterValue) {
    const formattedApiLogDate = filterValue?.dateRange?.split('|') ?? [];
    this.apiLogFromDate = formattedApiLogDate[0] ?? null;
    this.apiLogToDate = formattedApiLogDate[1] ?? null;
    this.selectedAuditType = filterValue?.apiLogType ?? null;
    this.coreApiLogsResponse = new BaseContinuationTokenResponse();
    this.continuationToken = null;
  }
  setNetsuiteFilterValues(filterValue) {
    const formattedNetsuiteLogDate = filterValue?.dateRange?.split('|') ?? [];
    this.netsuiteLogFromDate = formattedNetsuiteLogDate[0] ?? null;
    this.netsuiteLogToDate = formattedNetsuiteLogDate[1] ?? null;
  }

  setDispatchOrderFilterValues(filterValue) {
    const formattedDeliveryDate = filterValue?.deliveryDate?.split('|') ?? [];
    const formattedPickupDate = filterValue?.pickupDate?.split('|') ?? [];
    this.dispatchOrderRequest.patientUuid = filterValue?.patientUuid ?? null;
    this.dispatchOrderRequest.hospiceLocationId = filterValue?.hospiceLocationId ?? null;
    this.dispatchOrderRequest.itemId = null;
    this.dispatchOrderRequest.deliveryFromDate = formattedDeliveryDate[0] ?? null;
    this.dispatchOrderRequest.deliveryToDate = formattedDeliveryDate[1] ?? null;
    this.dispatchOrderRequest.pickUpFromDate = formattedPickupDate[0] ?? null;
    this.dispatchOrderRequest.pickUpToDate = formattedPickupDate[1] ?? null;
  }

  isCurrentView(view) {
    return this.activeTabView === view;
  }

  getInactivePatientsWithOnlyConsumables() {
    this.tableHeaders = [...this.inactivePatientWithConsumableHeaders];
    this.patientToolLoading = true;
    this.activeTableView = 'inactivePatientWithConsumable';
    this.systemService
      .getInactivePatientsWithConsumablesOnly()
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((patientsUuids: any) => {
        this.paginationResponse.totalRecordCount = patientsUuids.length;
        this.paginationResponse.pageNumber = 1;
        if (patientsUuids.length) {
          const intervalRange = 50;
          const looplength = Math.ceil(patientsUuids.length / intervalRange);
          [...Array(+looplength)].map((x, i) => {
            const baseInterval = i * intervalRange;
            const limitedPatientUuids = patientsUuids.slice(
              baseInterval,
              baseInterval + intervalRange
            );
            this.getPatients(limitedPatientUuids);
          });
        }
      });
  }

  previewInactivePatientWithConsumable(patient) {
    if (patient?.uniqueId) {
      this.patientToolLoading = true;
      this.selectedPatient = patient;
      this.systemService
        .fixInactivePatientsWithConsumablesOnly(this.selectedPatient.uniqueId, true)
        .pipe(finalize(() => (this.patientToolLoading = false)))
        .subscribe((resPreviewInventory: any) => {
          this.selectedPatientInventory = resPreviewInventory;
          this.showModalPatientInventoryConsumable = true;
        });
    }
  }

  fixPatientConsumableConfirmed() {
    this.showModalPatientInventoryConsumable = false;
    this.patientToolLoading = true;
    this.patientInventoryData = [];

    this.systemService
      .fixInactivePatientsWithConsumablesOnly(this.selectedPatient.uniqueId, false)
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe(patiendUuid => {
        this.toastService.showSuccess(`Consumable Inventory removed from patient`);
        this.setInitialRequestResponse();
        this.getInactivePatientsWithOnlyConsumables();
      });
  }

  getPatientWithInvalidStatus() {
    this.tableHeaders = [...this.patientWithInvalidStatusHeaders];
    this.patientToolLoading = true;
    this.activeTableView = 'patientWithInvalidStatus';
    this.systemService
      .getPatientsWithInvalidStatus()
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((patientsUuids: any) => {
        this.paginationResponse.totalRecordCount = patientsUuids.length;
        this.paginationResponse.pageNumber = 1;
        if (patientsUuids.length) {
          this.getPatients(patientsUuids);
        }
      });
  }

  getPatientsWithoutFhirRecord() {
    this.tableHeaders = [...this.patientBaseHeader];
    this.patientToolLoading = true;
    this.activeTableView = 'patientWithoutFhirRecord';
    this.systemService
      .getPatientsWithoutFhirRecord()
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((patientsUuids: any) => {
        this.paginationResponse.totalRecordCount = patientsUuids.length;
        this.paginationResponse.pageNumber = 1;
        if (patientsUuids.length) {
          const intervalRange = 50;
          const looplength = Math.ceil(patientsUuids.length / intervalRange);
          [...Array(+looplength)].map((x, i) => {
            const baseInterval = i * intervalRange;
            const limitedPatientUuids = patientsUuids.slice(
              baseInterval,
              baseInterval + intervalRange
            );
            this.getPatients(limitedPatientUuids);
          });
        } else {
          this.paginationResponse.records = [];
        }
      });
  }

  fixPatientWithInvalidStatus(patient) {
    if (patient?.uniqueId) {
      this.systemService
        .fixPatientWithInvalidStatus(patient?.uniqueId)
        .subscribe((response: any) => {
          this.toastService.showSuccess(`Patient's status has been fixed`);
          this.setInitialRequestResponse();
          this.getPatientWithInvalidStatus();
        });
    }
  }

  fixAllPatientWithInvalidStatus() {
    this.tableHeaders = [...this.patientWithInvalidStatusHeaders];
    this.patientToolLoading = true;
    this.systemService
      .fixAllPatientWithInvalidStatus()
      .pipe(finalize(() => (this.patientToolLoading = false)))
      .subscribe((response: any) => {
        this.toastService.showSuccess(`Patient's status has been fixed`);
        this.setInitialRequestResponse();
        this.getPatientWithInvalidStatus();
      });
  }

  receiveAction(event) {
    const {action, listTitle, data, headerBtnAction, tableHeaderLabel, actionBtn} = event;
    this.headerBtnAction = headerBtnAction;
    this.listTitle = listTitle;
    this.tableHeaderLabel = tableHeaderLabel;
    this.setInitialRequestResponse();
    this.selectedToolAction = action;
    this.selectedActionBtn = actionBtn;
    this[action](data);
  }

  setInitialRequestResponse() {
    this.paginationResponse = new BasePaginationReponse();
    this.sieveRequest = new SieveRequest();
    if (this.tableList) {
      this.tableList.reset();
    }
  }

  headerActionTrigger() {
    this[this.headerBtnAction]();
  }

  receiveActionBtnTrigger(event) {
    this[this.selectedActionBtn](event?.object);
  }

  filterChanged(changedFilters) {
    this.auditRequest.filters = changedFilters.filterString;
    this.auditRequest.filtersMap = changedFilters.filters;

    if (changedFilters) {
      if (this.selectedAuditType.name === 'Inventory') {
        if (!changedFilters.filters.hasOwnProperty('Modified On(range)')) {
          this.toastService.showInfo('Modified on(Range) filter is required');
          return;
        }
      }
    }
    this.dataTablesReset();
    this.auditLogResponse = new BaseContinuationTokenResponse();
    this.continuationToken = null;
    this.getAuditLogs();
  }

  searchField(event) {
    const {query, label} = event;

    switch (label) {
      case 'Order #':
        this.fetchOrderHeaders(query);
        break;
      case 'Modified By(Name)':
      case 'Target User':
        this.searchUsers(query, label);
        break;
      default:
        break;
    }
  }
  getEntityDetailsUrl() {
    const url = this.getUrlForAuditType(this.selectedAuditType?.name);
    return url;
  }

  getUrlForAuditType(auditType: string) {
    if (auditType) {
      return this.AudiTypeUrls[auditType];
    }
    return '';
  }

  ShowOrderDetails() {
    const orderReq = this.orderHeaderService.getOrderHeaderById(
      this.selectedAuditLog.entityId,
      true
    );
    forkJoin([orderReq])
      .pipe(
        mergeMap(([orderRes]: any[]) => {
          this.currentOrder = orderRes;
          this.currentOrder.orderNotes = orderRes?.orderNotes ?? [];
          this.currentOrder.nurse = orderRes?.orderingNurse ?? '';
          this.fulfilledItems = orderRes?.orderFulfillmentLineItems ?? [];
          this.currentOrder.createdByUser = orderRes?.createdByUser ?? '';
          this.currentOrder.modifiedByUser = orderRes?.modifiedByUser ?? '';
          this.currentOrder.assignedDriver = orderRes?.assignedDriver ?? '';
          const filters = [
            {
              field: 'uniqueId',
              operator: SieveOperators.Equals,
              value: [orderRes.patientUuid],
            },
          ];
          const patientReq = {
            filters: buildFilterString(filters),
          };
          this.getSiteInfo();
          return this.patientService.getPatients(patientReq);
        }),
        mergeMap((patientRes: any) => {
          this.currentOrder.patient = patientRes.records[0];
          this.currentOrder.patient.name = `${this.currentOrder.patient.firstName} ${this.currentOrder.patient.lastName}`;
          this.orderDetailViewOpen = true;
          return this.patientService.getPatientNotes(this.currentOrder.patient?.id);
        })
      )
      .subscribe((res: any) => {
        this.currentOrder.patientNotes = res;
        this.orderDetailViewOpen = true;
      });
  }

  getSiteInfo() {
    this.sitesService.searchSites({searchQuery: ''}).subscribe(res => {
      const site = res.records.find(x => x.id === this.currentOrder.siteId);
      this.currentOrder.site = site;
    });
  }

  getHospiceWithoutFHIR() {
    this.tableHeaders = [...this.hospiceHeaders];
    this.hospiceToolLoading = true;
    this.systemService
      .getAllHospicesWithoutFHIR()
      .pipe(finalize(() => (this.hospiceToolLoading = false)))
      .subscribe((res: any) => {
        this.paginationResponse.records = res.map(hospice => {
          return {
            name: hospice,
          };
        });
        this.paginationResponse.totalRecordCount = res.length;
        this.paginationResponse.pageNumber = 1;
      });
  }

  fixHospiceWithoutFHIROrganization() {
    this.hospiceToolLoading = true;
    this.systemService
      .fixHospicesWithoutFHIR()
      .pipe(finalize(() => (this.hospiceToolLoading = false)))
      .subscribe(() => {
        this.toastService.showSuccess(`Hospices without FHIR organization created were fixed`);
        this.setInitialRequestResponse();
      });
  }

  netsuiteLogSelected(event) {
    this.selectedNetsuiteLog = event.currentRow;
    this.netsuiteDetailsViewOpen = true;
  }

  prepareAuditLogRequest() {
    const auditLogRequest: any = {
      apiLogType: this.selectedAuditType.name,
      continuationToken: this.auditLogResponse.continuationToken,
      pageSize: this.auditRequest.pageSize,
    };

    const {filtersMap} = this.auditRequest;

    if (!filtersMap) {
      return auditLogRequest;
    }
    if (filtersMap.hasOwnProperty('Modified On(range)')) {
      if (filtersMap['Modified On(range)'].value) {
        const [fromDate, toDate] = filtersMap['Modified On(range)'].value;
        if (fromDate && fromDate.value) {
          auditLogRequest.fromDate = fromDate?.value[0];
        }
        if (toDate && toDate.value) {
          auditLogRequest.toDate = toDate?.value[0];
        }
      }
    }

    if (filtersMap.hasOwnProperty('Action Type') && filtersMap['Action Type'].value) {
      auditLogRequest.actionType = filtersMap['Action Type'].value[0];

      if (auditLogRequest.actionType === '*') {
        delete auditLogRequest.actionType;
      }
    }

    if (filtersMap.hasOwnProperty('Modified By(Name)') && filtersMap['Modified By(Name)'].value) {
      auditLogRequest.userId = Number(filtersMap['Modified By(Name)'].value[0]?.userId);
    }

    if (filtersMap.hasOwnProperty('Target User') && filtersMap['Target User'].value) {
      auditLogRequest.entityId = Number(filtersMap['Target User'].value[0]?.userId);
    }
    return auditLogRequest;
  }

  fetchOrderHeaders(query) {
    const filterString = buildFilterString([
      {
        field: 'OrderNumber',
        value: [query],
        operator: SieveOperators.Contains,
      },
    ]);
    const orderHeaderReq = new SieveRequest();
    orderHeaderReq.filters = filterString;
    this.orderHeaderService
      .getAllOrderHeaders(orderHeaderReq)
      .subscribe((res: PaginationResponse) => {
        const list = res.records.map(oh => {
          return {
            entityId: oh.id,
            name: oh.orderNumber,
          };
        });
        this.orderheaderAuditLogsExtraFilterConf[0].value = list;
      });
  }

  searchUsers(searchQuery, label) {
    const searchUserRequest = new SieveRequest();
    const params = {...searchUserRequest, searchQuery};

    this.userService.searchUser(params).subscribe((res: PaginationResponse) => {
      if (res && res.records) {
        const list = res.records.map(u => {
          return {
            userId: u.id,
            name: u.name,
          };
        });
        const index = this.auditLogsFilterConfiguration.findIndex(c => c.label === label);
        this.auditLogsFilterConfiguration[index].value = list;
      }
    });
  }

  fetchTargetUsersForAuditLog(userIds) {
    const getUserRequest = new SieveRequest();
    const filterValues = [
      {
        field: 'id',
        operator: SieveOperators.Equals,
        value: userIds,
      },
    ];
    getUserRequest.filters = buildFilterString(filterValues);
    this.userService.getAllUsers(getUserRequest).subscribe((res: PaginationResponse) => {
      if (res.records) {
        res.records.forEach(r => {
          if (!r.targetUser) {
            const usersInAuditLogResponse = this.auditLogResponse.records.filter(
              u => u.entityId === r.id
            );
            usersInAuditLogResponse.forEach(u => {
              u.targetUser = r;
            });
          }
        });
      }
    });
  }

  fetchOrderNumberFromOrderId(orderIds) {
    const filterString = buildFilterString([
      {
        field: 'id',
        operator: SieveOperators.Equals,
        value: orderIds,
      },
    ]);
    const orderHeaderReq = new SieveRequest();
    orderHeaderReq.filters = filterString;
    this.orderHeaderService
      .getAllOrderHeaders(orderHeaderReq)
      .subscribe((res: PaginationResponse) => {
        if (res.records) {
          res.records.forEach(r => {
            const orderHeadersInAuditLogResponse = this.auditLogResponse.records.filter(
              o => o.entityId === r.id
            );
            orderHeadersInAuditLogResponse.forEach(o => {
              o.orderNumber = r.orderNumber;
            });
          });
        }
      });
  }

  prepareCoreApiLogRequest() {
    const coreApiLogRequest: any = {
      continuationToken: this.continuationToken,
      apiLogType: this.selectedAuditType,
      pageSize: 20,
    };

    if (this.apiLogFromDate) {
      coreApiLogRequest.fromDate = this.apiLogFromDate;
      coreApiLogRequest.toDate = this.apiLogToDate;
    }

    return coreApiLogRequest;
  }
}
