using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
   public class WarehouseResponse
    {
        [JsonProperty("hmsInternalId")]
        public int Id { get; set; }

        [JsonProperty("netsuiteInternalId")]
        public int NetSuiteLocationId { get; set; }
    }
}
