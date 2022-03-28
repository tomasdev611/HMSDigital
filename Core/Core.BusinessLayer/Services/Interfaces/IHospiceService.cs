using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using NSViewModel = HMSDigital.Core.ViewModels.NetSuite;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IHospiceService
    {
        Task<PaginatedList<Hospice>> GetAllHospices(SieveModel sieveModel);

        Task<PaginatedList<Hospice>> SearchHospices(SieveModel sieveModel, string searchQuery);

        Task<Hospice> GetHospiceById(int hospiceId);

        Task<Hospice> ChangeCreditHoldStatus(int hospiceId, CreditHoldRequest creditHoldRequest);

        Task<IEnumerable<CreditHoldHistory>> GetCreditHoldHistory(int hospiceId, SieveModel sieveModel);

        Task<Hospice> GetHospiceByNetSuiteCustomerId(int netSuiteCustomerId);

        Task<NSViewModel.HospiceResponse> UpsertHospiceWithLocation(NSViewModel.NSHospiceRequest hospiceRequest);

        Task<IEnumerable<Role>> GetHospiceRoles(int hospiceId);

        Task<NSViewModel.HospiceResponse> DeleteCustomer(NSViewModel.HospiceDeleteRequest hospiceDeleteRequest);

        Task<CsvMapping<InputMappedItem>> GetInputCsvMapping(int hospiceId, string mappedTable);

        Task<CsvMapping<OutputMappedItem>> GetOutputCsvMapping(int hospiceId, string mappedTable);

        Task<CsvMapping<InputMappedItem>> PutInputCsvMapping(int hospiceId, string mappedTable, CsvMapping<InputMappedItem> inputMapping);

        Task<CsvMapping<OutputMappedItem>> PutOutputCsvMapping(int hospiceId, string mappedTable, CsvMapping<OutputMappedItem> outputMapping);

        Task<PaginatedList<ViewModels.Subscription>> GetHospiceSubscriptions(int hospiceId, SieveModel sieveModel);

        Task<PaginatedList<Hms2Contract>> GetHMS2Contracts(int hospiceId, SieveModel sieveModel);

        Task<PaginatedList<ViewModels.Hms2ContractItem>> GetHospiceHMS2ContractItemsByContract(int contractId, SieveModel sieveModel);

        Task UpsertHospiceSubscriptions(int hospiceId);

        Task UpsertHms2HospiceContracts(int hospiceId);

        Task<PaginatedList<ViewModels.SubscriptionItem>> GetHospiceSubscriptionItemsBySubscription(int hospiceId, SieveModel sieveModel);

        Task<ViewModels.ContractRecord> UpsertContractRecord(NSContractRecordRequest contractRecordRequest);
        
        Task<PaginatedList<ViewModels.ContractRecord>> GetHospiceContractRecords( SieveModel sieveModel);

        Task<NSViewModel.NSContractRecordResponse> DeleteContractRecord(int netSuiteContractRecordId);
    }
}
