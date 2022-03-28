using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class OrderStatusUpdateRequest
    {
        public IEnumerable<int> OrderIds { get; set; }

        public int? StatusId { get; set; }

        public int? DispatchStatusId { get; set; }
    }
}
