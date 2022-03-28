using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class MergePatientBaseRequest
    {
        public Guid DuplicatePatientUUID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal PatientHeight { get; set; }

        public int PatientWeight { get; set; }

        public bool IsInfectious { get; set; }

        public string Diagnosis { get; set; }

        public IEnumerable<PhoneNumberMinimal> PhoneNumbers { get; set; }
    }
}
