using Newtonsoft.Json;

namespace HMSDigital.EHRIntegration.FHIRServices.DTOs.Results
{
    public class FHIREntryDTO
    {
        [JsonProperty("fullUrl")]
        public string FullURL { get; set; }

        [JsonProperty("resource")]
        public object Resource { get; set; }
    }
}
