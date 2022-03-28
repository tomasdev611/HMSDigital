using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class ItemTransferRequests
    {
        public ItemTransferRequests()
        {
            DispatchInstructions = new HashSet<DispatchInstructions>();
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public int SourceLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public int StatusId { get; set; }
        public int ItemCount { get; set; }
        public int? DestinationSiteMemberId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int? AdditionalField1 { get; set; }
        public int? AdditionalField2 { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual SiteMembers DestinationSiteMember { get; set; }
        public virtual Items Item { get; set; }
        public virtual TransferRequestStatusTypes Status { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<DispatchInstructions> DispatchInstructions { get; set; }
    }
}
