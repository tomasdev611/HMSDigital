using Newtonsoft.Json;

namespace HMSDigital.EHRIntegration.FHIRServices.DTOs.Results
{
    public class FHIRIdentifierDTO
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
