using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class LinkPatientProducts
    {
        public int LinkId { get; set; }
        public int? OrderId { get; set; }
        public int? PatientId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public string AssetTag { get; set; }
        public int Delivered { get; set; }
        public string LotNum { get; set; }
        public DateTime? PickupTimestamp { get; set; }
        public DateTime? DeliveredTimestamp { get; set; }
        public int? Pickedup { get; set; }
        public string AssetTagPickup { get; set; }
        public int? AssetStatus { get; set; }
        public int? InventoryProcessed { get; set; }
        public int? EmailCheckProcessed { get; set; }
        public int? IssueNotification { get; set; }
        public int? OffCapItem { get; set; }
        public decimal? OffCapCost { get; set; }
        public int? OffCapUser { get; set; }
        public int? DmeServiced { get; set; }
        public int ExchangeId { get; set; }
        public string AssetTagExchange { get; set; }
        public int RespiteId { get; set; }
    }
}
