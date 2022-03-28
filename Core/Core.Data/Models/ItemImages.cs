using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class ItemImages
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Items Item { get; set; }
        public virtual Users UpdatedByUser { get; set; }
    }
}
