using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class Patientlineitems
    {
        public int? Lpm { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CreateTs { get; set; }
        public DateTime? DeliveryTs { get; set; }
        public string Instructions { get; set; }
        public int? InvId { get; set; }
        public DateTime LastChanged { get; set; }
        public string LotNumber { get; set; }
        public int? OrderedBy { get; set; }
        public string Other { get; set; }
        public int? PatientId { get; set; }
        public string PickupCode { get; set; }
        public int? PickupOrderedBy { get; set; }
        public DateTime? PickupReqTs { get; set; }
        public DateTime? PickupTs { get; set; }
        public decimal? Quantity { get; set; }
        public int? SnId { get; set; }
        public int? Status { get; set; }
        public int Id { get; set; }
        public int? PatientOrdersId { get; set; }
        public string HistoryArchive { get; set; }
    }
}
