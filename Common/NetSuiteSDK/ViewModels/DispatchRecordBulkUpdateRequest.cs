using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class DispatchRecordBulkUpdateRequest
    {
        [JsonProperty("dispatchRecords")]
        public IEnumerable<DispatchRecordUpdateRequest> DispatchRecords { get; set; }
    }
}
