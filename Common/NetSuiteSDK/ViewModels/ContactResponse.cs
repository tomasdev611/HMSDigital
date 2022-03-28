using Newtonsoft.Json;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class ContactResponse
    {
        public string Email { get; set; }

        [JsonProperty("internalContactId")]
        public int NetSuiteContactId { get; set; }

        public string Status { get; set; }
    }
}
