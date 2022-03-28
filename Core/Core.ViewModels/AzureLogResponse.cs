using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class AzureLogResponse<T> where T : class
    {
        public IEnumerable<T> APILogs { get; set; }

        public TableContinuationToken ContinuationToken { get; set; }
    }
}
