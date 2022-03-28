using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSAddInventoryRequest
    {
        public int SiteId { get; set; }

        public IEnumerable<NSInventoryLineRequest> Items { get; set; }
    }
}
