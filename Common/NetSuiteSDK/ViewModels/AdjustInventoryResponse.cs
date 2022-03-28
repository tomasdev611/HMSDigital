using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class AdjustInventoryResponse : AdjustInventoryErrorResponse
    {
        public IEnumerable<int> InventoryAdjustmentsCreated { get; set; }
    }
}
