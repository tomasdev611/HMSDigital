using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class AdjustBatchInventoryResponse
    {
        public IEnumerable<AdjustInventoryResult> Items { get; set; }
    }
}
