using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class InventoryDeleteRequest
    {
        [JsonProperty("hmsInternalId")]
        public int Id { get; set; }

        [JsonProperty("netsuiteInternalId")]
        public int NetSuiteInventoryId { get; set; }

        public string DeletedByUserEmail { get; set; }
    }
}
