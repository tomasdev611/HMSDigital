using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientSearchRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? HospiceId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string AddressType { get; set; }

        public AddressMinimal Address { get; set; }
    }
}
