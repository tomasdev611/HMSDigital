using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class OrderTypes
    {
        public OrderTypes()
        {
            OrderHeaders = new HashSet<OrderHeaders>();
            OrderLineItems = new HashSet<OrderLineItems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderHeaders> OrderHeaders { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItems { get; set; }
    }
}
