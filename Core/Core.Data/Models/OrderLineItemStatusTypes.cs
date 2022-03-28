using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class OrderLineItemStatusTypes
    {
        public OrderLineItemStatusTypes()
        {
            OrderLineItemsDispatchStatus = new HashSet<OrderLineItems>();
            OrderLineItemsStatus = new HashSet<OrderLineItems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderLineItems> OrderLineItemsDispatchStatus { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItemsStatus { get; set; }
    }
}
