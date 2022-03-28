using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Hospices
    {
        public Hospices()
        {
            ContractRecords = new HashSet<ContractRecords>();
            CreditHoldHistory = new HashSet<CreditHoldHistory>();
            CsvMappings = new HashSet<CsvMappings>();
            Facilities = new HashSet<Facilities>();
            Hms2Contracts = new HashSet<Hms2Contracts>();
            Hms2HmsDigitalHospiceMappings = new HashSet<Hms2HmsDigitalHospiceMappings>();
            HospiceLocations = new HashSet<HospiceLocations>();
            HospiceMember = new HashSet<HospiceMember>();
            OrderHeaders = new HashSet<OrderHeaders>();
            SubscriptionItems = new HashSet<SubscriptionItems>();
            Subscriptions = new HashSet<Subscriptions>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? NetSuiteCustomerId { get; set; }
        public int? AssignedSiteId { get; set; }
        public int? AddressId { get; set; }
        public int? PhoneNumberId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? Hms2Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public bool IsCreditOnHold { get; set; }
        public int? CreditHoldByUserId { get; set; }
        public DateTime? CreditHoldDateTime { get; set; }
        public string CreditHoldNote { get; set; }
        public Guid? FhirOrganizationId { get; set; }
        public int? NetSuiteContractingCustomerId { get; set; }

        public virtual Addresses Address { get; set; }
        public virtual Sites AssignedSite { get; set; }
        public virtual Users CreatedByUser { get; set; }
        public virtual Users CreditHoldByUser { get; set; }
        public virtual CustomerTypes CustomerType { get; set; }
        public virtual Users DeletedByUser { get; set; }
        public virtual PhoneNumbers PhoneNumber { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<ContractRecords> ContractRecords { get; set; }
        public virtual ICollection<CreditHoldHistory> CreditHoldHistory { get; set; }
        public virtual ICollection<CsvMappings> CsvMappings { get; set; }
        public virtual ICollection<Facilities> Facilities { get; set; }
        public virtual ICollection<Hms2Contracts> Hms2Contracts { get; set; }
        public virtual ICollection<Hms2HmsDigitalHospiceMappings> Hms2HmsDigitalHospiceMappings { get; set; }
        public virtual ICollection<HospiceLocations> HospiceLocations { get; set; }
        public virtual ICollection<HospiceMember> HospiceMember { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeaders { get; set; }
        public virtual ICollection<SubscriptionItems> SubscriptionItems { get; set; }
        public virtual ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
