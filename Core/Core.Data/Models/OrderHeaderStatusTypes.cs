using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class OrderHeaderStatusTypes
    {
        public OrderHeaderStatusTypes()
        {
            OrderHeadersDispatchStatus = new HashSet<OrderHeaders>();
            OrderHeadersStatus = new HashSet<OrderHeaders>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderHeaders> OrderHeadersDispatchStatus { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeadersStatus { get; set; }
    }
}
