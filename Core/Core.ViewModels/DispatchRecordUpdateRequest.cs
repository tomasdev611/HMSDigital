using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchRecordUpdateRequest
    {
        public int DispatchRecordId { get; set; }

        public DateTime? HmsDeliveryDate { get; set; }

        public DateTime? HmsPickupRequestDate { get; set; }

        public DateTime? PickupDate { get; set; }
    }
}
