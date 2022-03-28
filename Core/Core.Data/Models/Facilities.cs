using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Facilities
    {
        public Facilities()
        {
            FacilityPatient = new HashSet<FacilityPatient>();
            FacilityPatientHistory = new HashSet<FacilityPatientHistory>();
            FacilityPhoneNumber = new HashSet<FacilityPhoneNumber>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HospiceId { get; set; }
        public int? HospiceLocationId { get; set; }
        public int? AddressId { get; set; }
        public bool IsDisable { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public int? SiteId { get; set; }

        public virtual Addresses Address { get; set; }
        public virtual Users CreatedByUser { get; set; }
        public virtual Hospices Hospice { get; set; }
        public virtual HospiceLocations HospiceLocation { get; set; }
        public virtual Sites Site { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<FacilityPatient> FacilityPatient { get; set; }
        public virtual ICollection<FacilityPatientHistory> FacilityPatientHistory { get; set; }
        public virtual ICollection<FacilityPhoneNumber> FacilityPhoneNumber { get; set; }
    }
}
