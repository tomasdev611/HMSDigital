using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class AddressType
    {
        public AddressType()
        {
            PatientAddress = new HashSet<PatientAddress>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PatientAddress> PatientAddress { get; set; }
    }
}
