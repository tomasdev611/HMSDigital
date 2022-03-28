using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class Facility : FacilityMinimal
    {
        public int Id { get; set; }

        public HospiceLocation HospiceLocation { get; set; }

        public Site Site { get; set; }

        public Address Address { get; set; }

        public bool IsDisable { get; set; }
    }
}
