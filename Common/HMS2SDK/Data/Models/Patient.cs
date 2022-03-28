using System;

namespace HMS2SDK.Data.Models
{
    public partial class Patient
    {
        public string AcctNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CTaxCode { get; set; }
        public string City { get; set; }
        public string Class { get; set; }
        public string CoTaxCode { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public string Comment4 { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Diagnosis { get; set; }
        public bool IsPediatric { get; set; }
        public string Division { get; set; }
        public string Firstname { get; set; }
        public int? Inactive { get; set; }
        public DateTime LastChanged { get; set; }
        public string Lastname { get; set; }
        public string MInitial { get; set; }
        public string MapId { get; set; }
        public string Nurse { get; set; }
        public string NursingHome { get; set; }
        public string O2Order { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string STaxCode { get; set; }
        public string State { get; set; }
        public int? Taxable { get; set; }
        public string Team { get; set; }
        public string Zip { get; set; }
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public int? HospiceId { get; set; }
        public int? Dc { get; set; }
        public string DobMonth { get; set; }
        public string DobYear { get; set; }
        public string DobDay { get; set; }
        public string LocationType { get; set; }
        public string OtherContact { get; set; }
        public string Cpap { get; set; }
        public string Epap { get; set; }
        public string Ipap { get; set; }
        public int Indigent { get; set; }
        public DateTime? DcDate { get; set; }
        public int? PatientHeight { get; set; }
        public int? PatientWeight { get; set; }
        public string FacilityName { get; set; }
        public int? LocationId { get; set; }
        public int? CustomerId { get; set; }
        public int? DoNotCall { get; set; }
        public int? HeightFeet { get; set; }
        public int? HeightInches { get; set; }
        public int? FacilityId { get; set; }
        public int? DmeServiced { get; set; }
        public DateTime? PreviousDcDate { get; set; }
        public int? DefaultDme { get; set; }
    }
}
