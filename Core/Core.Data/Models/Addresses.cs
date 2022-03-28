using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Addresses
    {
        public Addresses()
        {
            Facilities = new HashSet<Facilities>();
            HospiceLocations = new HashSet<HospiceLocations>();
            Hospices = new HashSet<Hospices>();
            OrderHeadersDeliveryAddress = new HashSet<OrderHeaders>();
            OrderHeadersPickupAddress = new HashSet<OrderHeaders>();
            OrderLineItemsDeliveryAddress = new HashSet<OrderLineItems>();
            OrderLineItemsPickupAddress = new HashSet<OrderLineItems>();
            Sites = new HashSet<Sites>();
        }

        public int Id { get; set; }
        public int? NetSuiteAddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public int ZipCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int? Plus4Code { get; set; }
        public bool IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        public Guid? AddressUuid { get; set; }
        public string SkentityType { get; set; }
        public int? SkentityId { get; set; }

        public virtual ICollection<Facilities> Facilities { get; set; }
        public virtual ICollection<HospiceLocations> HospiceLocations { get; set; }
        public virtual ICollection<Hospices> Hospices { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeadersDeliveryAddress { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeadersPickupAddress { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItemsDeliveryAddress { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItemsPickupAddress { get; set; }
        public virtual ICollection<Sites> Sites { get; set; }
    }
}
