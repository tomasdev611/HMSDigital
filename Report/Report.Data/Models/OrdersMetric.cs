using System;

namespace HMSDigital.Report.Data.Models
{
    public class OrdersMetric : ReportBase
    {
        public int OrderHeaderId { get; set; }

        public int OrderTypeId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string OrderType { get; set; }

        public int OrderDateId { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public string OrderNumber { get; set; }

        public string Area { get; set; }

        public int AreaId { get; set; }

        public string Region { get; set; }

        public int RegionId { get; set; }
    }
}
