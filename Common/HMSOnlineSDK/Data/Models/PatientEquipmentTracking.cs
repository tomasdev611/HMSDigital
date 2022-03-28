using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class PatientEquipmentTracking
    {
        public int Id { get; set; }
        public int Hms2PatientId { get; set; }
        public int PickupOrderTrackingId { get; set; }
        public int DeliveryOrderTrackingId { get; set; }
        public int? ItemId { get; set; }
        public int? QuantityTrackedItemId { get; set; }
        public string AssetTag { get; set; }
        public string InvCode { get; set; }
        public bool? Pickedup { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
