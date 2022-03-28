using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Patient.Data.Models
{
    public partial class PatientDetails
    {
        public PatientDetails()
        {
            PatientAddress = new HashSet<PatientAddress>();
            PatientNotes = new HashSet<PatientNotes>();
            PhoneNumbers = new HashSet<PhoneNumbers>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double PatientHeight { get; set; }
        public int PatientWeight { get; set; }
        public bool? IsInfectious { get; set; }
        public int? HospiceId { get; set; }
        public int? HospiceLocationId { get; set; }
        public int? FacilityId { get; set; }
        public Guid? UniqueId { get; set; }
        public string LastOrderNumber { get; set; }
        public DateTime? LastOrderDateTime { get; set; }
        public string Diagnosis { get; set; }
        public int? Hms2Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? StatusChangedDate { get; set; }
        public int? StatusReasonId { get; set; }
        public Guid? DataBridgeRunUuid { get; set; }
        public DateTime? DataBridgeRunDateTime { get; set; }
        public Guid? FhirPatientId { get; set; }
        public bool? AdditionalField3 { get; set; }
        public string AdditionalField4 { get; set; }
        public decimal? AdditionalField5 { get; set; }
        public int? AdditionalField7 { get; set; }
        public int? AdditionalField8 { get; set; }
        public int? AdditionalField9 { get; set; }
        public bool? AdditionalField10 { get; set; }

        public virtual PatientStatusTypes Status { get; set; }
        public virtual PatientStatusTypes StatusReason { get; set; }
        public virtual ICollection<PatientAddress> PatientAddress { get; set; }
        public virtual ICollection<PatientNotes> PatientNotes { get; set; }
        public virtual ICollection<PhoneNumbers> PhoneNumbers { get; set; }
    }
}
