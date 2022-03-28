using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSContractRecordBulkResponse
    {
        public IEnumerable<NSContractRecordResponse> Response { get; set; }
    }
}
