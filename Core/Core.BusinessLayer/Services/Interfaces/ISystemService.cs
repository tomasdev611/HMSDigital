using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface ISystemService
    {
        Task<PaginatedList<User>> GetUsersWithoutIdentity(SieveModel sieveModel);

        Task<PaginatedList<HospiceMember>> GetMemberWithoutNetSuiteContact(SieveModel sieveModel);

        Task<PaginatedList<User>> GetInternalUsersWithoutNetSuiteContact(SieveModel sieveModel);

        Task<PaginatedList<User>> GetUsersWithoutEmail(SieveModel sieveModel);

        Task FixAllUsersWithoutIdentity();

        Task<long> FixNonVerifiedAddresses();

        Task<long> FixMemberWithoutNetSuiteContact();

        Task<long> FixInternalUsersWithoutNetSuiteContact();

        Task<PaginatedList<OrderHeader>> GetUnconfirmedFulfillmentOrders(SieveModel sieveModel);

        Task<PaginatedList<OrderHeader>> GetOrdersWithoutSite(SieveModel sieveModel);

        Task<long> FixUnconfirmedFulfillmentOrders(bool dispatchOnly, int batchSize, bool stopOnFirstError);

        Task<bool> FixUnconfirmedFulfillmentOrder(int orderId, bool dispatchOnly);

        Task<long> FixOrdersWithoutSite();

        Task<AzureLogResponse<APILog>> GetCoreApiLogs(APILogRequest apiLogRequest);

        Task<PaginatedList<Address>> GetNonVerifiedAddresses(SieveModel sieveModel);

        Task<Address> FixNonVerifiedAddress(int addressId);

        Task<Address> FixNonVerifiedHomeAddress(int addressId);

        Task<PaginatedList<OrderHeader>> GetOrdersWithIncorrectStatus(SieveModel sieveModel);

        Task<OrderHeader> FixOrderStatus(int orderId, bool previewChanges);

        Task<NetSuiteHmsLogResponse> GetNetSuiteLogs(ViewModels.NetSuiteLogRequest netsuiteLogRequest);

        Task<PaginatedList<NetSuiteHmsDispatchRecord>> GetNetSuiteDispatchRecords(ViewModels.NetSuiteDispatchRequest netSuiteDispatchRequest);

        Task<GetPatientInventoryWithIssuesResponce> GetPatientInventoryWithInvalidInventory(string orderNumber, string assetTagNumber);

        Task FixPatientInventoryWithInvalidInventory(FixPatientInventoryWithIssuesRequest request);

        Task<IEnumerable<Guid>> GetPatientsWithOnlyConsumableInventory();

        Task<IEnumerable<string>> GetPatientsWithInvalidStatus();

        Task<PatientStatus> FixPatientWithInvalidStatus(Guid patientUUID, bool previewChanges);

        Task<IEnumerable<PatientInventory>> FixPatientsWithOnlyConsumableInventory(Guid patientUUID, bool previewChanges);

        Task<PaginatedList<PatientInventory>> GetDeletedPatientInventory(SieveModel sieveModel);

        Task<PaginatedList<Address>> GetNonVerifiedHomeAddresses(SieveModel sieveModel);

        Task<long> FixNonVerifiedHomeAddresses();

        Task<IEnumerable<string>> GetHospicesWithoutFhirOrganization();

        Task<long> FixHospicesWithoutFhirOrganization();

        Task<IEnumerable<string>> GetPatientsWithoutFhirRecord();

        Task<long> FixPatientsWithoutFhirRecord();

        Task<IEnumerable<string>> FixAllPatientWithStatusIssues();

        Task<PatientInventoryWithInvalidItemResponse> GetPatientInventoryWithInvalidItem(string orderNumber, string assetTagNumber);

        Task FixPatientInventoryWithInvalidItem(FixPatientInventoryWithIssuesRequest request);
    }
}
