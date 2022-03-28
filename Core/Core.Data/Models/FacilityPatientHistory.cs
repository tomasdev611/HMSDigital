using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class FacilityPatientHistory
    {
        public int Id { get; set; }
        public int? FacilityId { get; set; }
        public int? AdditionalField1 { get; set; }
        public Guid? PatientUuid { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Facilities Facility { get; set; }
        public virtual Users UpdatedByUser { get; set; }
    }
}
