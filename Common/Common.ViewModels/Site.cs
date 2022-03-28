using System.Collections.Generic;

namespace HMSDigital.Common.ViewModels
{
    public class Site : Vehicle
    {
        public int SiteCode { get; set; }

        public Address Address { get; set; }

        public string LocationType { get; set; }

        public IEnumerable<SitePhoneNumber> SitePhoneNumber { get; set; }

        public bool IsDisable { get; set; }

        public List<Site> Vehicles { get; set; }
    }
}
