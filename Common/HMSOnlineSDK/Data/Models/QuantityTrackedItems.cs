using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class QuantityTrackedItems
    {
        public int Id { get; set; }
        public int? ProductVariantId { get; set; }
        public int? SiteId { get; set; }
        public int Available { get; set; }
        public int OnTruck { get; set; }
        public int NotReady { get; set; }
        public int OnPatient { get; set; }
        public int Disposed { get; set; }
        public int DisposalRequested { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
