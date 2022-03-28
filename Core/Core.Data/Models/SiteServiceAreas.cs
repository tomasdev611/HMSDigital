using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class SiteServiceAreas
    {
        public int Id { get; set; }
        public int? SiteId { get; set; }
        public int? ZipCode { get; set; }
        public decimal? AdditionalField1 { get; set; }
        public decimal? AdditionalField2 { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Sites Site { get; set; }
        public virtual Users UpdatedByUser { get; set; }
    }
}
