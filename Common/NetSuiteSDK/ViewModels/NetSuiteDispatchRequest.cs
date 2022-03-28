using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteDispatchRequest
    {
        [JsonProperty("dispatchRecordIds")]
        public IEnumerable<int> DispatchRecordIds { get; set; }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("customerLocationId")]
        public int? CustomerLocationId { get; set; }

        [JsonProperty("customerId")]
        public int? CustomerId { get; set; }

        [JsonProperty("patientGuid")]
        public string PatientGuid { get; set; }

        [JsonProperty("netsuiteItemId")]
        public int? NetSuiteItemId { get; set; }

        [JsonProperty("netsuiteWareHouseId")]
        public int? NetSuiteWareHouseId { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("deliveryStartDate")]
        public DateTime? DeliveryStartDate { get; set; }

        [JsonProperty("deliveryEndDate")]
        public DateTime? DeliveryEndDate { get; set; }

        [JsonProperty("pickupStartDate")]
        public DateTime? PickupStartDate { get; set; }

        [JsonProperty("pickupEndDate")]
        public DateTime? PickupEndDate { get; set; }

        [JsonProperty("pickupRequestStartDate")]
        public DateTime? PickupRequestStartDate { get; set; }

        [JsonProperty("pickupRequestEndDate")]
        public DateTime? PickupRequestEndDate { get; set; }
    }
}
