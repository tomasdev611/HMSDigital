using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class GetPurchaseOrdersRequest
    {
        [JsonProperty("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        [JsonProperty("nsOrderStatus")]
        public int? StatusId { get; set; }

        [JsonProperty("site")]
        public int NetSuiteLocationId { get; set; }
    }
}
