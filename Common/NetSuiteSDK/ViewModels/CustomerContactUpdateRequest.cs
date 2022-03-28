using Newtonsoft.Json;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class CustomerContactUpdateRequest
    {
        [JsonProperty("contact")]
        public CustomerContact Contact { get; set; }
    }
}
