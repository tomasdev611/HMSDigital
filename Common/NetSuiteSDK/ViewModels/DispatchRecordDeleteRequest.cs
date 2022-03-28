using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class DispatchRecordDeleteRequest
    {
        [JsonProperty("patientuuids")]
        public IEnumerable<Guid> PatientUuids { get; set; }
    }
}
