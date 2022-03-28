using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class MergeDuplicatePatientRequest
    {
        [JsonProperty("actualPatientUuid")]
        public Guid ActualPatientUuid { get; set; }

        [JsonProperty("duplicatePatientUuid")]
        public Guid DuplicatePatientUuid { get; set; }

    }
}
