using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSUpdateInventoryResponse
    {
        public int InventoryId { get; set; }

        public int NetSuiteInventoryId { get; set; }

        public int ItemId { get; set; }

        public int NetSuiteItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public string SerialNumber { get; set; }

        public string AssetTag { get; set; }
    }
}
