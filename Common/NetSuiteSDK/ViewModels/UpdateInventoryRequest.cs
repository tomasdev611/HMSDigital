using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class UpdateInventoryRequest
    {
        public int NetSuiteInventoryId { get; set; }

        public int? NetSuiteItemId { get; set; }

        public string AssetTag { get; set; }
    }
}
