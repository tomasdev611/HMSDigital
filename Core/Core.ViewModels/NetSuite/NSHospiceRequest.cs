using Newtonsoft.Json;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSHospiceRequest : HospiceRequest
    {
        public string Type { get; set; }

        public string CustomerType { get; set; }

        [JsonProperty("internalId")]
        public int NetSuiteCustomerId { get; set; }

        public string Email { get; set; }

        public IEnumerable<HospiceLocationCreateRequest> Locations { get; set; }

        public string CreatedByUserEmail { get; set; }

        public int? InternalWarehouseId { get; set; }

        public AddressRequest Address { get; set; }

        public int? NetSuiteContractingCustomerId { get; set; }

    }
}
