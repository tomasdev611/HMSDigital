using System;

namespace HMSDigital.Core.ViewModels
{
    public class NetSuiteLogRequest
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
