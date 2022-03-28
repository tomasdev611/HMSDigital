using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class Addresses
    {
        public Addresses()
        {
            PatientAddress = new HashSet<PatientAddress>();
        }

        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public int ZipCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int? Plus4Code { get; set; }
        public bool IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        public Guid? AddressUuid { get; set; }

        public virtual ICollection<PatientAddress> PatientAddress { get; set; }
    }
}
