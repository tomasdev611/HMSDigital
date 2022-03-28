using System;

namespace HMSDigital.Common.ViewModels
{
    public class Address : AddressMinimal
    {
        public int Id { get; set; }

        public bool IsVerified { get; set; }
        
        public string VerifiedBy { get; set; }
        
        public Guid AddressUuid { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string Country { get; set; }

        public string Results { get; set; }

        public bool IsValid { get; set; }
    }
}
