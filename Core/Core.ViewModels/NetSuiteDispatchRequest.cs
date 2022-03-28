using System;

namespace HMSDigital.Core.ViewModels
{
    public class NetSuiteDispatchRequest
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public DateTime? DeliveryFromDate { get; set; }

        public DateTime? DeliveryToDate { get; set; }

        public DateTime? PickUpFromDate { get; set; }

        public DateTime? PickUpToDate { get; set; }

        public DateTime? PickUpRequestFromDate { get; set; }

        public DateTime? PickUpRequestToDate { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? SiteId { get; set; }

        public Guid? PatientUuid { get; set; }

        public int? HospiceId { get; set; }

        public int? HospiceLocationId { get; set; }

        public int? ItemId { get; set; }
    }
}
