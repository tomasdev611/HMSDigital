using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class PatientAddress
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? AddressId { get; set; }
        public int? AddressTypeId { get; set; }

        public virtual Addresses Address { get; set; }
        public virtual AddressType AddressType { get; set; }
        public virtual PatientDetails Patient { get; set; }
    }
}
