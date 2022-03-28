using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class HospiceLocations
    {
        public HospiceLocations()
        {
            ContractRecords = new HashSet<ContractRecords>();
            Facilities = new HashSet<Facilities>();
            Hms2Contracts = new HashSet<Hms2Contracts>();
            Hms2HmsDigitalHospiceMappings = new HashSet<Hms2HmsDigitalHospiceMappings>();
            HospiceLocationMembers = new HashSet<HospiceLocationMembers>();
            OrderHeaders = new HashSet<OrderHeaders>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HospiceId { get; set; }
        public int? SiteId { get; set; }
        public int? NetSuiteCustomerId { get; set; }
        public int? AddressId { get; set; }
        public int? PhoneNumberId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? Hms2Id { get; set; }
        public int? NetSuiteContractingCustomerId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedDateTime { get; set; }
        public int? DeletedByUserId { get; set; }

        public virtual Addresses Address { get; set; }
        public virtual Users CreatedByUser { get; set; }
        public virtual CustomerTypes CustomerType { get; set; }
        public virtual Users DeletedByUser { get; set; }
        public virtual Hospices Hospice { get; set; }
        public virtual PhoneNumbers PhoneNumber { get; set; }
        public virtual Sites Site { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<ContractRecords> ContractRecords { get; set; }
        public virtual ICollection<Facilities> Facilities { get; set; }
        public virtual ICollection<Hms2Contracts> Hms2Contracts { get; set; }
        public virtual ICollection<Hms2HmsDigitalHospiceMappings> Hms2HmsDigitalHospiceMappings { get; set; }
        public virtual ICollection<HospiceLocationMembers> HospiceLocationMembers { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeaders { get; set; }
    }
}
