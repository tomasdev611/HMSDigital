using Newtonsoft.Json;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class CustomerContact : CustomerContactBase
    {
        [JsonProperty("internalContactId")]
        public int NetSuiteContactId { get; set; }
    }
}
