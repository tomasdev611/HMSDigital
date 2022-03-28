using Newtonsoft.Json;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class CustomerContactCreateRequest
    {
        [JsonProperty("contact")]
        public CustomerContactBase Contact { get; set; }
    }
}
