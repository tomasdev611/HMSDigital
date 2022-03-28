using Newtonsoft.Json;
using System.Collections.Generic;

namespace HMSDigital.EHRIntegration.FHIRServices.DTOs.Requests
{
    public class HL7V2DTO
    {
        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty("parameter")]
        public List<HL7V2ParameterDTO> Parameter { get; set; }
    }
}
