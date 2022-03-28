using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class FacilityPatient
    {
        public int Id { get; set; }
        public int? FacilityId { get; set; }
        public int? AdditionalField1 { get; set; }
        public Guid? PatientUuid { get; set; }
        public string PatientRoomNumber { get; set; }

        public virtual Facilities Facility { get; set; }
    }
}
