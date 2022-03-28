using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class DispatchRecordDeleteResponse
    {
        public bool Success { get; set; }

        public int Total { get; set; }

        public int Remaining { get; set; }

        public int Processed { get; set; }

        public bool IsComplete { get; set; }

        public IEnumerable<int> Deleted { get; set; }

        public IEnumerable<int> Failed { get; set; }

        public string Message { get; set; }
    }
}
