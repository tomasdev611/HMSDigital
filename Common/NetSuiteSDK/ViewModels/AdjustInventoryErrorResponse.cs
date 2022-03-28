using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class AdjustInventoryErrorResponse
    {
        public bool Success { get; set; }

        public ErrorResponse Error { get; set; }
    }
}
