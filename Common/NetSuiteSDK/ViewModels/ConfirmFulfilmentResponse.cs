using System.Collections.Generic;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class ConfirmFulfilmentResponse
    {
        public bool Success { get; set; }

        public IEnumerable<CreatedTransaction> CreatedTransactions { get; set; }

        public IEnumerable<DispatchRecord> DispatchRecords { get;set;}

        public ErrorResponse Error { get; set; }
    }
}
