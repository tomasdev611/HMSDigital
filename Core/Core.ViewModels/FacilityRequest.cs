using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class FacilityRequest : FacilityMinimal
    {
        public AddressMinimal Address { get; set; }
    }
}
