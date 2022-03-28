using Newtonsoft.Json;
using System.Collections.Generic;

namespace HMSDigital.EHRIntegration.FHIRServices.DTOs.Results
{
    public class FHIRBundleDTO
    {
        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }

        [JsonProperty("identifier")]
        public FHIRIdentifierDTO Identifier { get; set; }

        [JsonProperty("entry")]
        public IEnumerable<FHIREntryDTO> Entry { get; set; }
    }        
}
