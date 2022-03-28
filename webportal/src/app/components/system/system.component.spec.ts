import {HttpClientTestingModule} from '@angular/common/http/testing';
import {waitForAsync, ComponentFixture, TestBed} from '@angular/core/testing';
import {TransferState} from '@angular/platform-browser';
import {RouterTestingModule} from '@angular/router/testing';
import {OAuthService, OAuthModule} from 'angular-oauth2-oidc';
import {ConfirmationService, MessageService} from 'primeng/api';
import {
  SystemService,
  PatientService,
  AuditService,
  HospiceMemberService,
  SitesService,
  OrderHeadersService,
} from 'src/app/services';
import {SystemComponent} from './system.component';
import {ConfirmDialogComponent} from 'src/app/common/confirm-dialog/confirm-dialog.component';
import {
  BaseContinuationTokenResponse,
  BasePaginationReponse,
  PaginationResponse,
} from 'src/app/models';
import {BehaviorSubject, of} from 'rxjs';

describe('SystemComponent', () => {
  let component: SystemComponent;
  let fixture: ComponentFixture<SystemComponent>;
  let systemService: any;
  let patientService: any;
  let auditService: any;
  let hospiceMemberService: any;
  let sitesService: any;
  let orderHeaderService: any;

  beforeEach(
    waitForAsync(() => {
      const systemServiceStub = jasmine.createSpyObj('SystemService', [
        'getUsersWithoutEmail',
        'getMissingIdentityCount',
        'fixMissingIdentityUsers',
        'getNonVerifiedAddress',
        'fixNonVerifiedAddress',
        'fixNonVerifiedAddresses',
        'getCountWithMissingNetSuiteContact',
        'fixMembersWithMissingNetSuiteContact',
        'getInternalUsersCountWithMissingNetSuiteContact',
        'fixInternalUsersWithMissingNetSuiteContact',
        'getUnconfirmedOrders',
        'fixUnconfirmedOrders',
        'fixUnconfirmedOrder',
        'getOrdersWithoutAssignedSites',
        'fixOrdersWithoutAssignedSites',
        'getOrdersWithoutStatus',
        'fixOrderWithoutStatus',
        'getNetsuiteLogs',
        'getNetsuiteLogDetail',
        'getApiLogs',
        'getDispatchOrders',
        'getDeletedPatientInventory',
        'getInactivePatientsWithConsumablesOnly',
        'fixInactivePatientsWithConsumablesOnly',
        'getPatientsWithInvalidStatus',
        'fixPatientWithInvalidStatus',
      ]);
      systemServiceStub.getUsersWithoutEmail.and.returnValue(
        new BehaviorSubject<PaginationResponse>(paginationResponse)
      );
      systemServiceStub.getMissingIdentityCount.and.returnValue(
        new BehaviorSubject<PaginationResponse>(basePaginationReponse)
      );
      systemServiceStub.fixMissingIdentityUsers.and.returnValue(new BehaviorSubject(0));
      systemServiceStub.getNonVerifiedAddress.and.returnValue(
        new BehaviorSubject<PaginationResponse>(paginationResponse)
      );
      systemServiceStub.fixNonVerifiedAddress.and.returnValue(new BehaviorSubject(true));
      systemServiceStub.fixNonVerifiedAddresses.and.returnValue(new BehaviorSubject(0));
      systemServiceStub.getCountWithMissingNetSuiteContact.and.returnValue(
        new BehaviorSubject<PaginationResponse>(basePaginationReponse)
      );
      systemServiceStub.fixMembersWithMissingNetSuiteContact.and.returnValue(
        new BehaviorSubject(0)
      );
      systemServiceStub.getInternalUsersCountWithMissingNetSuiteContact.and.returnValue(
        new BehaviorSubject<PaginationResponse>(basePaginationReponse)
      );
      systemServiceStub.fixInternalUsersWithMissingNetSuiteContact.and.returnValue(
        new BehaviorSubject(0)
      );
      systemServiceStub.getUnconfirmedOrders.and.returnValue(
        new BehaviorSubject<PaginationResponse>(basePaginationReponse)
      );
      systemServiceStub.fixUnconfirmedOrders.and.returnValue(new BehaviorSubject(0));
      systemServiceStub.fixUnconfirmedOrder.and.returnValue(new BehaviorSubject(true));
      systemServiceStub.getOrdersWithoutAssignedSites.and.returnValue(
        new BehaviorSubject<PaginationResponse>(basePaginationReponse)
      );
      systemServiceStub.fixOrdersWithoutAssignedSites.and.returnValue(new BehaviorSubject(0));
      systemServiceStub.getOrdersWithoutStatus.and.returnValue(of(orderResponse));
      systemServiceStub.fixOrderWithoutStatus.and.returnValue(new BehaviorSubject(0));
      systemServiceStub.getNetsuiteLogs.and.returnValue(new BehaviorSubject(netSuitLogsResponse));
      systemServiceStub.getNetsuiteLogDetail.and.returnValue(
        new BehaviorSubject(netsuiteLogDetail)
      );
      systemServiceStub.getApiLogs.and.returnValue(new BehaviorSubject(apiLogsResponse));
      systemServiceStub.getDispatchOrders.and.returnValue(
        new BehaviorSubject(dispatchOrderResponse)
      );
      systemServiceStub.getDeletedPatientInventory.and.returnValue(
        new BehaviorSubject(deletedPatientInventoryResponse)
      );
      systemServiceStub.getInactivePatientsWithConsumablesOnly.and.returnValue(
        new BehaviorSubject(patientUuidsResponse)
      );
      systemServiceStub.fixInactivePatientsWithConsumablesOnly.and.returnValue(
        new BehaviorSubject(patientInventory)
      );
      systemServiceStub.getPatientsWithInvalidStatus.and.returnValue(
        new BehaviorSubject(patientUuidsResponse)
      );
      systemServiceStub.fixPatientWithInvalidStatus.and.returnValue(new BehaviorSubject(true));

      const patientServiceStub = jasmine.createSpyObj('PatientService', ['getPatients']);
      patientServiceStub.getPatients.and.returnValue(
        new BehaviorSubject<PaginationResponse>(patientResponse)
      );

      const auditServiceStub = jasmine.createSpyObj('AuditService', ['getAuditLogs']);
      auditServiceStub.getAuditLogs.and.returnValue(
        new BehaviorSubject<PaginationResponse>(paginationResponse)
      );

      const hospiceMemberServiceStub = jasmine.createSpyObj('HospiceMemberService', [
        'getAllHospiceMembers',
      ]);
      hospiceMemberServiceStub.getAllHospiceMembers.and.returnValue(
        new BehaviorSubject<PaginationResponse>(hospiceMemberServiceResponse)
      );

      const sitesServiceStub = jasmine.createSpyObj('SitesService', ['getSiteById']);
      sitesServiceStub.getSiteById.and.returnValue(new BehaviorSubject(sitesResponse));

      const orderHeaderServiceStub = jasmine.createSpyObj('OrderHeadersService', [
        'getOrderFulfillment',
      ]);
      orderHeaderServiceStub.getOrderFulfillment.and.returnValue(
        new BehaviorSubject<PaginationResponse>(orderFulfillmentResponse)
      );

      TestBed.configureTestingModule({
        declarations: [SystemComponent, ConfirmDialogComponent],
        imports: [RouterTestingModule, HttpClientTestingModule, OAuthModule.forRoot()],
        providers: [
          {
            provide: OAuthService,
          },
          {
            provide: SystemService,
            useValue: systemServiceStub,
          },
          {
            provide: PatientService,
            useValue: patientServiceStub,
          },
          {
            provide: AuditService,
            useValue: auditServiceStub,
          },
          {
            provide: HospiceMemberService,
            useValue: hospiceMemberServiceStub,
          },
          {
            provide: SitesService,
            useValue: sitesServiceStub,
          },
          {
            provide: OrderHeadersService,
            useValue: orderHeaderServiceStub,
          },
          MessageService,
          TransferState,
          ConfirmationService,
        ],
      }).compileComponents();
      systemService = TestBed.inject(SystemService);
      patientService = TestBed.inject(PatientService);
      auditService = TestBed.inject(AuditService);
      hospiceMemberService = TestBed.inject(HospiceMemberService);
      sitesService = TestBed.inject(SitesService);
      orderHeaderService = TestBed.inject(OrderHeadersService);
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    spyOn(component, 'getIdentityMissingUsers').and.callThrough();
    spyOn(component, 'getNonVerifiedAddresses').and.callThrough();
    spyOn(component, 'getMembersWithoutNetsuiteContactId').and.callThrough();
    spyOn(component, 'getInternalUsersWithoutNetsuiteContactId').and.callThrough();
    spyOn(component, 'getUnConfirmedOrderFulfillments').and.callThrough();
    spyOn(component, 'getOrdersWithoutSites').and.callThrough();
    spyOn(component, 'getOrdersWithoutStatuses').and.callThrough();
    spyOn(component, 'getNurse').and.callThrough();
    spyOn(component, 'getOrderFulfillment').and.callThrough();
    spyOn(component, 'getSite').and.callThrough();
    spyOn(component, 'setInitialRequestResponse').and.callThrough();
    spyOn(component, 'getPatients').and.callThrough();
    spyOn(component, 'getInactivePatientsWithOnlyConsumables').and.callThrough();
    spyOn(component, 'getPatientWithInvalidStatus').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call `getUsersWithoutEmail` on getUsers and match the result', () => {
    component.getUsers();
    expect(systemService.getUsersWithoutEmail).toHaveBeenCalled();
    expect(component.paginationResponse).toEqual(paginationResponse);
  });

  it('should call `getMissingIdentityCount` on getIdentityMissingUsers and match the result', () => {
    component.getIdentityMissingUsers();
    expect(systemService.getMissingIdentityCount).toHaveBeenCalled();
    expect(component.paginationResponse.totalRecordCount).toEqual(0);
  });

  it('should call `fixMissingIdentityUsers` on fixIdentityMissingUsers and match the result', () => {
    component.fixIdentityMissingUsers();
    expect(systemService.fixMissingIdentityUsers).toHaveBeenCalled();
    expect(component.getIdentityMissingUsers).toHaveBeenCalled();
  });

  it('should call `getNonVerifiedAddress` on getNonVerifiedAddresses and match the result', () => {
    component.getNonVerifiedAddresses();
    expect(systemService.getNonVerifiedAddress).toHaveBeenCalled();
    expect(component.paginationResponse).toEqual(paginationResponse);
  });

  it('should call `fixNonVerifiedAddresses` on fixNonVerifiedAddresses and match the result', () => {
    component.fixNonVerifiedAddresses();
    expect(systemService.fixNonVerifiedAddresses).toHaveBeenCalled();
    expect(component.getNonVerifiedAddresses).toHaveBeenCalled();
  });

  it('should call `getCountWithMissingNetSuiteContact` on getMembersWithoutNetsuiteContactId and match the result', () => {
    component.getMembersWithoutNetsuiteContactId();
    expect(systemService.getCountWithMissingNetSuiteContact).toHaveBeenCalled();
    expect(component.paginationResponse.totalRecordCount).toEqual(0);
  });

  it('should call `fixMembersWithMissingNetSuiteContact` on fixMembersWithoutNetsuiteId and match the result', () => {
    component.fixMembersWithoutNetsuiteId();
    expect(systemService.fixMembersWithMissingNetSuiteContact).toHaveBeenCalled();
    expect(component.getMembersWithoutNetsuiteContactId).toHaveBeenCalled();
  });

  it('should call `getInternalUsersCountWithMissingNetSuiteContact` on getInternalUsersWithoutNetsuiteContactId', () => {
    component.getInternalUsersWithoutNetsuiteContactId();
    expect(systemService.getInternalUsersCountWithMissingNetSuiteContact).toHaveBeenCalled();
    expect(component.paginationResponse.totalRecordCount).toEqual(0);
  });

  it('should call `fixInternalUsersWithMissingNetSuiteContact` on fixInternalUsersWithoutNetsuiteId and match the result', () => {
    component.fixInternalUsersWithoutNetsuiteId();
    expect(systemService.fixInternalUsersWithMissingNetSuiteContact).toHaveBeenCalled();
    expect(component.getInternalUsersWithoutNetsuiteContactId).toHaveBeenCalled();
  });

  it('should call `getUnconfirmedOrders` on getUnConfirmedOrderFulfillments and match the result', () => {
    component.getUnConfirmedOrderFulfillments();
    expect(systemService.getUnconfirmedOrders).toHaveBeenCalled();
    expect(component.paginationResponse.totalRecordCount).toEqual(0);
  });

  it('should call `fixUnconfirmedOrders` on fixUnConfirmedOrderFulfillments and match the result', () => {
    component.fixUnConfirmedOrderFulfillments();
    expect(systemService.fixUnconfirmedOrders).toHaveBeenCalled();
    expect(component.getUnConfirmedOrderFulfillments).toHaveBeenCalled();
  });

  it('should call `getOrdersWithoutAssignedSites` on getOrdersWithoutSites and match the result', () => {
    component.getOrdersWithoutSites();
    expect(systemService.getOrdersWithoutAssignedSites).toHaveBeenCalled();
    expect(component.paginationResponse.totalRecordCount).toEqual(0);
  });

  it('should call `fixOrdersWithoutAssignedSites` on fixOrdersWithoutAssignedSites and match the result', () => {
    component.fixOrdersWithoutAssignedSites();
    expect(systemService.fixOrdersWithoutAssignedSites).toHaveBeenCalled();
    expect(component.getOrdersWithoutSites).toHaveBeenCalled();
  });

  it('should call `getOrdersWithoutStatus` on getOrdersWithoutStatuses and match the result', () => {
    component.getOrdersWithoutStatuses();
    expect(systemService.getOrdersWithoutStatus).toHaveBeenCalled();
    expect(component.paginationResponse).toEqual(orderResponse);
    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.allPatients).toEqual(patientResponse.records);
  });

  it('should call `fixOrderWithoutStatus` on fixOrderStatusConfirmed and match the result', () => {
    component.selectedOrder = orderRecord;
    component.fixOrderStatusConfirmed();
    expect(systemService.fixOrderWithoutStatus).toHaveBeenCalled();
    expect(component.getOrdersWithoutStatuses).toHaveBeenCalled();
  });

  it('should call `getAuditLogs` on getAuditLogs and match the result', () => {
    component.auditRequest = component.getDefaultAuditRequest();
    component.selectedAuditType = {name: 'Users'};
    component.auditLogResponse = new BaseContinuationTokenResponse();
    component.getAuditLogs();
    expect(auditService.getAuditLogs).toHaveBeenCalled();
    expect(component.auditLogResponse).toEqual(continuationTokenResponse);
  });

  it('should call `getNetsuiteLogs` on getNetsuiteLogs and match the result', () => {
    component.netsuiteRequest = {
      pageNumber: 1,
      pageSize: 25,
    };
    component.getNetsuiteLogs();
    expect(systemService.getNetsuiteLogs).toHaveBeenCalled();
  });

  it('should call orderSelected and match the result', () => {
    component.orderSelected(orderSelectedEvent);
    expect(component.getNurse).toHaveBeenCalled();
    expect(component.getOrderFulfillment).toHaveBeenCalled();
    expect(component.getSite).toHaveBeenCalled();
  });

  it('should call `getAllHospiceMembers` on getNurse and match the result', () => {
    component.currentOrder = orderSelectedEvent.currentRow;
    component.getNurse(orderRecord);
    expect(hospiceMemberService.getAllHospiceMembers).toHaveBeenCalled();
  });

  it('should call `getSiteById` on getSite and match the result', () => {
    component.currentOrder = orderSelectedEvent.currentRow;
    component.getSite(orderRecord.siteId);
    expect(sitesService.getSiteById).toHaveBeenCalled();
    expect(component.currentOrder.site).toEqual(sitesResponse);
  });

  it('should call `getOrderFulfillment` on getOrderFulfillment and match the result', () => {
    component.getOrderFulfillment(orderRecord.id);
    expect(orderHeaderService.getOrderFulfillment).toHaveBeenCalled();
    expect(component.fulfilledItems).toEqual(orderFulfillmentResponse.records);
  });

  it('should call `fixOrderWithoutStatus` of SystemService on previewOrderStatus and match the result', () => {
    component.previewOrderStatus(orderRecord);
    expect(systemService.fixOrderWithoutStatus).toHaveBeenCalled();
  });

  it('should call `fixUnconfirmedOrder` of SystemService on fixUnConfirmedOrderFulfillment and match the result', () => {
    component.fixUnConfirmedOrderFulfillment(orderRecord);
    expect(systemService.fixUnconfirmedOrder).toHaveBeenCalled();
    expect(component.setInitialRequestResponse).toHaveBeenCalled();
    expect(component.getUnConfirmedOrderFulfillments).toHaveBeenCalled();
  });

  it('should call `fixNonVerifiedAddress` of SystemService on fixNonVerifiedAddress and match the result', () => {
    component.fixNonVerifiedAddress(address);
    expect(systemService.fixNonVerifiedAddress).toHaveBeenCalled();
    expect(component.setInitialRequestResponse).toHaveBeenCalled();
    expect(component.getNonVerifiedAddresses).toHaveBeenCalled();
  });

  it('should call `getApiLogs` of SystemService on fetchApiLogs and match the result', () => {
    component.fetchApiLogs();
    expect(systemService.getApiLogs).toHaveBeenCalled();
    expect(component.coreApiLogsResponse.hasOwnProperty('records')).toBeTruthy();
    expect(component.coreApiLogsResponse.hasOwnProperty('continuationToken')).toBeTruthy();
  });

  it('should call `getDispatchOrders` of SystemService on getDispatchOrders and match the result', () => {
    component.getDispatchOrders();
    expect(systemService.getDispatchOrders).toHaveBeenCalled();
    expect(component.dispatchOrderResponse).toEqual(dispatchOrderResponse);
    expect(component.getPatients).toHaveBeenCalled();
  });

  it('should call `getPatientInventoryWithIssues` of SystemService on fetchPatientInventoryWithIssues and match the result', () => {});

  it('should call `fixPatientInventoryWithIssues` of SystemService on fixPatientInventory and match the result', () => {});

  it('should call `getDeletedPatientInventory` of SystemService on fetchDeletedPatientInventory and match the result', () => {
    component.fetchDeletedPatientInventory({isAssetTagged: true});
    expect(systemService.getDeletedPatientInventory).toHaveBeenCalled();
    expect(component.paginationResponse).toEqual(deletedPatientInventoryResponse);
    expect(component.getPatients).toHaveBeenCalled();
  });

  it('should call `getPatients` of SystemService on getPatients and match the result', () => {
    component.activeTableView = 'inactivePatientWithConsumable';
    const patientUuids = ['ea73a27d-9c5d-4e8d-aa30-4e392817bd30'];
    component.paginationResponse = {
      pageNumber: 1,
      pageSize: 25,
      records: [],
      totalPageCount: 1,
      totalRecordCount: 0,
    };
    component.getPatients(patientUuids);
    expect(patientService.getPatients).toHaveBeenCalled();
    expect(component.paginationResponse.records).toEqual(patientResponse.records);
  });

  it('should call `getInactivePatientsWithConsumablesOnly` of SystemService on getInactivePatientsWithOnlyConsumables', () => {
    component.paginationResponse = {
      pageNumber: 1,
      pageSize: 25,
      records: [],
      totalPageCount: 1,
      totalRecordCount: 0,
    };
    component.getInactivePatientsWithOnlyConsumables();
    expect(systemService.getInactivePatientsWithConsumablesOnly).toHaveBeenCalled();
    expect(component.paginationResponse.totalRecordCount).toEqual(patientUuidsResponse.length);
    expect(component.paginationResponse.pageNumber).toEqual(1);
    expect(component.getPatients).toHaveBeenCalled();
  });

  it('should call `fixInactivePatientsWithConsumablesOnly` of SystemService on previewInactivePatientWithConsumable', () => {
    component.previewInactivePatientWithConsumable(patientRecord);
    expect(systemService.fixInactivePatientsWithConsumablesOnly).toHaveBeenCalled();
    expect(component.selectedPatientInventory).toEqual(patientInventory);
    expect(component.showModalPatientInventoryConsumable).toEqual(true);
  });

  it('should call `fixInactivePatientsWithConsumablesOnly` of SystemService on fixPatientConsumableConfirmed and match the result', () => {
    component.selectedPatient = patientRecord;
    component.fixPatientConsumableConfirmed();
    expect(systemService.fixInactivePatientsWithConsumablesOnly).toHaveBeenCalled();
    expect(component.setInitialRequestResponse).toHaveBeenCalled();
    expect(component.getInactivePatientsWithOnlyConsumables).toHaveBeenCalled();
  });

  it('should call `getPatientsWithInvalidStatus` of SystemService on getPatientWithInvalidStatus and match the result', () => {
    component.paginationResponse = {
      pageNumber: 1,
      pageSize: 25,
      records: [],
      totalPageCount: 1,
      totalRecordCount: 0,
    };
    component.getPatientWithInvalidStatus();
    expect(systemService.getPatientsWithInvalidStatus).toHaveBeenCalled();
    expect(component.paginationResponse.totalRecordCount).toEqual(patientUuidsResponse.length);
    expect(component.paginationResponse.pageNumber).toEqual(1);
    expect(component.getPatients).toHaveBeenCalled();
  });

  it('should call `fixPatientWithInvalidStatus` of SystemService on fixPatientWithInvalidStatus and match the result', () => {
    component.fixPatientWithInvalidStatus(patientRecord);
    expect(systemService.fixPatientWithInvalidStatus).toHaveBeenCalled();
    expect(component.setInitialRequestResponse).toHaveBeenCalled();
    expect(component.getPatientWithInvalidStatus).toHaveBeenCalled();
  });

  const patientRecord = {
    createdDateTime: '2020-09-26T11:21:48.8202506Z',
    dateOfBirth: '2020-01-09T00:00:00Z',
    diagnosis: null,
    firstName: 'Todd',
    hms2Id: null,
    hospiceId: 37,
    hospiceLocationId: 62,
    id: 1,
    isInfectious: false,
    name: 'Todd Loomis',
    lastName: 'Loomis',
    lastOrderDateTime: '2020-12-29T12:48:36.929124Z',
    lastOrderNumber: '434',
    patientAddress: [],
    patientHeight: 177.8,
    patientNotes: [],
    patientWeight: 170,
    phoneNumbers: [],
    uniqueId: '5eb30f32-2abf-4aba-a02f-1b117339f623',
  };

  const patientResponse = {
    records: [patientRecord],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const orderRecord = {
    confirmationNumber: '',
    deliveryAddress: {},
    requestedStartDateTime: '2020-12-22T09:39:07Z',
    requestedEndDateTime: '2020-12-22T11:39:07Z',
    fulfillmentEndDateTime: '0001-01-01T00:00:00Z',
    fulfillmentStartDateTime: '0001-01-01T00:00:00Z',
    dispatchStatus: 'Planned',
    dispatchStatusId: 10,
    hospice: {},
    hospiceId: 37,
    hospiceLocationId: 62,
    id: 644,
    netSuiteOrderId: 441682,
    orderDateTime: '2020-12-22T09:39:00Z',
    orderLineItems: [],
    orderNotes: [],
    orderNumber: '2222102576',
    orderRecipientUserId: 198,
    orderStatus: 'Pending Approval',
    orderType: 'Delivery',
    orderTypeId: 10,
    originatorUserId: 23,
    patient: {},
    patientUuid: '95bbf7c7-4303-435d-9671-853efbd08c3d',
    pickupAddress: null,
    shippingAddressId: 0,
    siteId: 8,
    statOrder: false,
    statusId: 19,
  };

  const orderResponse = {
    records: [orderRecord],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const paginationResponse = {
    records: [],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const continuationTokenResponse = new BaseContinuationTokenResponse();

  const basePaginationReponse = new BasePaginationReponse();

  const netSuiteLogItem = {
    created: null,
    custrecord_synchapi: null,
    custrecord_synchlogtype: null,
    custrecord_synchmessage: null,
    custrecord_synchpayload: null,
    id: 10000,
    isInactive: null,
    owner: null,
  };

  const netSuitLogsResponse = {
    count: 5,
    hasMore: true,
    items: [netSuiteLogItem],
    offset: 0,
    totalResults: 5000,
  };

  const netsuiteLogDetail = {
    created: '2020-09-10T16:38:00Z',
    custrecord_synchapi: {
      id: 11,
      refName: 'Patient',
    },
    custrecord_synchlogtype: {
      id: 2,
      refName: 'Error',
    },
    custrecord_synchmessage: '"404 :: "',
    custrecord_synchpayload: null,
    id: 10000,
    isInactive: false,
    owner: {
      id: 32114,
      refName: 'Richtsmeier, Tyler',
    },
  };

  const orderSelectedEvent = {
    checked: true,
    currentRow: orderRecord,
    selectedRows: orderRecord,
  };

  const hopsiceMember = {
    userId: 192,
  };

  const hospiceMemberServiceResponse = {
    records: [hopsiceMember],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const sitesResponse = {
    address: {},
    id: 8,
    isDisable: false,
    locationType: null,
    name: 'Harlingen, TX',
    netSuiteWarehouseId: 0,
    parentNetSuiteLocationId: 0,
    siteCode: 2430,
    sitePhoneNumber: [],
  };

  const orderFulfillment = {
    assetTag: null,
    deliveredStatus: 'Requested',
    fulfillmentEndAtLatitude: 0,
    fulfillmentEndAtLongitude: 0,
    fulfillmentEndDateTime: '2020-12-15T10:02:15.893Z',
    fulfillmentStartAtLatitude: 0,
    fulfillmentStartAtLongitude: 0,
    fulfillmentStartDateTime: '2020-12-15T10:02:15.893Z',
    fulfilledByDriverId: 5,
    fulfilledByDriverName: 'Jose Olvera',
    fulfilledBySiteId: 0,
    fulfilledByVehicleCvn: '217168',
    fulfilledByVehicleId: 455,
    id: 602,
    isFulfilmentConfirmed: false,
    itemName: 'PAP Mask - Small',
    lotNumber: null,
    netSuiteCustomerId: 2097,
    netSuiteDispatchRecordId: 0,
    netSuiteItemId: 24,
    netSuiteOrderId: 438227,
    orderHeaderId: 616,
    orderLineItemId: 5942,
    orderType: 'pickup',
    patientUuid: '16142463-865c-4db3-86d6-08809ece186a',
    quantity: 1,
    serialNumber: null,
  };

  const orderFulfillmentResponse = {
    records: [orderFulfillment],
    pageSize: 1,
    totalRecordCount: 1,
    pageNumber: 1,
    totalPageCount: 1,
  };

  const address = {
    addressLine1: '3353 Bradshaw Rd Ste 108 & 109',
    addressLine2: null,
    addressLine3: null,
    addressUuid: '7e876945-5f2a-4a75-b649-865e0d6fb7a8',
    city: 'Sacramento',
    country: 'United States of America',
    county: 'Sacramento',
    id: 82,
    isValid: false,
    isVerified: false,
    latitude: 38.56409,
    longitude: -121.32975,
    plus4Code: null,
    results: null,
    state: 'CA',
    verifiedBy: 'SmartyStreets',
    zipCode: 95827,
  };

  const apiLogsResponse = {
    apiLogs: [],
    continuationToken: null,
  };

  const dispatchOrderRecord = {
    assetTagNumber: '',
    consumableDeliveryDate: null,
    createdBy: '',
    createdDate: '2021-04-20T01:13:00Z',
    createdDt: null,
    customerId: 32521,
    customerLocationId: 31669,
    customerParent: null,
    deliveredBy: '',
    effectiveDate: null,
    expDeliveryDate: null,
    hmsActualDeliveryDate: null,
    hmsDeliveryDate: '2020-02-13T12:00:00Z',
    hmsDeliveryOrderType: '',
    hmsOrderStatus: 'Delivered',
    hmsOrderType: 'Delivery',
    hmsPickupOrderStatus: '',
    hmsPickupRequestDate: null,
    hmsStatus: '',
    hospiceId: null,
    isInactive: false,
    lineUniqueId: '',
    netsuiteItemId: 63,
    netsuiteWarehouseId: 464,
    nsDispatchId: 197205,
    nsInternalItemId: null,
    orderId: null,
    patientGuid: '49c2ee19-5515-4f1e-a248-983edc333405',
    patientId: null,
    patientName: 'Ernest Peters',
    pickUpRequestOrderType: '',
    pickupDate: null,
    priceLevel: '',
    pricingArea: '',
    qty: 1,
    reasonCd: '',
    scaDeliveryTransactionId: null,
    scaPickupTranOrderId: null,
    serielLotNumber: '',
    serviceItem: 4064,
    soDate: null,
    soLineUniqueId: '',
    statusCd: '',
    totalItemDelivered: null,
    totalItemsOrdered: null,
  };

  const dispatchOrderResponse = {
    count: 10000,
    currentPage: 1,
    pageSize: 25,
    results: [dispatchOrderRecord],
    totalPages: 40,
  };

  const patientInventory = {
    assetTagNumber: '20138203',
    cogsAccountName: '700T COS - Supplies',
    count: 1,
    deletedByUserId: 6,
    deletedByUserName: 'HospiceOne Restlet',
    deletedDateTime: '2021-04-20T12:57:39.2584261Z',
    deliveryAddressUuid: '00000000-0000-0000-0000-000000000000',
    id: 1620,
    inventoryId: 2163,
    isConsumable: false,
    isDME: true,
    itemDescription: 'CC1002-Wheelchair - Lightweight - 18 inch',
    itemId: 852,
    itemName: 'WHEELCHAIR LIGHTWEIGHT 18',
    itemNumber: 'CC1002',
    lotNumber: null,
    netSuiteInventoryId: 3097,
    netSuiteItemId: 5938,
    netSuiteOrderId: 0,
    netSuiteOrderLineItemId: 0,
    orderHeaderId: 0,
    patientName: 'Fred Rosenthal',
    patientUuid: 'ea73a27d-9c5d-4e8d-aa30-4e392817bd30',
    quantity: 1,
    serialNumber: 'U190403394',
  };

  const deletedPatientInventoryResponse = {
    pageNumber: 1,
    pageSize: 25,
    records: [patientInventory],
    totalPageCount: 1,
    totalRecordCount: 7,
  };

  const patientUuidsResponse = ['6a15f760-2b41-422b-98a6-2d98482f0ca3'];
});
