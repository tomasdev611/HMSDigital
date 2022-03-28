using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class Root
    {
        public string AuthenticationResultCode { get; set; }

        public string BrandLogoUri { get; set; }

        public string Copyright { get; set; }

        public List<ResourceSet> ResourceSets { get; set; }

        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string TraceId { get; set; }
    }
}
