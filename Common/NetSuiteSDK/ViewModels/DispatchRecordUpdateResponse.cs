using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class DispatchRecordUpdateResponse
    {
        public string Status { get; set; }

        [JsonProperty("netsuiteId")]
        public int DispatchRecordId { get; set; }

        public string Error { get; set; }

        public string Message { get; set; }
    }
}
