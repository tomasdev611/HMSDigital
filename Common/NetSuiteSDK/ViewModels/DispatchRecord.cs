using Newtonsoft.Json;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class DispatchRecord
    {
        public int NetSuiteItemId { get; set; }
        
        [JsonProperty("internalId")]
        public int DispatchRecordId { get; set; }

        public ErrorResponse Error { get; set; }

        public FulfilmentItem Item { get; set; }
    }
}
