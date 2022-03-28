using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class DispatchRecordUpdateValues
    {
        [JsonProperty("customerLocationId")]
        public int CustomerLocationId { get; set; }

        [JsonProperty("hmsPickupRequestDate")]
        public DateTime? HmsPickupRequestDate { get; set; }

        [JsonProperty("hmsDeliveryDate")]
        public DateTime? HmsDeliveryDate { get; set; }

        [JsonProperty("pickupDate")]
        public DateTime? PickupDate { get; set; }
    }
}
