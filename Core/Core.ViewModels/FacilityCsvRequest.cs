using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class FacilityCsvRequest : AddressMinimal
    {
        public string Name { get; set; }

        public string HospiceLocationName { get; set; }

        public long? PhoneNumber { get; set; }
    }
}
