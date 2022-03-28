using Newtonsoft.Json;
using System;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteLogRequest
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }
    }
}
