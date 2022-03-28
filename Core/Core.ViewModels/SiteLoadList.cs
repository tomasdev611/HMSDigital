using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class SiteLoadList
    {
        public int SiteId { get; set; }

        public IEnumerable<VehicleLoadlist> Loadlists { get; set; }

        public int TotalItemCount { get; set; }

        public int TotalInventoryCount { get; set; }

        public int TotalOrderCount { get; set; }
    }
}
