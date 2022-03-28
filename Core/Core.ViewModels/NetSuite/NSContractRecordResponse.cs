using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSContractRecordResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public int NetSuiteContractRecordId { get; set; }

        public int? HmsContractRecordId { get; set; }
    }
}
