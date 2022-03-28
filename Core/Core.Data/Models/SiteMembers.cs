using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class SiteMembers
    {
        public SiteMembers()
        {
            ItemTransferRequests = new HashSet<ItemTransferRequests>();
        }

        public int Id { get; set; }
        public int SiteId { get; set; }
        public int UserId { get; set; }
        public string Designation { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Sites Site { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<ItemTransferRequests> ItemTransferRequests { get; set; }
    }
}
