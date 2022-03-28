using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class AdjustInventoryResult : AdjustInventoryRequest
    {
        public IEnumerable<int> InventoryAdjustmentsCreated { get; set; }

        public bool Success { get; set; }

        public ErrorResponse Error { get; set; }
    }
}
