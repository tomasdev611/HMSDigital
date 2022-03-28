using Newtonsoft.Json;
using System;

namespace HMSDigital.EHRIntegration.HCHB.MLLPServer.DTOs
{
    public class HL7ADTDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
