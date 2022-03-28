using Newtonsoft.Json;
using System;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class FixPatientHospiceRequest
    {
        [JsonProperty("patientuuid")]
        public Guid PatientUuid { get; set; }

        [JsonProperty("customerLocation")]
        public int CustomerLocationId { get; set; }
    }
}
