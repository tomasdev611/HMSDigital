using System.Collections.Generic;

namespace MobileApp.Models
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
