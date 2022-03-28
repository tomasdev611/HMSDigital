using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospiceSource.Digital.NetSuite.SDK.Interfaces
{
    public interface INetSuiteService
    {
        Task<ContactResponse> CreateCustomerContact(CustomerContactBase contact);

        Task<ContactResponse> UpdateCustomerContact(CustomerContact contact);

        Task<ContactResponse> DeleteCustomerContact(int contactId);

        Task<ConfirmFulfilmentResponse> ConfirmOrderFulfilment(ConfirmFulfilmentRequest fulfilmentRequest);

        Task<ConfirmFulfilmentResponse> ConfirmInventoryMovement(InventoryMovementRequest movementRequest);

        Task<FixPatientHospiceResponse> FixPatientHospice(FixPatientHospiceRequest fixPatientHospiceRequest);

        Task<bool> CheckSystemStatus();

        Task<NetSuiteHmsLogResponse> GetNetSuiteHmsLogs(NetSuiteLogRequest netsuiteLogRequest);

        Task<NetSuiteHMSDispatchResponse> GetNetSuiteHmsDispatchRecords(NetSuiteDispatchRequest netsuiteLogRequest);

        Task<ZonePaginatedList<Subscription>> GetSubscriptions(IEnumerable<int> netSuiteCustomerIds, int? pageNumber = null, int? pageSize = null);

        Task<ZonePaginatedList<SubscriptionItem>> GetSubscriptionItemsBySubscription(int netSuiteSubscriptionId, int? pageNumber = null, int? pageSize = null);

        Task<ZonePaginatedList<SubscriptionItem>> GetSubscriptionItemsByCustomer(int netSuiteCustomerId, int? pageNumber = null, int? pageSize = null);

        Task<AdjustInventoryResponse> AdjustInventory(AdjustInventoryRequest inventoryRequest);

        Task<AdjustBatchInventoryResponse> AdjustBatchInventory(IEnumerable<AdjustInventoryRequest> inventoryRequests);

        Task<IEnumerable<DispatchRecordUpdateResponse>> UpdateDispatchRecords(IEnumerable<DispatchRecordUpdateRequest> dispatchRecordUpdateRequests);

        Task<DispatchRecordDeleteResponse> DeleteDispatchRecordsByPatient(IEnumerable<Guid> patientUUIDs);

        Task<MergeDuplicatePatientResponse> MergeDuplicatePatients(Guid patientUuid, Guid duplicatePatientUuid);

        Task<ZonePaginatedList<PurchaseOrder>> GetPurchaseOrders(GetPurchaseOrdersRequest purchaseOrderRequest);

        Task<PurchaseOrder> ReceivePurchaseOrder(ReceivePurchaseOrderRequest completeReceiptRequest);

        Task<IEnumerable<NetSuiteTransferOrder>> GetTransferOrders(GetTransferOrdersRequest getTransferOrdersRequest);

        Task<NetSuiteTransferOrder> CreateTransferOrder(NetSuiteTransferOrder netSuiteTransferOrder);

        Task<NetSuiteTransferOrder> FulfillReceiveTransferOrder(NetSuiteTOFulfillReceiveRequest netSuiteRequest);

        Task<NetSuiteInventoryResponse> UpdateInventory(UpdateInventoryRequest netSuiteUpdateInventoryRequest);

        Task<IEnumerable<InventoryItemResponse>> AddInventory(AddNetSuiteInventoryRequest netSuiteRequest);
    }
}
