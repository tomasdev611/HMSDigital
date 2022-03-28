using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class CsvMappings
    {
        public int Id { get; set; }
        public int HospiceId { get; set; }
        public string MappingInJson { get; set; }
        public string MappingType { get; set; }
        public string MappedTable { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Hospices Hospice { get; set; }
        public virtual Users UpdatedByUser { get; set; }
    }
}
