using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class MoveInventoryRequest
    {
            public int DestinationLocationId { get; set; }
            public int DestinationLocationTypeId { get; set; }
            public string SerialNumber { get; set; }
            public string AssetTagNumber { get; set; }
            public string ItemNumber { get; set; }
    }
}
