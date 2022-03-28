using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Hms2HmsDigitalHospiceMappings
    {
        public int Hms2id { get; set; }
        public string Hms2name { get; set; }
        public int? NetSuiteCustomerId { get; set; }
        public string NetSuiteName { get; set; }
        public int? HospiceId { get; set; }
        public string HospiceName { get; set; }
        public int? HospiceLocationId { get; set; }
        public string HospiceLocationName { get; set; }
        public string DigitalType { get; set; }

        public virtual Hospices Hospice { get; set; }
        public virtual HospiceLocations HospiceLocation { get; set; }
    }
}
