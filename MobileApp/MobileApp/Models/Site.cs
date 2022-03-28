using System;
using System.Collections.Generic;

namespace MobileApp.Models
{
    public class Site
    {
        public int Id { get; set; }

        public int SiteCode { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public IEnumerable<SitePhoneNumber> SitePhoneNumber { get; set; }
    }
}
