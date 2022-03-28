using Newtonsoft.Json;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class CustomerContactBase
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("internalCustomerId")]
        public int NetSuiteCustomerId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("canAccessWebStore")]
        public bool CanAccessWebStore { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }
    }
}
