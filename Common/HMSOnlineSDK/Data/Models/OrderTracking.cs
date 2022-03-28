using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class OrderTracking
    {
        public int Id { get; set; }
        public int Hms2PatientId { get; set; }
        public int Hms2OrderId { get; set; }
        public bool Loaded { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? DeliveryStartDate { get; set; }
        public DateTime? DeliveryEndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
