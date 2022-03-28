using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class FacilityMinimal
    {
        public string Name { get; set; }

        public int HospiceId { get; set; }

        public int HospiceLocationId { get; set; }

        public int? SiteId { get; set; }

        public IEnumerable<FacilityPhoneNumber> FacilityPhoneNumber { get; set; }
    }
}
