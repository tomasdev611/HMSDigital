using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Fulfillment.ViewModels
{
    public class ErrorResponse
    {
        public string AuthenticationResultCode { get; set; }

        public IEnumerable<string> ErrorDetails { get; set; }

        public IEnumerable<string> ResourceSets { get; set; }

        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }
    }
}
