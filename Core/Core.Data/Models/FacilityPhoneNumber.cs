using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class FacilityPhoneNumber
    {
        public int Id { get; set; }
        public int? FacilityId { get; set; }
        public int? PhoneNumberId { get; set; }

        public virtual Facilities Facility { get; set; }
        public virtual PhoneNumbers PhoneNumber { get; set; }
    }
}
