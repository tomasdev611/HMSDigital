using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSUpdateInventoryRequest
    {
        public int InventoryId { get; set; }
        public int? ItemId { get; set; }
        public string AssetTag { get; set; }
    }
}
