using Newtonsoft.Json;
using System;

namespace HMSDigital.EHRIntegration.FHIRServices.DTOs.Results
{
    public class HL7ADTDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public DateTime date { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
