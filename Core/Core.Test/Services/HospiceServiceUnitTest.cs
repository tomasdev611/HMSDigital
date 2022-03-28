using Core.Test.MockProvider;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Sieve.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class HospiceServiceUnitTest
    {
        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly IHospiceService _hospiceService;

        private readonly IHospiceMemberService _hospiceMemberService;

        private readonly IHospiceLocationService _hospiceLocationService;

        public HospiceServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _hospiceService = mockService.GetService<IHospiceService>();
            _hospiceMemberService = mockService.GetService<IHospiceMemberService>();
            _hospiceLocationService = mockService.GetService<IHospiceLocationService>();
            _sieveModel = new SieveModel();
        }

        #region Hospice

        [Fact]
        public async Task GetAllHospicesShouldReturnValidList()
        {
            var hospices = await _hospiceService.GetAllHospices(_sieveModel);
            Assert.NotEmpty(hospices.Records);
        }

        [Fact]
        public async Task SearchHospiceShouldReturnListForValidCriteria()
        {
            var hospices = await _hospiceService.SearchHospices(_sieveModel, "Hospice");
            Assert.NotEmpty(hospices.Records);
        }

        [Fact]
        public async Task GetHospiceByNetSuiteCustomerIdShouldReturnValidHospice()
        {
            var hospice = await _hospiceService.GetHospiceByNetSuiteCustomerId(1234);
            Assert.NotNull(hospice);
        }

        [Fact]
        public async Task GetHospiceRolesShouldReturnValidList()
        {
            var hospiceRoles = await _hospiceService.GetHospiceRoles(1);
            Assert.NotEmpty(hospiceRoles);
        }

        [Fact]
        public async Task HospiceShouldBeCreatedForValidData()
        {
            var hospiceRequest = _mockViewModels.GetHospiceRequest();
            var createdHospices = await _hospiceService.UpsertHospiceWithLocation(hospiceRequest);
            var hospice = await _hospiceService.GetHospiceById(createdHospices.Id);
            await ValidateUpsertHospiceAssertion(hospice, hospiceRequest);
        }

        [Fact]
        public async Task HospiceCreationShouldBeFailedForInvalidData()
        {
            var hospiceRequest = _mockViewModels.GetHospiceRequest();
            hospiceRequest.InternalWarehouseId = 2000;
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.UpsertHospiceWithLocation(hospiceRequest));
        }

        [Fact]
        public async Task HospiceShouldBeUpdatedForValidData()
        {
            var hospiceRequest = _mockViewModels.GetHospiceRequest();
            var createdHospices = await _hospiceService.UpsertHospiceWithLocation(hospiceRequest);

            var updateHospiceRequest = _mockViewModels.GetUpdateHospiceRequest();
            var updatedHospices = await _hospiceService.UpsertHospiceWithLocation(updateHospiceRequest);

            Assert.Equal(createdHospices.Id, updatedHospices.Id);
            Assert.Equal(createdHospices.NetSuiteCustomerId, updatedHospices.NetSuiteCustomerId);

            var hospice = await _hospiceService.GetHospiceById(updatedHospices.Id);
            await ValidateUpsertHospiceAssertion(hospice, updateHospiceRequest);
            foreach (var location in hospiceRequest.Locations)
            {
                var hospiceLocation = hospice.HospiceLocations.FirstOrDefault(l => l.NetSuiteCustomerId == location.NetSuiteCustomerId);
                Assert.Null(hospiceLocation);
            }
        }

        [Fact]
        public async Task HospiceShouldBeDeletedForValidNetSuiteCustomerId()
        {
            var hospiceDeleteRequest = new HospiceDeleteRequest()
            {
                NetSuiteCustomerId = 1235,
                Id = 1235
            };
            var hospiceBeforeDelete = await _hospiceService.GetHospiceById(hospiceDeleteRequest.Id);
            Assert.NotNull(hospiceBeforeDelete);
            var deletedHospice = await _hospiceService.DeleteCustomer(hospiceDeleteRequest);
            var hospiceAfterDelete = await _hospiceService.GetHospiceById(deletedHospice.Id);
            Assert.Null(hospiceAfterDelete);
        }

        [Fact]
        public async Task HospiceLocationShouldBeDeletedForValidNetSuiteCustomerId()
        {
            var hospiceDeleteRequest = new HospiceDeleteRequest()
            {
                NetSuiteCustomerId = 2020,
                Id = 2020
            };
            var locationBeforeDelete = await _hospiceLocationService.GetHospiceLocationById(hospiceDeleteRequest.Id);
            Assert.NotNull(locationBeforeDelete);
            var deletedHospice = await _hospiceService.DeleteCustomer(hospiceDeleteRequest);
            var locationAfterDelete = await _hospiceLocationService.GetHospiceLocationById(hospiceDeleteRequest.Id);
            Assert.Null(locationAfterDelete);
        }

        [Fact]
        public async Task CreditHoldStausShouldBeChangedForValidHospice()
        {
            var creditHoldRequest = new CreditHoldRequest()
            {
                IsCreditOnHold = true,
                CreditHoldNote = "Test"
            };
            var updatedHospice = await _hospiceService.ChangeCreditHoldStatus(1234, creditHoldRequest);
            var hospice = await _hospiceService.GetHospiceById(updatedHospice.Id);
            Assert.NotNull(hospice);
            Assert.True(hospice.IsCreditOnHold);
            var creditHistory = await _hospiceService.GetCreditHoldHistory(updatedHospice.Id, _sieveModel);
            Assert.Empty(creditHistory);
        }

        [Fact]
        public async Task CreditHoldStausUpdateShouldBeCreateHistory()
        {
            var creditHoldRequest = new CreditHoldRequest()
            {
                IsCreditOnHold = true,
                CreditHoldNote = "Test"
            };
            var updatedHospice = await _hospiceService.ChangeCreditHoldStatus(1234, creditHoldRequest);
            var hospice = await _hospiceService.GetHospiceById(updatedHospice.Id);
            Assert.NotNull(hospice);
            Assert.True(hospice.IsCreditOnHold);

            creditHoldRequest.IsCreditOnHold = false;
            var resetHospice = await _hospiceService.ChangeCreditHoldStatus(1234, creditHoldRequest);
            var restoredCreditHoldHospice = await _hospiceService.GetHospiceById(updatedHospice.Id);
            Assert.NotNull(hospice);
            Assert.False(restoredCreditHoldHospice.IsCreditOnHold);

            var creditHistory = await _hospiceService.GetCreditHoldHistory(resetHospice.Id, _sieveModel);
            Assert.NotEmpty(creditHistory);
        }

        [Fact]
        public async Task CreditHoldStausShouldBeFailedForInvalidHospice()
        {
            var creditHoldRequest = new CreditHoldRequest()
            {
                IsCreditOnHold = true,
                CreditHoldNote = "Test"
            };
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.ChangeCreditHoldStatus(2222, creditHoldRequest));
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.GetCreditHoldHistory(2222, _sieveModel));
        }

        #endregion

        #region CSV Mapping

        [Fact]
        public async Task PutCsvInputMappingShouldBeUpdatedForValidHospice()
        {
            var inputMappingRequest = _mockViewModels.GetInputCsvMappingRequest();
            
            var updatedMapping = await _hospiceService.PutInputCsvMapping(1234, "HospiceMember", inputMappingRequest);
            var inputMapping = await _hospiceService.GetInputCsvMapping(1234, "HospiceMember");
            Assert.NotNull(inputMapping);
            Assert.Equal(inputMappingRequest.ColumnNameMappings.Count, inputMapping.ColumnNameMappings.Count);
        }

        [Fact]
        public async Task PutCsvInputMappingShouldBeFailedForInvalidMappingType()
        {
            var inputMappingRequest = _mockViewModels.GetInputCsvMappingRequest();
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.PutInputCsvMapping(1234, "test", inputMappingRequest));
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.GetInputCsvMapping(1234, "test"));
        }

        [Fact]
        public async Task GetCsvInputMappingShouldBeReturnDefaultMappingWhenHospiceNotHaving()
        {
            var hospiceInputMapping = await _hospiceService.GetInputCsvMapping(1234, "HospiceMember");
            Assert.NotNull(hospiceInputMapping);
            Assert.Equal(5, hospiceInputMapping.ColumnNameMappings.Count);
            var facilityInputMapping = await _hospiceService.GetInputCsvMapping(1234, "Facility");
            Assert.NotNull(facilityInputMapping);
            Assert.Equal(8, facilityInputMapping.ColumnNameMappings.Count);
        }

        [Fact]
        public async Task PutCsvInputMappingShouldBeFailedForInvalidData()
        {
            var inputMappingRequest = _mockViewModels.GetInputCsvMappingRequest();
            inputMappingRequest.ColumnNameMappings.Add("InvalidType", new InputMappedItem() { Name = "Invalid Type", Type = "object"});
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.PutInputCsvMapping(1234, "HospiceMember", inputMappingRequest));
        }

        [Fact]
        public async Task PutCsvOutputMappingShouldBeUpdatedForValidHospice()
        {
            var outputMappingRequest = _mockViewModels.GetOutputCsvMappingRequest();

            var updatedMapping = await _hospiceService.PutOutputCsvMapping(1234, "Facility", outputMappingRequest);
            var outputMapping = await _hospiceService.GetOutputCsvMapping(1234, "Facility");
            Assert.NotNull(outputMapping);
            Assert.Equal(outputMappingRequest.ColumnNameMappings.Count, outputMapping.ColumnNameMappings.Count);
        }

        [Fact]
        public async Task PutCsvOutputMappingShouldBeFailedForInvalidMappingType()
        {
            var outputMappingRequest = _mockViewModels.GetOutputCsvMappingRequest();
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.PutOutputCsvMapping(1234, "test", outputMappingRequest));
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.GetOutputCsvMapping(1234, "test"));
        }

        [Fact]
        public async Task GetCsvOutputMappingShouldBeReturnDefaultMappingWhenHospiceNotHaving()
        {
            var facilityOutputMapping = await _hospiceService.GetOutputCsvMapping(1234, "Facility");
            Assert.NotNull(facilityOutputMapping);
            Assert.Equal(8, facilityOutputMapping.ColumnNameMappings.Count);
            var hospiceOutputMapping = await _hospiceService.GetOutputCsvMapping(1234, "HospiceMember");
            Assert.NotNull(hospiceOutputMapping);
            Assert.Equal(5, hospiceOutputMapping.ColumnNameMappings.Count);
        }

        [Fact]
        public async Task PutCsvOutputMappingShouldBeFailedForInvalidData()
        {
            var outputMappingRequest = _mockViewModels.GetOutputCsvMappingRequest();
            outputMappingRequest.ColumnNameMappings.Add("InvalidType", new OutputMappedItem() { Name = "Invalid Type", Type = "object" });
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.PutOutputCsvMapping(1234, "Facility", outputMappingRequest));
        }

        #endregion

        #region Contract Records

        [Fact]
        public async Task UpsertContractShouldBeCreatedContractRecord()
        {
            var contractRecordRequest = _mockViewModels.GetNSContractRecordRequest(1234, 5, 10);
            var createdContractRecords = await _hospiceService.UpsertContractRecord(contractRecordRequest);
            await ValidateUsertContractRecordsAssertion(contractRecordRequest);
        }

        [Fact]
        public async Task UpsertContractShouldBeUpdatedContractRecord()
        {
            var contractRecordRequest = _mockViewModels.GetNSContractRecordRequest(1234, 5, 10);
            var createdContractRecords = await _hospiceService.UpsertContractRecord(contractRecordRequest);
            await ValidateUsertContractRecordsAssertion(contractRecordRequest);

            contractRecordRequest.Rate = 50;
            var updatedContractRecords = await _hospiceService.UpsertContractRecord(contractRecordRequest);
            await ValidateUsertContractRecordsAssertion(contractRecordRequest);
        }

        [Fact]
        public async Task UpsertContractShouldBeFailedForInvalidHospice()
        {
            var contractRecordRequest = _mockViewModels.GetNSContractRecordRequest(2525, 5, 10);
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.UpsertContractRecord(contractRecordRequest));
        }

        [Fact]
        public async Task UpsertContractShouldBeFailedForInvalidItem()
        {
            var contractRecordRequest = _mockViewModels.GetNSContractRecordRequest(1234, 25, 10);
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.UpsertContractRecord(contractRecordRequest));
        }

        [Fact]
        public async Task ContractRecordShouldBeDeletedForValidNetSuiteContractRecordId()
        {
            var netSuiteRecordId = 1;
            _sieveModel.Filters = $"netSuiteContractRecordId=={netSuiteRecordId}";
            var ContractRecordBeforeDelete = await _hospiceService.GetHospiceContractRecords(_sieveModel);
            Assert.NotEmpty(ContractRecordBeforeDelete.Records);
            var deletedContractRecord = await _hospiceService.DeleteContractRecord(netSuiteRecordId);
            var contractRecordAfterDelete = await _hospiceService.GetHospiceContractRecords(_sieveModel);
            Assert.Empty(contractRecordAfterDelete.Records);
        }

        #endregion

        #region Subscription and Subscription items

        [Fact]
        public async Task GetHospiceSubscriptionsShouldReturnValidListForValidHospice()
        {
            var subscriptions = await _hospiceService.GetHospiceSubscriptions(1234, _sieveModel);
            Assert.NotEmpty(subscriptions.Records);
            foreach(var subscription in subscriptions.Records)
            {
                Assert.Equal(1234, subscription.HospiceId);
            }
        }
        [Fact]
        public async Task GetHospiceSubscriptionsShouldFailedForInvalidHospice()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.GetHospiceSubscriptions(2525, _sieveModel));
        }

        [Fact]
        public async Task GetHospiceSubscriptionItemsShouldReturnValidListForValidSubscription()
        {
            var subscriptionItems = await _hospiceService.GetHospiceSubscriptionItemsBySubscription(1234, _sieveModel);
            Assert.NotEmpty(subscriptionItems.Records);
            foreach (var item in subscriptionItems.Records)
            {
                Assert.Equal(1234, item.SubscriptionId);
            }
        }
        [Fact]
        public async Task GetHospiceSubscriptionItemsShouldFailedForInvalidSubscription()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceService.GetHospiceSubscriptionItemsBySubscription(2525, _sieveModel));
        }

        #endregion

        private async Task ValidateUpsertHospiceAssertion(Hospice hospice, NSHospiceRequest hospiceRequest)
        {
            var hospiceMembers = await _hospiceMemberService.GetAllHospiceMembers(hospice.Id, _sieveModel, string.Empty);
            Assert.NotEmpty(hospiceMembers.Records);
            Assert.Equal(hospiceRequest.Locations.Count(), hospice.HospiceLocations.Count());
            Assert.Equal(hospiceRequest.Name, hospice.Name);
            foreach (var member in hospiceMembers.Records)
            {
                Assert.Equal(hospiceRequest.Email, member.Email);
                Assert.Equal("Account", member.FirstName);
                Assert.Equal("Holder", member.LastName);
            }
            foreach (var location in hospiceRequest.Locations)
            {
                var hospiceLocation = hospice.HospiceLocations.FirstOrDefault(l => l.NetSuiteCustomerId == location.NetSuiteCustomerId);
                Assert.NotNull(hospiceLocation);
                Assert.Equal(location.Name, hospiceLocation.Name);
                Assert.Equal(location.NetSuiteCustomerId, hospiceLocation.NetSuiteCustomerId);
                Assert.Equal(location.Address.AddressLine1, hospiceLocation.Address.AddressLine1);
                Assert.Equal(location.Address.AddressLine2, hospiceLocation.Address.AddressLine2);
                Assert.Equal(location.Address.City, hospiceLocation.Address.City);
                Assert.Equal(location.Address.ZipCode, hospiceLocation.Address.ZipCode);
                Assert.Equal(1, hospiceLocation.SiteId);
            }
        }

        private async Task ValidateUsertContractRecordsAssertion(NSContractRecordRequest contractRecordRequest)
        {
            _sieveModel.Filters = $"NetSuiteCustomerId=={contractRecordRequest.NetSuiteCustomerId}";
            var contractRecords = await _hospiceService.GetHospiceContractRecords(_sieveModel);
            Assert.NotNull(contractRecords.Records);
            var contractRecord = contractRecords.Records.FirstOrDefault(c => c.NetSuiteContractRecordId == contractRecordRequest.NetSuiteContractRecordId);
            Assert.NotNull(contractRecord);
            Assert.Equal(contractRecordRequest.Rate, contractRecord.Rate);
            Assert.Equal(contractRecordRequest.NetSuiteRelatedItemId, contractRecord.NetSuiteRelatedItemId);
        }
    }
}
