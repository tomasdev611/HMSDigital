using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class GetTransferOrdersResponse
    {
        public IEnumerable<NetSuiteTransferOrder> Results { get; set; }

        public bool Success { get; set; }

        public ErrorResponse Error { get; set; }
    }
}
