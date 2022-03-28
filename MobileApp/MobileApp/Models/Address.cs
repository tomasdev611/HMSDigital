using System;

namespace MobileApp.Models
{
    public class Address
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public int ZipCode { get; set; }

        public int? Plus4Code { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid AddressUuid { get; set; }

        public int Id { get; set; }

        public bool IsVerified { get; set; }

        public string VerifiedBy { get; set; }

        public object Results { get; set; }

        public bool IsValid { get; set; }
    }
}