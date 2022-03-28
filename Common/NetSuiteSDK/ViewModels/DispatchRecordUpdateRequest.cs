using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class DispatchRecordUpdateRequest
    {
        [JsonProperty("netsuiteId")]
        public int DispatchRecordId { get; set; }

        [JsonProperty("values")]
        public DispatchRecordUpdateValues Values { get; set; }
    }
}
