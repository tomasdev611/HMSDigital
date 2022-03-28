using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientMinimal
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int HospiceId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal PatientHeight { get; set; }

        public int PatientWeight { get; set; }

        public bool IsInfectious { get; set; }

        public int? Hms2Id { get; set; }

        public IEnumerable<PhoneNumberMinimal> PhoneNumbers { get; set; }

        public int? HospiceLocationId { get; set; }

        public string Diagnosis { get; set; }

        public IEnumerable<PatientNote> PatientNotes { get; set; }
    }
}
