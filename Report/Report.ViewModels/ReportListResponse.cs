using System.Collections.Generic;

namespace HMSDigital.Report.ViewModels
{
    public class ReportListResponse
    {
        public int TotalCount { get; set; }

        public IList<ReportData> Data { get; set; }

        public IList<ReportTableHeader> TableHeader { get; set; }
    }
}
