using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using RestEase;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HospiceSource.Digital.NetSuite.SDK.API
{

    public interface INetSuiteRestletsAPI
    {
        [Header("Authorization")]
        string Authorization { get; set; }

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<CustomerContactResponse> CreateCustomerContact([QueryMap] IDictionary<string, string> urlParameters, [Body] CustomerContactCreateRequest customerContact);

        [Header("Content-Type", "application/json")]
        [Put("")]
        Task<CustomerContactResponse> UpdateCustomerContact([QueryMap] IDictionary<string, string> urlParameters, [Body] CustomerContactUpdateRequest customerContact);

        [Header("Content-Type", "application/json")]
        [Delete("")]
        Task<CustomerContactResponse> DeleteCustomerContact([QueryMap] IDictionary<string, string> urlParameters);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<ConfirmFulfilmentResponse> ConfirmOrderFulfilment([QueryMap] SortedDictionary<string, string> urlParameters, [Body] ConfirmFulfilmentRequest fulfilmentRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<ConfirmFulfilmentResponse> ConfirmInventoryMovement([QueryMap] SortedDictionary<string, string> urlParameters, [Body] InventoryMovementRequest movementRequest);

        [Header("Content-Type", "application/json")]
        [Get("")]
        Task<HelloWorldResponse> HelloWorld([QueryMap] SortedDictionary<string, string> urlParameters);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<NetSuiteHmsLogResponse> NetsuiteHmsLogs([QueryMap] SortedDictionary<string, string> urlParameters, [Body] NetSuiteLogRequest netsuiteLogRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<NetSuiteHMSDispatchResponse> NetsuiteHmsDispatchRecords([QueryMap] SortedDictionary<string, string> urlParameters, [Body] NetSuiteDispatchRequest netSuiteDispatchRequest);

        [Header("Content-Type", "application/json")]
        [Get("")]
        Task<ZonePaginatedList<Subscription>> GetZoneSubscriptions([QueryMap] SortedDictionary<string, string> urlParameters);

        [Header("Content-Type", "application/json")]
        [Get("")]
        Task<ZonePaginatedList<SubscriptionItem>> GetZoneSubscriptionItems([QueryMap] SortedDictionary<string, string> urlParameters);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<AdjustInventoryResponse> AdjustInventory([QueryMap] SortedDictionary<string, string> urlParameters, [Body] AdjustInventoryRequest inventoryRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<AdjustBatchInventoryResponse> AdjustBatchInventory([QueryMap] SortedDictionary<string, string> urlParameters, [Body] AdjustBatchInventoryRequest adjustBatchInventoryRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<IEnumerable<DispatchRecordUpdateResponse>> BulkUpdateDispatchRecord([QueryMap] SortedDictionary<string, string> urlParameters, [Body] DispatchRecordBulkUpdateRequest dispatchRecordBulkUpdateRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<DispatchRecordDeleteResponse> DeleteDispatchRecordByPatient([QueryMap] SortedDictionary<string, string> urlParameters, [Body] DispatchRecordDeleteRequest dispatchRecordDeleteRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<FixPatientHospiceResponse> FixPatientHospice([QueryMap] SortedDictionary<string, string> urlParameters, [Body] FixPatientHospiceRequest fixPatientHospiceRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<MergeDuplicatePatientResponse> MergeDuplicatePatient([QueryMap] SortedDictionary<string, string> urlParameters, [Body] MergeDuplicatePatientRequest mergeDuplicatePatientRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<ZonePaginatedList<PurchaseOrder>> GetPurchaseOrders([QueryMap] SortedDictionary<string, string> urlParameters, [Body] GetPurchaseOrdersRequest purchaseOrderRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<ReceivePurchaseOrderResponse> ReceivePurchaseOrder([QueryMap] SortedDictionary<string, string> urlParameters, [Body] ReceivePurchaseOrderRequest completeReceiptRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<GetTransferOrdersResponse> GetTransferOrders([QueryMap] SortedDictionary<string, string> urlParameters, [Body] GetTransferOrdersRequest transferOrderRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<CreateTransferOrderResponse> CreateTransferOrder([QueryMap] SortedDictionary<string, string> urlParameters, [Body] NetSuiteTransferOrder transferOrder);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<CreateTransferOrderResponse> FulfillReceiveTOrder([QueryMap] SortedDictionary<string, string> urlParameters, [Body] NetSuiteTOFulfillReceiveRequest fulfillReceiveRequest);

        [Header("Content-Type", "application/json")]
        [Post("")]
        Task<AddNetSuiteInventoryResponse> AddInventory([QueryMap] SortedDictionary<string, string> urlParameters, [Body] AddNetSuiteInventoryRequest addInventoryRequest);
    }
}
