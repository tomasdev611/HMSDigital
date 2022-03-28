using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
   public class DispatchRecordResponse
    {
        [JsonProperty("hmsInternalId")]
        public int Id { get; set; }

        [JsonProperty("netsuiteInternalId")]
        public int NetSuiteDispatchRecordId { get; set; }
    }
}
