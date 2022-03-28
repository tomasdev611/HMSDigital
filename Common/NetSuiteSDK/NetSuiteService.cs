using HospiceSource.Digital.NetSuite.SDK.API;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.OAuth;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospiceSource.Digital.NetSuite.SDK.Services
{
    public class NetSuiteService : INetSuiteService
    {
        private readonly RestletsConfig _restletsConfig;

        private readonly ILogger<NetSuiteService> _logger;

        private readonly NetSuiteConfig _netSuiteConfig;

        public NetSuiteService(IOptions<NetSuiteConfig> netSuiteOptions, ILogger<NetSuiteService> logger)
        {
            _netSuiteConfig = netSuiteOptions.Value;
            _restletsConfig = _netSuiteConfig.Restlets;
            _logger = logger;
        }

        public async Task<ContactResponse> CreateCustomerContact(CustomerContactBase contact)
        {
            var customer = new CustomerContactCreateRequest()
            {
                Contact = contact
            };
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.CustomerContactAPI },
                { "deploy", "1" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.CreateCustomerContact(urlParameters, customer);
                if (response != null && !response.Success)
                {
                    _logger.LogWarning($"{response.Error.Message}");
                }
                return response.Contact;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to create SSO contact on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create SSO contact on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<ContactResponse> UpdateCustomerContact(CustomerContact contact)
        {
            var customer = new CustomerContactUpdateRequest()
            {
                Contact = contact
            };
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.CustomerContactAPI },
                { "deploy", "1" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("PUT", urlParameters);
                var response = await netSuiteRestletClient.UpdateCustomerContact(urlParameters, customer);
                if (response != null && !response.Success)
                {
                    _logger.LogWarning($"{response.Error.Message}");
                }
                return response.Contact;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to update SSO contact on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create SSO delete on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<ContactResponse> DeleteCustomerContact(int contactId)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.CustomerContactAPI },
                { "deploy", "1" },
                { "internalContactId", contactId.ToString()}
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("DELETE", urlParameters);
                var response = await netSuiteRestletClient.DeleteCustomerContact(urlParameters);
                if (response == null)
                {
                    throw new ValidationException($"We are unable to complete the request. Please contact support.");
                }
                if (response != null && !response.Success)
                {
                    _logger.LogWarning($"{response.Error.Message}");
                    throw new ValidationException($"We are unable to complete the request. Please contact support.");
                }
                return response.Contact;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to delete SSO contact on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete SSO contact on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<ConfirmFulfilmentResponse> ConfirmOrderFulfilment(ConfirmFulfilmentRequest fulfilmentRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.ConfirmFulfilmentAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.ConfirmOrderFulfilment(urlParameters, fulfilmentRequest);
                if (response == null || !response.Success)
                {
                    _logger.LogError($"Failed to confirm order fulfilment on netsuite with response: {response?.Error?.Message}");
                }
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to confirm order fulfilment on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to confirm order fulfilment on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<ConfirmFulfilmentResponse> ConfirmInventoryMovement(InventoryMovementRequest movementRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.InventoryMovementAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.ConfirmInventoryMovement(urlParameters, movementRequest);
                if (response == null || !response.Success)
                {
                    _logger.LogError($"Failed to confirm inventory movement on netsuite with Error: {response?.Error.Message}");
                    return null;
                }
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to confirm inventory movement on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to confirm inventory movement on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<bool> CheckSystemStatus()
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.HelloWorldAPI },
                { "deploy", "1" },
                { "name", "test" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("GET", urlParameters);
                var response = await netSuiteRestletClient.HelloWorld(urlParameters);

                if (response.Error != null)
                {
                    return false;
                }
                return true;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to Check system status of netsuite: {ex.Content}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Check system status of netsuite: {ex.Message}");
            }
            return false;
        }

        public async Task<FixPatientHospiceResponse> FixPatientHospice(FixPatientHospiceRequest fixPatientHospiceRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.FixPatientHospiceAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.FixPatientHospice(urlParameters, fixPatientHospiceRequest);
                if (response == null || !response.Success)
                {
                    _logger.LogError($"Failed to fix patient hospice on netsuite with Error: {response?.Message}");
                    return null;
                }
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to fix patient hospice on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fix patient hospice on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<NetSuiteHmsLogResponse> GetNetSuiteHmsLogs(NetSuiteLogRequest netsuiteLogRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.NetsuiteHmsLogsAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.NetsuiteHmsLogs(urlParameters, netsuiteLogRequest);
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to get netsuite logs on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to get netsuite logs on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get netsuite logs on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to get netsuite logs on netsuite");
            }
        }

        public async Task<NetSuiteHMSDispatchResponse> GetNetSuiteHmsDispatchRecords(NetSuiteDispatchRequest netSuiteDispatchRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.NetsuiteHmsDispatchAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.NetsuiteHmsDispatchRecords(urlParameters, netSuiteDispatchRequest);
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to get netsuite dispatch records on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to get netsuite dispatch records on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get netsuite dispatch records on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to get netsuite dispatch records on netsuite");
            }
        }

        public async Task<ZonePaginatedList<Subscription>> GetSubscriptions(IEnumerable<int> netSuiteCustomerIds, int? pageNumber = null, int? pageSize = null)
        {
            var netSuiteCustomerIdsString = string.Join(",", netSuiteCustomerIds);
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.SubscriptionAPI },
                { "deploy", "customdeployzab_api_restlet" },
                { "export_id", "zab_subscription" },
                { "label_as_key", "true" },
                { "filter_anyof_custrecordzab_s_customer", netSuiteCustomerIdsString}
            };
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                urlParameters.Add("page_number", pageNumber?.ToString());
                urlParameters.Add("page_size", pageSize?.ToString());
            }
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("GET", urlParameters);
                var response = await netSuiteRestletClient.GetZoneSubscriptions(urlParameters);
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to Get Subscriptions for netsuite CustomerId ({netSuiteCustomerIdsString}) from netsuite: {ex.Content}");
                throw new ValidationException($"Failed to Get Subscriptions for netsuite CustomerId ({netSuiteCustomerIdsString}) from netsuite");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Get Subscriptions for netsuite CustomerId ({netSuiteCustomerIdsString}) from netsuite: {ex.Message}");
                throw new ValidationException($"Failed to Get Subscriptions for netsuite CustomerId ({netSuiteCustomerIdsString}) from netsuite");
            }
        }

        public async Task<ZonePaginatedList<SubscriptionItem>> GetSubscriptionItemsBySubscription(int netSuiteSubscriptionId, int? pageNumber = null, int? pageSize = null)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.SubscriptionAPI },
                { "deploy", "customdeployzab_api_restlet" },
                { "export_id", "zab_subscription_item" },
                { "label_as_key", "true" },
                { "filter_is_custrecordzab_si_subscription", netSuiteSubscriptionId.ToString()}
            };
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                urlParameters.Add("page_number", pageNumber?.ToString());
                urlParameters.Add("page_size", pageSize?.ToString());
            }

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("GET", urlParameters);
                var response = await netSuiteRestletClient.GetZoneSubscriptionItems(urlParameters);
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to Get Subscription Items for netsuite SubscripitionId ({netSuiteSubscriptionId}) from netsuite: {ex.Content}");
                throw new ValidationException($"Failed to Get Subscription Items for netsuite SubscripitionId ({netSuiteSubscriptionId}) from netsuite");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Get Subscription Items for netsuite SubscripitionId ({netSuiteSubscriptionId}) from netsuite: {ex.Message}");
                throw new ValidationException($"Failed to Get Subscription Items for netsuite SubscripitionId ({netSuiteSubscriptionId}) from netsuite");
            }
        }

        public async Task<ZonePaginatedList<SubscriptionItem>> GetSubscriptionItemsByCustomer(int netSuiteCustomerId, int? pageNumber = null, int? pageSize = null)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.SubscriptionAPI },
                { "deploy", "customdeployzab_api_restlet" },
                { "export_id", "zab_subscription_item" },
                { "label_as_key", "true" },
                {"filter_is_custrecordzab_si_customer", netSuiteCustomerId.ToString()}
            };
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                urlParameters.Add("page_number", pageNumber?.ToString());
                urlParameters.Add("page_size", pageSize?.ToString());
            }

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("GET", urlParameters);
                var response = await netSuiteRestletClient.GetZoneSubscriptionItems(urlParameters);
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to Get Subscription Items for netsuite CustomerId ({netSuiteCustomerId}) from netsuite: {ex.Content}");
                throw new ValidationException($"Failed to Get Subscription Items for netsuite CustomerId ({netSuiteCustomerId}) from netsuite");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Get Subscription Items for netsuite CustomerId ({netSuiteCustomerId}) from netsuite: {ex.Message}");
                throw new ValidationException($"Failed to Get Subscription Items for netsuite CustomerId ({netSuiteCustomerId}) from netsuite");
            }
        }

        public async Task<AdjustInventoryResponse> AdjustInventory(AdjustInventoryRequest inventoryRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.AdjustInventoryAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.AdjustInventory(urlParameters, inventoryRequest);
                if (response == null || !response.Success)
                {
                    _logger.LogError($"Failed to create physical inventory on netsuite with response: {response?.Error?.Message}");
                }
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to create physical inventory on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create physical inventory on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<AdjustBatchInventoryResponse> AdjustBatchInventory(IEnumerable<AdjustInventoryRequest> inventoryRequests)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.AdjustBatchInventoryAPI },
                { "deploy", "1" }
            };

            var adjustBatchInventoryRequest = new AdjustBatchInventoryRequest()
            {
                Items = inventoryRequests
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);

                var response = await netSuiteRestletClient.AdjustBatchInventory(urlParameters, adjustBatchInventoryRequest);
                if (response == null || response.Items.Any(i => !i.Success))
                {
                    _logger.LogError($"Failed to create batch physical inventory on netsuite");
                    _logger.LogError($"Total failed physical inventory requests: {response.Items.Count(i => !i.Success)}/{inventoryRequests.Count()}");
                }
                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to create batch physical inventory on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create batch physical inventory on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<IEnumerable<DispatchRecordUpdateResponse>> UpdateDispatchRecords(IEnumerable<DispatchRecordUpdateRequest> dispatchRecordUpdateRequests)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.DispatchRecordUpdateAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var dispatchRecordBulkUpdateRequest = new DispatchRecordBulkUpdateRequest()
                {
                    DispatchRecords = dispatchRecordUpdateRequests
                };
                var responseArray = await netSuiteRestletClient.BulkUpdateDispatchRecord(urlParameters, dispatchRecordBulkUpdateRequest);

                foreach (var response in responseArray)
                {
                    if (!string.Equals(response.Status, "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogError($"Failed to update dispatch record with Id ({response.DispatchRecordId}) with response: {response?.Message}");
                    }
                }

                return responseArray;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to update dispatch record on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to updated dispatch record on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<DispatchRecordDeleteResponse> DeleteDispatchRecordsByPatient(IEnumerable<Guid> patientUUIDs)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.DispatchRecordDeleteAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var dispatchRecordDeleteRequest = new DispatchRecordDeleteRequest()
                {
                    PatientUuids = patientUUIDs
                };
                var response = await netSuiteRestletClient.DeleteDispatchRecordByPatient(urlParameters, dispatchRecordDeleteRequest);

                if (!response.Success)
                {
                    _logger.LogError($"Failed to delete dispatch record for patient UUIDs ({ string.Join(", ", patientUUIDs)}) with response: {response?.Message}");
                }

                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to delete dispatch record on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete dispatch record on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<MergeDuplicatePatientResponse> MergeDuplicatePatients(Guid patientUuid, Guid duplicatePatientUuid)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.MergeDuplicatePatientAPI },
                { "deploy", "1" }
            };
            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var mergeDuplicatePatientRequest = new MergeDuplicatePatientRequest()
                {
                    ActualPatientUuid = patientUuid,
                    DuplicatePatientUuid = duplicatePatientUuid
                };
                var response = await netSuiteRestletClient.MergeDuplicatePatient(urlParameters, mergeDuplicatePatientRequest);

                if (!response.Success)
                {
                    _logger.LogError($"Failed to merge duplicate patient({duplicatePatientUuid}) with original patient({patientUuid}) with response: {response?.Message}");
                    throw new ValidationException($"Failed to merge duplication Patients with error: {response?.Message}");
                }

                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to merge patients on netsuite: {ex.Content}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to merge patients on netsuite: {ex.Message}");
                throw new ValidationException($"We are unable to complete the request. Please contact support.");
            }
        }

        public async Task<ZonePaginatedList<PurchaseOrder>> GetPurchaseOrders(GetPurchaseOrdersRequest purchaseOrderRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.GetPurchaseOrdersAPI },
                { "deploy", "customdeploy_app_get_purchaseorders" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.GetPurchaseOrders(urlParameters, purchaseOrderRequest);

                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to get purchase orders records on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to get purchase orders records on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get purchase orders records on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to get purchase orders records on netsuite");
            }
        }

        public async Task<PurchaseOrder> ReceivePurchaseOrder(ReceivePurchaseOrderRequest receivePurchaseOrderRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.ReceivePurchaseOrderAPI },
                { "deploy", "customdeploy_rl_app_receive_po" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.ReceivePurchaseOrder(urlParameters, receivePurchaseOrderRequest);

                if (response != null && !response.Success)
                {
                    throw new ValidationException(response.Message?.Message);
                }

                return response.PurchaseOrder;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to complete receipt on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to receive Purchase Order on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to complete receipt on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to receive Purchase Order on netsuite");
            }
        }

        public async Task<IEnumerable<NetSuiteTransferOrder>> GetTransferOrders(GetTransferOrdersRequest getTransferOrdersRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.GetTransferOrdersAPI },
                { "deploy", "customdeploy_app_get_transferorders" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.GetTransferOrders(urlParameters, getTransferOrdersRequest);

                if (response != null && !response.Success)
                {
                    throw new ValidationException(response.Error?.Message);
                }

                return response.Results;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to get transfer orders records on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to get transfer orders records on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get transfer orders records on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to get transfer orders records on netsuite");
            }
        }

        public async Task<NetSuiteTransferOrder> CreateTransferOrder(NetSuiteTransferOrder netSuiteTransferOrder)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.CreateTransferOrdersAPI },
                { "deploy", "customdeploy_app_create_transferorder" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.CreateTransferOrder(urlParameters, netSuiteTransferOrder);

                if (response != null && !response.Success)
                {
                    throw new ValidationException(response.Error?.Message);
                }

                return response.TransferOrder;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to create transfer order on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to create transfer order on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create transfer order on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to create transfer order on netsuite");
            }
        }

        public async Task<NetSuiteTransferOrder> FulfillReceiveTransferOrder(NetSuiteTOFulfillReceiveRequest netSuiteRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.FulfillReceiveTOrdersAPI },
                { "deploy", "customdeploy_rl_app_receive_fulfill_po" }
            };

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.FulfillReceiveTOrder(urlParameters, netSuiteRequest);

                if (response != null && !response.Success)
                {
                    throw new ValidationException(response.Error?.Message);
                }

                return response.TransferOrder;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to fulfill/receive transfer order on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to fulfill/receive transfer order on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fulfill/receive transfer order on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to fulfill/receive transfer order on netsuite");
            }
        }

        public async Task<NetSuiteInventoryResponse> UpdateInventory(UpdateInventoryRequest netSuiteUpdateInventoryRequest)
        {
            //TODO remove when netsuite endpoint is implemented
            return new NetSuiteInventoryResponse 
            {
                AssetTag = "AssetTag",
                ItemDescription = "ItemDescription",
                ItemName = "ItemName",
                NetSuiteInventoryId = 6514,
                NetSuiteItemId = 5822,
                SerialNumber = "SerialNumber"
            };
        }

        public async Task<IEnumerable<InventoryItemResponse>> AddInventory(AddNetSuiteInventoryRequest netSuiteRequest)
        {
            var urlParameters = new SortedDictionary<string, string>
            {
                { "script", _restletsConfig.ScriptIds.AddInventoryAPI },
                { "deploy", "customdeploy_rl_create_inventory" }
            };

            var jsonString = JsonConvert.SerializeObject(netSuiteRequest);

            try
            {
                var netSuiteRestletClient = GetNetSuiteRestletClient("POST", urlParameters);
                var response = await netSuiteRestletClient.AddInventory(urlParameters, netSuiteRequest);

                if (response != null && !response.Success)
                {
                    throw new ValidationException(response.Error?.Message);
                }

                return response.Inventory;
            }
            catch (ApiException ex)
            {
                _logger.LogError($"Failed to add inventory on netsuite: {ex.Content}");
                throw new ValidationException($"Failed to add inventory on netsuite");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add inventory on netsuite: {ex.Message}");
                throw new ValidationException($"Failed to add inventory on netsuite");
            }
        }

        private string GetAuthorizationHeader(string method, IDictionary<string, string> urlParameters, string restletsBaseUrl)
        {
            var parameters = string.Join("&", urlParameters.Select(x => $"{x.Key}={x.Value}"));
            var requestUrl = $"{restletsBaseUrl}?{parameters}";

            var client = new OAuthRequest()
            {
                ConsumerKey = _restletsConfig.ConsumerKey,
                ConsumerSecret = _restletsConfig.ConsumerSecret,
                Token = _restletsConfig.AccessToken,
                TokenSecret = _restletsConfig.TokenSecret,
                Method = method,
                Realm = _restletsConfig.Realm,
                RequestUrl = requestUrl,
                SignatureMethod = OAuthSignatureMethod.HmacSha256,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                Type = OAuthRequestType.AccessToken,
                Version = _restletsConfig.OAuthVersion,
            };

            return client.GetAuthorizationHeader();
        }

        private INetSuiteRestletsAPI GetNetSuiteRestletClient(string httpMethod, SortedDictionary<string, string> urlParameters)
        {
            var restletsBaseUrl = $"https://{_netSuiteConfig.Restlets.AccountId}.restlets.api.netsuite.com/app/site/hosting/restlet.nl";
            var netSuiteRestletsAPI = RestClient.For<INetSuiteRestletsAPI>(restletsBaseUrl);
            netSuiteRestletsAPI.Authorization = GetAuthorizationHeader(httpMethod, urlParameters, restletsBaseUrl);
            return netSuiteRestletsAPI;
        }
    }
}
