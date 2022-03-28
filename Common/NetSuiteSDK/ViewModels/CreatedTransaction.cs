using Newtonsoft.Json;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class CreatedTransaction
    {
        public string Type { get; set; }

        [JsonProperty("internalid")]
        public int InternalId { get; set; }
    }
}
