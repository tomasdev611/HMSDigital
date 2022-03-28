using Newtonsoft.Json;

namespace HMSDigital.EHRIntegration.FHIRServices.DTOs.Requests
{
    public class HL7V2ParameterDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("valueString")]
        public string ValueString { get; set; }        
    }
}
