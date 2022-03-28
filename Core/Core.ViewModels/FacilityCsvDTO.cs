using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class FacilityCsvDTO : FacilityCsvRequest
    {
        public int HospiceId { get; set; }

        public int HospiceLocationId { get; set; }

        public IEnumerable<FacilityPhoneNumber> FacilityPhoneNumber { get; set; }

        public Address Address { get; set; }
    }
}
