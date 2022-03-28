using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class ZonePaginatedList<T>
    {
        public bool Success { get; set; }

        [JsonProperty("page")]
        public int PageNumber { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPageCount { get; set; }

        [JsonProperty("total_results")]
        public int TotalRecordCount { get; set; }

        [JsonProperty("results_returned")]
        public int PageSize { get; set; }

        [JsonProperty("results")]
        public IEnumerable<T> Records { get; set; }

    }
}
