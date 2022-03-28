using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Sites
    {
        public Sites()
        {
            DispatchInstructions = new HashSet<DispatchInstructions>();
            DriversCurrentSite = new HashSet<Drivers>();
            Facilities = new HashSet<Facilities>();
            HospiceLocations = new HashSet<HospiceLocations>();
            Hospices = new HashSet<Hospices>();
            OrderFulfillmentLineItems = new HashSet<OrderFulfillmentLineItems>();
            OrderHeaders = new HashSet<OrderHeaders>();
            OrderLineItems = new HashSet<OrderLineItems>();
            SiteMembers = new HashSet<SiteMembers>();
            SitePhoneNumber = new HashSet<SitePhoneNumber>();
            SiteServiceAreas = new HashSet<SiteServiceAreas>();
        }

        public int Id { get; set; }
        public int? SiteCode { get; set; }
        public string Name { get; set; }
        public int? AddressId { get; set; }
        public bool IsDisabled { get; set; }
        public int? NetSuiteLocationId { get; set; }
        public int? ParentNetSuiteLocationId { get; set; }
        public string Vin { get; set; }
        public string Cvn { get; set; }
        public string LicensePlate { get; set; }
        public decimal? Capacity { get; set; }
        public decimal? Length { get; set; }
        public string LocationType { get; set; }
        public int? LocationTypeId { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime DeletedDateTime { get; set; }

        public virtual Addresses Address { get; set; }
        public virtual Users CreatedByUser { get; set; }
        public virtual Users DeletedByUser { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual Drivers DriversCurrentVehicle { get; set; }
        public virtual ICollection<DispatchInstructions> DispatchInstructions { get; set; }
        public virtual ICollection<Drivers> DriversCurrentSite { get; set; }
        public virtual ICollection<Facilities> Facilities { get; set; }
        public virtual ICollection<HospiceLocations> HospiceLocations { get; set; }
        public virtual ICollection<Hospices> Hospices { get; set; }
        public virtual ICollection<OrderFulfillmentLineItems> OrderFulfillmentLineItems { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeaders { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItems { get; set; }
        public virtual ICollection<SiteMembers> SiteMembers { get; set; }
        public virtual ICollection<SitePhoneNumber> SitePhoneNumber { get; set; }
        public virtual ICollection<SiteServiceAreas> SiteServiceAreas { get; set; }
    }
}
