using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class PhoneNumbers
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int CountryCode { get; set; }
        public long Number { get; set; }
        public int NumberTypeId { get; set; }
        public bool? IsPrimary { get; set; }
        public bool ReceiveEtaTextmessage { get; set; }
        public bool ReceiveSurveyTextMessage { get; set; }
        public string ContactName { get; set; }
        public bool IsSelfPhone { get; set; }

        public virtual PhoneNumberTypes NumberType { get; set; }
        public virtual PatientDetails Patient { get; set; }
    }
}
