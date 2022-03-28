using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class PhoneNumbers
    {
        public PhoneNumbers()
        {
            FacilityPhoneNumber = new HashSet<FacilityPhoneNumber>();
            HospiceLocations = new HashSet<HospiceLocations>();
            Hospices = new HashSet<Hospices>();
            SitePhoneNumber = new HashSet<SitePhoneNumber>();
        }

        public int Id { get; set; }
        public int CountryCode { get; set; }
        public long Number { get; set; }
        public bool IsVerified { get; set; }
        public int? NumberTypeId { get; set; }
        public bool? IsPrimary { get; set; }
        public string SkentityType { get; set; }
        public int? SkentityId { get; set; }
        public int? AdditionalField1 { get; set; }

        public virtual PhoneNumberTypes NumberType { get; set; }
        public virtual ICollection<FacilityPhoneNumber> FacilityPhoneNumber { get; set; }
        public virtual ICollection<HospiceLocations> HospiceLocations { get; set; }
        public virtual ICollection<Hospices> Hospices { get; set; }
        public virtual ICollection<SitePhoneNumber> SitePhoneNumber { get; set; }
    }
}
