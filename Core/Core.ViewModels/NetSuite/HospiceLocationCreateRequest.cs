using Newtonsoft.Json;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class HospiceLocationCreateRequest
    {
        public string CustomerType { get; set; }

        [JsonProperty("internalId")]
        public int NetSuiteCustomerId { get; set; }

        public string Name { get; set; }

        public AddressRequest Address { get; set; }

        public int? InternalWarehouseId { get; set; }

        public int? NetSuiteContractingCustomerId { get; set; }
    }
}
