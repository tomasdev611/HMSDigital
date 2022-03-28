import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {finalize, mergeMap} from 'rxjs/operators';
import {
  AzureService,
  DispatchService,
  FeedbackService,
  HospiceLocationService,
  HospiceMemberService,
  OrderHeadersService,
  PatientService,
  SitesService,
  ToastService,
} from 'src/app/services';
import {
  IsPermissionAssigned,
  getEnum,
  buildFilterString,
  getUniqArray,
  displayOrderStatus,
  showRequiredFields,
  sortBy,
  getIsInternalUser,
  getDifference,
  formatDateToString,
  redirectToSCA,
} from 'src/app/utils';
import {
  BasePaginationReponse,
  ConfirmDialog,
  PaginationResponse,
  SieveRequest,
} from 'src/app/models';
import {ActivatedRoute, Router} from '@angular/router';
import {EnumNames, SieveOperators} from 'src/app/enums';
import {CreateOrderModalComponent, TableVirtualScrollComponent} from 'src/app/common';
import {forkJoin, Subscription} from 'rxjs';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {OAuthService} from 'angular-oauth2-oidc';
import {DatePipe, Location} from '@angular/common';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit, OnDestroy {
  @ViewChild('createordermodal') orderingModal: CreateOrderModalComponent;
  @ViewChild('openOrdersTable ', {static: false})
  openOrdersTable: TableVirtualScrollComponent;
  @ViewChild('completedOrdersTable ', {static: false})
  completedOrdersTable: TableVirtualScrollComponent;

  isInternalUser = getIsInternalUser();
  // order Details
  currentOrder: any;
  allOrderHeaderCount = 0;
  allPatients = [];
  detailsViewOpen = false;
  baseOrderHeaders = [
    {
      label: 'Order #',
      field: 'orderNumber',
      sortable: true,
      sortField: 'orderNumber',
      addOnTdIcon: this.isInternalUser ? 'pi pi-circle-on p-ml-1 red' : '',
      addOnTdIconTooltip: this.isInternalUser ? 'Exception Fulfillment' : '',
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
      label: 'Status',
      field: 'orderStatus',
      sortable: true,
      sortField: 'statusId',
    },
    {
      label: 'Hospice Location',
      field: 'hospiceLocation',
    },
  ];
  openOrderHeaders = [...this.baseOrderHeaders];
  completedOrderHeaders = [
    ...this.baseOrderHeaders,
    {
      label: 'Completed Date',
      field: 'fulfillmentEndDateTime',
      sortable: true,
      sortField: 'fulfillmentEndDateTime',
      fieldType: 'Date',
    },
  ];
  orderHeadersResponse: PaginationResponse;
  orderHeadersFilter = new SieveRequest();
  loading = false;
  orderView = 'open';
  completedOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Completed'
  )?.id;
  cancelledOrderStatusId = getEnum(EnumNames.OrderHeaderStatusTypes).find(
    x => x.name === 'Cancelled'
  )?.id;
  personalAddressTypeId = getEnum(EnumNames.AddressTypes).find(x => x.name === 'Home')?.id;
  approverList: any;
  orderFilters = [];
  orderSearchText = '';
  orderPatientUUIDSearch = [];
  subscriptions: Subscription[] = [];
  fulfilledItems = null;
  totalCurrentRow = 0;
  hospices: PaginationResponse;
  filterString = '';

  feedbackOpen = false;
  feedbackForm: FormGroup;
  formSubmit = false;
  user: any = this.oAuthService.getIdentityClaims();
  @ViewChild('confirmDialog', {static: false}) confirmDialog;
  confirmData = new ConfirmDialog();
  hospiceLocations = [];
  allHospiceLocationsList = [];
  orderTypes = getEnum(EnumNames.OrderTypes);
  patientMoveId = this.orderTypes.find(x => x.name === 'Patient_Move');
  pickupId = this.orderTypes.find(x => x.name === 'Pickup');
  refreshTimer: any;

  constructor(
    private oAuthService: OAuthService,
    private orderHeaderService: OrderHeadersService,
    private hospiceMemberService: HospiceMemberService,
    private patientService: PatientService,
    private route: ActivatedRoute,
    private navbarSearchService: NavbarSearchService,
    private sitesService: SitesService,
    private hospiceLocationService: HospiceLocationService,
    private fb: FormBuilder,
    private toastService: ToastService,
    private feedbackService: FeedbackService,
    private dispatchService: DispatchService,
    private azureService: AzureService,
    private datePipe: DatePipe,
    private location: Location,
    private router: Router
  ) {
    this.setFeedbackForm();
  }

  setFeedbackForm() {
    this.feedbackForm = this.fb.group({
      email: new FormControl(this.user?.email, Validators.required),
      name: new FormControl(this.user?.name, Validators.required),
      subject: new FormControl(null, Validators.required),
      comments: new FormControl(null, Validators.required),
      type: new FormControl(null, Validators.required),
      hospiceLocation: new FormControl(null, Validators.required),
    });
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.navbarSearchService.search.subscribe(searchText => this.searchOrder(searchText))
    );

    this.route.queryParams.subscribe(params => {
      this.orderView = params.view || this.orderView || 'open';
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.orderView}).toString()
      );
    });

    this.getAllOrderHeaders();

    const approveActionBtn = {
      label: '',
      field: '',
      class: 'sm',
      conditionalBtn: 'Approve',
      conditionalField: 'ext_shouldShowApproveBtn',
    };

    const hasOrderApprovePermission = IsPermissionAssigned('Orders', 'Approve');
    if (hasOrderApprovePermission) {
      this.openOrderHeaders = [...this.openOrderHeaders, approveActionBtn];
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  addExceptionFulfillmentFilters(field, operator) {
    if (!this.isInternalUser) {
      const filter = {
        field,
        value: [true],
        operator,
      };
      this.orderFilters = [...this.orderFilters, filter];
    }
  }

  getAllOrderHeaders(silentRefresh = false) {
    if (!silentRefresh) {
      this.loading = true;
    }
    if (this.refreshTimer) {
      clearTimeout(this.refreshTimer);
    }
    switch (this.orderView) {
      case 'open':
        this.orderFilters = [
          {
            field: 'statusId',
            value: [this.completedOrderStatusId],
            operator: SieveOperators.NotEquals,
          },
          {
            field: 'statusId',
            value: [this.cancelledOrderStatusId],
            operator: SieveOperators.NotEquals,
          },
        ];
        this.addExceptionFulfillmentFilters('isExceptionFulfillment', SieveOperators.NotEquals);
        break;
      case 'completed':
        if (this.isInternalUser) {
          this.orderFilters = [
            {
              field: 'statusId',
              value: [this.completedOrderStatusId, this.cancelledOrderStatusId],
              operator: SieveOperators.Equals,
            },
          ];
        } else {
          this.orderFilters = [];
          this.addExceptionFulfillmentFilters(
            'completedOrdersWithException',
            SieveOperators.Equals
          );
        }
        break;
    }

    if (this.orderSearchText) {
      this.orderFilters.push({
        field: 'OrderNumber',
        value: [this.orderSearchText],
        operator: SieveOperators.CI_Contains,
      });
    }

    this.orderFilters.push({
      field: 'PatientUUID',
      value: this.orderPatientUUIDSearch,
      operator: SieveOperators.Equals,
    });
    this.orderHeadersFilter.filters = buildFilterString(this.orderFilters);
    if (this.filterString) {
      this.orderHeadersFilter.filters = this.orderHeadersFilter.filters + ',' + this.filterString;
    }
    const requests = [this.orderHeaderService.getAllOrderHeaders(this.orderHeadersFilter)];
    if (!this.approverList) {
      requests.push(this.hospiceMemberService.getApproverList());
    }
    forkJoin(requests)
      .pipe(
        mergeMap((responses: any) => {
          const [orderRes, approversRes, hospiceResponse] = responses;

          // append newly created order if not present in records
          if (this.orderView === 'open' && this.orderHeadersFilter.page === 1) {
            let newOrder: any = sessionStorage.getItem('newOrder');
            newOrder = newOrder ? JSON.parse(newOrder) : null;
            if (newOrder && newOrder?.action && newOrder?.action !== 'edit') {
              const orderIdx = orderRes.records.findIndex(
                x =>
                  x.patientUuid === newOrder.patientUuid &&
                  getDifference(newOrder.orderDateTime, x.orderDateTime, 'seconds') < 60 &&
                  this.checkItemsMatch(newOrder.orderLineItems, x.orderLineItems)
              );
              if (orderIdx === -1) {
                orderRes.records = [newOrder, ...orderRes.records];
                this.triggerOrderRefresh();
              } else {
                sessionStorage.removeItem('newOrder');
              }
            }
          }

          this.orderHeadersResponse = orderRes;
          this.totalCurrentRow = this.orderHeadersResponse.pageSize;
          if (!this.approverList && approversRes) {
            this.approverList = approversRes;
          }
          const patientUuids = getUniqArray(
            this.orderHeadersResponse.records.map(r => r.patientUuid)
          );
          const hospiceLocationIds = getUniqArray(
            this.orderHeadersResponse.records.map(r => r.hospiceLocationId)
          );
          this.loadHospiceLocations(hospiceLocationIds);
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
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((patientsResponse: any) => {
        this.allPatients = patientsResponse.records;
        this.allPatients.map(p => {
          p.name = `${p.firstName} ${p.lastName}`;
          return p;
        });
        const orderIds = this.orderHeadersResponse?.records.flatMap((x: any) => {
          return x.orderStatus === 'Enroute' ? [x.id] : [];
        });
        if (orderIds.length) {
          this.getOrdersCurrentLocation(orderIds);
        }
        this.orderHeadersResponse.records.map(o => {
          o.patient = this.allPatients.find(p => p.uniqueId === o.patientUuid);
          // append hospicelocationId if not available
          if (o.patient && !o.hospiceLocationId) {
            o.hospiceLocationId = o.patient?.hospiceLocationId;
          }
          o.ext_shouldShowApproveBtn =
            o.orderStatus === 'Pending Approval' &&
            (this.approverList.hospiceIds.includes(o.hospiceId) ||
              this.approverList.hospiceLocationIds.includes(o.hospiceLocationId));
          o.orderStatus = displayOrderStatus(o.orderStatus);
          if (o.isExceptionFulfillment && !this.isInternalUser) {
            o.orderStatus = 'Completed';
          }
          return o;
        });
      });
  }

  triggerOrderRefresh() {
    this.refreshTimer = setTimeout(() => {
      this.getAllOrderHeaders(true);
    }, 20000);
  }

  checkItemsMatch(newlineItems, oldLineItems) {
    let matched = true;
    newlineItems.map(x => {
      const itemIndex = oldLineItems.findIndex(
        oi => oi.item?.netSuiteItemId === +x.internalitemId && oi.itemCount === x.itemCount
      );
      if (itemIndex === -1) {
        matched = false;
      }
    });
    return matched;
  }

  getOrdersCurrentLocation(orderIds) {
    this.dispatchService.getDispatchOrderLocations(orderIds).subscribe((response: any) => {
      response.map(x => {
        const order = this.orderHeadersResponse.records.find(o => o.id === x.orderId);
        if (order) {
          const from = [x.latitude, x.longitude];
          const toAddress =
            order.orderTypeId === this.patientMoveId || order.orderTypeId === this.pickupId
              ? order.pickupAddress
              : order.deliveryAddress;
          if (toAddress?.latitude && toAddress.longitude) {
            const to = [toAddress.latitude, toAddress.longitude];
            this.azureService.getRouteDirection(from, to).subscribe(
              (result: any) => {
                const etaInSeconds = result?.routes[0]?.summary?.travelTimeInSeconds ?? 0;
                if (etaInSeconds) {
                  const date = new Date();
                  date.setSeconds(date.getSeconds() + etaInSeconds);
                  const etaTime = this.datePipe.transform(date, 'h:mm a');
                  order.orderStatus = order.orderStatus + ' (ETA - ' + etaTime + ')';
                }
              },
              _ => {}
            );
          }
        }
      });
    });
  }

  loadHospiceLocations(hospiceLocationIds = []) {
    const filters = [
      {
        field: 'id',
        operator: SieveOperators.Equals,
        value: hospiceLocationIds,
      },
    ];
    const request = {
      filters: buildFilterString(filters),
    };
    this.hospiceLocationService.getLocations(request).subscribe((hospices: any) => {
      this.hospiceLocations = hospices?.records?.map(hl => {
        return {label: hl.name, value: hl.name};
      });
      this.orderHeadersResponse?.records.map(x => {
        const hospice = hospices?.records?.find((h: any) => h.id === x.hospiceLocationId);
        if (hospice) {
          x.hospiceLocation = hospice.name;
        }
      });
    });
  }

  fetchNext({pageNum}) {
    if (!this.orderHeadersResponse || pageNum > this.orderHeadersResponse.totalPageCount) {
      return;
    }
    this.orderHeadersFilter.page = pageNum;
    this.getAllOrderHeaders();
  }

  onTabChange(event) {
    this.orderHeadersResponse = null;
    this.closeOrderDetails();
    this.orderView = '';
    switch (event.index) {
      case 0: {
        this.orderView = 'open';
        break;
      }
      case 1: {
        this.orderView = 'completed';
        break;
      }
    }
    if (this.orderView) {
      this.dataTablesReset();
      this.getAllOrderHeaders();
      this.location.replaceState(
        window.location.pathname,
        new URLSearchParams({view: this.orderView}).toString()
      );
    }
  }

  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }

  orderSelected(event) {
    this.currentOrder = event.currentRow;
    this.getSiteInfo();
    if (this.currentOrder.id) {
      this.getOrderDetailInformation();
    }
    this.detailsViewOpen = true;
  }

  getOrderDetailInformation() {
    this.fulfilledItems = [];
    this.orderHeaderService
      .getOrderHeaderById(this.currentOrder.id, true)
      .subscribe((response: any) => {
        this.currentOrder.orderNotes = response?.orderNotes ?? [];
        this.currentOrder.nurse = response?.orderingNurse ?? '';
        this.fulfilledItems = response?.orderFulfillmentLineItems ?? [];
        if (!this.isInternalUser && response?.isExceptionFulfillment) {
          response?.orderLineItems.map(x => {
            const itemIndex = this.fulfilledItems.findIndex(i => i.orderLineItemId === x.id);
            if (itemIndex === -1) {
              x.orderLineItemId = x.id;
              x.quantity = x.itemCount;
              x.assetTag = x.assetTagNumber;
              x.fulfillmentEndDateTime = this.fulfilledItems[0]?.fulfillmentEndDateTime ?? null;
              this.fulfilledItems = [...this.fulfilledItems, x];
            }
          });
        }
        this.currentOrder.createdByUser = response?.createdByUser ?? '';
        this.currentOrder.modifiedByUser = response?.modifiedByUser ?? '';
        this.currentOrder.assignedDriver = response?.assignedDriver ?? '';
        this.currentOrder.orderNotes = sortBy(
          this.currentOrder.orderNotes,
          'createdDateTime',
          'date'
        );
      });
    this.patientService
      .getPatientNotes(this.currentOrder.patient?.id)
      .subscribe((response: any) => {
        response = sortBy(response, 'createdDateTime', 'date');
        this.currentOrder.patientNotes = response;
      });
  }
  getSiteInfo() {
    if (this.checkPermission('Site', 'Read')) {
      this.sitesService.searchSites({searchQuery: ''}).subscribe(res => {
        const site = res.records.find(x => x.id === this.currentOrder.siteId);
        this.currentOrder.site = site;
      });
    }
  }

  closeOrderDetails(event?) {
    this.currentOrder = null;
    this.detailsViewOpen = false;
  }

  sortOrders(event) {
    switch (event.order) {
      case 0:
        this.orderHeadersFilter.sorts = '';
        break;
      case 1:
        this.orderHeadersFilter.sorts = event.field;
        break;
      case -1:
        this.orderHeadersFilter.sorts = '-' + event.field;
        break;
    }
    this.dataTablesReset();
    this.getAllOrderHeaders();
  }

  proceedToCreateOrder({orderType, patient}) {
    if (patient?.status === 'Inactive') {
      this.orderingModal.closeCreateOrderModal();
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
        orderType,
      };
      this.confirmDialog.showDialog(this.confirmData);
    } else {
      const redirectUrl = `/orders/sca/${orderType}/${patient.uniqueId}`;
      redirectToSCA(redirectUrl);
    }
  }

  confirmAccepted() {
    this.confirmDialog.close();
    const redirectUrl = `/orders/sca/${this.confirmData.data.orderType}/${this.confirmData.data.patient.uniqueId}`;
    redirectToSCA(redirectUrl);
  }

  openCreateOrderModal() {
    this.orderingModal.showOrdering();
  }

  navigateToApproveOrder({object}) {
    const redirectUrl = `/dashboard/approve-orders/${object.netSuiteOrderId}`;
    redirectToSCA(redirectUrl);
  }

  searchOrder(text) {
    this.dataTablesReset();
    this.orderHeadersFilter.page = 1;
    this.orderSearchText = '';
    this.orderPatientUUIDSearch = [];
    if (text === '') {
      this.getAllOrderHeaders();
    } else {
      const regExp = new RegExp('.*[0-9].*');
      if (regExp.test(text)) {
        this.orderSearchText = text;
        this.getAllOrderHeaders();
      } else if (text.length >= 3) {
        this.patientService
          .searchPatientsBySearchQuery({searchQuery: text})
          .subscribe((response: PaginationResponse) => {
            this.orderPatientUUIDSearch = response.records.map(record => record.uniqueId);
            if (this.orderPatientUUIDSearch.length) {
              this.getAllOrderHeaders();
            } else {
              this.orderHeadersResponse = new BasePaginationReponse();
              this.orderHeadersResponse.pageNumber = 1;
            }
          });
      }
    }
  }

  dataTablesReset() {
    if (this.openOrdersTable) {
      this.openOrdersTable.reset();
    }
    if (this.completedOrdersTable) {
      this.completedOrdersTable.reset();
    }
    this.orderHeadersFilter.page = 1;
  }

  filterOrders(filterString) {
    this.orderHeadersFilter.page = 1;
    this.filterString = filterString;
    this.getAllOrderHeaders();
  }

  closeFeedbackFlyout() {
    this.feedbackOpen = false;
  }
  openFeedbackFlyout() {
    this.feedbackOpen = true;
    if (!this.allHospiceLocationsList?.length) {
      this.hospiceLocationService.getLocations().subscribe((hospices: any) => {
        this.allHospiceLocationsList = hospices?.records?.map(hl => {
          return {label: hl.name, value: hl.name};
        });
      });
    }
    this.setFeedbackForm();
    if (this.hospiceLocations.length === 1) {
      this.feedbackForm.controls.hospiceLocation.setValue(this.hospiceLocations[0].value);
    }
  }
  onSubmitFeedback(feedback) {
    this.formSubmit = true;
    this.feedbackService
      .submitFeedback(feedback)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.toastService.showSuccess(`Your feedback has been submitted successfully`);
        this.closeFeedbackFlyout();
      });
  }

  checkFormValidity(form) {
    return showRequiredFields(form, 'FeedbackForm');
  }

  confirmRejected() {
    this.confirmDialog.close();
  }

  reloadOrders() {
    this.dataTablesReset();
    this.orderHeadersFilter = new SieveRequest();
    this.getAllOrderHeaders();
  }
}
