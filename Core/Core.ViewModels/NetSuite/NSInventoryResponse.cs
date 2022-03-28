using Newtonsoft.Json;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSInventoryResponse
    {
        [JsonProperty("hmsInternalId")]
        public int Id { get; set; }

        [JsonProperty("netsuiteInternalId")]
        public int NetSuiteInventoryId { get; set; }
    }
}
