using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Drivers
    {
        public Drivers()
        {
            OrderFulfillmentLineItems = new HashSet<OrderFulfillmentLineItems>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CurrentSiteId { get; set; }
        public int? CurrentVehicleId { get; set; }
        public decimal? LastKnownLatitude { get; set; }
        public decimal? LastKnownLongitude { get; set; }
        public DateTime? LocationUpdatedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Sites CurrentSite { get; set; }
        public virtual Sites CurrentVehicle { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<OrderFulfillmentLineItems> OrderFulfillmentLineItems { get; set; }
    }
}
