using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class OrderStatusUpdateRequest
    {
        public IEnumerable<int> OrderIds { get; set; }

        public int? StatusId { get; set; }

        public int? DispatchStatusId { get; set; }
    }
}
